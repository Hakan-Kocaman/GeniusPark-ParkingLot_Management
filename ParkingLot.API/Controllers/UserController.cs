using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkingLot.API.Services;

namespace ParkingLot.API.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;
        public UserController(UserService userService) {
        
            _userService = userService;
        }

        [HttpGet("{user_name}/{user_password}")]
        public async Task<IActionResult> Login(string user_name, string user_password)
        {
            var result = await _userService.Login(user_name, user_password);

            if (result == null)
                return (StatusCode(500, "Login service error"));

            return Ok(result);


        }
        
    }
}
