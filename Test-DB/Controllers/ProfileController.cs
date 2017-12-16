using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
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

        [AllowAnonymous]
        [HttpPut("{login}")]
        public IActionResult Put(string login, [FromBody] Profile value)
        {
            try
            {
                var x = new List<string>();

                if (value.Name != null)
                {
                    x.Add("'name': '" + value.Name + "'");
                }
                if (value.Online != null)
                {
                    x.Add("'online': " + value.Online.ToString().ToLower());
                }
                if (value.Phone != null)
                {
                    x.Add("'phone': '" + value.Phone + "'");
                }
                if (value.Email != null)
                {
                    x.Add("'email': '" + value.Email + "'");
                }
                if (value.Avatar != null)
                {
                    x.Add("'avatar': '" + value.Avatar + "'");
                }
                if (value.Gender != null)
                {
                    x.Add("'gender': '" + value.Gender + "'");
                }
                
                
                var list = x.ToArray();
                StringBuilder sendedParams = new StringBuilder("");
                for (int i = 0; i < list.Length; i++)
                {
                    sendedParams.Append(list[i]);
                    if (i != list.Length - 1)
                        sendedParams.Append(',');
                }
                var set = "{ $set : {" + sendedParams + "} }";
                
                _context.Profiles.FindOneAndUpdate(_ => _.Login == login, set);
                return Ok();
            }
            catch (Exception e)
            {
                return NotFound(e);
            }
        }
    }
}