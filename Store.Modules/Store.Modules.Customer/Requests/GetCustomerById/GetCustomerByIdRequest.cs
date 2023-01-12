using MediatR;
using Store.Domain.Entities;
using Store.Modules.Customers.Requests.GetCustomerById.Response;

namespace Store.Modules.Customers.Requests.GetCustomerById
{
    public class GetCustomerByIdRequest : IRequest<GetCustomerByIdResponse>
    {
        public Customer Customer { get; set; }
    }
}
