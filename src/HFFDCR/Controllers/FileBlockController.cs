using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using HFFDCR.Core;
using HFFDCR.Core.Models;
using HFFDCR.DbContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace HFFDCR.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FileBlockController : ControllerBase
    {
        private readonly ILogger<FileBlockController> _logger;
        private readonly HffdcrDbContext _db;

        public FileBlockController(ILogger<FileBlockController> logger, HffdcrDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        [HttpGet("{fileBlockId}")]
        public FileBlock Get([FromRoute] long fileBlockId)
        {
            DbContext.Models.FileBlock fileBlock = _db.FileBlocks.First(fb => fb.Id == fileBlockId);
            
            return new FileBlock()
            {
                Id = fileBlock.Id,
                FileId = fileBlock.FileId,
                Number = fileBlock.Number,
                Content = Convert.ToBase64String(fileBlock.Content)
            };
        }

        [HttpPost]
        public FileBlock Add([FromBody] FileBlock fileBlock)
        {
            DbContext.Models.FileBlock createdFileBlock;
            
            using (MD5 md5Hash = MD5.Create())
            {
                createdFileBlock = _db.FileBlocks.Add(new DbContext.Models.FileBlock()
                {
                    FileId = fileBlock.FileId,
                    Number = fileBlock.Number,
                    Content = Convert.FromBase64String(fileBlock.Content)
                }).Entity;
                
                _db.Checksums.Add(new Checksum()
                {
                    FileBlockId = createdFileBlock.Id,
                    Value = MD5Utils.GetMd5Hash(md5Hash, fileBlock.Content)
                });
            }
            _db.SaveChanges();
            
            
            return new FileBlock()
            {
                Id = createdFileBlock.Id,
                FileId = createdFileBlock.FileId,
                Number = createdFileBlock.Number,
                Content = Convert.ToBase64String(createdFileBlock.Content)
            };
        }

        [HttpDelete("{fileBlockId}")]
        public FileBlock Delete([FromRoute] long fileBlockId)
        {
            try
            {
                DbContext.Models.FileBlock deletedFileBlock = _db.FileBlocks.Remove(_db.FileBlocks.First(fb => fb.Id == fileBlockId)).Entity;
                _db.SaveChanges();

                return new FileBlock()
                {
                    Id = deletedFileBlock.Id,
                    FileId = deletedFileBlock.FileId,
                    Number = deletedFileBlock.Number,
                    Content = null
                };
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}