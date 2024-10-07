using eCommerceAPI.Application.Commands;
using eCommerceAPI.Application.Models;
using eCommerceAPI.Domain.Entities;
using eCommerceAPI.Domain.Interfaces.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceAPI.Application.Services
{
    public class ReviewService
    {
        private readonly IRepository<Review> _repository;

        public ReviewService(IRepository<Review> repository)
        {
            _repository = repository;
        }

        public async Task<int> AddReview(ReviewCommand command)
        {
            Review review = new Review()
            {
                CustomerId = command.CustomerId,
                ProductId = command.ProductId,
                Rate = command.Rate,
                TextReview = command.TextReview,                
                CreatedOn = DateTime.Now
            };
            return await _repository.Create(review);
        }

        public async Task UpdateReview(ReviewCommand command, int id)
        {
            Review review = await _repository.GetById(id);
            review.CustomerId = command.CustomerId;
            review.ProductId = command.ProductId;
            review.Rate = command.Rate;
            review.TextReview = command.TextReview;
            review.UpdatedOn = DateTime.Now;
            await _repository.Update(review);
        }

        public async Task DeleteReview(int id)
        {
            await _repository.Delete(id);
        }

        public async Task<IList<ReviewModel>> GetAllReviews()
        {
            IList<Review> reviewes = await _repository.GetAll();
            return reviewes.Select(r => new ReviewModel
            {
                Id = r.Id,
                CustomerId = r.CustomerId,
                ProductId=r.ProductId,
                Rate = r.Rate,
                TextReview = r.TextReview,
                CreatedOn = r.CreatedOn,
                UpdatedOn = r.UpdatedOn
            }).ToList();
        }

        public async Task<ReviewModel> GetReviewById(int id)
        {
            Review review = await _repository.GetById(id);
            if (review == null)
                return null;
            return new ReviewModel
            {
                Id = review.Id,
                CustomerId = review.CustomerId,
                ProductId = review.ProductId,
                Rate = review.Rate,
                TextReview = review.TextReview,
                UpdatedOn = review.UpdatedOn,
                CreatedOn = review.CreatedOn
            };
        }
    }
}
