using Microsoft.Extensions.Options;
using MongoDB.Driver;
using SingularisTestTask.Models;

namespace SingularisTestTask.Infrastructure;

public class IncrementCopyRepository : IIncrementCopyRepository
{
    private readonly IMongoCollection<IncrementCopyModel> _collection;

    public IncrementCopyRepository(IOptions<IncrementCopyRepositoryOptions> options)
    {
        var client = new MongoClient(options.Value.ConnectionString);
        var database = client.GetDatabase(options.Value.DatabaseName);
        _collection = database.GetCollection<IncrementCopyModel>("IncrementCopyModels");
    }

    public async Task CreateOrUpdateAsync(IncrementCopyModel model)
    {
        var existingModel = await Get(model.DestinationFolder);

        if (existingModel == null)
        {
            await _collection.InsertOneAsync(model);
        }
        else
        {
            await _collection.ReplaceOneAsync(GetFilter(model.DestinationFolder), model);
        }
    }

    public async Task DeleteAsync(string destinationFolder)
    {
        await _collection.DeleteOneAsync(GetFilter(destinationFolder));
    }

    public async Task<IncrementCopyModel?> Get(string destinationFolder)
    {
        return await _collection.Find(GetFilter(destinationFolder)).FirstOrDefaultAsync();
    }
    
    private FilterDefinition<IncrementCopyModel>? GetFilter(string destinationFolder)
    {
        return Builders<IncrementCopyModel>.Filter.Eq("DestinationFolder", destinationFolder);
    }
}