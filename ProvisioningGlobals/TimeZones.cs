using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Karabina.SharePoint.Provisioning
{
    public class TimeZone
    {
        public int TimeZoneId { get; set; }
        public string TimeZoneName { get; set; }

        public TimeZone() { }
    }

    public class TimeZoneCollection
    {
        List<TimeZone> _timeZones = null;

        public List<TimeZone> TimeZones
        {
            get { return _timeZones; }
            private set { _timeZones = value; }
        }

        public TimeZoneCollection()
        {
            _timeZones = new List<TimeZone>();

            _timeZones.Add(new TimeZone() { TimeZoneId = 39, TimeZoneName = "(UTC-12:00) International Date Line West" });
            _timeZones.Add(new TimeZone() { TimeZoneId = 95, TimeZoneName = "(UTC-11:00) Coordinated Universal Time-11" });
            _timeZones.Add(new TimeZone() { TimeZoneId = 15, TimeZoneName = "(UTC-10:00) Hawaii" });
            _timeZones.Add(new TimeZone() { TimeZoneId = 14, TimeZoneName = "(UTC-09:00) Alaska" });
            _timeZones.Add(new TimeZone() { TimeZoneId = 78, TimeZoneName = "(UTC-08:00) Baja California" });
            _timeZones.Add(new TimeZone() { TimeZoneId = 13, TimeZoneName = "(UTC-08:00) Pacific Time (US and Canada)" });
            _timeZones.Add(new TimeZone() { TimeZoneId = 38, TimeZoneName = "(UTC-07:00) Arizona" });
            _timeZones.Add(new TimeZone() { TimeZoneId = 77, TimeZoneName = "(UTC-07:00) Chihuahua, La Paz, Mazatlan" });
            _timeZones.Add(new TimeZone() { TimeZoneId = 12, TimeZoneName = "(UTC-07:00) Mountain Time (US and Canada)" });
            _timeZones.Add(new TimeZone() { TimeZoneId = 55, TimeZoneName = "(UTC-06:00) Central America" });
            _timeZones.Add(new TimeZone() { TimeZoneId = 11, TimeZoneName = "(UTC-06:00) Central Time (US and Canada)" });
            _timeZones.Add(new TimeZone() { TimeZoneId = 37, TimeZoneName = "(UTC-06:00) Guadalajara, Mexico City, Monterrey" });
            _timeZones.Add(new TimeZone() { TimeZoneId = 36, TimeZoneName = "(UTC-06:00) Saskatchewan" });
            _timeZones.Add(new TimeZone() { TimeZoneId = 35, TimeZoneName = "(UTC-05:00) Bogota, Lima, Quito" });
            _timeZones.Add(new TimeZone() { TimeZoneId = 10, TimeZoneName = "(UTC-05:00) Eastern Time (US and Canada)" });
            _timeZones.Add(new TimeZone() { TimeZoneId = 34, TimeZoneName = "(UTC-05:00) Indiana (East)" });
            _timeZones.Add(new TimeZone() { TimeZoneId = 88, TimeZoneName = "(UTC-04:30) Caracas" });
            _timeZones.Add(new TimeZone() { TimeZoneId = 91, TimeZoneName = "(UTC-04:00) Asuncion" });
            _timeZones.Add(new TimeZone() { TimeZoneId = 9, TimeZoneName = "(UTC-04:00) Atlantic Time (Canada)" });
            _timeZones.Add(new TimeZone() { TimeZoneId = 81, TimeZoneName = "(UTC-04:00) Cuiaba" });
            _timeZones.Add(new TimeZone() { TimeZoneId = 33, TimeZoneName = "(UTC-04:00) Georgetown, La Paz, Manaus, San Juan" });
            _timeZones.Add(new TimeZone() { TimeZoneId = 28, TimeZoneName = "(UTC-03:30) Newfoundland" });
            _timeZones.Add(new TimeZone() { TimeZoneId = 8, TimeZoneName = "(UTC-03:00) Brasilia" });
            _timeZones.Add(new TimeZone() { TimeZoneId = 85, TimeZoneName = "(UTC-03:00) Buenos Aires" });
            _timeZones.Add(new TimeZone() { TimeZoneId = 32, TimeZoneName = "(UTC-03:00) Cayenne, Fortaleza" });
            _timeZones.Add(new TimeZone() { TimeZoneId = 60, TimeZoneName = "(UTC-03:00) Greenland" });
            _timeZones.Add(new TimeZone() { TimeZoneId = 90, TimeZoneName = "(UTC-03:00) Montevideo" });
            _timeZones.Add(new TimeZone() { TimeZoneId = 103, TimeZoneName = "(UTC-03:00) Salvador" });
            _timeZones.Add(new TimeZone() { TimeZoneId = 65, TimeZoneName = "(UTC-03:00) Santiago" });
            _timeZones.Add(new TimeZone() { TimeZoneId = 96, TimeZoneName = "(UTC-02:00) Coordinated Universal Time-02" });
            _timeZones.Add(new TimeZone() { TimeZoneId = 30, TimeZoneName = "(UTC-02:00) Mid-Atlantic" });
            _timeZones.Add(new TimeZone() { TimeZoneId = 29, TimeZoneName = "(UTC-01:00) Azores" });
            _timeZones.Add(new TimeZone() { TimeZoneId = 53, TimeZoneName = "(UTC-01:00) Cabo Verde" });
            _timeZones.Add(new TimeZone() { TimeZoneId = 86, TimeZoneName = "(UTC) Casablanca" });
            _timeZones.Add(new TimeZone() { TimeZoneId = 93, TimeZoneName = "(UTC) Coordinated Universal Time" });
            _timeZones.Add(new TimeZone() { TimeZoneId = 2, TimeZoneName = "(UTC) Dublin, Edinburgh, Lisbon, London" });
            _timeZones.Add(new TimeZone() { TimeZoneId = 31, TimeZoneName = "(UTC) Monrovia, Reykjavik" });
            _timeZones.Add(new TimeZone() { TimeZoneId = 4, TimeZoneName = "(UTC+01:00) Amsterdam, Berlin, Bern, Rome, Stockholm, Vienna" });
            _timeZones.Add(new TimeZone() { TimeZoneId = 6, TimeZoneName = "(UTC+01:00) Belgrade, Bratislava, Budapest, Ljubljana, Prague" });
            _timeZones.Add(new TimeZone() { TimeZoneId = 3, TimeZoneName = "(UTC+01:00) Brussels, Copenhagen, Madrid, Paris" });
            _timeZones.Add(new TimeZone() { TimeZoneId = 57, TimeZoneName = "(UTC+01:00) Sarajevo, Skopje, Warsaw, Zagreb" });
            _timeZones.Add(new TimeZone() { TimeZoneId = 69, TimeZoneName = "(UTC+01:00) West Central Africa" });
            _timeZones.Add(new TimeZone() { TimeZoneId = 83, TimeZoneName = "(UTC+01:00) Windhoek" });
            _timeZones.Add(new TimeZone() { TimeZoneId = 79, TimeZoneName = "(UTC+02:00) Amman" });
            _timeZones.Add(new TimeZone() { TimeZoneId = 5, TimeZoneName = "(UTC+02:00) Athens, Bucharest, Istanbul" });
            _timeZones.Add(new TimeZone() { TimeZoneId = 80, TimeZoneName = "(UTC+02:00) Beirut" });
            _timeZones.Add(new TimeZone() { TimeZoneId = 49, TimeZoneName = "(UTC+02:00) Cairo" });
            _timeZones.Add(new TimeZone() { TimeZoneId = 98, TimeZoneName = "(UTC+02:00) Damascus" });
            _timeZones.Add(new TimeZone() { TimeZoneId = 50, TimeZoneName = "(UTC+02:00) Harare, Pretoria" });
            _timeZones.Add(new TimeZone() { TimeZoneId = 59, TimeZoneName = "(UTC+02:00) Helsinki, Kyiv, Riga, Sofia, Tallinn, Vilnius" });
            _timeZones.Add(new TimeZone() { TimeZoneId = 101, TimeZoneName = "(UTC+02:00) Istanbul" });
            _timeZones.Add(new TimeZone() { TimeZoneId = 27, TimeZoneName = "(UTC+02:00) Jerusalem" });
            _timeZones.Add(new TimeZone() { TimeZoneId = 7, TimeZoneName = "(UTC+02:00) Minsk (old)" });
            _timeZones.Add(new TimeZone() { TimeZoneId = 104, TimeZoneName = "(UTC+02:00) E. Europe" });
            _timeZones.Add(new TimeZone() { TimeZoneId = 100, TimeZoneName = "(UTC+02:00) Kaliningrad (RTZ 1)" });
            _timeZones.Add(new TimeZone() { TimeZoneId = 26, TimeZoneName = "(UTC+03:00) Baghdad" });
            _timeZones.Add(new TimeZone() { TimeZoneId = 74, TimeZoneName = "(UTC+03:00) Kuwait, Riyadh" });
            _timeZones.Add(new TimeZone() { TimeZoneId = 109, TimeZoneName = "(UTC+03:00) Minsk" });
            _timeZones.Add(new TimeZone() { TimeZoneId = 51, TimeZoneName = "(UTC+03:00) Moscow, St. Petersburg, Volgograd (RTZ 2)" });
            _timeZones.Add(new TimeZone() { TimeZoneId = 56, TimeZoneName = "(UTC+03:00) Nairobi" });
            _timeZones.Add(new TimeZone() { TimeZoneId = 25, TimeZoneName = "(UTC+03:30) Tehran" });
            _timeZones.Add(new TimeZone() { TimeZoneId = 24, TimeZoneName = "(UTC+04:00) Abu Dhabi, Muscat" });
            _timeZones.Add(new TimeZone() { TimeZoneId = 54, TimeZoneName = "(UTC+04:00) Baku" });
            _timeZones.Add(new TimeZone() { TimeZoneId = 106, TimeZoneName = "(UTC+04:00) Izhevsk, Samara (RTZ 3)" });
            _timeZones.Add(new TimeZone() { TimeZoneId = 89, TimeZoneName = "(UTC+04:00) Port Louis" });
            _timeZones.Add(new TimeZone() { TimeZoneId = 82, TimeZoneName = "(UTC+04:00) Tbilisi" });
            _timeZones.Add(new TimeZone() { TimeZoneId = 84, TimeZoneName = "(UTC+04:00) Yerevan" });
            _timeZones.Add(new TimeZone() { TimeZoneId = 48, TimeZoneName = "(UTC+04:30) Kabul" });
            _timeZones.Add(new TimeZone() { TimeZoneId = 58, TimeZoneName = "(UTC+05:00) Ekaterinburg (RTZ 4)" });
            _timeZones.Add(new TimeZone() { TimeZoneId = 87, TimeZoneName = "(UTC+05:00) Islamabad, Karachi" });
            _timeZones.Add(new TimeZone() { TimeZoneId = 47, TimeZoneName = "(UTC+05:00) Tashkent" });
            _timeZones.Add(new TimeZone() { TimeZoneId = 23, TimeZoneName = "(UTC+05:30) Chennai, Kolkata, Mumbai, New Delhi" });
            _timeZones.Add(new TimeZone() { TimeZoneId = 66, TimeZoneName = "(UTC+05:30) Sri Jayawardenepura" });
            _timeZones.Add(new TimeZone() { TimeZoneId = 62, TimeZoneName = "(UTC+05:45) Kathmandu" });
            _timeZones.Add(new TimeZone() { TimeZoneId = 71, TimeZoneName = "(UTC+06:00) Astana" });
            _timeZones.Add(new TimeZone() { TimeZoneId = 102, TimeZoneName = "(UTC+06:00) Dhaka" });
            _timeZones.Add(new TimeZone() { TimeZoneId = 46, TimeZoneName = "(UTC+06:00) Novosibirsk (RTZ 5)" });
            _timeZones.Add(new TimeZone() { TimeZoneId = 61, TimeZoneName = "(UTC+06:30) Yangon (Rangoon)" });
            _timeZones.Add(new TimeZone() { TimeZoneId = 22, TimeZoneName = "(UTC+07:00) Bangkok, Hanoi, Jakarta" });
            _timeZones.Add(new TimeZone() { TimeZoneId = 64, TimeZoneName = "(UTC+07:00) Krasnoyarsk (RTZ 6)" });
            _timeZones.Add(new TimeZone() { TimeZoneId = 45, TimeZoneName = "(UTC+08:00) Beijing, Chongqing, Hong Kong, Urumqi" });
            _timeZones.Add(new TimeZone() { TimeZoneId = 63, TimeZoneName = "(UTC+08:00) Irkutsk (RTZ 7)" });
            _timeZones.Add(new TimeZone() { TimeZoneId = 21, TimeZoneName = "(UTC+08:00) Kuala Lumpur, Singapore" });
            _timeZones.Add(new TimeZone() { TimeZoneId = 73, TimeZoneName = "(UTC+08:00) Perth" });
            _timeZones.Add(new TimeZone() { TimeZoneId = 75, TimeZoneName = "(UTC+08:00) Taipei" });
            _timeZones.Add(new TimeZone() { TimeZoneId = 94, TimeZoneName = "(UTC+08:00) Ulaanbaatar" });
            _timeZones.Add(new TimeZone() { TimeZoneId = 20, TimeZoneName = "(UTC+09:00) Osaka, Sapporo, Tokyo" });
            _timeZones.Add(new TimeZone() { TimeZoneId = 72, TimeZoneName = "(UTC+09:00) Seoul" });
            _timeZones.Add(new TimeZone() { TimeZoneId = 70, TimeZoneName = "(UTC+09:00) Yakutsk (RTZ 8)" });
            _timeZones.Add(new TimeZone() { TimeZoneId = 19, TimeZoneName = "(UTC+09:30) Adelaide" });
            _timeZones.Add(new TimeZone() { TimeZoneId = 44, TimeZoneName = "(UTC+09:30) Darwin" });
            _timeZones.Add(new TimeZone() { TimeZoneId = 18, TimeZoneName = "(UTC+10:00) Brisbane" });
            _timeZones.Add(new TimeZone() { TimeZoneId = 76, TimeZoneName = "(UTC+10:00) Canberra, Melbourne, Sydney" });
            _timeZones.Add(new TimeZone() { TimeZoneId = 43, TimeZoneName = "(UTC+10:00) Guam, Port Moresby" });
            _timeZones.Add(new TimeZone() { TimeZoneId = 42, TimeZoneName = "(UTC+10:00) Hobart" });
            _timeZones.Add(new TimeZone() { TimeZoneId = 99, TimeZoneName = "(UTC+10:00) Magadan" });
            _timeZones.Add(new TimeZone() { TimeZoneId = 68, TimeZoneName = "(UTC+10:00) Vladivostok, Magadan (RTZ 9)" });
            _timeZones.Add(new TimeZone() { TimeZoneId = 107, TimeZoneName = "(UTC+11:00) Chokurdakh (RTZ 10)" });
            _timeZones.Add(new TimeZone() { TimeZoneId = 41, TimeZoneName = "(UTC+11:00) Solomon Is., New Caledonia" });
            _timeZones.Add(new TimeZone() { TimeZoneId = 108, TimeZoneName = "(UTC+12:00) Anadyr, Petropavlovsk-Kamchatsky (RTZ 11)" });
            _timeZones.Add(new TimeZone() { TimeZoneId = 17, TimeZoneName = "(UTC+12:00) Auckland, Wellington" });
            _timeZones.Add(new TimeZone() { TimeZoneId = 97, TimeZoneName = "(UTC+12:00) Coordinated Universal Time+12" });
            _timeZones.Add(new TimeZone() { TimeZoneId = 40, TimeZoneName = "(UTC+12:00) Fiji" });
            _timeZones.Add(new TimeZone() { TimeZoneId = 92, TimeZoneName = "(UTC+12:00) Petropavlovsk-Kamchatsky - Old" });
            _timeZones.Add(new TimeZone() { TimeZoneId = 67, TimeZoneName = "(UTC+13:00) Nuku'alofa" });
            _timeZones.Add(new TimeZone() { TimeZoneId = 16, TimeZoneName = "(UTC+13:00) Samoa" });

        }
    }

}
