namespace Service.MappingProfiles
{
    internal class ProductProfiles : Profile
    {
        public ProductProfiles()
        {
            CreateMap<Product , ProductDto>()
                .ForMember(dest => dest.ProductBrand , options => options.MapFrom(src => src.productBrand.Name))
                .ForMember(dest => dest.ProductType , options => options.MapFrom(src => src.productType.Name))
                .ForMember(dest => dest.PictureUrl , options => options.MapFrom<PictureUrlResolver>());

            CreateMap<ProductBrand , BrandDto>();
            CreateMap<ProductType , TypeDto>();

        }

    }
}
