using Sharpenter.ResumeParser.Model;
using Sharpenter.ResumeParser.Model.Models;
using Sharpenter.ResumeParser.ResumeProcessor.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Sharpenter.ResumeParser.ResumeProcessor.Parsers
{
    public class PersonalParser : IParser
    {
        private static readonly Regex EmailRegex = new Regex(@"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static readonly Regex PhoneRegex = new Regex(@"(\(\+[0-9]{1,3}\)[\.\s]?)?[0-9]{7,14}(?:x.+)?", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static readonly Regex SocialProfileRegex = new Regex(@"(http(s)?:\/\/)?([\w]+\.)?(linkedin\.com|facebook\.com|github\.com|stackoverflow\.com|bitbucket\.org|sourceforge\.net|(\w+\.)?codeplex\.com|code\.google\.com).*?(?=\s)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static readonly Regex SplitByWhiteSpaceRegex = new Regex(@"\s+|,", RegexOptions.Compiled);
        private readonly HashSet<string> _firstNameLookUp;
        private readonly List<string> _countryLookUp;

        public PersonalParser(IResourceLoader resourceLoader)
        {
            var assembly = Assembly.GetExecutingAssembly();

            _firstNameLookUp = resourceLoader.LoadIntoHashSet(assembly, "FirstName.txt", ',');
            _countryLookUp = new List<string>(resourceLoader.Load(assembly, "Countries.txt", '|'));            
        }

        public void Parse(Section section, Resume resume)
        {
            var firstNameFound = false;
            var addressFound = false;
            var genderFound = false;
            var emailFound = false;
            var phoneFound = false;            

            foreach (var line in section.Content)
            {                
                addressFound = ExtractAddress(resume, addressFound, line);

                firstNameFound = ExtractFirstAndLastName(resume, firstNameFound, line);

                genderFound = ExtractGender(resume, genderFound, line);

                emailFound = ExtractEmail(resume, emailFound, line);

                phoneFound = ExtractPhone(resume, phoneFound, line);

                ExtractSocialProfiles(resume, line);
            }
        }

        private bool ExtractAddress(Resume resume, bool addressFound, string line)
        {
            if (addressFound) return addressFound;

            var country =
                _countryLookUp.FirstOrDefault(
                    c => line.IndexOf(c, StringComparison.InvariantCultureIgnoreCase) > -1);
            if (country == null) return addressFound;            

            //Assume address is in one line and ending with country name
            //Working backward to the beginning of the line to get the address
            resume.Location = line.Substring(0, line.IndexOf(country) + country.Length);

            addressFound = true;

            return addressFound;
        }

        private void ExtractSocialProfiles(Resume resume, string line)
        {
            var socialProfileMatches = SocialProfileRegex.Matches(line);
            foreach (Match socialProfileMatch in socialProfileMatches)
            {
                resume.SocialProfiles.Add(socialProfileMatch.Value);
            }
        }

        private bool ExtractPhone(Resume resume, bool phoneFound, string line)
        {
            if (phoneFound) return phoneFound;

            var phoneMatch = PhoneRegex.Match(line);
            if (!phoneMatch.Success) return phoneFound;

            resume.PhoneNumbers = phoneMatch.Value;

            phoneFound = true;

            return phoneFound;
        }

        private bool ExtractGender(Resume resume, bool genderFound, string line)
        {
            if (genderFound) return genderFound;

            if (line.IndexOf("male", StringComparison.InvariantCultureIgnoreCase) > -1)
            {
                resume.Gender = "male";

                genderFound = true;
            }

            if (line.IndexOf("female", StringComparison.InvariantCultureIgnoreCase) > -1)
            {
                resume.Gender = "female";

                genderFound = true;
            }

            return genderFound;
        }

        private bool ExtractFirstAndLastName(Resume resume, bool firstNameFound, string line)
        {
            var words = SplitByWhiteSpaceRegex.Split(line);

            if (firstNameFound) return firstNameFound;

            for (var i = 0; i < words.Length; i++)
            {
                var word = words[i].Trim();
                if (!firstNameFound && _firstNameLookUp.Contains(word))
                {
                    resume.FirstName = word;

                    //Consider the rest of the line as part of last name
                    resume.LastName = string.Join(" ", words.Skip(i + 1));

                    firstNameFound = true;
                }
            }

            return firstNameFound;
        }

        private bool ExtractEmail(Resume resume, bool emailFound, string line)
        {
            if (emailFound) return emailFound;

            var emailMatch = EmailRegex.Match(line);
            if (!emailMatch.Success) return emailFound;

            resume.EmailAddress = emailMatch.Value;

            emailFound = true;

            return emailFound;
        }        
    }
}
