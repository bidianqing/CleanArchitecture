using Dapper.Contrib.Extensions;

namespace Domain.AggregatesModel.ToDoAggregate
{
    [Table("tb_todo")]
    public class ToDo : Entity, IAggregateRoot
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime CreateTime { get; set; }
    }
}
