using System;
using System.IO;
using System.Linq;
using HFFDCR.Core.Models;
using File = System.IO.File;

namespace Client
{
    class Program
    {
        private static HFFDCRClient _hffdcrClient = new HFFDCRClient("https://localhost:5001");
        
        static void Main(string[] args)
        {
            string path = @"<filepath>";

            HFFDCR.Core.Models.File file = _hffdcrClient.GetFiles().First(f => f.Name == Path.GetFileName(path));

            FileStream sourceFileStream = File.OpenRead(path);
            long fileSizeInBytes = sourceFileStream.Length;
            long blockSizeInBytes = (long) file.BlockSizeInBytes;
            long blockCount = (long) Math.Ceiling((double) fileSizeInBytes / blockSizeInBytes);

            byte[] buffer = new byte[blockSizeInBytes];
            for (long currentBlock = 0; currentBlock < blockCount; currentBlock++)
            {
                sourceFileStream.Seek(currentBlock * blockSizeInBytes, SeekOrigin.Begin);
                sourceFileStream.Read(buffer, 0, (int) blockSizeInBytes);
                _hffdcrClient.SetFileBlockContent(file.Id, new FileBlockInfo()
                {
                    Number = currentBlock,
                    Value = Convert.ToBase64String(buffer)
                });
            }
            
            Console.WriteLine("Hello World!");
        }
    }
}