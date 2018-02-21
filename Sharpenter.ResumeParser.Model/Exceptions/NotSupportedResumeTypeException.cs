using System;

namespace Sharpenter.ResumeParser.Model.Exceptions
{
    public class NotSupportedResumeTypeException : ResumeParserException
    {
        public NotSupportedResumeTypeException(string message)
            :base(message)
        {            
        }

        public NotSupportedResumeTypeException(string message, Exception innerException)
            :base(message, innerException)
        {            
        }
    }
}
