using System;
using System.IO;
using System.Net;
using System.Text;

namespace me.cqp.yt.zanuka.Code
{
    public static class Net
    {
        public static string Get(string url) //取网页内容
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                WebClient client = new WebClient();
                client.Encoding = System.Text.Encoding.GetEncoding("UTF-8");
                Uri uri = new Uri(url);
                return client.DownloadString(uri);
            }
            catch
            {
                return "*nothing*";
            }
        }
        public static string HttpGet(string url)
        {
            //ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
            Encoding encoding = Encoding.UTF8;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.Accept = "text/html, application/xhtml+xml, */*";
            request.ContentType = "application/json";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
