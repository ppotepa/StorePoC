using Microsoft.EntityFrameworkCore;
using Store.Domain.Entities.Enumerations;
using Store.Extensions;
using System.Linq;

namespace Store.Domain.Context
{
    public partial class ExtendedDbContext : DbContext, IApplicationContext
    {
        private StoreContextEnumerations _enumerations;
        
        public DbSet<AddressType> AddressTypes { get; set; }

        public DbSet<CompanySizeType> CompanySizeTypes { get; set; }
        public DbSet<CountryCodeType> CountryCodeTypes { get; set; }
        public StoreContextEnumerations Enumerations
        {
            get
            {
                if (_enumerations is null)
                {
                    _enumerations = new StoreContextEnumerations
                    (
                        this.CompanySizeTypes.ToDictionary(type => type.Value, instance => instance),
                        this.CountryCodeTypes.ToDictionary(type => type.Value, instance => instance),
                        this.AddressTypes.ToDictionary(type => type.Value, instance => instance)
                    );
                }
                return _enumerations;
            }
            set => _enumerations = value;
        }
    }
}
