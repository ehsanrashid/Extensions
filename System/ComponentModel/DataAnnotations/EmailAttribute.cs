using System.Text.RegularExpressions;

namespace System.ComponentModel.DataAnnotations
{
    [Serializable]
    public class EmailAttribute : RegexAttribute
    {
        public EmailAttribute()
            : base(@"^[\w-\.]{1,}\@([\w]{1,}\.){1,}[a-z]{2,4}$", RegexOptions.IgnoreCase)
        { }
    }

}
