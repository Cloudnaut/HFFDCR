using System;
using System.Collections.Generic;
using System.Linq;
using HFFDCR.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace HFFDCR.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FileController : ControllerBase
    {
        private readonly ILogger<FileController> _logger;
        private readonly HffdcrDbContext _db;

        public FileController(ILogger<FileController> logger, HffdcrDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        [HttpGet]
        public IEnumerable<File> List() => _db.Files;

        [HttpGet("{fileId}")]
        public File Get([FromRoute] long fileId) => _db.Files.First(f => f.Id == fileId);

        [HttpPost]
        public File Add([FromBody] File file)
        {
            File createdFile = _db.Files.Add(new File()
            {
                Name = file.Name,
                BlockSize = file.BlockSize,
                SizeInBytes = file.SizeInBytes
            }).Entity;

            _db.SaveChanges();

            return createdFile;
        }

        [HttpDelete]
        public File Delete(long fileId)
        {
            try
            {
                File deletedFile = _db.Files.Remove(_db.Files.First(f => f.Id == fileId)).Entity;
                _db.SaveChanges();
                return deletedFile;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}