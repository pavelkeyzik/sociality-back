using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using Test_DB.Models;

namespace Test_DB.Controllers
{
    [Route("[controller]")]
    public class RegistrationController : Controller
    {
        private readonly UserContext _context = null;
        private readonly ProfileContext _contextProfile = null;
        
        public RegistrationController(IOptions<Settings> settings)
        {
            _context = new UserContext(settings);
            _contextProfile = new ProfileContext(settings);
        }
        
        [HttpPost]
        public IActionResult Post([FromBody] User value)
        {
            if (value.Password != value.RepeatedPassword)
                return BadRequest();
            
            var person = _context.Users.Find(_ => _.Login == value.Login).FirstOrDefault();
            if (person != null)
                return BadRequest();
    
            try
            {
                User user = new User()
                {
                    Login = value.Login,
                    Password = new MD5(value.Password).getHash(),
                    Role = "user"
                };
                
                _context.Users.InsertOne(user);
    
                var profile = new Profile()
                {
                    Login = user.Login,
                    Name = user.Login,
                    Online = false,
                    Gender = value.Gender
                };
                
                _contextProfile.Profiles.InsertOne(profile);
                return Ok(new {message = "Ok"});
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}