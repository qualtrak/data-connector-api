namespace DataConnector
{
    using System.Web;
    using System.Web.Http;

    public class WebApiApplication : HttpApplication 
    {
        protected void Application_Start()
        {
            WebApiConfig.Register(GlobalConfiguration.Configuration);
        }
    }
}