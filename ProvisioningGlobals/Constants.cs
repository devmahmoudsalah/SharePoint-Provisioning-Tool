
namespace Karabina.SharePoint.Provisioning
{
    public enum SharePointVersion
    {
        SharePoint_Invalid = 0,
        SharePoint_2013_On_Premises = 2013,
        SharePoint_2016_On_Premises = 20160,
        SharePoint_2016_OnLine = 20161

    } //SharePointVersion

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

    } //RegionalSettingProperties

    public enum ComposedLookProperties
    {
        Name = 0,
        BackgroundFile = 1,
        ColorFile = 2,
        FontFile = 3,
        Version = 4

    } //ComposedLookProperties

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

    } //WebSettingProperties

    public enum TemplateControlType
    {
        Form = 0,
        ListBox = 1,
        TextBox = 2,
        ListView = 3

    } //ControlType

    public enum TemplateItemType
    {
        Template = 0,
        RegionalSetting = 1,
        ComposedLook = 2,
        ContentTypeList = 3,
        ContentTypeGroup = 4,
        ContentTypeItem = 5,
        SiteFieldList = 6,
        SiteFieldGroup = 7,
        SiteFieldItem = 8,
        ListList = 9,
        ListItem = 10,
        ListFieldList = 11,
        ListFieldItem = 12,
        ListViewList = 13,
        ListViewItem = 14,
        FileList = 15,
        FileItem = 16,
        WebSetting = 17,
        PropertyBagEntriesList = 18,
        TermGroupList = 19,
        TermGroupItem = 20,
        TermSetList = 21,
        TermSetItem = 22,
        WorkflowDefinitionList = 23,
        WorkflowDefinitionItem = 24,
        WorkflowSubscriptionList = 25,
        WorkflowSubscriptionItem = 26,
        AddInList = 27,
        AddInItem = 28,
        SiteCustomActionList = 29,
        SiteCustomActionItem = 30,
        WebCustomActionList = 31,
        WebCustomActionItem = 32,
        SiteFeatureList = 33,
        WebFeatureList = 34,
        LocalizationsList = 35,
        LocalizationsItem = 36,
        PageList = 37,
        PageItem = 38,
        PropertiesList = 39,
        PublishingList = 40,
        SupportedUILanguagesList = 41,
        FileWebPartsList = 42,
        FileWebPartItem = 43,
        FileWebPartItemContent = 44,
        SecurityItem = 45

    } //TemplateItemType

    public static class Constants
    {
        public static readonly string SharePoint_2013_On_Premises = "SharePoint 2013 On Premises";
        public static readonly string SharePoint_2016_On_Premises = "SharePoint 2016 On Premises";
        public static readonly string SharePoint_2016_Online = "SharePoint 2016 Online";
        public static readonly string File_Dialog_Filter = "SharePoint Provisioning Template Files (*.pnp)|*.pnp|All Files (*.*)|*.*";
        public static readonly string Enterprise_Wiki_TemplateId = "ENTERWIKI#0";
        public static readonly string Publishing_Feature_Property = "__PublishingFeatureActivated";
        public static readonly string Include = "Include";
        public static readonly string Apply = "Apply";
        public static readonly string Deploy = "Deploy";
        public static readonly string String00 = "String00";
        public static readonly string Source00 = "Source00";
        public static readonly string Edit00 = "Edit00";
        public static readonly string Target00 = "Target00";
        public static readonly string Progress00 = "Progress00";
        public static readonly string Option00 = "Option00";
        public static readonly string Version00 = "Version00";
        public static readonly int Hijri_CalendarId = 6;
        public static readonly int Karabina_Red = 204;
        public static readonly int Karabina_Green = 0;
        public static readonly int Karabina_Blue = 0;
        public static readonly string Add_New = "<Add New>";
        public static readonly string PnP_Supports_SPO_Platform = "PnP_Supports_SPO_Platform";
        public static readonly string PnP_Supports_SP2016_Platform = "PnP_Supports_SP2016_Platform";
        public static readonly string PnP_Supports_SP2013_Platform = "PnP_Supports_SP2013_Platform";

    } //Constants

}
