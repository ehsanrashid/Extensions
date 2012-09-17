namespace System.SystemDependencies
{
    /// <summary>
    ///   Class to use for replacing System date used by extensions methods.
    ///   <para> This class is for testing purpose </para>
    ///   <example>
    ///     <code>using (var presentTime = new SubForSystemDate(new DateTime(2010, 1, 1)))
    ///       {
    ///       // Code that use Clock.Now static property
    ///       }</code>
    ///   </example>
    /// </summary>
    public class SubForSystemDate : IDisposable
    {

        public SubForSystemDate(DateTime substitute)
        {
            Clock.SubstituteForNow = substitute;
        }

        public void Dispose()
        {
            Clock.SubstituteForNow = null;
        }
    }
}