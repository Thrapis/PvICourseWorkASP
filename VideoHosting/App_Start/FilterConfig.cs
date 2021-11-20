using System.Web;
using System.Web.Mvc;
using VideoHosting.Models.Database.Connections;
using VideoHosting.Models.Database.Contexts;

namespace VideoHosting
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
