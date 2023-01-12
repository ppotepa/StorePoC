using MediatR;
using Store.Event;
using Store.Services.Mailing;
using System.Threading;
using System.Threading.Tasks;

namespace Store.Modules.Customers.Requests.CreateCustomer.Events
{
    public class SendRegistrationMailEvent : IEvent<CreateCustomerRequest>
    {
        private readonly IMailingService mailingService;
   
        public SendRegistrationMailEvent(IMailingService mailingService)
        {
            this.mailingService = mailingService;
        }

        public async Task<Unit> Handle(CreateCustomerRequest request, CancellationToken cancelationToken) 
        {
            Unit result = await this.mailingService.SendNewCustomerRegistrationMessage(request.Customer, cancelationToken);
            return result;
        }
    }
}
