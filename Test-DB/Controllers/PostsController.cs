using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Test_DB.Models;

namespace Test_DB.Controllers
{
    [Route("[controller]")]
    public class PostsController : Controller
    {
        private readonly ProfileContext _contextProfile = null;
        private readonly PostContext _context = null;
        private IHostingEnvironment _env;
    
        public PostsController(IOptions<Settings> settings, IHostingEnvironment env)
        {
            _env = env;
            _context = new PostContext(settings);
            _contextProfile = new ProfileContext(settings);
        }

        [Authorize]
        [HttpGet("{id}")]
        public IActionResult GetPosts(string id)
        {
            var user = _contextProfile.Profiles.Find(_ => _.Id == id).FirstOrDefault();
            
            if (user == null)
                return BadRequest();
            
            try
            {
                var posts = _context.Posts.Find(_ => _.MeId == user.Id).ToListAsync();
                
                if (posts == null)
                    return NotFound();
                
                return Ok(posts);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [Authorize]
        [HttpPost]
        public IActionResult AddPost([FromBody] Post post)
        {
            var user = _contextProfile.Profiles.Find(_ => _.Login == User.Identity.Name).FirstOrDefault();
            if (user == null) return BadRequest();

            try
            {
                post.MeId = user.Id;

                _context.Posts.InsertOne(post);
                return Ok("[]");
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [Authorize]
        [HttpDelete("{id}")]
        public IActionResult RemovePost(string id)
        {
            if (id == null) return BadRequest();
            
            var user = _contextProfile.Profiles.Find(_ => _.Login == User.Identity.Name).FirstOrDefault();
            
            if (user == null) return NotFound();
            try
            {
                _context.Posts.FindOneAndDelete(_ => _.MeId == user.Id && _.Id == id);
                return Ok("[]");
            }
            catch (Exception e)
            {
                return NotFound(e);
            }
        }
    }
}