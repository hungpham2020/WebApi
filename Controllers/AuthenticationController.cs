using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApi.Model;
using WebApi.Model.DTO;

namespace WebApi.Controllers
{
    [EnableCors("corsapp")]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<CustomUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration config;

        public AuthenticationController(UserManager<CustomUser> userManager,RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            config = configuration;
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await userManager.FindByNameAsync(model.UserName);
            if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
            {
                var userRoles = await userManager.GetRolesAsync(user);
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim("UserID", user.Id),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWT:Secret"]));

                var token = new JwtSecurityToken(
                    issuer: config["JWT:ValidIssuer"],
                    audience: config["JWT:ValidAudience"],
                    expires: DateTime.Now.AddDays(1),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expration = token.ValidTo
                });
            }
            return Unauthorized();
        }

        [Authorize]
        [HttpGet]
        [Route("getuserinfo")]
        public IActionResult GetUserInfo()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            return Ok(new { 
                userName = identity?.FindFirst(ClaimTypes.Name)?.Value,
                userRole = identity?.FindFirst(ClaimTypes.Role)?.Value,
            });
        }
    }
}
