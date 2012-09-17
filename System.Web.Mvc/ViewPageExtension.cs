
namespace System.Web.Mvc
{
    public static class ViewPageExtension
    {

        public static string GetDefaultPageTitle<T>(this ViewPage<T> viewPage)
            where T : class
        {
            return "";
        }

    }
}