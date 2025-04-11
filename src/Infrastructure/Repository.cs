using Domain.SeedWork;
using SqlSugar;

namespace Infrastructure
{
    public class Repository<T>(ISqlSugarClient context) : SimpleClient<T>(context), IRepository<T> where T : class, new()
    {

    }
}
