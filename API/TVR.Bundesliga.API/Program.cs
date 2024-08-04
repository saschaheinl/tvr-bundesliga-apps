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

app.MapGet("/events/previous", (EventDb db) =>
    {
        var now = DateTimeOffset.UtcNow;
        return db.Events.Where(e => e.Date < now).ToListAsync();
    })
    .Produces(StatusCodes.Status200OK);

app.MapGet("/events/{eventId:int}", async (int eventId, EventDb db) =>
        await db.Events.FindAsync(eventId))
    .Produces(StatusCodes.Status200OK)
    .ProducesProblem(StatusCodes.Status404NotFound);

app.MapGet("/events/upcoming", (EventDb db) =>
    {
        var now = DateTimeOffset.UtcNow;
        return db.Events.Where(e =>
            e.Date >= now).ToListAsync();
    })
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

app.MapPost("/guests", async (GuestDb db, Guest guestToCreate) =>
{
    var foundGuest = await db.Guests.Where(g =>
        g.EmailAddress == guestToCreate.EmailAddress).ToListAsync();
    if (foundGuest.Count is not 0)
    {
        return Results.BadRequest("This Email address already belongs to a created guest.");
    }

    db.Guests.Add(guestToCreate);
    await db.SaveChangesAsync();

    return Results.Created($"/guests/{guestToCreate.Id}", guestToCreate);
});

app.MapGet("/guests/{guestId:int}", async (GuestDb db, int guestId) =>
    await db.Guests.FirstOrDefaultAsync(g => g.Id == guestId));

app.MapGet("/guests/search", async (GuestDb db, int? id, string? name, string? mailAddress) =>
{
    if(id is null && name is null && mailAddress is null)
    {
        return [];
    }
    
    if (id is not null)
    {
        return await db.Guests.Where(g => g.Id == id).ToListAsync();
    }

    if (name is not null && mailAddress is not null)
    {
        return await db.Guests.Where(g => g.EmailAddress.Contains(mailAddress) && g.Name.Contains(name)).ToListAsync();
    }

    if (name is null && mailAddress is not null)
    {
        return await db.Guests.Where(g => g.EmailAddress.Contains(mailAddress)).ToListAsync();
    }

    return await db.Guests.Where(g => g.Name.Contains(name!)).ToListAsync();
});

app.MapGet("guests/emailId={mailAddress}", async (GuestDb db, string mailAddress) =>
    await db.Guests.FirstOrDefaultAsync(g => g.EmailAddress == mailAddress));

app.MapGet("guests/name={name}", async (GuestDb db, string name) =>
    await db.Guests.Where(g => g.Name.Contains(name)).ToListAsync());

app.MapGet("/guests", async (GuestDb db) =>
    await db.Guests.ToListAsync());

app.MapPost("/tickets", async (TicketDb tickets, EventDb events, GuestDb guests, Ticket newTicket) =>
{
    var existingGuest = await guests.Guests.FindAsync(newTicket.Guest.EmailAddress);
    if (existingGuest is not null)
    {
        newTicket.Guest = existingGuest;
    }

    var existingEvent = await events.Events.FindAsync(newTicket.Event);
    if (existingEvent is not null)
    {
        newTicket.Event.Id = existingEvent.Id;
    }

    var existingTicket =
        await tickets.Tickets.Where(t => t.Event.Id == newTicket.Event.Id && t.Guest.Id == newTicket.Guest.Id)
            .ToListAsync();
    if (existingTicket.Count is not 0)
    {
        return Results.Conflict("This ticket already exists.");
    }

    tickets.Add(newTicket);
    await tickets.SaveChangesAsync();

    return Results.Created($"/tickets/{newTicket.Id}", newTicket);
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors("AllowAll");
}

app.UseHttpsRedirection();

app.Run();
