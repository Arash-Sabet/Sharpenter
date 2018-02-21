using System.Diagnostics;
using System.IO;
using System.Linq;
using Sharpenter.ResumeParser.OutputFormatter.Json;

namespace Sharpenter.ResumeParser.UI.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var outputFolder = Path.Combine(Directory.GetCurrentDirectory(), "Output");
            if (Directory.Exists(outputFolder))
            {                
                Directory.Delete(outputFolder, true);    
            }

            Directory.CreateDirectory(outputFolder);

            var processor = new ResumeProcessor.ResumeProcessor(new JsonOutputFormatter());

            var files = Directory.GetFiles("Resumes").Select(Path.GetFullPath);            
            foreach (var file in files)
            {
                var output = processor.Process(file);

                System.Console.WriteLine(output);

                var outputFileName = file.Substring(file.LastIndexOf(Path.DirectorySeparatorChar) + 1) + ".txt";
                using (var writer = new StreamWriter(Path.Combine(outputFolder, outputFileName)))
                {
                    writer.Write(output);
                }
            }                       
        }
    }
}
