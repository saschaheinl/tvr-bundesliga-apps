using System.Reflection;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TVR.Bundesliga.API.Contracts.Requests;
using TVR.Bundesliga.API.Contracts.Requests.Event;
using TVR.Bundesliga.API.Contracts.Requests.Guest;
using TVR.Bundesliga.API.Core.Commands;
using TVR.Bundesliga.API.Core.Context;
using TVR.Bundesliga.API.Core.Queries;
using TVR.Bundesliga.API.Core.Requests;
using TVR.Bundesliga.API.Domain.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var dbConnectionString = Environment.GetEnvironmentVariable("POSTGRESQL_CONNECTION_STRING") ?? string.Empty;
if (string.IsNullOrEmpty(dbConnectionString))
{
    throw new ArgumentException("Critical environment variable is missing.");
}

Console.WriteLine($"The Google Project is ${Environment.GetEnvironmentVariable("GOOGLE_PROJECT_NAME")}");

builder.Services.AddDbContext<EventDb>(d => d.UseNpgsql(dbConnectionString));
builder.Services.AddDbContext<GuestDb>(d => d.UseNpgsql(dbConnectionString));
builder.Services.AddDbContext<TicketDb>(d => d.UseNpgsql(dbConnectionString));
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.Load("TVR.Bundesliga.API.Core")));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        corsBuilder =>
        {
            corsBuilder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority =
            $"https://securetoken.google.com/{Environment.GetEnvironmentVariable("GOOGLE_PROJECT_NAME")}";
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = $"https://securetoken.google.com/{Environment.GetEnvironmentVariable("GOOGLE_PROJECT_NAME")}",
            ValidateAudience = true,
            ValidAudience = Environment.GetEnvironmentVariable("GOOGLE_PROJECT_NAME"),
            ValidateLifetime = true
        };
    });
builder.Services.AddAuthorization();

var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/", () => "Es gibt nur ein Gas: Vollgas! Es gibt nur ein Rat: Refrath!");

app.MapGet("/events", (IMediator mediator, CancellationToken cancellationToken) =>
        mediator.Send(new GetAllEventsQuery(), cancellationToken))
    .Produces<List<Event>>()
    .RequireAuthorization();

app.MapGet("/events/previous", (IMediator mediator, CancellationToken cancellationToken) =>
    {
        var request = new GetEventsByTimeframeQuery(DateTimeOffset.UtcNow, Timeframe.Previous);
        return mediator.Send(request, cancellationToken);
    })
    .Produces<List<Event>>()
    .RequireAuthorization();

app.MapGet("/events/{eventId:int}", (int eventId, IMediator mediator, CancellationToken cancellationToken) =>
        {
            var request = new GetEventByIdQuery(eventId);
            return mediator.Send(request, cancellationToken);
        }
    )
    .Produces<Event>()
    .ProducesProblem(StatusCodes.Status404NotFound)
    .RequireAuthorization();

app.MapGet("/events/upcoming", (IMediator mediator, CancellationToken cancellationToken) =>
    {
        var request = new GetEventsByTimeframeQuery(DateTimeOffset.UtcNow, Timeframe.Upcoming);
        return mediator.Send(request, cancellationToken);
    })
    .Produces<List<Event>>()
    .RequireAuthorization();

app.MapPost("/events", async (CreateEventRequest request, IMediator mediator, CancellationToken cancellationToken) =>
        {
            var command = new AddNewEventCommand(request.Name, request.League, request.Date);
            var result = await mediator.Send(command, cancellationToken);

            return Results.Created($"/events/{result.Id}", result);
        }
    )
    .Produces<Event>(StatusCodes.Status201Created)
    .ProducesProblem(StatusCodes.Status400BadRequest)
    .RequireAuthorization();

app.MapPut("/events/{eventId:int}",
        async (int eventId, UpdateEventRequest request, IMediator mediator, CancellationToken cancellationToken) =>
        {
            var command = new UpdateEventByIdCommand(eventId, request.Name, request.League, request.Date);
            await mediator.Send(command, cancellationToken);

            return Results.NoContent();
        })
    .Produces(StatusCodes.Status204NoContent)
    .ProducesProblem(StatusCodes.Status400BadRequest)
    .RequireAuthorization();

app.MapPost("/guests", async (CreateGuestRequest request, IMediator mediator, CancellationToken cancellationToken) =>
    {
        var command = new AddNewGuestCommand(request.Name, request.EmailAddress);
        var result = await mediator.Send(command, cancellationToken);

        return Results.Created($"/guests/{result.Id}", result);
    })
    .Produces<List<Guest>>();

app.MapGet("/guests/{guestId:int}", (int guestId, IMediator mediator, CancellationToken cancellationToken) =>
    {
        var query = new GetGuestByIdQuery(guestId);
        return mediator.Send(query, cancellationToken);
    })
    .Produces<Guest>()
    .ProducesProblem(StatusCodes.Status400BadRequest)
    .RequireAuthorization();

app.MapGet("/guests/search",
        async (int? id, string? name, string? mailAddress, IMediator mediator, CancellationToken cancellationToken) =>
        {
            var query = new SearchGuestQuery(id, name, mailAddress);
            var result = await mediator.Send(query, cancellationToken);

            return result;
        })
    .Produces<List<Event>>()
    .ProducesProblem(StatusCodes.Status400BadRequest)
    .RequireAuthorization();

app.MapGet("/guests", (IMediator mediator, CancellationToken cancellationToken) =>
    mediator.Send(new GetAllGuestsQuery(), cancellationToken))
    .Produces<Guest>()
    .ProducesProblem(StatusCodes.Status400BadRequest)
    .RequireAuthorization();

app.MapPost("/tickets", async (CreateTicketRequest request, IMediator mediator, CancellationToken cancellationToken) =>
    {
        var query = new AddNewTicketCommand(request.EventId, request.Type, request.GuestId, request.IncludedVisits,
            request.Price);
        var ticketToAdd = await mediator.Send(query, cancellationToken);

        return Results.Created($"/tickets/{ticketToAdd.Id}", ticketToAdd);
    })
    .Produces(StatusCodes.Status201Created)
    .Produces(StatusCodes.Status400BadRequest)
    .Produces(StatusCodes.Status409Conflict)
    .RequireAuthorization();

app.MapGet("/tickets/{ticketId:int}", (int ticketId, IMediator mediator, CancellationToken cancellationToken) =>
    {
        var query = new GetTicketByIdQuery(ticketId);

        return mediator.Send(query, cancellationToken);
    })
    .Produces<Ticket>()
    .ProducesProblem(StatusCodes.Status400BadRequest)
    .RequireAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors("AllowAll");
}

app.Run();
