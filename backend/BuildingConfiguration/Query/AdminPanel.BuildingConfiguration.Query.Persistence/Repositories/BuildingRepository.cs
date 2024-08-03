using AdminPanel.BuildingConfiguration.Query.Domain.Entities;
using AdminPanel.BuildingConfiguration.Query.Domain.Repositories;
using AdminPanel.BuildingConfiguration.Query.Persistence.DataAccess;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace AdminPanel.BuildingConfiguration.Query.Persistence.Repositories;

public class BuildingRepository : IBuildingRepository
{
    private readonly IMongoCollection<Building> _collection;
    public BuildingRepository(IOptions<MongoDbConfig> config)
    {
        var mongoClient = new MongoClient(config.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(config.Value.Database);

        _collection = mongoDatabase.GetCollection<Building>(config.Value.Collection);
    }

    public async Task CreateAsync(Building building)
    {
        await _collection.InsertOneAsync(building).ConfigureAwait(false);
    }

    public async Task UpdateAsync(Building building)
    {
        var filter = Builders<Building>.Filter.Eq(b => b.Id, building.Id);

        var updatedBuilding = Builders<Building>.Update
            .Set(b => b.BuildingCost, building.BuildingCost)
            .Set(b => b.ConstructionTime, building.ConstructionTime);

        await _collection.UpdateOneAsync(filter, updatedBuilding);
    }

    public async Task DeleteAsync(Building building)
    {
        var filter = Builders<Building>.Filter.Eq(b => b.Id, building.Id);

        await _collection.DeleteOneAsync(filter);
    }

    public async Task<Building?> GetByIdAsync(Guid buildingId)
    {
        var filter = Builders<Building>.Filter.Eq(b => b.Id, buildingId);

        var building = await _collection.FindAsync(filter);

        return building.SingleOrDefault();
    }

    public async Task<List<Building>> GetAllAsync()
    {
        return await _collection.Find(FilterDefinition<Building>.Empty).ToListAsync().ConfigureAwait(false);
    }
}