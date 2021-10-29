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
            HttpResponseData response = req.CreateResponse();
            try
            {
                await response.WriteAsJsonAsync(_reviewService.GetAllReviewsAsync());
                response.StatusCode = HttpStatusCode.OK;
            }
            catch(Exception e)
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                await response.WriteStringAsync(e.Message, Encoding.UTF8);
            }
            return response;
        }
        [Function("GetByReviewId")]
        public async Task<HttpResponseData> GetByReviewId([HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "reviews/{reviewId}")] HttpRequestData req, string reviewId, FunctionContext executionContext)
        {
            HttpResponseData response = req.CreateResponse();
            try
            {
                await response.WriteAsJsonAsync(_reviewService.GetReviewByIdAsync(reviewId));

                response.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception e)
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                await response.WriteStringAsync(e.Message, Encoding.UTF8);
            }
            return response;
        }

        [Function("AddReview")]
        public async Task<HttpResponseData> AddReview([HttpTrigger(AuthorizationLevel.Anonymous, "Post", Route = "reviews")] HttpRequestData req, FunctionContext executionContext)
        {
            HttpResponseData response = req.CreateResponse();
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            ReviewDTO reviewDTO = JsonConvert.DeserializeObject<ReviewDTO>(requestBody);
            try
            {
                await response.WriteAsJsonAsync(_reviewService.AddReviewAsync(reviewDTO));
                response.StatusCode = HttpStatusCode.Created;
            }
            catch (Exception e)
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                await response.WriteStringAsync(e.Message, Encoding.UTF8);
            }
            return response;
        }


        [Function("UpdateReview")]
        public async Task<HttpResponseData> UpdateReview([HttpTrigger(AuthorizationLevel.Anonymous, "Put", Route = "reviews/{reviewId}")] HttpRequestData req, string reviewId, FunctionContext executionContext)
        {
            HttpResponseData response = req.CreateResponse();
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            ReviewDTO reviewDTO = JsonConvert.DeserializeObject<ReviewDTO>(requestBody);
            try
            {
                await response.WriteAsJsonAsync(_reviewService.UpdateReviewAsync(reviewDTO, reviewId));
                response.StatusCode = HttpStatusCode.Accepted;
            }
            catch (Exception e)
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                await response.WriteStringAsync(e.Message, Encoding.UTF8);
            }
            return response;
        }

        [Function("DeleteReview")]
        public async Task<HttpResponseData> DeleteReview([HttpTrigger(AuthorizationLevel.Anonymous, "Delete", Route = "reviews/{reviewId}")] HttpRequestData req, string reviewId, FunctionContext executionContext)
        {
            HttpResponseData response = req.CreateResponse();
            try
            {
                await _reviewService.DeleteReviewAsync(reviewId);
                response.StatusCode = HttpStatusCode.Accepted;
                await response.WriteStringAsync("Review has been deleted successfully!", Encoding.UTF8);
            }
            catch (Exception e)
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                await response.WriteStringAsync(e.Message, Encoding.UTF8);
            }
            return response;
        }
    }
}
