using Core.Entities;

namespace Core.Specifications
{
    public class ReviewOfProductWithPagingSpecification : BaseSpecification<Review>
    {
        public ReviewOfProductWithPagingSpecification(int productId, ReviewSpecificationParams specParams) : base(r => r.ProductId == productId)
        {
            AddInclude(x => x.AppUser);
            SetOrderByDescending(x => x.ReviewDate);
            ApplyPaging(specParams.PageSize * (specParams.PageIndex - 1),  specParams.PageSize);
        }

        public ReviewOfProductWithPagingSpecification(int productId) : base(r => r.ProductId == productId)
        {
        }
    }
}