using AutoMapper;
using ProductManager.Domain.Entities;
using ProductManager.Features.Products.Commands;
using ProductManager.ViewModel;

namespace ProductManager
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<CreateProductViewModel, CreateProductCommand>();
            CreateMap<CreateProductCommand, Product>().ForMember(d => d.Id, opt => opt.Ignore())
                .ForMember(d => d.IsDeleted, opt => opt.Ignore())
                .ForMember(d => d.CreatedAt, opt => opt.Ignore())
                .ForMember(d => d.UpdatedAt, opt => opt.Ignore())
                .ForMember(d => d.Analytics, opt => opt.Ignore());
            CreateMap<Product, ProductViewModel>()
                .ForMember(d => d.Currency, opt => opt.Ignore())
                .ForMember(d => d.ViewCount, opt => opt.MapFrom(s => s.Analytics.ViewCount));
        }
    }
}
