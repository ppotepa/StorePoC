using MediatR;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Store.Domain.Context;
using Store.Domain.Entities;
using Store.Modules.Customers.Requests.CreateCustomer;
using Store.Modules.Customers.Requests.CreateCustomer.Model;
using Store.Modules.Customers.Requests.CreateCustomer.Responses;
using System.Threading;
using System.Threading.Tasks;

namespace Store.Modules.Customers.Requests
{
    public class CreateCustomerRequestHandler : IRequestHandler<CreateCustomerRequest, CreateCustomerRequestResponse>
    {
        private readonly IApplicationContext context;

        public CreateCustomerRequestHandler(IApplicationContext context)
        {
            this.context = context;
        }

        public async Task<CreateCustomerRequestResponse> Handle(CreateCustomerRequest request, CancellationToken cancellationToken)
        {
            EntityEntry<Customer> entry = await this.context.Customers.AddAsync(request.Customer, cancellationToken);

            return new CreateCustomerRequestResponse
            {
                ResponseText = $"CustomerId : {entry.Entity.Id}",
                Result = new CreateCustomerRequestResponseModel
                {
                    Customer = new CustomerCreatedDTO
                    {
                        Id = entry.Entity.Id
                    }
                }
            };
        }
    }
}
