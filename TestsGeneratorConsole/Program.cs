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
            //using (StreamReader fs = new StreamReader("test.cs"))
            //{
            //    //var t = new Generator();
            //    //t.GenerateUnitTestClass(fs.ReadToEnd());
            //    //Console.ReadKey();
            //}
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

            //var t = new Generator();
            //t.CreateClass();
            //Console.ReadKey();
        }
    }
}
