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
            if (reviewDTO.ProductId != Guid.Empty)
            {
                Review review = new Review();
                review.ReviewId = Guid.NewGuid();
                review.ProductId = reviewDTO.ProductId;
                review.UserId = reviewDTO.UserId;
                review.Comments = reviewDTO.Comments;
                review.PartitionKey = reviewDTO.ProductId.ToString();
                return await _reviewWriteRepository.AddAsync(review);
            }
            else
            {
                throw new Exception("Please enter a product ID");
            }
            
        }

        public async Task DeleteReviewAsync(string reviewId)
        {
            if (!string.IsNullOrEmpty(reviewId))
            {
                Review review = await GetReviewByIdAsync(reviewId);
                await _reviewWriteRepository.Delete(review);
            }
            else
            {
                throw new Exception("Review Id provided does not exist");
            }
        }

        public async Task<IEnumerable<Review>> GetAllReviewsAsync()
        {
            return await _reviewReadRepository.GetAll().ToListAsync();
        }

        public async Task<Review> GetReviewByIdAsync(string reviewId)
        {
            try
            {
                Guid id = Guid.Parse(reviewId);
                var review = await _reviewReadRepository.GetAll().FirstOrDefaultAsync(r => r.ReviewId == id);

                if(review == null)
                {
                    throw new Exception("The review you are looking for does not exist");
                }
                return review;
            }
            catch
            {
                throw new Exception("Please provide a proper ID");
            }
        }

        public async Task<Review> UpdateReviewAsync(ReviewDTO reviewDTO, string reviewId)
        {
            Review updateReview = await GetReviewByIdAsync(reviewId);
            updateReview.Comments = reviewDTO.Comments;
            return await _reviewWriteRepository.Update(updateReview);
        }
    }
}
