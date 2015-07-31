namespace DataConnector.Handler
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Web;
    using System.Web.Configuration;

    /// <summary>
    /// Developed for Rostrvm to handle urls that require basic authentication. The user credentails for the basic auth are in the web.config file:
    /// <code>
    /// 
    /// appSettings:
    ///         coach:basicauth:username
    ///         coach:basicauth:password 
    /// </code>
    /// <remarks>
    /// Note: If we need to handle basic auth for another data source then we'll have to refactor the code to be more robust
    /// </remarks>
    /// </summary>
    public class BasicAuthenticationGetMediaHandler : IHttpHandler
    {
        /// <summary>
        /// You will need to configure this handler in the Web.config file of your 
        /// web and register it with IIS before being able to use it. For more information
        /// see the following link: http://go.microsoft.com/?linkid=8101007
        /// </summary>
        
        public bool IsReusable
        {
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {

            Trace.WriteLine(string.Format("Referrer: [{0}], qs: [{1}]", context.Request.UrlReferrer, context.Request.QueryString));
            string uri;
            string url = context.Request.QueryString["url"] != null ? context.Request.QueryString["url"].ToString() : "";
            string ashttp = context.Request.QueryString["ashttp"] != null ? context.Request.QueryString["ashttp"].ToString() : "";
            string index = context.Request.QueryString["index"] != null ? context.Request.QueryString["index"].ToString() : "";
            string username = WebConfigurationManager.AppSettings["coach:basicauth:username"] != null ? WebConfigurationManager.AppSettings["coach:basicauth:username"].ToString() : "";
            string password = WebConfigurationManager.AppSettings["coach:basicauth:password"] != null ? WebConfigurationManager.AppSettings["coach:basicauth:password"].ToString() : "";
            Stream stream = new MemoryStream(1000000 * 10);

            if (string.IsNullOrEmpty(ashttp))
            {
                uri = string.Format("{0}&INDEX={1}", url, index);   
            }
            else
            {
                uri = string.Format("{0}?ASHTTP={1}&INDEX={2}", url, ashttp, index);
            }
            
            using (var client = new HttpClient() { BaseAddress = new Uri(uri, UriKind.Absolute) })
            {
                var byteArray = Encoding.ASCII.GetBytes(string.Format("{0}:{1}", username, password));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

                try
                {
                    var response = client.GetByteArrayAsync("").Result;
                    stream = new MemoryStream(response);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }

            try
            {
                long bytesToRead = stream.Length;
                context.Response.ContentType = "audio/x-wav";
                context.Response.AddHeader("Content-Disposition", "attachment; filename=recording.wav");
                while (bytesToRead > 0)
                {
                    if (context.Response.IsClientConnected)
                    {
                        byte[] buffer = new Byte[10000];
                        int length = stream.Read(buffer, 0, 10000);
                        context.Response.OutputStream.Write(buffer, 0, length);
                        context.Response.Flush();
                        bytesToRead = bytesToRead - length;
                    }
                    else
                    {
                        bytesToRead = -1;
                    }
                }
            }
            catch (Exception ex)
            {
                context.Response.Write(ex.Message);
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
            }
        }
    }
}
