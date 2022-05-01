using AuctionsDenis.Service.UserService;
using AuctionsProject.Models;
using AutoMapper;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Options;
using WebApi.Authorization;
using WebApi.Helpers;
using WebApi.Models.Users;

namespace AuctionsDenis.Controllers;


    [Authorize]
    [ApiController]
    [Route("[controller]/")]
    public class UsersController : ControllerBase
    {
        private IUserService _userService;
        private readonly AppSettings _appSettings;

        public UsersController(
            IUserService userService,
            IOptions<AppSettings> appSettings)
        {
            _userService = userService;
            _appSettings = appSettings.Value;
        }

        /// <summary>
        /// Login
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("Login")]
        public IActionResult Authenticate(AuthenticateRequest model)
        {
            var response = _userService.Authenticate(model);
            return Ok(response);
        }

        
        /// <summary>
        /// to create a new account
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register(RegisterRequest model)
        {
            _userService.Register(model);
            return Ok(new { message = "Registration successful" });
        }
        /// <summary>
        /// to return all users info
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("AllUsers")]
        public IActionResult GetAll()
        {
            var users = _userService.GetAll();
            IEnumerable<GetUserResponse> results;
            results = users.Adapt<IEnumerable<GetUserResponse>>();
            return Ok(results);
        }
        /// <summary>
        /// get the user info by id
        /// </summary>
        /// <param name="id">userId</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var user = _userService.GetById(id);
            var results = new GetUserResponse();
            results = user.Adapt<GetUserResponse>();
            return Ok(results);
        }
        /// <summary>
        /// to uptade user info by id
        /// </summary>
        /// <param name="id">userId</param>
        /// <param name="model"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPut("{id}")]
        public IActionResult Update(int id, UpdateRequest model)
        {
            _userService.Update(id, model);
            return Ok(new { message = "User updated successfully" });
        }
        /// <summary>
        /// to delete account by user id
        /// </summary>
        /// <param name="id">userId</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _userService.Delete(id);
            return Ok(new { message = "User deleted successfully" });
        }

    }
