using Microsoft.EntityFrameworkCore;
using Store.Domain.Context;
using Store.Domain.Entities;
using System;

namespace Store.Infrastructure
{
    public class RepositoryBase<TEntityType> : DbSet<TEntityType> 
        where TEntityType : Entity
    {
        private readonly ExtendedDbContext context;
        public RepositoryBase(ExtendedDbContext context)
        {
            this.context = context;
        }

        public Guid Insert(TEntityType entity) 
            => this.context.Add(entity).Entity.Id;
    }
}
