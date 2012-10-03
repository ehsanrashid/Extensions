namespace System.ComponentModel.DataAnnotations
{
    [Serializable]
    public class PriceAttribute : ValidationAttribute
    {
        public double MinPrice { get; set; }

        public override bool IsValid(object value)
        {
            if (null == value) return true;

            var price = (double) value;
            if (price < MinPrice) return false;

            var cents = price - Math.Truncate(price);

            return !(cents < 0.99 || cents >= 0.995);
        }
    }
}
