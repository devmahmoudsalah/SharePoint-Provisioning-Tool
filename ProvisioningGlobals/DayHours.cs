using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Karabina.SharePoint.Provisioning
{
    public class DayHour
    {
        public int DayHourId { get; set; }

        public string DayHourName { get; set; }

        public int MinutesFromMidnight
        {
            get { return DayHourId * 60; }
            private set { }

        } //MinutesFromMidnight

        public DayHour() { }

    } //DayHour

    public class DayHourCollection
    {
        private List<DayHour> _dayHours = null;

        public List<DayHour> DayHours
        {
            get { return _dayHours; }
            private set { _dayHours = value; }

        } //DayHours

        public string DisplayMember
        {
            get { return "DayHourName"; }

        } //DisplayMember

        public string ValueMember
        {
            get { return "DayHourId"; }

        } //ValueMember

        public DayHourCollection()
        {
            _dayHours = new List<DayHour>();
            for (int i = 0; i < 24; i++)
            {
                _dayHours.Add(new DayHour() { DayHourId = i, DayHourName = string.Format("{0:00}:00", i) });

            }

        } //DayHourCollection

    } //DayHourCollection

}