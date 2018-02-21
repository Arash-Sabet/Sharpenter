using Sharpenter.ResumeParser.Model;
using Sharpenter.ResumeParser.Model.Models;

namespace Sharpenter.ResumeParser.ResumeProcessor.Parsers
{
    public interface IParser
    {
        void Parse(Section section, Resume resume);
    }
}
