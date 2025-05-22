using SqlSugar;
using System.Runtime.InteropServices;

namespace Domain.SeedWork
{
    public interface IRepository<T> : ISimpleClient<T> where T : class, IAggregateRoot, new()
    {
        ISqlSugarClient Context { get; set; }
    }
}
