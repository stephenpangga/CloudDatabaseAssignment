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

namespace WidgetAndCoAPI
{
    public class UserHttpTrigger
    {
        private ILogger Logger { get; }

        private readonly IUserService _userService;

        public UserHttpTrigger(ILogger<OrderHttpTrigger> Logger, IUserService userService)
        {
            this.Logger = Logger;
            _userService = userService;
        }

        [Function("GetAllUsers")]
        public async Task<HttpResponseData> GetAllUsers([HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "users")] HttpRequestData req, FunctionContext executionContext)
        {
            HttpResponseData response = req.CreateResponse();
            try
            {
                await response.WriteAsJsonAsync(_userService.GetAllUsersAsync());
                response.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception e)
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                await response.WriteStringAsync(e.Message, Encoding.UTF8);
            }
            return response;
        }
        [Function("GetByUserId")]
        public async Task<HttpResponseData> GetAUserById([HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "users/{UserId}")] HttpRequestData req, string userId, FunctionContext executionContext)
        {
            HttpResponseData response = req.CreateResponse();
            try
            {
                await response.WriteAsJsonAsync(_userService.GetUserByIdAsync(userId));
                response.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception e)
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                await response.WriteStringAsync(e.Message, Encoding.UTF8);
            }
            return response;
        }

        [Function("AddUser")]
        public async Task<HttpResponseData> AddUser([HttpTrigger(AuthorizationLevel.Anonymous, "Post", Route = "users")] HttpRequestData req, FunctionContext executionContext)
        {
            HttpResponseData response = req.CreateResponse();
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            UserDTO userDTO = JsonConvert.DeserializeObject<UserDTO>(requestBody);

            try
            {
                await response.WriteAsJsonAsync(_userService.AddUserAsync(userDTO));
                response.StatusCode = HttpStatusCode.Created;
            }
            catch (Exception e)
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                await response.WriteStringAsync(e.Message, Encoding.UTF8);
            }
            return response;
        }


        [Function("UpdateUser")]
        public async Task<HttpResponseData> UpdateUser([HttpTrigger(AuthorizationLevel.Anonymous, "Put", Route = "users/{UserId}")] HttpRequestData req, string userId, FunctionContext executionContext)
        {
            HttpResponseData response = req.CreateResponse();
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            UserDTO userDTO = JsonConvert.DeserializeObject<UserDTO>(requestBody);
            try
            {
                await response.WriteAsJsonAsync(await _userService.UpdateUserAsync(userDTO, userId));
                response.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception e)
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                await response.WriteStringAsync(e.Message, Encoding.UTF8);
            }
            return response;
        }

        [Function("DeleteUser")]
        public async Task<HttpResponseData> DeleteUser([HttpTrigger(AuthorizationLevel.Anonymous, "Delete", Route = "users/{UserId}")] HttpRequestData req, string userId, FunctionContext executionContext)
        {
            HttpResponseData response = req.CreateResponse();
            try
            {
                await _userService.DeleteUserAsync(userId);
                response.StatusCode = HttpStatusCode.Accepted;
                await response.WriteStringAsync("User has been deleted successfully!", Encoding.UTF8);
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
