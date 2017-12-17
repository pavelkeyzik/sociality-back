using System;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using Test_DB.Models;

namespace Test_DB.Controllers
{
    [Route("[controller]")]
    public class MessagesController : Controller
    {
        private readonly MessageContext _context = null;
        private readonly ProfileContext _contextProfiles = null;
        private IHostingEnvironment _env;

        public MessagesController(IOptions<Settings> settings, IHostingEnvironment env)
        {
            _env = env;
            _context = new MessageContext(settings);
            _contextProfiles = new ProfileContext(settings);
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetMessages()
        {
            var user = _contextProfiles.Profiles.Find(_ => _.Login == User.Identity.Name).FirstOrDefault();
            
            if (user == null)
                return BadRequest();
            
            string id = user.Id;
            try
            {
                var messages = _context.Messages.Find(_ => _.RecipientId == id).ToListAsync();
                
                if (messages == null)
                    return NotFound();
                
                return Ok(messages);
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }
    }
}