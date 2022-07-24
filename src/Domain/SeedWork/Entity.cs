using Dapper.Contrib.Extensions;

namespace Domain.SeedWork
{
    public abstract class Entity
    {
        [Key]
        public virtual int Id { get; set; }
    }
}
