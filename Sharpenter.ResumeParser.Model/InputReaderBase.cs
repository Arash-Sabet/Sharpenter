using System.Collections.Generic;
using Sharpenter.ResumeParser.Model.Exceptions;

namespace Sharpenter.ResumeParser.Model
{
    public abstract class InputReaderBase : IInputReader
    {
        public IInputReader NextReader { get; set; }

        public IList<string> ReadIntoList(string location)
        {
            if (CanHandle(location))
            {
                return Handle(location);
            }

            if (NextReader != null)
            {
                return NextReader.ReadIntoList(location);
            }

            throw new NotSupportedResumeTypeException("No reader registered for this type of resume: " + location);
        }

        protected abstract bool CanHandle(string location);
        protected abstract IList<string> Handle(string location);        
    }
}
