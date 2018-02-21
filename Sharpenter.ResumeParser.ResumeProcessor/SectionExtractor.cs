using System.Collections.Generic;
using System.Text.RegularExpressions;
using Sharpenter.ResumeParser.Model;
using Sharpenter.ResumeParser.Model.Models;

namespace Sharpenter.ResumeParser.ResumeProcessor
{
    public class SectionExtractor
    {
        private const int SectionTitleNumberOfWordsLimit = 4;
        private static readonly Regex SplitByWhiteSpaceRegex = new Regex(@"\s+", RegexOptions.Compiled);
        private readonly SectionMatchingService _sectionMatchingService;

        public SectionExtractor()
        {
            _sectionMatchingService = new SectionMatchingService();
        }

        public IList<Section> ExtractFrom(IList<string> rawInput)
        {
            var sections = new List<Section>();            

            var i = 0;

            //Assume personal information always at the top
            var personalSection = new Section
            {
                Type = SectionType.Personal
            };

            while (i < rawInput.Count - 1 &&
                   FindSectionType(rawInput[i].ToLower()) == SectionType.Unknown)
            {                
                if (!string.IsNullOrWhiteSpace(rawInput[i]))
                {
                    personalSection.Content.Add(rawInput[i]);
                }

                i++;
            }

            sections.Add(personalSection);

            while (i < rawInput.Count)
            {
                var input = rawInput[i].ToLower();
                var sectionType = FindSectionType(input);
                if (sectionType != SectionType.Unknown)
                {
                    //Starting of a new section
                    var section = new Section
                    {
                        Type = sectionType
                    };

                    while (i < rawInput.Count - 1 &&                         
                        FindSectionType(rawInput[i + 1].ToLower()) == SectionType.Unknown)
                    {
                        i++;

                        if (!string.IsNullOrWhiteSpace(rawInput[i]))
                        {
                            section.Content.Add(rawInput[i]);
                        }                        
                    }

                    sections.Add(section);
                } 

                i++;
            }            

            return sections;
        }

        private SectionType FindSectionType(string input)
        {
            var elements = SplitByWhiteSpaceRegex.Split(input);
            return elements.Length < SectionTitleNumberOfWordsLimit ? _sectionMatchingService.FindSectionTypeMatching(input) : SectionType.Unknown;
        }
    }    
}
