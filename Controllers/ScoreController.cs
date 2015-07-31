namespace DataConnector.Controllers
{
    using System;
    using System.Diagnostics;
    using System.Web.Http;
    using App_Start;
    using Ninject;
    using Qualtrak.Coach.DTO.Integration;
    using Qualtrak.Coach.DTO.Integration.Contracts;

    public class ScoreController : ApiController
    {
        public void Post(DataConnectorEvaluationScore evaluationScore)
        {
            var client = NinjectWebCommon.Kernel.Get<IApiFacade>();
            try
            {
                client.PostEvaluationScore(evaluationScore.TenantCode, evaluationScore.Username,
                    evaluationScore.Password, evaluationScore.EvaluationId, evaluationScore.HeadlineScore,
                    evaluationScore.ExtraScore, evaluationScore.UserId, evaluationScore.RecordingId);
            }
            catch (Exception ex)
            {
                Trace.TraceError("connector : [{0}]", ex.Message);
            }
        }
    }
}