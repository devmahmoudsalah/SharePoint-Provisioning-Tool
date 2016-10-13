using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Karabina.SharePoint.Provisioning
{
    public class FirstWeek
    {
        public int FirstWeekId { get; set; }

        public string FirstWeekName { get; set; }

        public FirstWeek() { }

    }

    public class FirstWeekCollection
    {
        private List<FirstWeek> _firstWeeks = null;

        public List<FirstWeek> FirstWeeks
        {
            get { return _firstWeeks; }
            private set { _firstWeeks = value; }
        }

        public string DisplayMember
        {
            get { return "FirstWeekName"; }

        } //DisplayMember

        public string ValueMember
        {
            get { return "FirstWeekId"; }

        } //ValueMember

        public FirstWeekCollection()
        {
            _firstWeeks = new List<FirstWeek>();

            _firstWeeks.Add(new FirstWeek() { FirstWeekId = 0, FirstWeekName = "Starts on Jan 1" });
            _firstWeeks.Add(new FirstWeek() { FirstWeekId = 1, FirstWeekName = "First full week" });
            _firstWeeks.Add(new FirstWeek() { FirstWeekId = 2, FirstWeekName = "First 4 - day week" });

        }

    }

}
