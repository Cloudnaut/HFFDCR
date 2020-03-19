namespace HFFDCR.Core.Models
{
    public class Checksum
    {
        public long Id { get; set; }
        public long FileBlockId { get; set; }
        public string Value { get; set; }
    }
}