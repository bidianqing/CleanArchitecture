using Domain.AggregatesModel.OrderAggregate;
using OneAspNet.Repository.Dapper;
using System;
using System.Data.Common;

namespace Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ConnectionFactory _connectionFactory;
        public OrderRepository(ConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public void Test()
        {
            using DbConnection connection = _connectionFactory.CreateConnection();

            Console.WriteLine("执行Test方法");
        }
    }
}
