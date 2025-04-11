using SqlSugar;

namespace Domain.SeedWork
{
    public abstract class Entity<TKey>
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public virtual TKey Id { get; set; }
    }
}
