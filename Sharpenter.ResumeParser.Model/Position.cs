using System.Collections.Generic;

namespace Sharpenter.ResumeParser.Model
{
    public class Position
    {
        public string Title { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Company { get; set; }        
        public List<string> Summary { get; set; }        

        public Position()
        {
            Summary = new List<string>();
        }
    }
}