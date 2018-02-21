using System;

namespace Sharpenter.ResumeParser.Model.Exceptions
{
    public class ResumeParserException : ApplicationException
    {
        public ResumeParserException(string message)
            :base(message)
        {            
        }

        public ResumeParserException(string message, Exception innerException)
            :base(message, innerException)
        {            
        }
    }
}
