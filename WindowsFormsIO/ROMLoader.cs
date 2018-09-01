using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace WindowsFormsApp
{
    class ROMLoader
    {
        private readonly List<string> filePaths;

        public ROMLoader(string romDirectoryPath)
        {
            filePaths = "hgfe".Select(rl => Path.Combine(romDirectoryPath, "invaders." + rl)).ToList();
        }

        public byte[] Load()
        {
            byte[] buffer = new byte[0x10000];

            for (var i = 0; i < filePaths.Count; i++)
            {
                var filePath = filePaths[i];
                var fileContent = File.ReadAllBytes(filePath);
                Array.Copy(fileContent, 0, buffer, 0x800 * i, fileContent.Length);
            }
            return buffer;
        }
    }
}