using Microsoft.EntityFrameworkCore;
using TVR.Bundesliga.API.Context;
using TVR.Bundesliga.API.Models;

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

app.MapGet("/events", async (EventDb db) => 
    await db.Events.ToListAsync());

app.MapGet("/events/previous", async (EventDb db) =>
    await db.Events.Where(e => 
        e.Date < DateTimeOffset.Now).ToListAsync())
    .Produces(StatusCodes.Status200OK);

app.MapGet("/events/{eventId:int}", async (int eventId, EventDb db) =>
    await db.Events.FindAsync(eventId) is { } foundEvent ? Results.Ok(foundEvent) : Results.NotFound())
    .Produces(StatusCodes.Status200OK)
    .ProducesProblem(StatusCodes.Status404NotFound);

app.MapGet("/events/upcoming", async (EventDb db) => 
    await db.Events.Where(e => 
        e.Date >= DateTimeOffset.Now).ToListAsync())
    .Produces(StatusCodes.Status200OK);

app.MapPost("/events", async (Event newEvent, EventDb db) =>
    {
        db.Events.Add(newEvent);
        await db.SaveChangesAsync();

        return Results.Created($"/events/{newEvent.Id}", newEvent);
    }
)
.Produces(StatusCodes.Status201Created)
.ProducesProblem(StatusCodes.Status400BadRequest);

app.MapPut("/events/{eventId:int}", async (int eventId, Event eventChanges, EventDb db) =>
{
    var eventToChange = await db.Events.FindAsync(eventId);
    if (eventToChange is null)
    {
        return Results.NotFound();
    }

    eventToChange.Date = eventChanges.Date;
    eventToChange.League = eventChanges.League;
    eventToChange.Name = eventChanges.Name;

    await db.SaveChangesAsync();

    return Results.NoContent();
})
.Produces(StatusCodes.Status200OK)
.ProducesProblem(StatusCodes.Status400BadRequest);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

   
}

app.UseCors("AllowAll");
app.UseHttpsRedirection();

app.Run();
