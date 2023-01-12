﻿using Faithlife.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Store.Domain.Entities;
using Store.Domain.Utilities;
using Store.Extensions;
using Store.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Store.Domain.Context
{
    public partial class ExtendedDbContext : DbContext, IApplicationContext
    {
        private readonly bool LoggingEnabled;
        private readonly Guid EnumerationNamespace = Guid.Parse("8570b57c-2ffd-4ff3-8bd8-6411fc052822");
        private readonly HttpContext httpContext;
        private readonly IServiceProvider serviceProvider;
        private readonly EnvironmentVariables environmentVariables;
        private readonly string outputFileName;
        private readonly Type[] currentAssemblyTypes;
        private readonly IConfiguration configuration;
        private readonly IHttpContextAccessor accessor;

        private static string FormattedDateTime => DateTime.Now.ToString("yyyy-dd-MM-HH-mm-ss");

        public ExtendedDbContext(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;            
            this.configuration = this.serviceProvider.GetService<IConfiguration>();
            this.accessor = this.serviceProvider.GetService<IHttpContextAccessor>();

            environmentVariables = this.configuration.GetEnvironmentVariables();
            currentAssemblyTypes = Assembly.GetExecutingAssembly().GetTypes();
            //Database.EnsureCreated();
        }

        public DatabaseFacade DataBase => this.Database;

        public IEnumerable<object> Entries
                    => ((IEnumerable<object>)this.ChangeTracker.Entries<Entity>()).Concat(this.ChangeTracker.Entries<IEnumerationEntity>());

        public EntityEntry<TEntity> AttachEntity<TEntity>(TEntity entity) where TEntity : class
            => this.Attach(entity);

        public void BeginTransaction() =>
            this.Database.BeginTransaction();

        public void CommitTransaction() =>
            this.Database.CommitTransaction();

        public void Detach<TEntity>(TEntity entity) where TEntity : Entity
        {
            this.Entry(entity).State = EntityState.Detached;
        }

        public IEnumerable<EntityEntry<TEntity>> NewEntries<TEntity>() where TEntity : class
                                    => this.ChangeTracker.Entries<TEntity>().Where(entry => entry.State is EntityState.Added);

        public IEnumerable<EntityEntry> NewEntries() =>
           this.ChangeTracker.Entries<Entity>().Where(entry => entry.State is EntityState.Added);

        EnumerationEntity<TEnumType> IApplicationContext.ResolveEnum<TEnumType>(int value)
            => this.Find(typeof(EnumerationEntity<TEnumType>), new object[] { value }) as EnumerationEntity<TEnumType>;

        public void RollBack()
            => this.Database.RollbackTransaction();

        private static Guid TemporaryUserId = Guid.Parse("240415e0-a8b2-477a-9e00-e0b2d8c2190e");
        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            DateTime currentDateTime = DateTime.Now;
            List<EntityEntry<Entity>> entities = ChangeTracker.Entries<Entity>().ToList();

            entities.ForEach(entry =>
            {
                dynamic dynamicEntry = entry as dynamic;

                if (dynamicEntry.State is EntityState.Added)
                {
                    dynamic dynamicEntity = dynamicEntry.Entity;

                    if (dynamicEntity.GetType().GetProperty(nameof(Attachment)) != null)
                    {
                        dynamicEntity.Attachment.RegardingObject = new RegardingObject
                        {
                            EntityName = dynamicEntity.GetType().Name,
                            RegardingObjectId = dynamicEntity.Id,
                            CreatedOn = currentDateTime,
                        };
                    }

                    dynamicEntity.CreatedOn = currentDateTime;
                }

                if (dynamicEntry.State is EntityState.Modified)
                {
                    dynamicEntry.Entity.ModifiedOn = currentDateTime;
                }

                if (entry.State is EntityState.Deleted)
                {
                    dynamicEntry.Entity.MarkAsDeleted();
                    dynamicEntry.Entity.DeletedOn = currentDateTime;
                    entry.State = EntityState.Modified;
                }
            });

            return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        void IApplicationContext.Update<TEntity>(TEntity entity)
            => this.Update(entity);

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();

            if (LoggingEnabled)
            {
                optionsBuilder.LogTo(text => File.AppendAllText(outputFileName, text));
            }

            optionsBuilder
                .UseLazyLoadingProxies()
                .UseSqlServer(environmentVariables.DB_CONNECTIONSTRING_DEV);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            GenerateCustomersTable(modelBuilder);
            AddGenericEntityFilter(modelBuilder);
            GenerateAddressTable(modelBuilder);
            GenerateEnumerationTables(modelBuilder);            
        }

        private static string GetEnumUniqueName(object value, Type currentEnum)
            => Enum.GetName(currentEnum, value) + "_" + value.ToString();

        private void AddGenericEntityFilter(ModelBuilder modelBuilder)
        {
            CurrentEntities.Where(e => e.BaseType == typeof(Entity) && e.ContainsGenericParameters is false).ForEach(entity =>
            {
                dynamic expression = ExpressionMethod(entity).Invoke(this, null);
                modelBuilder.Entity(entity).HasQueryFilter(expression);
            });
        }

        private MethodInfo ExpressionMethod(Type entity) => this.GetType()
                          .GetMethod(nameof(GetGlobalFilterExpression), BindingFlags.Instance | BindingFlags.NonPublic)
                          .MakeGenericMethod(entity);

        private void GenerateAddressTable(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Address>().HasOne(x => x.AddressType);
            modelBuilder.Entity<Address>().HasOne(x => x.Customer);
            modelBuilder.Entity<Address>().HasOne(x => x.CountryCodeType);
        }

        private void GenerateCustomersTable(ModelBuilder builder)
        {
            builder.Entity<User>().HasKey(x => new { x.Id });
            builder.Entity<User>().HasAlternateKey(x => new { x.EmailAddress });
            
            builder.Entity<Customer>().HasOne(x => x.CompanySizeType);
            builder.Entity<Customer>().HasMany(x => x.Addresses).WithOne(x => x.Customer).HasForeignKey(x => x.CustomerId).OnDelete(DeleteBehavior.Cascade);           
            builder.Entity<Customer>().HasOne(x => x.Registration).WithOne(x => x.Customer).HasForeignKey<Registration>(x => x.CustomerId).OnDelete(DeleteBehavior.Cascade);
          
        }

        private void GenerateEnumerationTables(ModelBuilder builder)
        {
            Type[] enumerationEntities = EnumerationUtility.GetCurrentAssemblyEnumerations();
            Type[] currentAssemblyEnums = EnumerationUtility.GetCurrentAssemblyEnumerationTypes();

            enumerationEntities.ForEach((entity, index) =>
            {
                Type currentEnum = currentAssemblyEnums[index];

                var currentEnumDataSeed = Enum.GetValues(currentEnum).Cast<object>()
                .Select(value =>
                {
                    return new
                    {
                        Id = GuidUtility.Create(this.EnumerationNamespace, GetEnumUniqueName(value, currentEnum)),
                        Value = Convert.ChangeType(value, currentEnum),
                        Name = Enum.GetName(currentEnum, value)
                    };
                })
                .ToArray();

                builder.Entity(entity).HasKey(new[] { "Id" });
                builder.Entity(entity).HasData(currentEnumDataSeed);
            });
        }
        
        private Expression<Func<TEntity, bool>> GetGlobalFilterExpression<TEntity>() where TEntity : Entity
            => (entity) => EF.Property<bool>(entity, "Deleted").Equals(false);

        public EntityEntry AttachEntity(object entity)
        {
            Type entityType = entity.GetType();
            if (entityType != typeof(Entity))
                throw new InvalidOperationException($"Unable to attach object of type : {entityType}.");
            else return this.Attach(entity);
        }
       
    }
}
