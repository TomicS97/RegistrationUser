using System.Web;
using System.Web.Mvc;

namespace Registracija_SrdjanTomic
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
