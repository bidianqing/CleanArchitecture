using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.AggregatesModel.ToDoAggregate
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface IToDoRepository
    {
        Task<int> Add(ToDo todo);

        Task<ToDo> Get(int id);
    }
}
