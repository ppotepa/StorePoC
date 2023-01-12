using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Store.Domain.Entities;
using Store.Domain.Entities.Enumerations;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Store.Domain.Context
{
    public interface IApplicationContext 
    {
        public DbSet<AddressType> AddressTypes { get; set; }

        public ChangeTracker ChangeTracker { get; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<CompanySizeType> CompanySizeTypes { get; set; }
        public DbSet<CountryCodeType> CountryCodeTypes { get; set; }

        public DbSet<Customer> Customers { get; set; }
        public DatabaseFacade DataBase { get; }
        public IEnumerable<object> Entries { get; }
        public StoreContextEnumerations Enumerations { get; set; }
        public DbSet<Registration> Registrations { get; set; }        
        public DbSet<User> Users { get; set; }
        public EntityEntry<TEntity> AttachEntity<TEntity>(TEntity entity) where TEntity : class;
        public EntityEntry AttachEntity(object entity);

        public void BeginTransaction();

        public void CommitTransaction();

        void Detach<TEntity>(TEntity entity) where TEntity : Entity;

        public IEnumerable<EntityEntry<TEntity>> NewEntries<TEntity>() where TEntity : class;

        public IEnumerable<EntityEntry> NewEntries();

        public EnumerationEntity<TEnumType> ResolveEnum<TEnumType>(int value) where TEnumType : Enum;

        void RollBack();

        public int SaveChanges();

        public Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default);

        void Update<TEntity>(TEntity entity) where TEntity : Entity;
    }
}

