using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ATTTTSHackTest
{
    public class DBFunctions
    {
        public void addCalebdarEvent(CalendarEvent ce)
        {
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                db.CalendarEvents.InsertOnSubmit(ce);
                db.SubmitChanges();
            }
        }

        public VehicleLocation getMostRecentLocation()
        {
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                return (from t in db.VehicleLocations orderby t.dateTimeStamp descending select t).First();
            }
        }

        public CalendarEvent getPendingEvent()
        {
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                return (from t in db.CalendarEvents where t.NotificationSent == "pending" select t).First();
            }
        }
        public void addResponse(Response resp)
        {
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                db.Responses.InsertOnSubmit(resp);
                db.SubmitChanges();
            }            
        }
    }

}