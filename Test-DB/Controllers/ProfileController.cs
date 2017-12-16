using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using Test_DB.Models;

namespace Test_DB.Controllers
{
    [Route("[controller]")]
    public class ProfileController : Controller
    {
        private readonly ProfileContext _context = null;
        
        public ProfileController(IOptions<Settings> settings)
        {
            _context = new ProfileContext(settings);
        }
        
        [Authorize]
        [HttpGet("{login}")]
        public IActionResult Get(string login)
        {
            try
            {
                var profile = _context.Profiles.Find(_ => _.Login == login).FirstOrDefault();
                if (profile == null)
                    profile = _context.Profiles.Find(_ => _.Login == User.Identity.Name).FirstOrDefault();
                return Ok(profile);
            }
            catch (Exception e)
            {
                return NotFound(e);
            }
        }
    }
}