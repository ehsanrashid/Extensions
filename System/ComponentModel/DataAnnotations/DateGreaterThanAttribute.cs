namespace System.ComponentModel.DataAnnotations
{
    [Serializable]
    public sealed class DateGreaterThanAttribute : ValidationAttribute
    {
        const String DefaultErrorMessage = "'{0}' must be greater than '{1}'";
        readonly String _basePropertyName;

        public DateGreaterThanAttribute(String basePropertyName)
            : base(DefaultErrorMessage)
        {
            _basePropertyName = basePropertyName;
        }

        //Override default FormatErrorMessage Method
        public override String FormatErrorMessage(String name)
        {
            return String.Format(DefaultErrorMessage, name, _basePropertyName);
        }

        //Override IsValid
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            //Get PropertyInfo Object
            var basePropertyInfo = validationContext.ObjectType.GetProperty(_basePropertyName);

            //Get Value of the property
            var startDate = (DateTime) basePropertyInfo.GetValue(validationContext.ObjectInstance, null);


            var thisDate = (DateTime) value;

            //Actual comparision
            if (thisDate <= startDate)
            {
                var message = FormatErrorMessage(validationContext.DisplayName);
                return new ValidationResult(message);
            }

            //Default return - This means there were no validation error
            return null;
        }

    }
	
}