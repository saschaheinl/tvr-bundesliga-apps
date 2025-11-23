using System.Reflection;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using TVR.Bundesliga.API.Contracts.Requests;
using TVR.Bundesliga.API.Core.Commands;
using TVR.Bundesliga.API.Core.Queries;
using TVR.Bundesliga.API.Domain.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.Load("TVR.Bundesliga.API.Core")));

builder.Services.AddSingleton<IMongoClient>(
    new MongoClient(Environment.GetEnvironmentVariable("MONGO_DB_CONNECTION_STRING")) ??
        throw new ArgumentException("Missing Environment variable"));

var base64EncodedBytes = Convert.FromBase64String(Environment.GetEnvironmentVariable("STORAGE_ACCOUNT") ?? string.Empty);
var creds = GoogleCredential.FromJson(System.Text.Encoding.UTF8.GetString(base64EncodedBytes));
builder.Services.AddSingleton<StorageClient>(await StorageClient.CreateAsync(creds));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        corsBuilder =>
        {
            corsBuilder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
    options.AddPolicy("Allow Ticket Smasher",
        policy =>
        {
            policy.WithOrigins("https://localhost:9000", "https://smasher.tvr.saschahei.nl")
                .SetIsOriginAllowedToAllowWildcardSubdomains()
                .AllowAnyHeader()
                .AllowAnyMethod()
                .SetIsOriginAllowed(_ => true);
        });
    options.AddPolicy("AllowEntryDrop",
        policy =>
        {
            policy.WithOrigins("https://n8n.saschahei.nl","https://localhost:3000", "https://drop.tvr.saschahei.nl")
                .SetIsOriginAllowedToAllowWildcardSubdomains()
                .AllowAnyHeader()
                .AllowAnyMethod()
                .SetIsOriginAllowed(_ => true);
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

app.MapPost("/tickets", async (CreateV2TicketRequest request, IMediator mediator, CancellationToken cancellationToken) =>
{
    var query = new AddV2TicketCommand(
        request.GuestName,
        request.ShouldSendEmail,
        request.GuestEmail,
        request.IncludedVisits,
        request.CreatedBy);
    var createdTicket = await mediator.Send(query, cancellationToken);
    
    return Results.Created($"/tickets/{createdTicket.Id}", createdTicket);
});

app.MapGet("/tickets/{ticketId}", (string ticketId, IMediator mediator, CancellationToken cancellationToken) =>
    {
        var query = new GetTicketByIdQuery(ticketId);

        return mediator.Send(query, cancellationToken);
    })
    .Produces<V2Ticket>()
    .ProducesProblem(StatusCodes.Status400BadRequest)
    .RequireAuthorization();

app.MapPost("tickets/{ticketId}/scans",
    (string ticketId, ScanTicketRequest request, IMediator mediator, CancellationToken cancellationToken) =>
    {
        var query = new ScanTicketCommand(ticketId, request.Guests, request.Username);
        
        return mediator.Send(query, cancellationToken);
    })
    .Produces<V2Ticket>()
    .RequireAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors("AllowAll");
}

if (app.Environment.IsProduction())
{
    app.UseCors("Allow Ticket Smasher");
    app.UseCors("AllowEntryDrop");
}

app.Run();
