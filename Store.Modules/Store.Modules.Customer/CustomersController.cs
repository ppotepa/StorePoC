using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Store.Domain.Context;
using Store.Modules.Customers.Model;
using Store.Modules.Customers.Requests.ActivateCustomer;
using Store.Modules.Customers.Requests.ActivateCustomer.Model;
using Store.Modules.Customers.Requests.CreateCustomer;
using Store.Modules.Customers.Requests.GetCustomerById;
using Store.Modules.Customers.Requests.GetCustomerById.Model;
using Store.Utilities.Abstractions;
using System.Threading.Tasks;

namespace Store.Modules.Customers
{
    /// <summary>
    /// Users Domain Controller
    /// </summary>    
    public class CustomersController : ModuleController
    {
        public CustomersController(IMediator mediator, IMapper mapper, IApplicationContext context) 
            : base(mediator, mapper, context) { }


        /// <summary>
        /// Gets User
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Activate([FromQuery] ActivateCustomerModel request)
            => await Handle(request: this.mapper.Map<ActivateCustomerModel, ActivateCustomerRequest>(request));

        /// <summary>
        /// Creates new Customer.
        /// </summary>
        /// <param name="request">UserDTO</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create(CreateCustomerModel request) 
            => await Handle(request: this.mapper.Map<CreateCustomerModel, CreateCustomerRequest>(request));

        [HttpGet]
        public async Task<IActionResult> GetCustomerById(GetCustomerByIdResponseModel request)
            => await Handle(request: this.mapper.Map<GetCustomerByIdResponseModel, GetCustomerByIdRequest>(request));
       
        
    }
}
