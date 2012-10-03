namespace System.ComponentModel.DataAnnotations
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class EqualToPropertyAttribute : ValidationAttribute
    {
        public string CompareProperty { get; set; }

        public EqualToPropertyAttribute(string compareProperty)
        {
            CompareProperty = compareProperty;
            ErrorMessage = ""; //string.Format(Messages.EqualToError, compareProperty);
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
