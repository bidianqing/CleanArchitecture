namespace Domain.AggregatesModel.OrderAggregate
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface IOrderRepository
    {
        void Test();
    }
}
