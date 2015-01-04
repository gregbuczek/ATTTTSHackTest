using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI.HtmlControls;

namespace ATTTTSHackTest
{
    /// <summary>
    /// Summary description for TTS
    /// </summary>
    public class TTS : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            String textToConvert = context.Request["textToConvert"] ?? "";
            HelperFunction hf = new HelperFunction();
            //String parContent = "And now one from the new project";
            HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(@"https://api.att.com/speech/v3/textToSpeech");
            //httpRequest.Headers.Add("Authorization", "Bearer EjyjAyWBrKkiPdKyZQ8yCQuRwG7QBvm6");
            httpRequest.Headers.Add("Authorization", "Bearer B7s2AO4WNSDiFzbiPKPD9BysVfshsvcK");
            httpRequest.Headers.Add("X-SpeechContext", "ClientApp=C#ClientApp,ClientVersion=2_2,ClientScreen=Browser,ClientSdk=C#Restful,DeviceType=WebServer,DeviceOs=Windows");
            httpRequest.ContentLength = textToConvert.Length;
            httpRequest.ContentType = @"text/plain";
            httpRequest.Accept = "audio/x-wav";
            httpRequest.Method = "POST";
            httpRequest.KeepAlive = true;

            UTF8Encoding encoding = new UTF8Encoding();
            byte[] postBytes = encoding.GetBytes(textToConvert);
            httpRequest.ContentLength = postBytes.Length;

            using (Stream writeStream = httpRequest.GetRequestStream())
            {
                writeStream.Write(postBytes, 0, postBytes.Length);
                writeStream.Close();
            }
            HttpWebResponse speechResponse = (HttpWebResponse)httpRequest.GetResponse();
            System.Security.Principal.WindowsIdentity newId = hf.ImpersonateForFileUpload();
            using (System.Security.Principal.WindowsImpersonationContext impersonatedUser = newId.Impersonate())
            {
                //using (var fileStream = File.Create(@"C:\Users\Greg\Documents\Visual Studio 2012\Projects\RTS\WUAAdmin\WUAAdmin\pdf\test.wav"))
                using (var fileStream = File.Create(@"C:\inetpub\wwwroot\hackathon\out\test.wav"))
                {
                    speechResponse.GetResponseStream().CopyTo(fileStream);
                }
            }


            context.Response.ContentType = "application/json; charset=utf-8";
            context.Response.Write("{\"data\" : \"success\"}");
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }


}