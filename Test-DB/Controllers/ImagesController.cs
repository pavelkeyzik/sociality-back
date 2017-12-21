using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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
                Byte[] b = System.IO.File.ReadAllBytes(_env.ContentRootPath + "/wwwroot/images/" + image.Url);
                return File(b, "image/jpeg");
            }
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult UploadImage()
        {
            IFormFile myFile = Request.Form.Files[0];
            if (myFile != null)
            {
                try
                {
                    string currentDate = DateTime.Now.ToString("dd-MM-yyyyTHH:mm:ss");
                    string fileName = currentDate + myFile.FileName;
                    var path = Path.Combine(_env.WebRootPath, "images", fileName);
                    FileStream fs = System.IO.File.Create(path);
                    myFile.CopyToAsync(fs);

                    var image = new Image()
                    {
                        Url = fileName,
                        Title = myFile.FileName
                    };
                    _context.Images.InsertOne(image);
                }
                catch (Exception e)
                {
                    return BadRequest(e);
                }
            }
            return Ok("[]");
        }
    }
}