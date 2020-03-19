using HFFDCR.Models;
using Microsoft.EntityFrameworkCore;

namespace HFFDCR
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