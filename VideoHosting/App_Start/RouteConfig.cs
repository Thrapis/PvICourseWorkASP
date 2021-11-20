using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using VideoHosting.Handlers;

namespace VideoHosting
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.Add("VideoStatusRoute", new Route("videostatus/{path*}", new VideoProgressRouteHandler()));
            routes.Add("NewCommentRoute", new Route("newcomments/{path*}", new NewCommentRouteHandler()));

            routes.MapMvcAttributeRoutes();

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}/{more}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional, more = UrlParameter.Optional }
            );
        }

        public class VideoProgressRouteHandler : IRouteHandler
        {
            public IHttpHandler GetHttpHandler(RequestContext requestContext)
            {
                return new VideoProgressHandler();
            }
        }

        public class NewCommentRouteHandler : IRouteHandler
        {
            public IHttpHandler GetHttpHandler(RequestContext requestContext)
            {
                return new NewCommentHandler();
            }
        }
    }
}
