namespace Domain.SeedWork
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface IRepository
    {
        Task<int> Insert<T>(T entity) where T : Entity, new();
    }
}
