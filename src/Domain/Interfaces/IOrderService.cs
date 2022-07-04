namespace Domain.Services.Interfaces
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface IOrderService
    {
        void Test();
    }
}
