using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;

namespace ImgStoApi.Utils
{
    public class UploadFilePro : MultipartMemoryStreamProvider
    {
        public string ext = "";
        public override Stream GetStream(HttpContent parent, HttpContentHeaders headers)
        {
            var extensions = new[] { "jpg", "heic", "png","jfif","jpeg" };
            var filename = headers.ContentDisposition.FileName.Replace("\"", string.Empty);

            if (filename.IndexOf('.') < 0)
                return Stream.Null;

            var extension = filename.Split('.').Last();
            ext = extension;
            return extensions.Any(i => i.Equals(extension, StringComparison.InvariantCultureIgnoreCase))
                       ? base.GetStream(parent, headers)
                       : Stream.Null;

        }
    }
}