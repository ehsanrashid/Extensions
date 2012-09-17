namespace System.ComponentModel
{
    /// <summary>
    /// 	Extension methods for IComponent data type.
    /// </summary>
    public static class ComponentExtension
    {
        /// <summary>
        /// 	Returns <c>true</c> if component is in design mode.
        /// 	Othervise returns <c>false</c>.
        /// </summary>
        /// <param name = "component">Target component. Can not be null.</param>
        public static bool IsInDesignMode(this IComponent component)
        {
            var site = component.Site;
            return default(ISite) != site && site.DesignMode;
        }

        /// <summary>
        /// 	Returns <c>true</c> if component is NOT in design mode.
        /// 	Othervise returns <c>false</c>.
        /// </summary>
        /// <param name = "component">Target component.</param>
        public static bool IsInRuntimeMode(this IComponent component)
        {
            return !IsInDesignMode(component);
        }

    }
}