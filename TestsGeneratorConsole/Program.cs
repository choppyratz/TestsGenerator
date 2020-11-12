using System;
using TestsGenerator;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace TestsGeneratorConsole
{
    class Program
    {
        static async Task Main(string[] args)
        {
            List<string> files = new List<string>();
            files.Add("test.cs");

            string resultDirectoryPath = "";
            DataFlowPipeline pipeline = new DataFlowPipeline("C:/Users/German/Desktop/res");
            pipeline.SetPipelineParametrs(2, 2, 3);
            await pipeline.Processing(files);
            //string readedString = " ";
            //while (true)
            //{
            //    Console.Write("Write file name: ");
            //    readedString = Console.ReadLine();
            //    if (readedString == "")
            //    {
            //        break;
            //    }
            //    files.Add(readedString);
            //}

            //Console.Write("Write final directory path: ");
            //resultDirectoryPath = Console.ReadLine();

            //Console.ReadKey();
        }
    }
}
