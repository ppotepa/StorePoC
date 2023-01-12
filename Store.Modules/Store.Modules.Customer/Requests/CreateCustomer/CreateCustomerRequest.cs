using MediatR;
using Store.Domain.Entities;
using Store.Modules.Customers.Requests.CreateCustomer.Responses;

namespace Store.Modules.Customers.Requests.CreateCustomer
{
    public class CreateCustomerRequest : IRequest<CreateCustomerRequestResponse>
    { 
    
        public CreateCustomerRequest() { }
        public Customer Customer { get; set; }
    }
}
