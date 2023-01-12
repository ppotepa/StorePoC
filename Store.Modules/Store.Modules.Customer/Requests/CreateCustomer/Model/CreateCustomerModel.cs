using AutoMapper;
using Store.Domain.Entities;
using Store.Modules.Customers.CreateCustomer.DTO;
using Store.Modules.Customers.Requests.CreateCustomer;

namespace Store.Modules.Customers.Model
{
    [AutoMap(typeof(CreateCustomerRequest))]
    public class CreateCustomerModel
    {
        public CreateCustomerDTO Customer { get; set; }
    }
    
    public class CreateCustomerMappingProfile : Profile
    {
        public CreateCustomerMappingProfile()
        {
            CreateMap<Customer, CreateCustomerDTO>().ReverseMap();
            
            CreateMap<CreateCustomerModel, CreateCustomerRequest>();
            CreateMap<CreateCustomerRequest, CreateCustomerModel>();
        }
    }
}
