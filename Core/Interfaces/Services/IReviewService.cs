using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Interfaces.Services
{
    public interface IReviewService
    {
        Task<Review> CreateReviewAsync(string userId, int productId, string comment, int stars);
    }
}