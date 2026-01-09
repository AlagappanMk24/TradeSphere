namespace TradeSphere.Application.Mappings.CategoryMapping
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, CategoryListDto>().ReverseMap();
            CreateMap<Category, CategoryAddDto>().ReverseMap().IgnoreAllPropertiesWithAnInaccessibleSetter();
            CreateMap<Product, ProductListDto>().ReverseMap().IgnoreAllPropertiesWithAnInaccessibleSetter();
        }
    }
}