using Day1.DTO;
using Day1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Day1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IConfiguration configuration;

        public AccountController(UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            this.userManager = userManager;
            this.configuration = configuration;
        }


        #region Create Registerations


        [HttpPost("register")]
        public async Task<IActionResult> register(RegistartionDTO registartionDTO)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser applicationUser = new();
                applicationUser.UserName = registartionDTO.UserName;
                applicationUser.PasswordHash = registartionDTO.Password;
                applicationUser.Email = registartionDTO.Email;
                IdentityResult result = await userManager.CreateAsync(applicationUser, registartionDTO.Password);
                if (result.Succeeded)
                {
                    return Ok("Account Created Successfully");
                }
                else
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                        return BadRequest(ModelState);
                    }
                }
            }
            return BadRequest(ModelState);
        }
        #endregion


        #region Login 
        [HttpPost("login")]
        public async Task<IActionResult> login(LoginDTO loginDTO)
        {
            if (ModelState.IsValid)
            {
                // Check User Name 
               ApplicationUser user  = await userManager.FindByNameAsync(loginDTO.userName);
                if (user != null)
                {
                // Check Password
                   var Bool =  await userManager.CheckPasswordAsync(user, loginDTO.Password);
                    if (Bool)
                    {
                        // Create Token 
                        #region Create Token


                        #region Create Claims 
                        // Create Claims 
                        var Claims = new List<Claim>();
                        Claims.Add(new Claim(ClaimTypes.Name, user.UserName));
                        Claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
                        Claims.Add(new Claim(JwtRegisteredClaimNames.Jti , Guid.NewGuid().ToString()));

                        // Get Role
                       var Roles =  await userManager.GetRolesAsync(user);
                        foreach (var role in Roles)
                        {
                            Claims.Add(new Claim(ClaimTypes.Role, role));
                        }

                        #endregion

                        #region Get Key From SigninCredenitials
                        // Get Key From SigninCredenitials 
                        SecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:SecrutKey"]));
                        SigningCredentials signing = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);


                        #endregion


                        JwtSecurityToken token = new  JwtSecurityToken(
                            issuer: configuration["JWT:Validissuer"],
                            audience: configuration["JWT:ValidAudiance"],
                            claims: Claims,
                            expires: DateTime.Now.AddHours(1),
                            signingCredentials: signing
                            );


                        return Ok(new
                        {
                            Token=new JwtSecurityTokenHandler().WriteToken(token),
                            Expiration=token.ValidTo
                        });
                        #endregion
                    }
                }
                return Unauthorized();
            }
            return Unauthorized();
        }
        #endregion


    }
}
