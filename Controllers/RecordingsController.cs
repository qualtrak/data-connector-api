namespace DataConnector.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.IO.Compression;
    using System.Linq;
    using System.Text;
    using System.Web.Http;
    using DataConnector.App_Start;
    using Microsoft.AspNet.WebApi.MessageHandlers.Compression.Attributes;
    using Newtonsoft.Json;
    using Ninject;
    using Qualtrak.Coach.DTO.Integration;
    using Qualtrak.Coach.DTO.Integration.Contracts;

    public class RecordingsController : ApiController
    {
        [Compression(Enabled = false)]
        public string Get(string tenantCode, string username, string password, string recordingId, string originalRecordingUrl)
        {
            var client = NinjectWebCommon.Kernel.Get<IApiFacade>();
            string url = String.Empty;

            try
            {
                url = client.GetRecordingUrl(recordingId, originalRecordingUrl, username, password);
            }
            catch (Exception ex)
            {
                Trace.TraceError("connector : [{0}]", ex.Message);
            }

            return url;
        }

        
        public IEnumerable<RecordingInfo> Post(DataContractUesrsRecordingListFilter filter)
        {
            var client = NinjectWebCommon.Kernel.Get<IApiFacade>();

            try
            {   
                var response = client.GetRecordingsForUsers(filter.Limit, filter.TenantCode, filter.UserIds, filter.SearchCriteria, filter.Username, filter.Password);
                return response;
            }
            catch (Exception ex)
            {   
                Trace.TraceError("connector : [{0}]", ex.Message);
            }

            return new List<RecordingInfo>();
        }

        private void ResponseToJsonToFile(ICollection<RecordingInfo> recorderRecordings)
        {
            try
            {
                DateTime now = DateTime.Now;
                string filePath = string.Format(@"c:\source\recordings-{0}-{1}-{2}_{3}{4}_{5}.gz", 
                    now.Year, now.Month, now.Day, now.Minute, now.Second, now.Millisecond);
                string json = JsonConvert.SerializeObject(recorderRecordings, Formatting.None);

                UnicodeEncoding uniEncode = new UnicodeEncoding();

                byte[] bytesToCompress = uniEncode.GetBytes(json);

                using (FileStream fileToCompress = File.Create(filePath))
                {
                    using (GZipStream compressionStream = new GZipStream(fileToCompress, CompressionMode.Compress))
                    {
                        compressionStream.Write(bytesToCompress, 0, bytesToCompress.Length);
                    }
                }
              
            } catch (Exception exe) {

                Console.WriteLine(exe);
            }
        }
    }
}