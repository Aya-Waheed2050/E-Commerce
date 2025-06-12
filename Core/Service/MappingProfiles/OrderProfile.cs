namespace Service.MappingProfiles
{
    internal class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<AddressDto , OrderAddress>().ReverseMap();

            CreateMap<Order , OrderToReturnDto>()
                   .ForMember(dest => dest.DeliveryMethod , o => o.MapFrom(src => src.DeliveryMethod.ShortName));

            CreateMap<OrderItem , OrderItemDto>()
                   .ForMember(dest => dest.ProductName , o => o.MapFrom(src => src.Product.ProductName))
                   .ForMember(dest => dest.PictureUrl , o => o.MapFrom<OrderItemPictureUrlResolver>());

            CreateMap<DeliveryMethod, DeliveryMethodDto>();

        }


    }

}
