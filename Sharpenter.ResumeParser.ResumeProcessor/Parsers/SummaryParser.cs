using Sharpenter.ResumeParser.Model;
using Sharpenter.ResumeParser.Model.Models;

namespace Sharpenter.ResumeParser.ResumeProcessor.Parsers
{
    public class SummaryParser : IParser
    {
        public void Parse(Section section, Resume resume)
        {
            resume.SummaryDescription = string.Join("; ", section.Content);
        }
    }
}
