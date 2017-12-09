using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using MongoDB.Driver.Core.WireProtocol.Messages;
using Test_DB.Models;

namespace Test_DB.Controllers
{
    [Route("[controller]")]
    public class ValuesController : Controller
    {
        private readonly UserContext _context = null;
        
        public ValuesController(IOptions<Settings> settings)
        {
            _context = new UserContext(settings);
        }
        // GET values
        [Authorize(Roles="admin")]
        [HttpGet]
        public async Task<IEnumerable<User>> GetAllNotes()
        {
            try
            {
                return await _context.Users
                    .Find(_ => true).ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // GET values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // PUT values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}