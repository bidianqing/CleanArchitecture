using Domain.SeedWork;
using SqlSugar;

namespace Infrastructure.Repositories
{
    public class Repository<T>(ISqlSugarClient context) : SimpleClient<T>(context), IRepository<T> where T : class, new()
    {

    }
}
