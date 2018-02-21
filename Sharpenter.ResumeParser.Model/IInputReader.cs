using System.Collections.Generic;

namespace Sharpenter.ResumeParser.Model
{
    public interface IInputReader
    {
        IInputReader NextReader { get; set; }
        IList<string> ReadIntoList(string location);        
    }
}
