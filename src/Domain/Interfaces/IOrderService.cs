using Domain.SeedWork;
using Microsoft.Extensions.DependencyInjection;

namespace Domain.Services.Interfaces
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface IOrderService
    {
        void Test();
    }
}
