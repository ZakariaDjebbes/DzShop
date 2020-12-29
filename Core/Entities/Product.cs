using System.Linq;
using System.Collections.Generic;
namespace Core.Entities
{
    public class Product : BaseEntity
    {
        public Product()
        {
        }

        public Product(IReadOnlyList<Review> reviews, string name, string description, decimal price, string pictureUrl, ProductType productType, int productTypeId, ProductBrand productBrand, int productBrandId)
        {
            Name = name;
            Description = description;
            Price = price;
            PictureUrl = pictureUrl;
            ProductType = productType;
            ProductTypeId = productTypeId;
            ProductBrand = productBrand;
            ProductBrandId = productBrandId;
            Reviews = reviews;
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string PictureUrl { get; set; }
        public ProductType ProductType { get; set; }
        public int ProductTypeId { get; set; }
        public ProductBrand ProductBrand { get; set; }
        public int ProductBrandId { get; set; }
        public IReadOnlyList<Review> Reviews { get; set; }

		public int GetReviewsAverage()
		{
            if(Reviews != null && Reviews.Count > 0)
			    return (int)Reviews.Select(x => x.Stars).DefaultIfEmpty(0).Average();
            else
                return 0;
		}
    }
}
