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
        [HttpGet("{friendId}")]
        public IActionResult GetMessages(string friendId)
        {
            var user = _contextProfiles.Profiles.Find(_ => _.Login == User.Identity.Name).FirstOrDefault();
            
            if (user == null)
                return BadRequest();
            
            string id = user.Id;
            try
            {
                var messages = _context.Messages.Find(_ => _.RecipientId == id && _.FriendId == friendId).ToListAsync();
                
                if (messages == null)
                    return NotFound();
                
                return Ok(messages);
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }

        [Authorize]
        [HttpPost("{friendId}")]
        public IActionResult SendMessage(string friendId, [FromBody] Message message)
        {
            var user = _contextProfiles.Profiles.Find(_ => _.Login == User.Identity.Name).FirstOrDefault();
            
            if (user == null)
                return BadRequest();
            
            try
            {
                var messageForMe = new Message()
                {
                    AuthorId = user.Id,
                    RecipientId = user.Id,
                    FriendId = friendId,
                    Type = message.Type,
                    MessageText = message.MessageText
                };

                var messageForFriend = message;
                messageForFriend.AuthorId = user.Id;
                messageForFriend.RecipientId = friendId;
                messageForFriend.FriendId = user.Id;
                
                _context.Messages.InsertOne(messageForMe);
                _context.Messages.InsertOne(messageForFriend);
                
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }
    }
}