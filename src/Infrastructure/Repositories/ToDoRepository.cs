using Domain.AggregatesModel.ToDoAggregate;
using SqlSugar;

namespace Infrastructure.Repositories
{
    public class ToDoRepository : Repository<ToDo>, IToDoRepository
    {
        public ToDoRepository(ISqlSugarClient context) : base(context)
        {

        }
    }
}
