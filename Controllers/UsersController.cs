using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Web.Http;
using DataConnector.App_Start;
using Ninject;
using Qualtrak.Coach.Integration.Core.Contracts;
using Qualtrak.Coach.Integration.Core.DTO;

namespace DataConnector.Controllers
{
    public class UsersController : ApiController
    {
        public async Task<IEnumerable<RecorderUserInfo>> Get(string tenantCode, string username, string password)
        {
            var list = new List<RecorderUserInfo>();
            var client = NinjectWebCommon.Kernel.Get<IApiFacade>();

            try
            {
                list = await client.GetUsersAsync(tenantCode, username, password);
            }
            catch (Exception ex)
            {
                Trace.TraceError("connector : [{0}]", ex.Message);
            }

            return list;
        }
    }
}