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
        
        public RegistrationController(IOptions<Settings> settings)
        {
            _context = new UserContext(settings);
        }
        
        [HttpPost]
        public IActionResult Post([FromBody] User value)
        {
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
                return Ok();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}