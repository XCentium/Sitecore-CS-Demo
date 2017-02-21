using CSDemo.App_Start;
using Sitecore.Diagnostics;
using Sitecore.Pipelines;
using System.Web.Routing;

namespace CSDemo.BL.Pipelines
{
    public class LoadRoutes
    {
        public void Process(PipelineArgs args)
        {
            Log.Info("Sitecore is starting", this);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}