using Domain.AggregatesModel.OrderAggregate;
using Domain.Services.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System;

namespace Domain.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ILogger<OrderService> _logger;
        private readonly IDistributedCache _cache;

        public OrderService(IOrderRepository orderRepository, ILogger<OrderService> logger, IDistributedCache cache)
        {
            _orderRepository = orderRepository;
            _logger = logger;
            _cache = cache;
        }

        public void Test()
        {
            _logger.LogInformation("日志测试");
            _cache.SetString("name", "tom", new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(10)
            });
            _orderRepository.Test();
        }
    }
}
