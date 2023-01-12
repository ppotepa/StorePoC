using Microsoft.EntityFrameworkCore;
using Store.Domain.Entities;
using System;
using System.Linq;

namespace Store.Domain.Extensions
{
    public static partial class DbSetDomainExtensions
    {
        public static IQueryable<TEntity> GetById<TEntity>(this DbSet<TEntity> @this, Guid entityId, string include = "") where TEntity : Entity
            => @this.Where(x => x.Id == entityId).AsQueryable();

        public static IQueryable<TEntity> GetByIdAsync<TEntity>(this DbSet<TEntity> @this, Guid entityId, string include = "") where TEntity : Entity
            => @this.Where(x => x.Id == entityId).AsQueryable();
    }
}
