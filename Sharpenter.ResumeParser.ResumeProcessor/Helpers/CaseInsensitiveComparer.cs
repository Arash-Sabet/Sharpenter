using System;
using System.Collections.Generic;

namespace Sharpenter.ResumeParser.ResumeProcessor.Helpers
{
    public class CaseInsensitiveComparer : IComparer<string>, IEqualityComparer<string>
    {
        public int Compare(string x, string y)
        {
            return string.Compare(x, y, StringComparison.InvariantCultureIgnoreCase);
        }

        public bool Equals(string x, string y)
        {
            return string.Equals(x, y, StringComparison.InvariantCultureIgnoreCase);
        }

        public int GetHashCode(string obj)
        {
            return obj.GetHashCode();
        }
    }
}
