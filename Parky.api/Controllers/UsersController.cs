using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Parky.api.Models;
using Parky.api.Repository.Interfaces;

namespace Parky.api.Controllers
{
    [Authorize]
    [Route("api/v{version:apiVersion}/Users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepo;

        public UsersController(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        [AllowAnonymous] //obviously we want anyone to be able to call Authenticate, or else ... we'll always have auth issues
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] User model)
        {
            var user = _userRepo.Authenticate(model.UserName, model.Password);
            if (user == null)
            {
                return BadRequest(new {message = "Username or Password is incorrect"});
            }

            return Ok(user);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register([FromBody] User model)
        {
            var unique = _userRepo.IsUniqueUser(model.UserName);
            if (!unique)
            {
                return BadRequest(new {message = "Username already exists"});
            }

            var user = _userRepo.Register(model.UserName, model.Password);
            if (user == null)
            {
                return BadRequest(new {message = "Error occurred while registering"});
            }

            return Ok();
        }
    }
}
