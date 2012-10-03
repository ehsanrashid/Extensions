namespace System.ComponentModel.DataAnnotations
{
    [Serializable]
    public sealed class DateStartAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var dateStart = (DateTime) value;
            // Meeting must start in the future time.
            return (dateStart > DateTime.Now);
        }
    }
}
