using AutoMapper;
using WebStore.Domain.Entities;
using WebStore.Domain.Entities.Employees;
using WebStore.ViewMoodel;

namespace WebStore.Infrastructure.AutoMapper
{
    public class ViewModelsMapping : Profile
    {
        public ViewModelsMapping()
        {
            CreateMap<Product, ProductViewModel>()
                .ForMember(view_model => view_model.Brand, opt => opt.MapFrom(product => product.Brand.Name))
                .ReverseMap();

            CreateMap<Employee, EmployeeViewModel>()
                .ForMember(view_model => view_model.FirstName, opt => opt.MapFrom(employee => employee.FirstName))
                .ReverseMap();
        }
    }
}
