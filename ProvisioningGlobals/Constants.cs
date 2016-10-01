using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Karabina.SharePoint.Provisioning
{
    public enum SharePointVersion
    {
        SharePoint_Invalid = 0,
        SharePoint_2013_On_Premise = 1,
        SharePoint_2016_On_Premise = 2,
        SharePoint_2016_OnLine = 3
    }

    public enum RegionalSettingProperties
    {
        AdjustHijriDays = 0,
        AlternateCalendarType = 1,
        CalendarType = 2,
        Collation = 3,
        FirstDayOfWeek = 4,
        FirstWeekOfYear = 5,
        LocaleId = 6,
        ShowWeeks = 7,
        Time24 = 8,
        TimeZone = 9,
        WorkDayEndHour = 10,
        WorkDays = 11,
        WorkDayStartHour = 12
    }

    public enum WebSettingProperties
    {
        AlternateCSS = 0,
        CustomMasterPageUrl = 1,
        Description = 2,
        MasterPageUrl = 3,
        NoCrawl = 4,
        RequestAccessEmail = 5,
        SiteLogo = 6,
        Title = 7,
        WelcomePage = 8
    }

    public static class Constants
    {
        public static readonly string SharePoint_2013_On_Premise = "SharePoint 2013 On Premise";
        public static readonly string SharePoint_2016_On_Premise = "SharePoint 2016 On Premise";
        public static readonly string SharePoint_2016_Online = "SharePoint 2016 Online";
        public static readonly string File_Dialog_Filter = "SharePoint Provisioning Template Files (*.pnp)|*.pnp|All Files (*.*)|*.*";
        public static readonly string Enterprise_Wiki_TemplateId = "ENTERWIKI#0";
        public static readonly string Publishing_Feature_Property = "__PublishingFeatureActivated";
        public static readonly string Include = "Include";
        public static readonly string Apply = "Apply";
        public static readonly string String0 = "String0";
        public static readonly string Source0 = "Source0";
        public static readonly string Edit0 = "Edit0";
        public static readonly string Target0 = "Target0";
        public static readonly string Progress0 = "Progress0";
    }
}
