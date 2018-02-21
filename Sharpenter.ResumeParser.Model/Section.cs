using System.Collections.Generic;
using Sharpenter.ResumeParser.Model.Models;

namespace Sharpenter.ResumeParser.Model
{
    public class Section
    {
        public SectionType Type { get; set; }
        public List<string> Content { get; set; }

        public Section()
        {
            Content = new List<string>();
        }
    }
}
