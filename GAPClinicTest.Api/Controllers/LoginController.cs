using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace GAPClinicTest.Api.Controllers
{

   

    [Route("[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        [AllowAnonymous]
        [HttpPost]
        public ActionResult<LoginDetails> LoginUser([FromBody] LoginDetails loginDetails)
        {
            try
            {

                var userID = Guid.NewGuid().ToString();
                var secretToken = "9ecc0154-2a82-4633-a567-db9b742c32b4";

               if (loginDetails==null || !loginDetails.Password.Equals("password") || !loginDetails.User .Equals("admin"))
                {
                    throw new ValidationException("Wrong User!");
                }

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(secretToken);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                    new Claim(ClaimTypes.Name, userID)
                    }),
                    Expires = DateTime.UtcNow.AddDays(7),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(token);
                loginDetails.Token = tokenString;


                return Ok(loginDetails);


            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }

    public class LoginDetails
    {
        public string Password { get; set; }
        public string User { get; set; }
        public string Token { get; set; }
    }
}