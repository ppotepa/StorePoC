using Microsoft.EntityFrameworkCore;
using Store.Domain.Entities;
using System;
using System.Linq;

namespace Store.Domain.Context
{
    public partial class ExtendedDbContext : DbContext, IApplicationContext
    {
        private Type[] _currentEntities = default;
        public DbSet<Attachment> Attachments { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<RegardingObject> RegardingObjects { get; set; }
        public DbSet<Registration> Registrations { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<User> Users { get; set; }        

        private Type[] CurrentEntities
        {
            get
            {                
                if (_currentEntities is null)
                {
                    _currentEntities = currentAssemblyTypes.Where(t => t.IsSubclassOf(typeof(Entity))).ToArray();
                }
                return _currentEntities;
            }
        }
    }
}
