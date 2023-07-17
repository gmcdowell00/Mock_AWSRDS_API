using System;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;
using Mock_AWS_API.Controllers;
using Mock_AWSRDS_API.Interface;
using Mock_AWSRDS_API.Helper;

namespace Mock_AWSRDS_API.Controllers
{
    [ApiController]
    [Route("/api/v1/user")]
    public class UsersController : ControllerBase
    {
        private readonly IMockService _mockService;
        private readonly ILogger<ShinobiController> _logger;
       

        public UsersController(IMockService mockService, ILogger<ShinobiController> logger)
        {
            _mockService = mockService;
            _logger = logger;    
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpPost("CreateUser")]
        public IActionResult AdduUser(string username, string password)
        {
            try
            {
                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                    return BadRequest("Username or password is empty");

                if (_mockService.AddUser(username, password))
                    return Ok($"Added new user {username}");
                else
                    return BadRequest($"Could not add new user");

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }           
        }

        /// <summary>
        /// Retrieve Jwt Token
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpPost("Login")]
        public IActionResult Login(string username, string password)
        {
            string jwtToken = _mockService.Login(username, password);
            return string.IsNullOrEmpty(jwtToken) ? BadRequest("Invalid username or password") : Ok(jwtToken);
        }
    }
}
