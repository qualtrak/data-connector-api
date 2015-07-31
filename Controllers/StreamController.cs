using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DataConnector.Controllers
{
    using System.Diagnostics;
    using System.IO;
    using System.Net.Http.Headers;
    using System.Web.Http.Results;
    using App_Start;
    using Ninject;
    using Qualtrak.Coach.DTO.Integration;
    using Qualtrak.Coach.DTO.Integration.Contracts;

    public class StreamController : ApiController
    {
        private IList<MediaFileType> _listOfMediaFileTypes;

        public StreamController()
        {
            // TODO: Must test this list with all supported browsers (G. Kitchen)
            _listOfMediaFileTypes = new List<MediaFileType>
            {
                new MediaFileType(".flv", "video/x-flv"),
                new MediaFileType(".mp4", "video/mp4"),
                new MediaFileType(".mov", "video/quicktime"),
                new MediaFileType(".avi", "video/x-msvideo"),
                new MediaFileType(".wmv", "video/x-ms-wmv"),
                new MediaFileType(".ogv", "video/ogg"),
                new MediaFileType(".webm", "video/webm"),
                new MediaFileType(".wma", "audio/x-ms-wma"),
                new MediaFileType(".wav", "audio/x-wav"),
                new MediaFileType(".mp3", "audio/mpeg"),
                new MediaFileType(".ogg", "audio/ogg"),
                new MediaFileType(".oga", "audio/ogg")
            };
        }

        // GET api/<controller>
        public System.Net.Http.HttpResponseMessage Get(string url)
        {
            var client = NinjectWebCommon.Kernel.Get<IApiFacade>();
            try
            {
                var stream = client.GetStream(url);
                bool match = false;
                HttpResponseMessage output;

                output = this.Request.CreateResponse(HttpStatusCode.OK);
                output.Content = new StreamContent(stream);
                output.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");

                foreach (var item in this._listOfMediaFileTypes)
                {
                    if (url.Contains(item.Ext))
                    {
                        output.Content.Headers.ContentType = new MediaTypeHeaderValue(item.MimeType);
                        output.Content.Headers.ContentDisposition.FileName = "recording" + item.Ext;
                        match = true;
                        break;
                    }
                }

                if (!match)
                {
                    output.Content.Headers.ContentType = new MediaTypeHeaderValue("audio/x-wav");
                    output.Content.Headers.ContentDisposition.FileName = "recording.wav";
                }

                return output;
            }
            catch (Exception ex)
            {
                Trace.TraceError("connector : [{0}]", ex.Message);
            }

            return new HttpResponseMessage(HttpStatusCode.NotFound);
        }
    }

    public class MediaFileType
    {
        public string Ext { get; set; }
        public string MimeType { get; set; }

        public MediaFileType(string ext, string mime)
        {
            this.Ext = ext;
            this.MimeType = mime;
        }
    }
}