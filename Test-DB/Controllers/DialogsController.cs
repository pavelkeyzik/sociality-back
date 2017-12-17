using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using Newtonsoft.Json;
using Test_DB.Models;

namespace Test_DB.Controllers
{
    [Route("[controller]")]
    public class DialogsController : Controller
    {
        private readonly DialogContext _context = null;
        private readonly ProfileContext _contextProfile = null;
        private IHostingEnvironment _env;

        public DialogsController(IOptions<Settings> settings, IHostingEnvironment env)
        {
            _env = env;
            _context = new DialogContext(settings);
            _contextProfile = new ProfileContext(settings);
        }
        
        [Authorize]
        [HttpGet]
        public IActionResult GetDialogs()
        {
            var user = _contextProfile.Profiles.Find(_ => _.Login == User.Identity.Name).FirstOrDefault();
            
            if (user == null)
                return BadRequest();
            
//            string id = user.Id;
            try
            {
//                var koko = ObjectId.Parse("5a34ea801f265bc318e74753").ToString();
                string koka = user.Id;
                var dialogs = _context.Dialogs.Find(_ => _.MeId == user.Id).ToListAsync();

//                var lol = JsonSerializer(dialogs);
                
                if (dialogs == null)
                    return NotFound();
                
                return Ok(dialogs);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
    }
}