using Sharpenter.ResumeParser.Model;
using Sharpenter.ResumeParser.Model.Models;

namespace Sharpenter.ResumeParser.ResumeProcessor.Parsers
{
    public class AwardsParser : IParser
    {        
        public void Parse(Section section, Resume resume)
        {
            resume.Awards = section.Content;
        }
    }
}
