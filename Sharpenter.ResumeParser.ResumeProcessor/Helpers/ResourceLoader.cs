using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Sharpenter.ResumeParser.ResumeProcessor.Helpers
{
    public class ResourceLoader : IResourceLoader
    {                
        public HashSet<string> LoadIntoHashSet(Assembly assembly, string resourceName, char delimiter)
        {           
            var lines = Load(assembly, resourceName, delimiter);
            return new HashSet<string>(lines, StringComparer.InvariantCultureIgnoreCase);
        }
        
        public IEnumerable<string> Load(Assembly assembly, string resourceName, char delimiter)
        {            
            var fullResourcePath = string.Format("Sharpenter.ResumeParser.ResumeProcessor.Data.{0}", resourceName);
            using (var stream = assembly.GetManifestResourceStream(fullResourcePath))
            {
                using (var reader = new StreamReader(stream))
                {
                    var text = reader.ReadToEnd();
                    return text.Split(delimiter);                    
                }
            }
        }
    }
}
