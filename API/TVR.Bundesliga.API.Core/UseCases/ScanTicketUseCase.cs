using MediatR;
using MongoDB.Driver;
using TVR.Bundesliga.API.Core.Queries;
using TVR.Bundesliga.API.Domain.Models;

namespace TVR.Bundesliga.API.Core.UseCases;

public class ScanTicketUseCase(IMongoClient mongoClient) : IRequestHandler<ScanTicketCommand, V2Ticket>
{
    public async Task<V2Ticket> Handle(ScanTicketCommand request, CancellationToken cancellationToken)
    {
        var ticketId = request.TicketId;
        var guests = request.NumberOfGuests;
        var user = request.Username;
        var dbName = Environment.GetEnvironmentVariable("MONGO_DB_DATABASE_NAME");
        if (dbName is null)
        {
            throw new ArgumentNullException();
        }

        var database = mongoClient.GetDatabase(dbName);
        var collection = database.GetCollection<V2Ticket>("Tickets");
        var filter = Builders<V2Ticket>.Filter.Eq("_id", ticketId);
        var ticket = await collection.Find(filter).FirstOrDefaultAsync(cancellationToken);
        if (ticket is null)
        {
            throw new ArgumentException();
        }

        var scanInformation = new Scan(user, DateTimeOffset.UtcNow, guests);
        var update = Builders<V2Ticket>.Update
            .Push(t => t.Scans, scanInformation)
            .Set(t => t.LastModified, DateTimeOffset.UtcNow);
        await collection.UpdateOneAsync(filter, update,null, cancellationToken);

        ticket.Scans.Add(scanInformation);
        ticket.LastModified = DateTimeOffset.UtcNow;
        return ticket;

    }
}
