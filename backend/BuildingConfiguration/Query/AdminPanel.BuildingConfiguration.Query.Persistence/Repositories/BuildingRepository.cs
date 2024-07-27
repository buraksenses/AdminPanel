using AdminPanel.BuildingConfiguration.Query.Domain.Entities;
using AdminPanel.BuildingConfiguration.Query.Domain.Repositories;
using AdminPanel.BuildingConfiguration.Query.Persistence.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace AdminPanel.BuildingConfiguration.Query.Persistence.Repositories;

public class BuildingRepository : IBuildingRepository
{
    private readonly BuildingDbContext _dbContext;
    public BuildingRepository(BuildingDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task CreateAsync(Building building)
    {
        await _dbContext.Buildings.AddAsync(building);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Building building)
    {
        _dbContext.Buildings.Update(building);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(Building building)
    {
        _dbContext.Buildings.Remove(building);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<Building?> GetByIdAsync(Guid buildingId)
    {
        return await _dbContext.Buildings.FindAsync(buildingId);
    }

    public async Task<List<Building>> GetAllAsync()
    {
        return await _dbContext.Buildings.AsNoTracking().ToListAsync();
    }
}