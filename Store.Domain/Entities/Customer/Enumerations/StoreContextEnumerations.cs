using System.Collections.Generic;

namespace Store.Domain.Entities.Enumerations
{
    public class StoreContextEnumerations
    {
        public StoreContextEnumerations
        (
            Dictionary<CompanySizeTypeEnum, CompanySizeType> companySizeTypes,
            Dictionary<CountryCodeTypeEnum, CountryCodeType> countryCodeTypes,
            Dictionary<AddressTypeEnum, AddressType> addressTypes)
        {
            this.CompanySizeTypes = companySizeTypes;
            this.CountryCodeTypes = countryCodeTypes;
            this.AddressTypes = addressTypes;            
        }

        public Dictionary<AddressTypeEnum, AddressType> AddressTypes { get; }
        public Dictionary<CompanySizeTypeEnum, CompanySizeType> CompanySizeTypes { get; }
        public Dictionary<CountryCodeTypeEnum, CountryCodeType> CountryCodeTypes { get; }
    }
}
