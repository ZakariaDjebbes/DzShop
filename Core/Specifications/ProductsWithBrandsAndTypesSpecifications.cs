using Core.Entities;

namespace Core.Specifications
{
	public class ProductsWithBrandsAndTypesSpecifications : BaseSpecification<Product>
	{
		public ProductsWithBrandsAndTypesSpecifications()
		{
			AddInclude(x => x.ProductBrand);
			AddInclude(x => x.ProductType);
		}

		public ProductsWithBrandsAndTypesSpecifications(int id)
			: base(x => x.Id == id)
		{
			AddInclude(x => x.ProductBrand);
			AddInclude(x => x.ProductType);
		}
	}
}