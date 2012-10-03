using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace System.ComponentModel.DataAnnotations
{
    [Serializable]
    public sealed class CreditCardAttribute : ValidationAttribute
    {
        const int DefaultMinLength = 13;
        const int MaxLength = 19;
        int _minLength = DefaultMinLength;
        const String Regex = @"^\d{{{0},{1}}}$";
        const int Zero = '0';

        public CreditCardAttribute()
        {
            ErrorMessage = "Please enter a valid credit card number";
        }

        public int MinLength
        {
            get { return _minLength; }
            set
            {
                if ((8 > value) || (value > MaxLength))
                {
                    _minLength = DefaultMinLength;
                }
                else
                {
                    _minLength = value;
                }
            }
        }

        public override bool IsValid(object value)
        {
            var pan = value as String;
            if (String.IsNullOrEmpty(pan))
            {
                return false;
            }
            var panRegex = new Regex(String.Format(Regex, MinLength.ToString(CultureInfo.InvariantCulture.NumberFormat),
                MaxLength.ToString(CultureInfo.InvariantCulture.NumberFormat)));
            if (!panRegex.IsMatch(pan)) return false;

            // Validate the check digit using the Luhn algorithm 
            var reversedPan = new String((pan.ToCharArray()).Reverse().ToArray());
            var sum = 0;
            var multiplier = 0;
            //foreach (var ch in reversedPan)
            //{
            //    var product = (ch - Zero) * (multiplier + 1);
            //    sum = sum + (product / 10) + (product % 10);
            //    multiplier = (multiplier + 1) % 2;
            //}

            foreach (var product in reversedPan.Select(ch => (ch - Zero) * (multiplier + 1)))
            {
                sum = sum + (product / 10) + (product % 10);
                multiplier = (multiplier + 1) % 2;
            }
            return (sum % 10 == 0);
        }
    }
}
