using System.Collections.Generic;
using System.Reflection;

namespace Sharpenter.ResumeParser.ResumeProcessor.Helpers
{
    public class CachedResourceLoader : IResourceLoader
    {
        private readonly Dictionary<string, IEnumerable<string>> _cacheResources;
        private readonly IResourceLoader _decorated;

        public CachedResourceLoader(IResourceLoader decorated)
        {
            _cacheResources = new Dictionary<string, IEnumerable<string>>();
            _decorated = decorated;
        }

        public HashSet<string> LoadIntoHashSet(Assembly assembly, string resourceName, char delimiter)
        {
            var key = "LoadIntoHashSet:" + resourceName;
            if (!_cacheResources.ContainsKey(key))
            {                
                _cacheResources[key] = _decorated.LoadIntoHashSet(assembly, resourceName, delimiter);                
            }

            return _cacheResources[key] as HashSet<string>;
        }

        public IEnumerable<string> Load(Assembly assembly, string resourceName, char delimiter)
        {
            var key = "Load:" + resourceName;
            if (!_cacheResources.ContainsKey(key))
            {                
                _cacheResources[key] = _decorated.Load(assembly, resourceName, delimiter);
            }

            return _cacheResources[key];
        }
    }
}
