using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace TestsGeneratorConsole
{
    class PipelineIO
    {
        public string FileName { get; }
        public string FolderPath { get; }
        public string FileContent { get; set; }

        public PipelineIO(string folderPath, string filePath)
        {
            FolderPath = folderPath;
            FileName = "UintTest" + Path.GetFileName(filePath);
        }

        public async Task ReadFromFile(string filePath)
        {
            using (StreamReader reader = new StreamReader(filePath))
            {
                FileContent = await reader.ReadToEndAsync();
            }
        }

        public async Task Write()
        {
            using (StreamWriter writer = new StreamWriter(FolderPath + "/" + FileName))
            {
                await writer.WriteAsync(FileContent);
            }
        }
    }
}
