using MediatR;
using Microsoft.EntityFrameworkCore;
using Store.Domain.Context;
using Store.Modules.Customers.Requests.GetCustomerById.DTO;
using Store.Modules.Customers.Requests.GetCustomerById.Model;
using Store.Modules.Customers.Requests.GetCustomerById.Response;
using System.Threading;
using System.Threading.Tasks;

namespace Store.Modules.Customers.Requests.GetCustomerById
{
    internal class GetCustomerByIdRequestHandler : IRequestHandler<GetCustomerByIdRequest, GetCustomerByIdResponse>
    {
        private readonly IApplicationContext context;
        public GetCustomerByIdRequestHandler(IApplicationContext context)
        {
            this.context = context;
        }

        public async Task<GetCustomerByIdResponse> Handle(GetCustomerByIdRequest request, CancellationToken cancellationToken)
        {
            Domain.Entities.Customer result = await this.context
                .Customers
                .FirstOrDefaultAsync(customer => customer.Id == request.Customer.Id);

            return new GetCustomerByIdResponse
            {
                Result = new GetCustomerByIdResponseModel
                {
                    Customer = new CustomerResponseDTO 
                    {
                        Id = result.Id
                    }
                }
            };
        }
    }
}
