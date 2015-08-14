using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Web.Http;
using Ninject;
using Qualtrak.Coach.DataConnector.Core.Recorder;
using Qualtrak.Coach.DataConnector.Core.Recorder.Args;

namespace Qualtrak.Coach.DataConnector.Controllers.Recorder
{
    [RoutePrefix("api")]
    public class RecorderEvaluationScoreController : BaseApiController
    {
        [Route("recorder/evalutionscore")]
        public async Task PostAsync(SendEvaluationScoreArgs args)
        {
            var client = NinjectWebCommon.Kernel.Get<IRecorderApiFacade>();

            try
            {
               await client.SendEvaluationScoreAsync(args, this.GetDataConnectorProperties());
            }
            catch (Exception ex)
            {
                Trace.TraceError("connector : [{0}]", ex.Message);
            }
        }
    }
}