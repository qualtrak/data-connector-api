using System.Web;
using System.Web.Http;

namespace Qualtrak.Coach.DataConnector
{
    public class WebApiApplication : HttpApplication 
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}