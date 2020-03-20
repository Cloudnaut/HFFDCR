namespace HFFDCR.DbContext.Models
{
    public class FileBlock
    {
        public long Id { get; set; }
        public long FileId { get; set; }
        public long Number { get; set; }
        public byte[] Content { get; set; }
        public string Checksum { get; set; }
    }
}