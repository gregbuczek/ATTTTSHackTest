using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Geocoding.Google;
using Geocoding;
//using GeoCoding.Esri;
//using Esri.ArcGISRuntime;
namespace ATTTTSHackTest
{
    /// <summary>
    /// Summary description for AddCalendarEvent
    /// </summary>
    public class AddCalendarEvent : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            
            String recipName = context.Request["recipName"] ?? "";
            String recipPhoneNumber = context.Request["recipPhoneNumber"] ?? "";
            String recipEmailAddress = context.Request["recipEmailAddress"] ?? "";
            String meetingLocation = context.Request["meetingLocation"] ?? "";
            String meetingDateTime = context.Request["meetingDateTime"] ?? "";
            String senderName = context.Request["senderName"] ?? "";
            //String howToSend = context.Request["howToSend"] ?? "";

            IGeocoder geocoder = new GoogleGeocoder() {  };

            IEnumerable<Address> addresses = geocoder.Geocode(meetingLocation);
            //IEnumerable<Address> addresses = geocoder.Geocode("2525 N Nellis Blvd, Las Vegas, NV 89115, USA");
            double lat = addresses.First().Coordinates.Latitude;
            double lon = addresses.First().Coordinates.Longitude;

            CalendarEvent ce = new CalendarEvent();
            ce.CalendarEventGUID = Guid.NewGuid().ToString();
            ce.RecipName = recipName;
            ce.RecipPhoneNumber = recipPhoneNumber;
            ce.RecipEmailAddress = recipEmailAddress;
            ce.MeetingLatitude = Convert.ToDecimal(lat);
            ce.MeetingLongitude = Convert.ToDecimal(lon);
            ce.MeetingDateTime = DateTime.Parse(meetingDateTime);
            ce.NotificationSent = "no";
            ce.SenderName = senderName;

            DBFunctions dbFunctions = new DBFunctions();
            dbFunctions.addCalebdarEvent(ce);
            /*
            Esri.ArcGISRuntime.Tasks.Geocoding.LocatorTask locatorTask;
            locatorTask = new Esri.ArcGISRuntime.Tasks.Geocoding.OnlineLocatorTask(new Uri("http://geocode.arcgis.com/arcgis/rest/services/World/GeocodeServer"), string.Empty);
            locatorTask.
             * */
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