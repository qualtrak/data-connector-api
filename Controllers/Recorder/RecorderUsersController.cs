using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Web.Http;
using Ninject;
using Qualtrak.Coach.DataConnector.Core.Recorder;
using Qualtrak.Coach.DataConnector.Handler;

namespace Qualtrak.Coach.DataConnector.Controllers.Recorder
{
    [RoutePrefix("api")]
    public class RecorderUsersController : BaseApiController
    {
        [DeflateCompression]
        [Route("recorder/users")]
        public async Task<IEnumerable<RecorderUser>> GetAsync()
        {
            var client = NinjectWebCommon.Kernel.Get<IRecorderApiFacade>();

            try
            {
                return await client.GetUsersAsync(this.GetDataConnectorProperties());
            }
            catch (Exception ex)
            {
                Trace.TraceError("connector : [{0}]", ex.Message);
            }

            return await Task.FromResult(new List<RecorderUser>());
        }
    }
}