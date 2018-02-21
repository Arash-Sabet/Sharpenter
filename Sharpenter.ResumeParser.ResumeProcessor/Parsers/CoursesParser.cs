using Sharpenter.ResumeParser.Model;
using Sharpenter.ResumeParser.Model.Models;

namespace Sharpenter.ResumeParser.ResumeProcessor.Parsers
{
    public class CoursesParser : IParser
    {        
        public void Parse(Section section, Resume resume)
        {
            resume.Courses = section.Content;
        }
    }
}
