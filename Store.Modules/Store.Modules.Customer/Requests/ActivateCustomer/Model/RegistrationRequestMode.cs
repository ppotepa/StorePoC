using AutoMapper;
using Store.Domain.Entities;
using Store.Modules.Customers.Requests.ActivateCustomer.DTO;

namespace Store.Modules.Customers.Requests.ActivateCustomer.Model
{
    public class ActivateCustomerModel
    {
        public RegistrationDTO Registration { get; set; }
    }

    public class ActivateCustomerModelMappingProfile : Profile
    {
        public ActivateCustomerModelMappingProfile()
        {
            CreateMap<Registration, RegistrationDTO>().ReverseMap();
            CreateMap<ActivateCustomerModel, ActivateCustomerRequest>().ReverseMap();
        }
    }
}
