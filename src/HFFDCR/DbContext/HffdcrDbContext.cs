using HFFDCR.Core.Models;
using Microsoft.EntityFrameworkCore;
using FileBlock = HFFDCR.DbContext.Models.FileBlock;

namespace HFFDCR.DbContext
{
    public class HffdcrDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public HffdcrDbContext()
        {
            Database.EnsureCreated();
        }
        
        public DbSet<File> Files { get; set; }
        public DbSet<FileBlock> FileBlocks { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=hffdcr.db");
        }
    }
}