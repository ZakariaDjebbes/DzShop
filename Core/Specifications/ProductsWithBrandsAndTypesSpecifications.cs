using Core.Entities;

namespace Core.Specifications
{
	public class ProductsWithBrandsAndTypesSpecifications : BaseSpecification<Product>
	{
		public ProductsWithBrandsAndTypesSpecifications(ProductSpecificationParams specParams) 
			: base(x => 
			(string.IsNullOrEmpty(specParams.Search) || x.Name.ToLower().Contains(specParams.Search)) &&
			(!specParams.BrandId.HasValue || x.ProductBrandId == specParams.BrandId) &&
			(!specParams.TypeId.HasValue || x.ProductTypeId == specParams.TypeId)
		)
		{
			AddInclude(x => x.ProductBrand);
			AddInclude(x => x.ProductType);
			SetOrderBy(x => x.Name);
			ApplyPaging(specParams.PageSize * (specParams.PageIndex - 1),  specParams.PageSize); 

			if (!string.IsNullOrEmpty(specParams.Sort))
			{
				switch (specParams.Sort)
				{
					case "priceAsc":
						SetOrderBy(x => x.Price);
						break;
					case "priceDesc":
						SetOrderByDescending(x => x.Price);
						break;
					default:
						SetOrderBy(x => x.Name);
						break;
				}
			}
		}

		public ProductsWithBrandsAndTypesSpecifications(int id)
			: base(x => x.Id == id)
		{
			AddInclude(x => x.ProductBrand);
			AddInclude(x => x.ProductType);
		}
	}
}