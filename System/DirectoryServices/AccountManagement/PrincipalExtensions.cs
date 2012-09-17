namespace System.DirectoryServices.AccountManagement
{
    /// <summary>
    ///   Extension methods for the Princiapl data type
    /// </summary>
    public static class PrincipalExtensions
    {
        /// <summary>
        ///   Gets property value of a Principals underlying DirectoryEntry object.
        /// </summary>
        /// <param name="principal"> The principal. </param>
        /// <param name="name"> The name. </param>
        /// <returns> </returns>
        public static object GetProperty(this Principal principal, string name)
        {
            var directoryEntry = (principal.GetUnderlyingObject() as DirectoryEntry);
            if (null != directoryEntry && directoryEntry.Properties.Contains(name))
            {
                var property = directoryEntry.Properties[name];
                if (null != property) return property.Value;
            }
            return null;
        }
    }
}