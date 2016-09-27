using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Karabina.SharePoint.Provisioning
{
    public class Calendar
    {
        public int CalendarId { get; set; }
        public string CalendarName { get; set; }

        public Calendar() { }
    }

    public class CalendarCollection
    {
        private List<Calendar> _calendars = null;
        public List<Calendar> Calendars
        {
            get { return _calendars; }
            set { _calendars = value; }
        }

        public CalendarCollection()
        {
            _calendars = new List<Calendar>();

            _calendars.Add(new Calendar() { CalendarId = 0, CalendarName = "None" });
            _calendars.Add(new Calendar() { CalendarId = 1, CalendarName = "Gregorian" });
            _calendars.Add(new Calendar() { CalendarId = 3, CalendarName = "Japanese Emperor Era" });
            _calendars.Add(new Calendar() { CalendarId = 5, CalendarName = "Korean Tangun Era" });
            _calendars.Add(new Calendar() { CalendarId = 6, CalendarName = "Hijri" });
            _calendars.Add(new Calendar() { CalendarId = 7, CalendarName = "Buddhist" });
            _calendars.Add(new Calendar() { CalendarId = 8, CalendarName = "Hebrew Lunar" });
            _calendars.Add(new Calendar() { CalendarId = 9, CalendarName = "Gregorian Middle East French Calendar" });
            _calendars.Add(new Calendar() { CalendarId = 10, CalendarName = "Gregorian Arabic Calendar" });
            _calendars.Add(new Calendar() { CalendarId = 11, CalendarName = "Gregorian Transliterated English Calendar" });
            _calendars.Add(new Calendar() { CalendarId = 12, CalendarName = "Gregorian Transliterated French Calendar" });
            _calendars.Add(new Calendar() { CalendarId = 16, CalendarName = "Saka Era" });
            _calendars.Add(new Calendar() { CalendarId = 23, CalendarName = "Umm al-Qura" });

        }

    }

}
