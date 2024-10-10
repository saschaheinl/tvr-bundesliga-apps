using MediatR;
using MongoDB.Driver;
using QRCoder;
using TVR.Bundesliga.API.Core.Commands;
using TVR.Bundesliga.API.Domain.Models;

namespace TVR.Bundesliga.API.Core.UseCases;

public class AddV2TicketUseCase(IMongoClient mongoClient) : IRequestHandler<AddV2TicketCommand, V2Ticket>
{
    public async Task<V2Ticket> Handle(AddV2TicketCommand request, CancellationToken cancellationToken)
    {
        var name = request.GuestName;
        var includedVisits = request.IncludedVisits;
        var shouldSendEmail = request.ShouldSendEmail;
        var creator = request.CreatedBy;
        var mailAddress = string.Empty;
        if (shouldSendEmail)
        {
            mailAddress = request.GuestEmail;
        }
        
        var ticketId = Guid.NewGuid();
        var qrcodeGen = new QRCodeGenerator();
        var data = qrcodeGen.CreateQrCode(ticketId.ToString(), QRCodeGenerator.ECCLevel.Q);
        var base64String = new Base64QRCode(data);
        var qrCodeDetails = new QrCodeDetails(string.Empty, base64String.GetGraphic(20));
        var newTicket = new V2Ticket(
            ticketId.ToString(),
            name,
            mailAddress,
            includedVisits,
            includedVisits,
            0,
            shouldSendEmail,
            DateTimeOffset.UtcNow,
            creator,
            qrCodeDetails,
            []);
        
        var dbName = Environment.GetEnvironmentVariable("MONGO_DB_DATABASE_NAME");
        if (dbName is null)
        {
            throw new ArgumentNullException();
        }

        var database = mongoClient.GetDatabase(dbName);
        var collection = database.GetCollection<V2Ticket>("Tickets");
        await collection.InsertOneAsync(newTicket, null, cancellationToken);

        return newTicket;
    }
}
