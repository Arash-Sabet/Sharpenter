using System.Text.RegularExpressions;
using Sharpenter.ResumeParser.Model;

namespace Sharpenter.ResumeParser.ResumeProcessor.Helpers
{
    public class DateHelper
    {
        private const string ShortMonth = "Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec";
        private const string FullMonth = "January|February|March|April|May|June|July|August|September|October|November|December";
        private static readonly Regex StartAndEndDateRegex =
            new Regex(
                string.Format(
                    @"(?<Start>({0}|{1}|\d{{1,2}})[/\s-–](20)?\d{{2}})[/\s-–—]+(?<End>({0}|{1}|\d{{1,2}})[/\s-–](20)?\d{{2}}|Current|Now|Present)",
                    ShortMonth, FullMonth), RegexOptions.Compiled);

        public static Period ParseStartAndEndDate(string input)
        {
            var match = StartAndEndDateRegex.Match(input);
            if (match.Success)
            {
                var startDate = match.Groups["Start"].Value;
                var endDate = match.Groups["End"].Value;

                return new Period(startDate, endDate);
            }

            return null;
        }
    }
}
