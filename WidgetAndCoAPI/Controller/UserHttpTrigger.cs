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
            HttpResponseData response = req.CreateResponse(System.Net.HttpStatusCode.OK);
            await response.WriteAsJsonAsync(_userService.GetAllUsersAsync());
            return response;
        }
        [Function("GetByUserId")]
        public async Task<HttpResponseData> GetAUserById([HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "users/{UserId}")] HttpRequestData req, string userId, FunctionContext executionContext)
        {
            HttpResponseData response = req.CreateResponse(System.Net.HttpStatusCode.OK);
            await response.WriteAsJsonAsync(_userService.GetUserByIdAsync(userId));
            return response;
        }

        [Function("AddUser")]
        public async Task<HttpResponseData> AddUser([HttpTrigger(AuthorizationLevel.Anonymous, "Post", Route = "users")] HttpRequestData req, FunctionContext executionContext)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            UserDTO userDTO = JsonConvert.DeserializeObject<UserDTO>(requestBody);
            HttpResponseData response = req.CreateResponse(System.Net.HttpStatusCode.OK);
            await response.WriteAsJsonAsync(_userService.AddUserAsync(userDTO));
            return response;
        }


        [Function("UpdateUser")]
        public async Task<HttpResponseData> UpdateUser([HttpTrigger(AuthorizationLevel.Anonymous, "Put", Route = "users/{UserId}")] HttpRequestData req, string userId, FunctionContext executionContext)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            UserDTO userDTO = JsonConvert.DeserializeObject<UserDTO>(requestBody);
            HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(await _userService.UpdateUserAsync(userDTO, userId));
            return response;
        }

        [Function("DeleteUser")]
        public async Task<HttpResponseData> DeleteUser([HttpTrigger(AuthorizationLevel.Anonymous, "Delete", Route = "users/{UserId}")] HttpRequestData req, string userId, FunctionContext executionContext)
        {
            HttpResponseData response = req.CreateResponse();
            await _userService.DeleteUserAsync(userId);
            response.StatusCode = HttpStatusCode.Accepted;
            await response.WriteStringAsync("Project deleted successfully!", Encoding.UTF8);
            return response;
        }
    }
}
