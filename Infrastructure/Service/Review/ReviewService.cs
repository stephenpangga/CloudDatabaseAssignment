using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Domain.DTO;
using Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Service
{
    public class ReviewService : IReviewService
    {
        private readonly ICosmosReadRepository<Review> _reviewReadRepository;
        private readonly ICosmosWriteRepository<Review> _reviewWriteRepository;

        public ReviewService(ICosmosReadRepository<Review> reviewReadRepository, ICosmosWriteRepository<Review> reviewWriteRepository)
        {
            _reviewReadRepository = reviewReadRepository;
            _reviewWriteRepository = reviewWriteRepository;
        }

        public async Task<Review> AddReviewAsync(ReviewDTO reviewDTO)
        {
            Review review = new Review();
            review.ReviewId = Guid.NewGuid();
            review.ProductId = reviewDTO.ProductId;
            review.Comments = reviewDTO.Comments;
            review.PartitionKey = reviewDTO.ProductId.ToString();
            return await _reviewWriteRepository.AddAsync(review);
        }

        public async Task DeleteReviewAsync(string reviewId)
        {
            Review review = await GetReviewByIdAsync(reviewId);
            await _reviewWriteRepository.Delete(review);
        }

        public async Task<IEnumerable<Review>> GetAllReviewsAsync()
        {
            return await _reviewReadRepository.GetAll().ToListAsync();
        }

        public async Task<Review> GetReviewByIdAsync(string reviewId)
        {
            Guid id = Guid.Parse(reviewId);
            return await _reviewReadRepository.GetAll().FirstOrDefaultAsync(r => r.ReviewId == id);
        }

        public async Task<Review> UpdateReviewAsync(ReviewDTO reviewDTO)
        {
            Review updateReview = new Review();
            updateReview.Comments = reviewDTO.Comments;
            return await _reviewWriteRepository.Update(updateReview);
        }
    }
}
