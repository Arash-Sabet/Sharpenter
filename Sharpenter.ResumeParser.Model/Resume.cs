using System.Collections.Generic;

namespace Sharpenter.ResumeParser.Model
{
    public class Resume
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumbers { get; set; }
        public string Languages { get; set; }
        public string SummaryDescription { get; set; }
        public List<string> Skills { get; set; }
        public string Location { get; set; }
        public List<Position> Positions { get; set; }
        public List<Project> Projects { get; set; }
        public List<string> SocialProfiles { get; set; }
        public List<Education> Educations { get; set; }
        public List<string> Courses { get; set; }
        public List<string> Awards { get; set; }

        public Resume()
        {
            Skills = new List<string>();
            Positions = new List<Position>();
            Projects = new List<Project>();
            SocialProfiles = new List<string>();
            Educations = new List<Education>();
            Courses = new List<string>();
            Awards = new List<string>();
        }
    }
}
