using Sharpenter.ResumeParser.Model;
using Sharpenter.ResumeParser.Model.Models;
using System.Collections.Generic;
using System.Linq;

namespace Sharpenter.ResumeParser.ResumeProcessor.Parsers
{
    public class SkillsParser : IParser
    {        
        public void Parse(Section section, Resume resume)
        {
            resume.Skills = new List<string>();

            foreach (var line in section.Content)
            {
                var indexOfColon = line.IndexOf(':');
                var skills = indexOfColon > -1 ? line.Substring(indexOfColon + 1) : line;
                var elements = skills.Split(',');
                resume.Skills.AddRange(elements.Select(e => e.Trim()));
            }
        }
    }
}
