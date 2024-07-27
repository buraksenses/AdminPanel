using AdminPanel.BuildingConfiguration.Query.Domain.Entities;

namespace AdminPanel.BuildingConfiguration.Query.Domain.Repositories;

public interface IBuildingRepository
{
    Task CreateAsync(Building building);
    
    Task UpdateAsync(Building building);
    
    Task DeleteAsync(Building building);

    Task<Building?> GetByIdAsync(Guid buildingId);

    Task<List<Building>> GetAllAsync();
}