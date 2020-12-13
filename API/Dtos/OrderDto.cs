using System.ComponentModel.DataAnnotations;

namespace API.Dtos
{
    public class OrderDto
    {
        [Required]
        public string basketId { get; set; }
        [Required]
        public int deliveryMethodId { get; set; }
        [Required]
        public AddressDto shippingAddress { get; set; }        
    }
}