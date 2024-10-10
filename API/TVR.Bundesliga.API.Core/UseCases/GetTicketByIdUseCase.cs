using MediatR;
using MongoDB.Driver;
using TVR.Bundesliga.API.Core.Queries;
using TVR.Bundesliga.API.Domain.Models;

namespace TVR.Bundesliga.API.Core.UseCases;

public class GetTicketByIdUseCase (IMongoClient mongoClient) : IRequestHandler<GetTicketByIdQuery, V2Ticket>
{
    public Task<V2Ticket> Handle(GetTicketByIdQuery request, CancellationToken cancellationToken)
    {
        var dbName = Environment.GetEnvironmentVariable("MONGO_DB_DATABASE_NAME");
        if (dbName is null)
        {
            throw new ArgumentNullException();
        }

        var database = mongoClient.GetDatabase(dbName);
        var collection = database.GetCollection<V2Ticket>("Tickets");
        var filter = Builders<V2Ticket>.Filter.Eq("_id", request.TicketId);
        
        return collection.Find(filter).FirstOrDefaultAsync(cancellationToken);
        
        
    }
}
