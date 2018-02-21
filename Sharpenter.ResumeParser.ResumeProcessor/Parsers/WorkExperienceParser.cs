using System;
using System.Linq;
using System.Text.RegularExpressions;
using Sharpenter.ResumeParser.Model;
using Sharpenter.ResumeParser.Model.Models;
using Sharpenter.ResumeParser.ResumeProcessor.Helpers;
using System.Collections.Generic;
using System.Reflection;

namespace Sharpenter.ResumeParser.ResumeProcessor.Parsers
{
    public class WorkExperienceParser : IParser
    {
        private static readonly Regex SplitByWhiteSpaceRegex = new Regex(@"\s+", RegexOptions.Compiled);        
        private readonly List<string> _jobLookUp;
        private readonly List<string> _countryLookUp;        

        public WorkExperienceParser(IResourceLoader resourceLoader)
        {
            var assembly = Assembly.GetExecutingAssembly();

            _jobLookUp = new List<string>(resourceLoader.Load(assembly, "JobTitles.txt", ','));
            _countryLookUp = new List<string>(resourceLoader.Load(assembly, "Countries.txt", '|'));            
        }

        public void Parse(Section section, Resume resume)
        {
            resume.Positions = new List<Position>();

            var i = 0;
            Position currentPosition = null;
            while (i < section.Content.Count)
            {
                var line = section.Content[i];
                var title = FindJobTitle(line);
                if (string.IsNullOrWhiteSpace(title))
                {
                    if (currentPosition != null)
                    {
                        var startAndEndDate = DateHelper.ParseStartAndEndDate(line);                        
                        if (startAndEndDate != null)
                        {
                            currentPosition.StartDate = startAndEndDate.Start;
                            currentPosition.EndDate = startAndEndDate.End;
                        }
                        else
                        {
                            var country =
                                    _countryLookUp.FirstOrDefault(
                                        c => line.IndexOf(c, StringComparison.InvariantCultureIgnoreCase) > -1);
                            if (country == null)
                            {
                                currentPosition.Summary.Add(line);
                            }
                            else
                            {
                                currentPosition.Company = line.Substring(0, line.IndexOf(country) + country.Length);
                            }                            
                        }                        
                    }
                }
                else
                {
                    currentPosition = new Position
                    {
                        Title = title
                    };

                    resume.Positions.Add(currentPosition);
                }

                i++;
            }            
        }

        private string FindJobTitle(string line)
        {
            var elements = SplitByWhiteSpaceRegex.Split(line);
            if (elements.Length > 4)
            {
                return string.Empty;
            }

            return _jobLookUp.FirstOrDefault(job => line.IndexOf(job, StringComparison.InvariantCultureIgnoreCase) > -1);
        }
    }
}
