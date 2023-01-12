using Microsoft.EntityFrameworkCore;
using Store.Domain.Entities;
using System.Threading.Tasks;

namespace Store.Domain.Extensions
{
    public static partial class CustomerDomainExtensions
    {
        public static async Task<bool> CheckIfEmailIsTaken(this DbSet<Customer> @this, string email)
        {
            bool result = await @this.AnyAsync(c => c.CompanyName == email);
            return result;
        }

        public static void ChangeCompanyName(Customer @this, string newCompanyName)
        {
            @this.CompanyName = newCompanyName;
        }
    }
}
