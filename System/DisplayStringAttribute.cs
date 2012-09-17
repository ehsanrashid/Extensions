namespace System
{
    /// <summary>
    /// Specifies description for a member of the enum type for display to the UI
    /// </summary>
    /// <see cref="EnumExtension.DisplayString"/>
    /// <example>
    ///     <code>
    ///         enum OperatingSystem
    ///         {
    ///            [DisplayString("MS-DOS")]
    ///            Msdos,
    ///         
    ///            [DisplayString("Windows 98")]
    ///            Win98,
    ///         
    ///            [DisplayString("Windows XP")]
    ///            Xp,
    ///         
    ///            [DisplayString("Windows Vista")]
    ///            Vista,
    ///         
    ///            [DisplayString("Windows 7")]
    ///            Seven,
    ///         }
    ///         
    ///         public String GetMyOSName()
    ///         {
    ///             var myOS = OperatingSystem.Seven;
    ///             return myOS.DisplayString();
    ///         }
    ///     </code>
    /// </example>
    /// <remarks>
    /// 	Contributed by nagits, http://about.me/AlekseyNagovitsyn
    /// </remarks>
    [AttributeUsage(AttributeTargets.Field)]
    public class DisplayStringAttribute : Attribute
    {
        /// <summary>
        /// The default value for the attribute <c>DisplayStringAttribute</c>, which is an empty String
        /// </summary>
        public static readonly DisplayStringAttribute Default = new DisplayStringAttribute();

        private readonly String _displayString;

        /// <summary>
        /// The value of this attribute
        /// </summary>
        public String DisplayString
        {
            get { return _displayString; }
        }

        /// <summary>
        /// Initializes a new instance of the class <c>DisplayStringAttribute</c> with default value (empty String)
        /// </summary>
        public DisplayStringAttribute()
            : this(String.Empty) {}

        /// <summary>
        /// Initializes a new instance of the class <c>DisplayStringAttribute</c> with specified value
        /// </summary>
        /// <param name="displayString">The value of this attribute</param>
        public DisplayStringAttribute(String displayString)
        {
            _displayString = displayString;
        }

        public override bool Equals(Object obj)
        {
            if (obj is DisplayStringAttribute)
            {
                DisplayStringAttribute dispString = obj as DisplayStringAttribute;
                return _displayString.Equals(dispString._displayString);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return _displayString.GetHashCode();
        }

        public override bool IsDefaultAttribute()
        {
            return Equals(Default);
        }
    }
}