using System;
using System.Collections.Generic;
using System.Linq;
using HFFDCR.Core.Models;
using HFFDCR.DbContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HFFDCR.Controllers
{
    [ApiController]
    [Route("File/{fileId}/[controller]")]
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
        public IEnumerable<FileBlockInfo> List([FromRoute] long fileId)
        {
            return _db.FileBlocks.Where(fb => fb.FileId == fileId)
                .OrderBy(fb => fb.Number)
                .Select(fb => new FileBlockInfo()
                {
                    Number = fb.Number,
                    Value = fb.Checksum
                });
        }
    }
}