namespace Domain.AggregatesModel.OrderAggregate
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface IOrderRepository : IRepository<Order>
    {

    }
}
