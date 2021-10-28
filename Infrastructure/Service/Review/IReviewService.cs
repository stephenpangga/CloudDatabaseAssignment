using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Domain.DTO;

namespace Infrastructure.Service
{
    public interface IReviewService
    {
        Task<IEnumerable<Review>> GetAllReviewsAsync();

        Task<Review> GetReviewByIdAsync(string reviewId);

        Task<Review> AddReviewAsync(ReviewDTO reviewDTO);

        Task<Review> UpdateReviewAsync(ReviewDTO reviewDTO, string reviewId);

        Task DeleteReviewAsync(string reviewId);
    }
}
