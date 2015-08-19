using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Web.Http;
using Ninject;
using Qualtrak.Coach.DataConnector.Core.Recorder;
using Qualtrak.Coach.DataConnector.Core.Recorder.Args;
using Qualtrak.Coach.DataConnector.Handler;

namespace Qualtrak.Coach.DataConnector.Controllers.Recorder
{
    [RoutePrefix("api")]
    public class RecorderMediaController : BaseApiController
    {
        [Route("recorder/media")]
        [HttpGet]
        public async Task<string> GetAsync(string id, string originalUrl)
        {
            var client = NinjectWebCommon.Kernel.Get<IRecorderApiFacade>();
            string result = string.Empty;

            try
            {                
                result = await client.GetMediaUrlAsync(id, originalUrl, this.GetDataConnectorProperties());
            }
            catch (Exception ex)
            {
                Trace.TraceError("connector : [{0}]", ex.Message);
            }

            return result;
        }

        [DeflateCompression]
        [Route("recorder/media/{userId}")]
        [HttpPost] 
        public async Task<IEnumerable<Media>> PostAsync(string userId, MediaForUserArgs args)
        {
            var client = NinjectWebCommon.Kernel.Get<IRecorderApiFacade>();
            try
            {
                return await client.GetMediaForUserAsync(userId, args, this.GetDataConnectorProperties());
            }
            catch (Exception ex)
            {
                Trace.TraceError("connector : [{0}]", ex.Message);
            }

            return await Task.FromResult(new List<Media>());
        }

        [DeflateCompression]
        [Route("recorder/media")]
        [HttpPost]
        public async Task<IEnumerable<MediaUser>> PostAsync(MediaForUsersArgs args)
        {
            var client = NinjectWebCommon.Kernel.Get<IRecorderApiFacade>();
            try
            {
                return await client.GetMediaForUsersAsync(args, this.GetDataConnectorProperties());
            }
            catch (Exception ex)
            {
                Trace.TraceError("connector : [{0}]", ex.Message);
            }

            return await Task.FromResult(new List<MediaUser>());
        }

        [DeflateCompression]
        [Route("recorder/media/full")]
        [HttpPost]
        public async Task<IEnumerable<Media>> PostAsync(MediaByIds args)
        {
            var client = NinjectWebCommon.Kernel.Get<IRecorderApiFacade>();
            try
            {
                var response = await client.GetMediaByIdsAsync(args.Ids, this.GetDataConnectorProperties());
                return response;
            }
            catch (Exception ex)
            {
                Trace.TraceError("connector : [{0}]", ex.Message);
            }

            return await Task.FromResult(new List<Media>());
        }
    }
}