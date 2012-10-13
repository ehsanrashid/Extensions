namespace System.ComponentModel.DataAnnotations
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class EqualToPropertyAttribute : ValidationAttribute
    {
        public String CompareProperty { get; set; }

        public EqualToPropertyAttribute(String compareProperty)
        {
            CompareProperty = compareProperty;
            ErrorMessage = ""; //String.Format(Messages.EqualToError, compareProperty);
        }

        public override bool IsValid(object value)
        {
            if (null == value) return true;

            var properties = TypeDescriptor.GetProperties(value);
            var property = properties.Find(CompareProperty, true);
            var comparePropertyValue = property.GetValue(value).ToString();

            return comparePropertyValue == value.ToString();
        }
    }
}
