using Store.Domain.Entities;

namespace Store.Infrastructure
{
    public interface IRepository<TEntityType> where TEntityType : Entity
    { 
    }
}
