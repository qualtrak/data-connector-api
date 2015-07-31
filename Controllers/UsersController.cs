using System;
using System.Collections.Generic;
using System.Web.Http;
using Qualtrak.Coach.DTO.Integration;

namespace DataConnector.Controllers
{
    using System.Diagnostics;
    using App_Start;
    using Ninject;
    using Qualtrak.Coach.DTO.Integration.Contracts;

    public class UsersController : ApiController
    {
        public IEnumerable<RecorderUserInfo> Get(string tenantCode, string username, string password)
        {
            var list = new List<RecorderUserInfo>();
            var client = NinjectWebCommon.Kernel.Get<IApiFacade>();

            try
            {
                list = client.GetUsers(tenantCode, username, password);
            }
            catch (Exception ex)
            {
                Trace.TraceError("connector : [{0}]", ex.Message);
            }

            return list;
        }
    }
}