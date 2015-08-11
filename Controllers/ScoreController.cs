using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Web.Http;
using DataConnector.App_Start;
using Ninject;
using Qualtrak.Coach.Integration.Core.Contracts;
using Qualtrak.Coach.Integration.Core.DTO;

namespace DataConnector.Controllers
{
    public class ScoreController : ApiController
    {
        public async Task Post(DataConnectorEvaluationScore evaluationScore)
        {
            var client = NinjectWebCommon.Kernel.Get<IApiFacade>();
            try
            {
               await client.PostEvaluationScoreAsync(evaluationScore.TenantCode, evaluationScore.EvaluationId, evaluationScore.HeadlineScore,
                    evaluationScore.ExtraScore, evaluationScore.UserId, evaluationScore.RecordingId, evaluationScore.Username,
                    evaluationScore.Password);
            }
            catch (Exception ex)
            {
                Trace.TraceError("connector : [{0}]", ex.Message);
            }
        }
    }
}