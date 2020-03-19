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
    [Route("[controller]")]
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
        public IEnumerable<Checksum> List() => _db.Checksums;

        [HttpGet("{checksumId}")]
        public Checksum Get([FromRoute] long fileId) => _db.Checksums.First(f => f.Id == fileId);

        [HttpPost]
        public Checksum Add([FromBody] Checksum checksum)
        {
            Checksum createdChecksum = _db.Checksums.Add(new Checksum()
            {
                FileBlockId = checksum.FileBlockId,
                Value = checksum.Value
            }).Entity;

            _db.SaveChanges();

            return createdChecksum;
        }

        [HttpDelete("{checksumId}")]
        public Checksum Delete([FromRoute] long checksumId)
        {
            try
            {
                Checksum deletedChecksum = _db.Checksums.Remove(_db.Checksums.First(c => c.Id == checksumId)).Entity;
                _db.SaveChanges();
                return deletedChecksum;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}