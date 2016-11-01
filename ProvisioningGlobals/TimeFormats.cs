using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Karabina.SharePoint.Provisioning
{
    public class TimeFormat
    {
        public int TimeFormatId { get; set; }

        public string TimeFormatName { get; set; }

        public TimeFormat() { }

    } //TimeFormat

    public class TimeFormatCollection
    {
        private List<TimeFormat> _timeFormats = null;

        public List<TimeFormat> TimeFormats
        {
            get { return _timeFormats; }
            private set { _timeFormats = value; }

        } //TimeFormats

        public string DisplayMember
        {
            get { return "TimeFormatName"; }

        } //DisplayMember

        public string ValueMember
        {
            get { return "TimeFormatId"; }

        } //ValueMember

        public TimeFormatCollection()
        {
            _timeFormats = new List<TimeFormat>();

            _timeFormats.Add(new TimeFormat() { TimeFormatId = 0, TimeFormatName = "12 Hour" });
            _timeFormats.Add(new TimeFormat() { TimeFormatId = 1, TimeFormatName = "24 Hour" });

        } //TimeFormatCollection

    } //TimeFormatCollection

}
