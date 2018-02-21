using System;
using System.IO;
using Sharpenter.ResumeParser.Model;
using Sharpenter.ResumeParser.Model.Exceptions;
using Sharpenter.ResumeParser.ResumeProcessor.Parsers;
using Sharpenter.ResumeParser.ResumeProcessor.Helpers;

namespace Sharpenter.ResumeParser.ResumeProcessor
{
    public class ResumeProcessor
    {
        private readonly IOutputFormatter _outputFormatter;
        private readonly IInputReader _inputReaders; 

        public ResumeProcessor(IOutputFormatter outputFormatter)
        {
            if (outputFormatter == null)
            {
                throw new ArgumentNullException("outputFormatter");    
            }

            _outputFormatter = outputFormatter;
            IInputReaderFactory inputReaderFactory = new InputReaderFactory(new ConfigFileApplicationSettingsAdapter());
            _inputReaders = inputReaderFactory.LoadInputReaders();
        }

        public string Process(string location)
        {
            try
            {
                var rawInput = _inputReaders.ReadIntoList(location);

                var sectionExtractor = new SectionExtractor();
                var sections = sectionExtractor.ExtractFrom(rawInput);

                IResourceLoader resourceLoader = new CachedResourceLoader(new ResourceLoader());
                var resumeBuilder = new ResumeBuilder(resourceLoader);
                var resume = resumeBuilder.Build(sections);

                var formatted = _outputFormatter.Format(resume);

                return formatted;
            }
            catch (IOException ex)
            {
                throw new ResumeParserException("There's a problem accessing the file, it might still being opened by other application", ex);
            }            
        }
    }
}
