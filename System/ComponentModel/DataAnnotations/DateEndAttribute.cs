using System.Web;

namespace System.ComponentModel.DataAnnotations
{
    [Serializable]
    public sealed class DateEndAttribute : ValidationAttribute
    {
        public string DateStartProperty { get; set; }

        public override bool IsValid(object value)
        {
            // Get Value of the DateStart property
            var dateStartString = HttpContext.Current.Request[DateStartProperty];
            var dateEnd = (DateTime) value;
            var dateStart = DateTime.Parse(dateStartString);

            // Meeting start time must be before the end time
            return dateStart < dateEnd;
        }
    }
}
