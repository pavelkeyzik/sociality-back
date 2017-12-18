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
        private readonly DialogContext _contextDialogs = null;
        private IHostingEnvironment _env;

        public MessagesController(IOptions<Settings> settings, IHostingEnvironment env)
        {
            _env = env;
            _context = new MessageContext(settings);
            _contextProfiles = new ProfileContext(settings);
            _contextDialogs = new DialogContext(settings);
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

                var messageDate = DateTime.UtcNow;
                
                var dialogMe = _contextDialogs.Dialogs.Find(_ => _.MeId == user.Id && _.FriendId == friendId).FirstOrDefault();
                var dialogFriend = _contextDialogs.Dialogs.Find(_ => _.MeId == friendId && _.FriendId == user.Id).FirstOrDefault();

                bool dialog1 = false, dialog2 = false;
                
                var unreadedMe = 0;
                if (dialogMe != null)
                {
                    dialog1 = true;
                }

                var unreadedFriend = 0;
                if (dialogFriend != null)
                {
                    dialog2 = true;
                    unreadedFriend = dialogFriend.Unreaded;
                    unreadedFriend += 1;
                }

                var dialogForMe = new
                {
                    meId = user.Id,
                    friendId = friendId,
                    lastMessage = message.MessageText,
                    dateMessage = messageDate,
                    unreaded = unreadedMe
                };
                
                var dialogForFriend = new
                {
                    meId = friendId,
                    friendId = user.Id,
                    lastMessage = message.MessageText,
                    dateMessage = messageDate,
                    unreaded = unreadedFriend
                };

                var sukaMe = dialogForMe.ToBsonDocument();
                var setMe = "{ $set : " + sukaMe + " }";

                var sukaFriend = dialogForFriend.ToBsonDocument();
                var setFriend = "{ $set : " + sukaFriend + " }";

                if (dialog1 || dialog2)
                {
                    if(dialog1)
                        _contextDialogs.Dialogs.FindOneAndUpdate(_ => _.MeId == user.Id, setMe);
                    if(dialog2)
                        _contextDialogs.Dialogs.FindOneAndUpdate(_ => _.FriendId == user.Id, setFriend);
                }
                else
                {
                    var lolMe = new Dialog();
                    lolMe.MeId = dialogForMe.meId;
                    lolMe.FriendId = dialogForMe.friendId;
                    lolMe.LastMessage = dialogForMe.lastMessage;
                    lolMe.DateMessage = dialogForMe.dateMessage;
                    lolMe.Unreaded = 0;
                    _contextDialogs.Dialogs.InsertOne(lolMe);
                    
                    var lolFriend = new Dialog();
                    lolFriend.MeId = dialogForFriend.meId;
                    lolFriend.FriendId = dialogForFriend.friendId;
                    lolFriend.LastMessage = dialogForFriend.lastMessage;
                    lolFriend.DateMessage = dialogForFriend.dateMessage;
                    lolFriend.Unreaded = 1;
                    
                    _contextDialogs.Dialogs.InsertOne(lolFriend);
                }
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }

        [Authorize]
        [HttpGet("read/{friendId}")]
        public IActionResult ReadMessage(string friendId)
        {
            try
            {
                var user = _contextProfiles.Profiles.Find(_ => _.Login == User.Identity.Name).FirstOrDefault();
                var set = "{ $set : { 'unreaded': 0 } }";
                _contextDialogs.Dialogs.FindOneAndUpdate(_ => _.MeId == user.Id && _.FriendId == friendId, set);
            }
            catch
            {
                return NotFound();
            }
            return Ok("[]");
        }
    }
}