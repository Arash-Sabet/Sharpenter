using System.Collections.Generic;
using Sharpenter.ResumeParser.Model;
using Sharpenter.ResumeParser.Model.Models;
using Sharpenter.ResumeParser.ResumeProcessor.Helpers;

namespace Sharpenter.ResumeParser.ResumeProcessor.Parsers
{
    public class ProjectsParser : IParser
    {        
        public void Parse(Section section, Resume resume)
        {
            resume.Projects = new List<Project>();

            var i = 0;
            Project currentProject = null;
            while (i < section.Content.Count)
            {
                var line = section.Content[i];
                var startAndEndDate = DateHelper.ParseStartAndEndDate(line);
                if (startAndEndDate == null)
                {
                    if (currentProject != null)
                    {
                        currentProject.Summary.Add(line);
                    }
                    else if (line.IndexOf(':') > -1)
                    {
                        var elements = line.Split(':');
                        var project = new Project
                        {
                            Title = elements[0]                            
                        };
                        project.Summary.Add(elements[1]);

                        resume.Projects.Add(project);
                    }
                }
                else
                {
                    currentProject = new Project
                    {
                        StartDate = startAndEndDate.Start,
                        EndDate = startAndEndDate.End,
                        Title = line.Substring(0, line.IndexOf(startAndEndDate.Start)).Trim()
                    };

                    resume.Projects.Add(currentProject);
                }

                i++;
            }
        }
    }
}
