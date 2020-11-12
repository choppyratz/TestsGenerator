using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using TestsGenerator;

namespace TestsGeneratorConsole
{
    class DataFlowPipeline
    {
        private int readCount;
        private int generateCount;
        private int writeCount;
        private string FileFolder;

        public void SetPipelineParametrs(int _readCount, int _generateCount, int _writeCount)
        {
            readCount = _readCount;
            generateCount = _generateCount;
            writeCount = _writeCount;
        }

        public DataFlowPipeline(string fileFolder)
        {
            FileFolder = fileFolder;
        }

        public async Task Processing(List<string> filesPath)
        {
            var readingBlock = new TransformBlock<string, PipelineIO>(
                async filePath => await ReadFileAsync(filePath),
                new ExecutionDataflowBlockOptions
                {
                    MaxDegreeOfParallelism = readCount
                });
            var generatingBlock = new TransformBlock<PipelineIO, PipelineIO>(
                async file => await GenerateTestAsync(file),
                new ExecutionDataflowBlockOptions
                {
                    MaxDegreeOfParallelism = generateCount
                });
            var writingBlock = new ActionBlock<PipelineIO>(
                async file => await WriteFileAsync(file),
                new ExecutionDataflowBlockOptions
                {
                    MaxDegreeOfParallelism = writeCount
                });

            readingBlock.LinkTo(generatingBlock, new DataflowLinkOptions { PropagateCompletion = true });
            generatingBlock.LinkTo(writingBlock, new DataflowLinkOptions { PropagateCompletion = true });

            for (int i = 0; i < filesPath.Count; i++)
            {
                readingBlock.Post(filesPath[i]);
            }
            readingBlock.Complete();

            await writingBlock.Completion;
        }

        private async Task<PipelineIO> ReadFileAsync(string filePath)
        {
            PipelineIO file = new PipelineIO(FileFolder, filePath);
            await file.ReadFromFile(filePath);
            return file;
        }

        private async Task<PipelineIO> GenerateTestAsync(PipelineIO file)
        {
            file.FileContent = await new Generator().GenerateUnitTestClassAsync(file.FileContent);
            return file;
        }

        private async Task WriteFileAsync(PipelineIO file)
        {
            await file.Write();
        }
    }
}
