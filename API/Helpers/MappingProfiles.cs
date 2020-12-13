using API.Dtos;
using AutoMapper;
using Core.Entities;
using Core.Entities.Order;

namespace API.Helpers
{
	public class MappingProfiles : Profile
	{
		public MappingProfiles()
		{
			CreateMap<Product, ProductToReturnDto>()
				.ForMember(d => d.ProductBrand, o => o.MapFrom(s => s.ProductBrand.Name))
				.ForMember(d => d.ProductType, o => o.MapFrom(s => s.ProductType.Name))
				.ForMember(d => d.PictureUrl, o => o.MapFrom<ProductUrlResolver>());
			CreateMap<Core.Entities.Identity.Address, AddressDto>().ReverseMap();
			CreateMap<CustomerBasketDto, CustomerBasket>().ReverseMap();
			CreateMap<BasketItemDto, BasketItem>();
			CreateMap<AddressDto, Address>();
			CreateMap<Order, OrderToReturnDto>()
				.ForMember(o => o.DeliveryMethod, o => o.MapFrom(s => s.DeliveryMethod.ShortName))
				.ForMember(o => o.ShippingPrice, o => o.MapFrom(s => s.DeliveryMethod.Price));
			CreateMap<OrderItem, OrderItemDto>()
				.ForMember(oi => oi.ProductName, o => o.MapFrom(s => s.ItemOrdered.ProductName))
				.ForMember(oi => oi.ProductId, o => o.MapFrom(s => s.ItemOrdered.ProductItemId))
				.ForMember(oi => oi.PictureUrl, o => o.MapFrom(s => s.ItemOrdered.PictureUrl))
				.ForMember(oi => oi.PictureUrl, o => o.MapFrom<OrderItemUrlResolver>());
		}
	}
}