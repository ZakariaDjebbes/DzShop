using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;

namespace Infrastructure.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ReviewService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Review> CreateReviewAsync(string userId, int productId, string comment, int stars)
        {
            var review = new Review(userId, productId, comment, stars);
			_unitOfWork.Repository<Review>().Add(review);

            var results = await _unitOfWork.Complete();

            if (results <= 0)
            {
                return null;
            }        
            
            return review;
        }
    }
}