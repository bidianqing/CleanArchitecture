using Domain.AggregatesModel.OrderAggregate;
using Domain.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace Domain.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ILogger<OrderService> _logger;

        public OrderService(IOrderRepository orderRepository, ILogger<OrderService> logger)
        {
            _orderRepository = orderRepository;
            _logger = logger;
        }

        public void Test()
        {
            _logger.LogInformation("日志测试");
            _orderRepository.Test();
        }
    }
}
