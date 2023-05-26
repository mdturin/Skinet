using Core.Entities;

namespace Core.Interfaces;

public interface IBulkRepository<T> where T : BaseEntity
{
    Task<T> GetByIdAsync(int id);
    Task<IReadOnlyList<T>> ListAllAsync();
}
