using System;
using System.Collections.Generic;
using System.Linq;
using HFFDCR.Core.Models;
using HFFDCR.DbContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace HFFDCR.Controllers
{
    [ApiController]
    [Route("File/{fileId}/[controller]/{fileBlockNumber}")]
    public class ChecksumController : ControllerBase
    {
        private readonly ILogger<ChecksumController> _logger;
        private readonly HffdcrDbContext _db;

        public ChecksumController(ILogger<ChecksumController> logger, HffdcrDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        [HttpGet]
        public IEnumerable<Checksum> List([FromRoute] long fileId, [FromRoute] long fileBlockNumber)
        {
            throw new NotImplementedException();
            
            var tst = (
                from c in _db.Checksums
                join fb in _db.FileBlocks
                    on c.FileBlockId equals fb.Id
                    into a
                from b in a.DefaultIfEmpty()
                select new
                {
                    b.Number,
                    c.Value
                }
            );
            
            
            
            return _db.Checksums;
        }
    }
}