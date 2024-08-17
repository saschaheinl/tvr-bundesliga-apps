using System.Reflection;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TVR.Bundesliga.API.Contracts.Requests;
using TVR.Bundesliga.API.Contracts.Requests.Event;
using TVR.Bundesliga.API.Contracts.Requests.Guest;
using TVR.Bundesliga.API.Core.Commands;
using TVR.Bundesliga.API.Core.Context;
using TVR.Bundesliga.API.Core.Queries;
using TVR.Bundesliga.API.Core.Requests;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var dbConnectionString = Environment.GetEnvironmentVariable("POSTGRESQL_CONNECTION_STRING") ?? string.Empty;
if (string.IsNullOrEmpty(dbConnectionString))
{
    throw new ArgumentException("Critical environment variable is missing.");
}

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

var app = builder.Build();

app.MapGet("/", () => "Es gibt nur ein Gas: Vollgas! Es gibt nur ein Rat: Refrath!");

app.MapGet("/events", (IMediator mediator, CancellationToken cancellationToken) =>
    mediator.Send(new GetAllEventsQuery(), cancellationToken));

app.MapGet("/events/previous", (IMediator mediator, CancellationToken cancellationToken) =>
    {
        var request = new GetEventsByTimeframeQuery(DateTimeOffset.UtcNow, Timeframe.Previous);
        return mediator.Send(request, cancellationToken);
    })
    .Produces(StatusCodes.Status200OK);

app.MapGet("/events/{eventId:int}", (int eventId, IMediator mediator, CancellationToken cancellationToken) =>
        {
            var request = new GetEventByIdQuery(eventId);
            return mediator.Send(request, cancellationToken);
        }
    )
    .Produces(StatusCodes.Status200OK)
    .ProducesProblem(StatusCodes.Status404NotFound);

app.MapGet("/events/upcoming", (IMediator mediator, CancellationToken cancellationToken) =>
    {
        var request = new GetEventsByTimeframeQuery(DateTimeOffset.UtcNow, Timeframe.Upcoming);
        return mediator.Send(request, cancellationToken);
    })
    .Produces(StatusCodes.Status200OK);

app.MapPost("/events", async (CreateEventRequest request, IMediator mediator, CancellationToken cancellationToken) =>
        {
            var command = new AddNewEventCommand(request.Name, request.League, request.Date);
            var result = await mediator.Send(command, cancellationToken);

            return Results.Created($"/events/{result.Id}", result);
        }
    )
    .Produces(StatusCodes.Status201Created)
    .ProducesProblem(StatusCodes.Status400BadRequest);

app.MapPut("/events/{eventId:int}", async (int eventId, UpdateEventRequest request, IMediator mediator, CancellationToken cancellationToken) =>
    {
        var command = new UpdateEventByIdCommand(eventId, request.Name, request.League, request.Date);
        await mediator.Send(command, cancellationToken);
        
        return Results.NoContent();
    })
    .Produces(StatusCodes.Status204NoContent)
    .ProducesProblem(StatusCodes.Status400BadRequest);

app.MapPost("/guests", async (CreateGuestRequest request, IMediator mediator, CancellationToken cancellationToken) =>
{
    var command = new AddNewGuestCommand(request.Name, request.EmailAddress);
    var result = await mediator.Send(command, cancellationToken);

    return Results.Created($"/guests/{result.Id}", result);
});

app.MapGet("/guests/{guestId:int}", (int guestId, IMediator mediator, CancellationToken cancellationToken) =>
{
    var query = new GetGuestByIdQuery(guestId);
    return mediator.Send(query, cancellationToken);
});

app.MapGet("/guests/search", async (int? id, string? name, string? mailAddress, IMediator mediator, CancellationToken cancellationToken) =>
{
    var query = new SearchGuestQuery(id, name, mailAddress);
    var result = await mediator.Send(query, cancellationToken);

    return result;
});

app.MapGet("/guests", (IMediator mediator, CancellationToken cancellationToken) =>
    mediator.Send(new GetAllGuestsQuery(), cancellationToken));

app.MapPost("/tickets", async (CreateTicketRequest request, IMediator mediator, CancellationToken cancellationToken) =>
    {
        var query = new AddNewTicketCommand(request.EventId, request.Type, request.GuestId, request.IncludedVisits,
            request.Price);
        var ticketToAdd = await mediator.Send(query, cancellationToken);

        return Results.Created($"/tickets/{ticketToAdd.Id}", ticketToAdd);
    })
    .Produces(StatusCodes.Status201Created)
    .Produces(StatusCodes.Status400BadRequest)
    .Produces(StatusCodes.Status409Conflict);

app.MapGet("/tickets/{ticketId:int}", (int ticketId, IMediator mediator, CancellationToken cancellationToken) =>
{
    var query = new GetTicketByIdQuery(ticketId);

    return mediator.Send(query, cancellationToken);
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors("AllowAll");
}

app.UseHttpsRedirection();

app.Run();
