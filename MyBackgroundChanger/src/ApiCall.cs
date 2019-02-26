using System;
using System.IO;
using System.Net;

namespace MyBackgroundChanger
{
    internal class ApiCall
    {
        // Returns JSON string
        public string Get(string url)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            try
            {
                var response = request.GetResponse();
                using (var responseStream = response.GetResponseStream())
                {
                    var reader = new StreamReader(responseStream ?? throw new InvalidOperationException(), System.Text.Encoding.UTF8);
                    return reader.ReadToEnd();
                }
            }
            catch (WebException ex)
            {
                var errorResponse = ex.Response;
                using (var responseStream = errorResponse.GetResponseStream())
                {
                    var reader = new StreamReader(responseStream ?? throw new InvalidOperationException(), System.Text.Encoding.GetEncoding("utf-8"));
                    reader.ReadToEnd();
                    // log errorText
                }
                throw;
            }
        }
    }
}
