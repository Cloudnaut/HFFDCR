namespace HFFDCR.Core.Models
{
    public class File
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public ulong BlockSize { get; set; }
        public ulong SizeInBytes { get; set; }
    }
}