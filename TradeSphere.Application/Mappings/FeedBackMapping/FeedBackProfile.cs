namespace TradeSphere.Application.Mappings.FeedBackMapping
{
    public class FeedBackProfile : Profile
    {
        public FeedBackProfile()
        {
            CreateMap<FeedBackAddDto, FeedBack>().ReverseMap();
            CreateMap<FeedBackUpdateDto, FeedBack>().ReverseMap();
            CreateMap<FeedBack, FeedBackReadDto>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.ApplicationUser.FirstName + "" + src.ApplicationUser.LastName))
            .ReverseMap();
        }
    }
}