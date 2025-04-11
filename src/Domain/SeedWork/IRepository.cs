using SqlSugar;

namespace Domain.SeedWork
{
    public interface IRepository<T> : ISimpleClient<T> where T : class, new()
    {
        ISqlSugarClient Context { get; set; }
    }
}
