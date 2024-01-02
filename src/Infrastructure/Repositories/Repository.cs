using Dapper.Contrib.Extensions;
using Domain.SeedWork;

namespace Infrastructure.Repositories
{
    public class Repository : IRepository
    {
        private readonly ConnectionFactory _connectionFactory;
        public Repository(ConnectionFactory connectionFactory) 
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<int> Insert<T>(T entity) where T : Entity, new()
        {
            using DbConnection connection = _connectionFactory.CreateConnection();

            return await connection.InsertAsync(entity);
        }
    }
}
