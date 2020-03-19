namespace HFFDCR.Core.Models
{
    public class FileBlock
    {
        public long Id { get; set; }
        public long FileId { get; set; }
        public long Number { get; set; }
        public string Content { get; set; }
    }
}