using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Web.Http;
using DataConnector.App_Start;
using DataConnector.Handler;
using Ninject;
using Qualtrak.Coach.Integration.Core.Contracts;
using Qualtrak.Coach.Integration.Core.DTO;

namespace DataConnector.Controllers
{
    public class RecordingsController : ApiController
    {
        public async Task<string> Get(string tenantCode, string username, string password, string recordingId, string originalRecordingUrl)
        {
            var client = NinjectWebCommon.Kernel.Get<IApiFacade>();
            string url = String.Empty;

            try
            {
                url = await client.GetRecordingUrlAsync(recordingId, originalRecordingUrl, username, password);
            }
            catch (Exception ex)
            {
                Trace.TraceError("connector : [{0}]", ex.Message);
            }

            return url;
        }

        [DeflateCompression]
        public async Task<IEnumerable<RecordingUser>> Post(DataContractUesrsRecordingListFilter filter)
        {
            var client = NinjectWebCommon.Kernel.Get<IApiFacade>();
            try
            {   
                var response = await client.GetRecordingsForUsersAsync(filter.Limit, filter.TenantCode, filter.UserIds, filter.SearchCriteria, filter.Username, filter.Password);
                return response;
            }
            catch (Exception ex)
            {   
                Trace.TraceError("connector : [{0}]", ex.Message);
            }

            // TODO: New empty list as a task
            return await Task.FromResult(new List<RecordingUser>());
        }
    }
}