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
    public class FileBlockController : ControllerBase
    {
        private readonly ILogger<FileBlockController> _logger;
        private readonly HffdcrDbContext _db;

        public FileBlockController(ILogger<FileBlockController> logger, HffdcrDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        [HttpGet]
        public IEnumerable<FileBlock> List() => _db.FileBlocks;

        [HttpGet("{fileBlockId}")]
        public FileBlock Get([FromRoute] long fileBlockId) => _db.FileBlocks.First(fb => fb.Id == fileBlockId);

        [HttpPost]
        public FileBlock Add([FromBody] FileBlock fileBlock)
        {
            FileBlock createdFileBlock = _db.FileBlocks.Add(new FileBlock()
            {
                FileId = fileBlock.FileId,
                Number = fileBlock.Number,
                Content = fileBlock.Content,
                Checksum = null
            }).Entity;

            _db.SaveChanges();

            return createdFileBlock;
        }

        [HttpDelete]
        public FileBlock Delete(long fileBlockId)
        {
            try
            {
                FileBlock deletedFileBlock = _db.FileBlocks.Remove(_db.FileBlocks.First(fb => fb.Id == fileBlockId)).Entity;
                _db.SaveChanges();
                return deletedFileBlock;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}