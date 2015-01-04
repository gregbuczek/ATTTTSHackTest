using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
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
    public partial class Map : System.Web.UI.Page
    {
        public String currentLocationLon;
        public String currentLocationLat;
        public String meetingLocationLon;
        public String meetingLocationLat;
        public String duration;

        private DBFunctions dbFunctions = new DBFunctions();

        protected void Page_Load(object sender, EventArgs e)
        {
            VehicleLocation currentLocation = dbFunctions.getMostRecentLocation();
            currentLocationLon = currentLocation.lng.ToString();
            currentLocationLat = currentLocation.lat.ToString();

            CalendarEvent meetingLocation = dbFunctions.getPendingEvent();
            meetingLocationLat = meetingLocation.MeetingLatitude.ToString();
            meetingLocationLon = meetingLocation.MeetingLongitude.ToString();
            duration = meetingLocation.MinutesToArrive.ToString();

        }

        protected void butSubmit_Click(object sender, EventArgs e)
        {
            HelperFunction hf = new HelperFunction();
            //String parContent = "And now one from the new project";
            HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(@"https://api.att.com/speech/v3/textToSpeech");
            //httpRequest.Headers.Add("Authorization", "Bearer EjyjAyWBrKkiPdKyZQ8yCQuRwG7QBvm6");
            httpRequest.Headers.Add("Authorization", "Bearer B7s2AO4WNSDiFzbiPKPD9BysVfshsvcK");
            httpRequest.Headers.Add("X-SpeechContext", "ClientApp=C#ClientApp,ClientVersion=2_2,ClientScreen=Browser,ClientSdk=C#Restful,DeviceType=WebServer,DeviceOs=Windows");
            httpRequest.ContentLength = txtMessageToSend.Text.Length;
            httpRequest.ContentType = @"text/plain";
            httpRequest.Accept = "audio/x-wav";
            httpRequest.Method = "POST";
            httpRequest.KeepAlive = true;

            UTF8Encoding encoding = new UTF8Encoding();
            byte[] postBytes = encoding.GetBytes(txtMessageToSend.Text);
            httpRequest.ContentLength = postBytes.Length;

            using (Stream writeStream = httpRequest.GetRequestStream())
            {
                writeStream.Write(postBytes, 0, postBytes.Length);
                writeStream.Close();
            }
            Response resp = new Response();
            resp.ResponseGUID = Guid.NewGuid().ToString();
            resp.Status = "not";
            dbFunctions.addResponse(resp);
            HttpWebResponse speechResponse = (HttpWebResponse)httpRequest.GetResponse();
            System.Security.Principal.WindowsIdentity newId = hf.ImpersonateForFileUpload();
            using (System.Security.Principal.WindowsImpersonationContext impersonatedUser = newId.Impersonate())
            {
                //using (var fileStream = File.Create(@"C:\Users\Greg\Documents\Visual Studio 2012\Projects\RTS\WUAAdmin\WUAAdmin\pdf\test.wav"))
                using (var fileStream = File.Create(@"C:\inetpub\wwwroot\hackathon\out\" + resp.ResponseGUID + ".wav"))
                {
                    speechResponse.GetResponseStream().CopyTo(fileStream);
                }
            }

            dbFunctions.addResponse(resp);


        }

       
    }
}