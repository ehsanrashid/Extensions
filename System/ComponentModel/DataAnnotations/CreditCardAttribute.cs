namespace System.ComponentModel.DataAnnotations
{
    using Globalization;
    using Linq;
    using Text.RegularExpressions;


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
            if (pan.IsNullOrEmpty())
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
            foreach (var ch in reversedPan)
            {
                var product = (ch - Zero) * (multiplier + 1);
                sum = sum + (product / 10) + (product % 10);
                multiplier = (multiplier + 1) % 2;
            }

            //foreach (var product in reversedPan.Select(ch => (ch - Zero) * (multiplier + 1)))
            //{
            //    sum = sum + (product / 10) + (product % 10);
            //    multiplier = (multiplier + 1) % 2;
            //}

            return (sum % 10 == 0);
        }
    }

    /*
     public class CreditCardAttribute : ValidationAttribute, IClientValidatable
    {
        private CardType _cardTypes;
        public CardType AcceptedCardTypes
        {
            get { return _cardTypes; }
            set { _cardTypes = value; }
        }
 
        public CreditCardAttribute()
        {
            _cardTypes = CardType.All;
        }
 
        public CreditCardAttribute(CardType acceptedCardTypes)
        {
            _cardTypes = acceptedCardTypes;
        }
 
        public override bool IsValid(object value)
        {
            var number = Convert.ToString(value);
 
            if (String.IsNullOrEmpty(number))
                return true;
 
            return IsValidType(number, _cardTypes) && IsValidNumber(number);
        }
 
        public override String FormatErrorMessage(String name)
        {
            return "The " + name + " field contains an invalid credit card number.";
        }
 
        [Flags]
        public enum CardType
        {
            Unknown = 1,
            Visa = 2,
            MasterCard = 4,
            Amex = 8,
            Diners = 16,
 
            All = Visa | MasterCard | Amex | Diners,
            AllOrUnknown = Unknown | Visa | MasterCard | Amex | Diners
        }
 
        private static bool IsValidType(String cardNumber, CardType cardType)
        {
            // Visa
            if (Regex.IsMatch(cardNumber, "^(4)")
                && ((cardType & CardType.Visa) != 0))
                return cardNumber.Length == 13 || cardNumber.Length == 16;
 
            // MasterCard
            if (Regex.IsMatch(cardNumber, "^(51|52|53|54|55)")
                && ((cardType & CardType.MasterCard) != 0))
                return cardNumber.Length == 16;
 
            // Amex
            if (Regex.IsMatch(cardNumber, "^(34|37)")
                && ((cardType & CardType.Amex) != 0))
                return cardNumber.Length == 15;
 
            // Diners
            if (Regex.IsMatch(cardNumber, "^(300|301|302|303|304|305|36|38)")
                && ((cardType & CardType.Diners) != 0))
                return cardNumber.Length == 14;
 
            //Unknown
            if ((cardType & CardType.Unknown) != 0)
                return true;
 
            return false;
        }
 
        private static bool IsValidNumber(String number)
        {
            var deltas = new[] { 0, 1, 2, 3, 4, -4, -3, -2, -1, 0 };
            var checksum = 0;
            var chars = number.ToCharArray();
            for (var i = chars.Length - 1; i > -1; i--)
            {
                var j = chars[i] - 48;
                checksum += j;
                if (((i - chars.Length) % 2) == 0)
                    checksum += deltas[j];
            }
 
            return ((checksum % 10) == 0);
        }
 
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            yield return new ModelClientValidationRule
            {
                ErrorMessage = ErrorMessage,
                ValidationType = "creditcard"
            };
        }
    }
     */
}
