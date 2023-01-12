using MediatR;
using Store.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace Store.Services.Mailing
{
    public interface IMailingService
    {
        public Task<Unit> SendNewCustomerRegistrationMessage(Customer customer, CancellationToken cancelationToken, string text = null);        
    }
}