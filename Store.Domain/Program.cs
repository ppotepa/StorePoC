using Store.Domain.Context;
using Store.Domain.Entities;
using Store.Domain.Entities.Enumerations;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Store.Domain
{
    public class Program
    {
        public static async Task Main()
        {          
            ExtendedDbContext ctx = new ExtendedDbContext(null);
            ctx.Add(NewCustomer(ctx, DateTime.Now.Millisecond));
            ctx.SaveChanges();
            var customer = ctx.Customers.First();
        }

        private static Customer NewCustomer(ExtendedDbContext ctx, int index)
        {
            Customer newCustomer = new Customer
            {
                CompanySizeType = ctx.Enumerations.CompanySizeTypes[CompanySizeTypeEnum.LessThan10],
                Addresses = new[]
                {
                    new Address
                    {
                        AddressType = ctx.Enumerations.AddressTypes[AddressTypeEnum.Shipping],
                        CountryCodeType = ctx.Enumerations.CountryCodeTypes[CountryCodeTypeEnum.AD],
                        PhoneNumber = "123 123 123",
                        Street = "Jakas ulica 20",
                    }
                },
                EmailAddress = $"robert.shmidt{index}@someCompany.com",
                CompanyName = "Shmidt and Family",
                RegistrationNumber = "00/01/2000",
                VATNumber = "000-000-000-000-000",
                Website = "http://shmidt-ompany.de",
            };

            return newCustomer;
        }
    }
}
