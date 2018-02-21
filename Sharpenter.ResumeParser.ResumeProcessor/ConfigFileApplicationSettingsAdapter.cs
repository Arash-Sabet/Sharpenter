using System.Configuration;
using Sharpenter.ResumeParser.Model;

namespace Sharpenter.ResumeParser.ResumeProcessor
{
    internal class ConfigFileApplicationSettingsAdapter : IApplicationSettings
    {
        public string InputReaderLocation
        {
            get { return ConfigurationManager.AppSettings["InputReaderLocation"]; }
        }
    }
}
