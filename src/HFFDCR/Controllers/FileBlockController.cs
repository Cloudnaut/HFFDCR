using System;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using HFFDCR.Core;
using HFFDCR.Core.Models;
using HFFDCR.DbContext;
using HFFDCR.DbContext.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace HFFDCR.Controllers
{
    [ApiController]
    [Route("/File/{fileId}/[controller]")]
    public class FileBlockController : ControllerBase
    {
        private readonly ILogger<FileBlockController> _logger;
        private readonly HffdcrDbContext _db;

        public FileBlockController(ILogger<FileBlockController> logger, HffdcrDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        [HttpGet("{number}")]
        public FileBlockInfo Get([FromRoute] long fileId, [FromRoute] long number)
        {
            DbContext.Models.FileBlock fileBlock = _db.FileBlocks.First(fb => fb.FileId == fileId && fb.Number == number);
            
            return new FileBlockInfo()
            {
                Number = fileBlock.Number,
                Value = Convert.ToBase64String(fileBlock.Content)
            };
        }

        [HttpPost]
        public IActionResult Add([FromRoute] long fileId, [FromBody] FileBlockInfo fileBlockInfo)
        {
            FileBlock fileBlock = _db.FileBlocks.FirstOrDefault(fb => fb.FileId == fileId && fb.Number == fileBlockInfo.Number);
            
            using (MD5 md5Hash = MD5.Create())
            {
                byte[] rawContent = Convert.FromBase64String(fileBlockInfo.Value);
                
                if (fileBlock == null) //Create new fileBlock
                {
                    _db.FileBlocks.Add(new FileBlock()
                    {
                        FileId = fileId,
                        Number = fileBlockInfo.Number,
                        Content = rawContent,
                        Checksum = MD5Utils.GetMd5Hash(md5Hash, rawContent.ToString())
                    });
                }
                else //Update existing fileBlock
                {
                    fileBlock.Content = rawContent;
                    fileBlock.Checksum = MD5Utils.GetMd5Hash(md5Hash, rawContent.ToString());
                    _db.FileBlocks.Update(fileBlock);
                }

                _db.SaveChanges();
            }

            return StatusCode((int) HttpStatusCode.OK);
        }

        [HttpDelete("{number}")]
        public IActionResult Delete([FromRoute] long fileId, [FromRoute] long number)
        {
            try
            {
                _db.FileBlocks.Remove(_db.FileBlocks.First(fb => fb.FileId == fileId && fb.Number == number));
                _db.SaveChanges();

                return StatusCode((int) HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}