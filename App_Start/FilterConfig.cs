using System.Web;
using System.Web.Mvc;

namespace TECHSUPPORTTICKET_MANAGEMENTSYSTEM
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
