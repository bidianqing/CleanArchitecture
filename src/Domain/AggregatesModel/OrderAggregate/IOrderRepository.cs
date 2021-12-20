using Domain.SeedWork;
using Microsoft.Extensions.DependencyInjection;

namespace Domain.AggregatesModel.OrderAggregate
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface IOrderRepository
    {
        void Test();
    }
}
