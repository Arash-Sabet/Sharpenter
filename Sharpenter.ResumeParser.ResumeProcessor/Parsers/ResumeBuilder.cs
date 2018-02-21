using System.Collections.Generic;
using System.Linq;
using Sharpenter.ResumeParser.Model;
using Sharpenter.ResumeParser.Model.Models;
using Sharpenter.ResumeParser.ResumeProcessor.Helpers;

namespace Sharpenter.ResumeParser.ResumeProcessor.Parsers
{
    public class ResumeBuilder
    {
        private readonly Dictionary<SectionType, dynamic> _parserRegistry;
        public ResumeBuilder(IResourceLoader resourceLoader)
        {
            _parserRegistry = new Dictionary<SectionType, dynamic>
            {
                {SectionType.Personal, new PersonalParser(resourceLoader)},
                {SectionType.Summary, new SummaryParser()},
                {SectionType.Education, new EducationParser()},
                {SectionType.Projects, new ProjectsParser()},
                {SectionType.WorkExperience, new WorkExperienceParser(resourceLoader)},
                {SectionType.Skills, new SkillsParser()},
                {SectionType.Courses, new CoursesParser()},
                {SectionType.Awards, new AwardsParser()}
            };
        }
        
        public Resume Build(IList<Section> sections)
        {
            var resume = new Resume();

            foreach (var section in sections.Where(section => _parserRegistry.ContainsKey(section.Type)))
            {
                _parserRegistry[section.Type].Parse(section, resume);
            }

            return resume;
        }
    }
}
