using System.Collections.Generic;
using System.Linq;
using Sharpenter.ResumeParser.Model;

namespace Sharpenter.ResumeParser.InputReader.Doc
{
    public class InteropDocInputReader : InputReaderBase
    {
        protected override bool CanHandle(string location)
        {
            return location.EndsWith("doc");
        }

        protected override IList<string> Handle(string location)
        {
            var word = new Microsoft.Office.Interop.Word.Application();
            object miss = System.Reflection.Missing.Value;
            object path = location;
            object readOnly = true;

            var doc = word.Documents.Open(ref path, ref miss, ref readOnly, ref miss, ref miss, ref miss, ref miss,
                ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss);
            

            var lines = doc.Paragraphs.Cast<object>().Select((t, i) => doc.Paragraphs[i + 1].Range.Text).ToList();

            doc.Close();
            word.Quit();

            return lines;
        }
    }
}
