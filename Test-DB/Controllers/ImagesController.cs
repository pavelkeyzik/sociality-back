using System;
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
    public class ImagesController : Controller
    {
        private readonly ImageContext _context = null;
        private IHostingEnvironment _env;
        
        public ImagesController(IOptions<Settings> settings, IHostingEnvironment env)
        {
            _env = env;
            _context = new ImageContext(settings);
        }
        [AllowAnonymous]
        [HttpGet]
        public IActionResult GetImage(string id)
        {
            if(id == null){ return NotFound(); }
            else
            {
                var image = _context.Images.Find(_ => _.Id == ObjectId.Parse(id)).FirstOrDefault();
                Byte[] b = System.IO.File.ReadAllBytes(_env.ContentRootPath + "/wwwroot/" + image.Url);
                return File(b, "image/jpeg");
            }
        }
    }
}