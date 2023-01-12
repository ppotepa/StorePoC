using Store.Domain.Entities.Enumerations;
using System;

namespace Store.Domain.Entities
{
    public class Address : Entity
    {
        public virtual Customer Customer { get; set; }
        public virtual Guid CustomerId { get; set; }
        public virtual AddressType AddressType { get; set; }
        public Guid AddressTypeId { get; set; }
        public virtual CountryCodeType CountryCodeType { get; set; }
        public Guid CountryCodeTypeId { get; set; }
        public string PhoneNumber { get; set; }
        public string PostalCode { get; set; }
        public string Street { get; set; }
        public string BuildingNumber { get; set; }
        public bool Active { get; set; }
        
    }
}
