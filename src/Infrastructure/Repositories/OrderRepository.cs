using Domain.AggregatesModel.OrderAggregate;
using OneAspNet.Repository.Dapper;
using StackExchange.Redis;
using System;
using System.Data.Common;

namespace Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ConnectionFactory _connectionFactory;
        private readonly ConnectionMultiplexer _redis;
        private readonly IDatabase _database;

        public OrderRepository(ConnectionFactory connectionFactory, ConnectionMultiplexer redis)
        {
            _connectionFactory = connectionFactory;
            _redis = redis;
            _database = redis.GetDatabase();
        }

        public void Test()
        {
            using DbConnection connection = _connectionFactory.CreateConnection();

            _database.StringSet("time", DateTime.Now.ToString(), new TimeSpan(0, 0, 30));

            Console.WriteLine("执行Test方法");
        }
    }
}
