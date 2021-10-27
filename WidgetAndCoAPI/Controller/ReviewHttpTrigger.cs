using Domain.DTO;
using Infrastructure;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Service;

namespace WidgetAndCoAPI
{
    public class ReviewHttpTrigger
    {
        private ILogger Logger { get; }

        private readonly IReviewService _reviewService;

        public ReviewHttpTrigger(ILogger<OrderHttpTrigger> Logger, IReviewService reviewService)
        {
            this.Logger = Logger;
            _reviewService = reviewService;
        }

        [Function("GetAllReview")]
        public async Task<HttpResponseData> GetAllReview([HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "reviews")] HttpRequestData req, FunctionContext executionContext)
        {
            HttpResponseData response = req.CreateResponse(System.Net.HttpStatusCode.OK);
            await response.WriteAsJsonAsync(_reviewService.GetAllReviewsAsync());
            return response;
        }
        [Function("GetByReviewId")]
        public async Task<HttpResponseData> GetByReviewId([HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "reviews/{reviewId}")] HttpRequestData req, string reviewId, FunctionContext executionContext)
        {
            HttpResponseData response = req.CreateResponse(System.Net.HttpStatusCode.OK);
            await response.WriteAsJsonAsync(_reviewService.GetReviewByIdAsync(reviewId));
            return response;
        }

        [Function("AddReview")]
        public async Task<HttpResponseData> AddReview([HttpTrigger(AuthorizationLevel.Anonymous, "Post", Route = "reviews")] HttpRequestData req, FunctionContext executionContext)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            ReviewDTO reviewDTO = JsonConvert.DeserializeObject<ReviewDTO>(requestBody);
            HttpResponseData response = req.CreateResponse(System.Net.HttpStatusCode.OK);
            await response.WriteAsJsonAsync(_reviewService.AddReviewAsync(reviewDTO));
            return response;
        }


        [Function("UpdateReview")]
        public async Task<HttpResponseData> UpdateReview([HttpTrigger(AuthorizationLevel.Anonymous, "Update", Route = "reviews/{reviewId}")] HttpRequestData req, string reviewId, FunctionContext executionContext)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            ReviewDTO reviewDTO = JsonConvert.DeserializeObject<ReviewDTO>(requestBody);
            HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(_reviewService.UpdateReviewAsync(reviewDTO));
            return response;
        }

        [Function("DeleteReview")]
        public async Task<HttpResponseData> DeleteReview([HttpTrigger(AuthorizationLevel.Anonymous, "Delete", Route = "reviews/{reviewId}")] HttpRequestData req, string reviewId, FunctionContext executionContext)
        {
            HttpResponseData response = req.CreateResponse();
            await _reviewService.DeleteReviewAsync(reviewId);
            response.StatusCode = HttpStatusCode.Accepted;
            await response.WriteStringAsync("Project deleted successfully!", Encoding.UTF8);
            return response;
        }
    }
}
