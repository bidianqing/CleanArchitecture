using SqlSugar;

namespace Infrastructure.Repositories
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        public OrderRepository(ISqlSugarClient context) : base(context)
        {

        }
    }
}
