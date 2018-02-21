using Sharpenter.ResumeParser.Model.Models;

namespace Sharpenter.ResumeParser.Model
{
    public interface IOutputFormatter
    {
        string Format(Resume resume);
    }
}
