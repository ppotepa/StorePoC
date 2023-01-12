using MediatR;
using Store.Domain.Entities;
using Store.Modules.Customers.Requests.ActivateCustomerRequest.Response;

namespace Store.Modules.Customers.Requests.ActivateCustomer
{
    public class ActivateCustomerRequest : IRequest<ActivateCustomerRequestResponse>
    { 
        public ActivateCustomerRequest() { }
        public Registration Registration { get; set; }
    }
}
