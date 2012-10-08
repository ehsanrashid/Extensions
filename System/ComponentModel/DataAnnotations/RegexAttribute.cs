using System.Text.RegularExpressions;

namespace System.ComponentModel.DataAnnotations
{
    [Serializable]
    public class RegexAttribute : ValidationAttribute
    {
        public String Pattern { get; set; }

        public RegexOptions Options { get; set; }

        public RegexAttribute(String pattern, RegexOptions options = RegexOptions.None)
        {
            Pattern = pattern;
            Options = options;
        }

        public override bool IsValid(object value)
        {
            return IsValid(value as String);
        }

        public bool IsValid(String value)
        {
            return String.IsNullOrEmpty(value) || new Regex(Pattern, Options).IsMatch(value);
        }
    }
}