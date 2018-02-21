using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Sharpenter.ResumeParser.Model;

namespace Sharpenter.ResumeParser.ResumeProcessor
{
    internal class InputReaderFactory : IInputReaderFactory
    {
        private readonly IApplicationSettings _applicationSettings;

        public InputReaderFactory(IApplicationSettings applicationSettings)
        {
            _applicationSettings = applicationSettings;
        }

        public IInputReader LoadInputReaders()
        {
            var parserLocation = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _applicationSettings.InputReaderLocation);

            var parsers = new List<IInputReader>();
            foreach (var dll in Directory.GetFiles(parserLocation, "*.dll", SearchOption.AllDirectories))
            {
                try
                {
                    var loadedAssembly = Assembly.LoadFile(dll);
                    var instances = from t in loadedAssembly.GetTypes()
                                    where t.GetInterfaces().Contains(typeof(IInputReader))
                                    select Activator.CreateInstance(t) as IInputReader;
                    parsers.AddRange(instances);
                }
                catch (FileLoadException)
                {
                    // The Assembly has already been loaded, ignore  
                }
                catch (BadImageFormatException)
                {
                    // If a BadImageFormatException exception is thrown, the file is not an assembly, ignore    
                } 

            }

            if (!parsers.Any())
            {
                throw new ApplicationException("No parsers registered");
            }

            for (var i = 0; i < parsers.Count - 1; i++)
            {
                parsers[i].NextReader = parsers[i + 1];
            }

            return parsers[0];
        }
    }
}
