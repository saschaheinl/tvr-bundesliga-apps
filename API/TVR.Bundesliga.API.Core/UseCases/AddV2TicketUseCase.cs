using Google.Apis.Auth.OAuth2;
using Google.Apis.Storage.v1.Data;
using Google.Cloud.Storage.V1;
using MediatR;
using MongoDB.Driver;
using QRCoder;
using TVR.Bundesliga.API.Core.Commands;
using TVR.Bundesliga.API.Domain.Models;

namespace TVR.Bundesliga.API.Core.UseCases;

public class AddV2TicketUseCase(IMongoClient mongoClient, StorageClient storage) : IRequestHandler<AddV2TicketCommand, V2Ticket>
{
    public async Task<V2Ticket> Handle(AddV2TicketCommand request, CancellationToken cancellationToken)
    {
        // 1. Grab information from request
        // 2. Create QR Code
        // 3. If E-Mail is needed, upload QR Code to Google Cloud Storage
        // 4. Create Ticket entity
        // 5. Upsert into MongoDB
        // 6. If E-Mail is needed, send E-Mail via Sendgrid
            // TODO: Use Event & Cloud Function
        // 7. Return Ticket
        
        // 1. Grab information from request
        
        var name = request.GuestName;
        var includedVisits = request.IncludedVisits;
        var shouldSendEmail = request.ShouldSendEmail;
        var creator = request.CreatedBy;
        var mailAddress = string.Empty;
        if (shouldSendEmail)
        {
            mailAddress = request.GuestEmail;
        }
        
        // 2. Create QR Code
        
        var ticketId = Guid.NewGuid();
        var qrcodeGen = new QRCodeGenerator();
        var data = qrcodeGen.CreateQrCode(ticketId.ToString(), QRCodeGenerator.ECCLevel.Q);
        var base64String = new Base64QRCode(data);
        using var stream = new MemoryStream(Convert.FromBase64String(base64String.GetGraphic(20)));
        var qrCodeDetails = new QrCodeDetails(string.Empty, base64String.GetGraphic(20));
        
        // 3. If E-Mail is needed, upload QR Code to Google Cloud Storage

        Google.Apis.Storage.v1.Data.Object? uploadedObject;
        var bucket = Environment.GetEnvironmentVariable("QR_CODE_BUCKET") ?? throw new ArgumentException("No QR code bucket environment variable");
        uploadedObject = await storage.UploadObjectAsync(
            bucket, 
            $"{ticketId.ToString()}.png",
            "image/png", 
            stream,
            cancellationToken: cancellationToken);
        qrCodeDetails.LocationOfImage = uploadedObject.SelfLink;
        
        if (shouldSendEmail)
        {
            uploadedObject.Acl ??= new List<ObjectAccessControl>();
            await storage.UpdateObjectAsync(uploadedObject, new UpdateObjectOptions { PredefinedAcl = PredefinedObjectAcl.PublicRead }, cancellationToken);
            qrCodeDetails.PublicLink = $"{Environment.GetEnvironmentVariable("QR_CODE_BUCKET_BASEURL")}/{ticketId}.png";
        }
        
        // 4. Create Ticket entity
        
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
        
        // 5. Upsert into MongoDB
        
        var dbName = Environment.GetEnvironmentVariable("MONGO_DB_DATABASE_NAME");
        if (dbName is null)
        {
            throw new ArgumentNullException();
        }

        var database = mongoClient.GetDatabase(dbName);
        var collection = database.GetCollection<V2Ticket>("Tickets");
        await collection.InsertOneAsync(newTicket, null, cancellationToken);

        // 6. If E-Mail is needed, send E-Mail via Sendgrid
        

        // 7. Return Ticket
        
        return newTicket;
    }
}
