using System.Linq;
using System.Web.Http;
using Qualtrak.Coach.DataConnector.Core.Shared;

namespace Qualtrak.Coach.DataConnector.Controllers
{
    public class BaseApiController : ApiController
    {
        private const string TenantCodeHeaderKey = "TenantCode";
        private const string UsernameHeaderKey = "Username";
        private const string PasswordHeaderKey = "Password";

        protected DataConnectorProperties GetDataConnectorProperties()
        {
            var result = new DataConnectorProperties();
            result.TenantCode = this.GetValueFromHeaderKey(TenantCodeHeaderKey);
            result.Username = this.GetValueFromHeaderKey(UsernameHeaderKey);
            result.Password = this.GetValueFromHeaderKey(PasswordHeaderKey);
            return result;
        }

        private string GetValueFromHeaderKey(string key)
        {
            if (this.Request.Headers.Contains(key))
            {
                return this.Request.Headers.GetValues(key).First();
            }

            return null;
        }
    }
}