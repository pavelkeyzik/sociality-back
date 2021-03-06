﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
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
            var image = _context.Images.Find(_ => _.Id == id).FirstOrDefault();
            string filePath = Path.Combine(_env.WebRootPath, "images", image.Url);
            
            using (var stream = System.IO.File.Open(filePath, FileMode.Open)) {
                byte[] b;
                using (BinaryReader br = new BinaryReader(stream))
                {
                    b = br.ReadBytes((int)stream.Length);
                    return File(b, "image/jpeg"); 
                }
            }
        }
        
        [AllowAnonymous]
        [HttpPost]
        public IActionResult UploadImage()
        {
            IFormFile myFile = Request.Form.Files[0];
            string imageId = "";
            if (myFile != null)
            {
                try
                {
                    string currentDate = DateTime.Now.ToString("dd-MM-yyyyTHH:mm:ss");
                    string extension = Path.GetExtension(myFile.FileName);
                    string fileName = currentDate + extension;
                    var path = Path.Combine(_env.WebRootPath, "images", fileName);
                    using (FileStream fs = System.IO.File.Create(path))
                    {
                        myFile.CopyTo(fs);
                    }

                    var image = new Image()
                    {
                        Url = fileName,
                        Title = myFile.FileName
                    };
                    _context.Images.InsertOne(image);
                    imageId = _context.Images.Find(_ => _.Url == image.Url).FirstOrDefault().Id;
                }
                catch (Exception e)
                {
                    return BadRequest(e);
                }
            }
            return Ok(new { imageId = imageId });
        }
    }
}