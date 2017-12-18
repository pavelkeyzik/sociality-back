using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using Test_DB.Models;

namespace Test_DB.Controllers
{
    [Route("[controller]")]
    public class AuthorizationController : Controller
    {
        private readonly UserContext _context = null;
        private readonly ProfileContext _contextProfile = null;
        
        public AuthorizationController(IOptions<Settings> settings)
        {
            _context = new UserContext(settings);
            _contextProfile = new ProfileContext(settings);
        }
        
        [HttpPost]
        public IActionResult Post([FromBody] User value)
        {
            value.Password = new MD5(value.Password).getHash();
            var identity = GetIdentity(value.Login, value.Password);
            if (identity == null)
            {
                return NotFound();
            }

            var profile = _contextProfile.Profiles.Find(_ => _.Login == value.Login).FirstOrDefault();

            return Ok(new
            {
                access_token = GenerateToken(identity),
                id = profile.Id
            });
        }
        
        private ClaimsIdentity GetIdentity(string username, string password)
        {
            User person = _context.Users.Find(_ => _.Login == username && _.Password == password).FirstOrDefault();
            if (person != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, person.Login),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, person.Role)
                };
                ClaimsIdentity claimsIdentity =
                    new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                        ClaimsIdentity.DefaultRoleClaimType);
                return claimsIdentity;
            }
 
            return null;
        }

        private string GenerateToken(ClaimsIdentity identity)
        {
            var now = DateTime.UtcNow;
            // создаем JWT-токен
            var jwt = new JwtSecurityToken(
                issuer: AuthOptions.ISSUER,
                audience: AuthOptions.AUDIENCE,
                notBefore: now,
                claims: identity.Claims,
                expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }
}