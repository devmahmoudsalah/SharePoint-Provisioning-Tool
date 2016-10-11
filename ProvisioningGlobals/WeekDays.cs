using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Karabina.SharePoint.Provisioning
{
    public class WeekDay
    {
        public int WeekDayId { get; set; }

        public int WeekDayBit { get; set; }

        public string WeekDayLongName { get; set; }

        public string WeekDayShortName { get; set; }

        public WeekDay() { }

    } //WeekDay

    public class WeekDayColllection
    {
        private List<WeekDay> _weekDays = null;

        public List<WeekDay> WeekDays
        {
            get { return _weekDays; }
            private set { _weekDays = value; }
        }

        public WeekDayColllection()
        {
            _weekDays = new List<WeekDay>();
            _weekDays.Add(new WeekDay() { WeekDayId = 0, WeekDayBit = 64, WeekDayLongName = "Sunday", WeekDayShortName = "Sun" });
            _weekDays.Add(new WeekDay() { WeekDayId = 1, WeekDayBit = 32, WeekDayLongName = "Monday", WeekDayShortName = "Mon" });
            _weekDays.Add(new WeekDay() { WeekDayId = 2, WeekDayBit = 16, WeekDayLongName = "Tueday", WeekDayShortName = "Tue" });
            _weekDays.Add(new WeekDay() { WeekDayId = 3, WeekDayBit = 8, WeekDayLongName = "Wednesday", WeekDayShortName = "Wed" });
            _weekDays.Add(new WeekDay() { WeekDayId = 4, WeekDayBit = 4, WeekDayLongName = "Thursday", WeekDayShortName = "Thu" });
            _weekDays.Add(new WeekDay() { WeekDayId = 5, WeekDayBit = 2, WeekDayLongName = "Friday", WeekDayShortName = "Fri" });
            _weekDays.Add(new WeekDay() { WeekDayId = 6, WeekDayBit = 1, WeekDayLongName = "Saterday", WeekDayShortName = "Sat" });

        }

    } //WeekDayColllection

}
