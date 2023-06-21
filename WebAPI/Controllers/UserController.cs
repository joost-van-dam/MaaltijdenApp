using Core.Domain;
using Core.DomainServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<PackageController> _logger;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public UserController(ILogger<PackageController> logger, UserManager<IdentityUser> userMgr, SignInManager<IdentityUser> signInMgr)
        {
            _logger = logger;
            _userManager = userMgr;
            _signInManager = signInMgr;
        }

        [HttpPost(Name = "Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
        {
            var user = await _userManager.FindByEmailAsync(loginModel.EmailAddress);
            if (user != null)
            {
                await _signInManager.SignOutAsync();
                if ((await _signInManager.PasswordSignInAsync(user, loginModel.Password, false, false)).Succeeded)
                {
                    return Ok(user);
                }
            }
            return StatusCode(403, new { status = 403, message = "Emailaddress or password incorrect." });
        }

        [HttpGet(Name = "Logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok(new { status = 200, message = "Logged out." });
        }
    }
}