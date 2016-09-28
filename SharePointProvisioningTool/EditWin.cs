using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Karabina.SharePoint.Provisioning
{
    public partial class EditWin : Form
    {
        private string _activeNodePath = string.Empty;

        private SharePointVersion _selectedVersion = SharePointVersion.SharePoint_Invalid;

        public SharePointVersion SelectedVersion
        {
            get { return _selectedVersion; }
            set { _selectedVersion = value; }
        }

        public SharePoint2013OnPrem SP2013OP { get; set; }

        public SharePoint2016OnPrem SP2016OP { get; set; }

        public SharePoint2016Online SP2016OL { get; set; }

        public delegate string OpenTemplateDelegate();

        public OpenTemplateDelegate OpenTemplate;

        public delegate void SetStatusTextDelegate(string message);

        public SetStatusTextDelegate SetStatusBarText;

        public EditWin()
        {
            InitializeComponent();
        }

        private void bBrowse_Click(object sender, EventArgs e)
        {
            string fileName = OpenTemplate();
            if (!string.IsNullOrWhiteSpace(fileName))
            {
                tbTemplate.Text = fileName;

                try
                {
                    Cursor = Cursors.WaitCursor;

                    int slashPosition = tbTemplate.Text.LastIndexOf('\\');
                    string path = tbTemplate.Text.Substring(0, slashPosition);
                    slashPosition++;
                    string name = tbTemplate.Text.Substring(slashPosition).Replace(".pnp", "");

                    switch (_selectedVersion)
                    {
                        case SharePointVersion.SharePoint_2013_On_Premise:
                            SP2013OP.OpenTemplateForEdit(path, name, tvTemplate);
                            break;
                        case SharePointVersion.SharePoint_2016_On_Premise:
                            break;
                        case SharePointVersion.SharePoint_2016_OnLine:
                            break;
                    } // switch

                } // try
                catch
                {

                }
                finally
                {
                    Cursor = Cursors.Default;
                }

            } // if

        } //bBrowse_Click

        private void bClose_Click(object sender, EventArgs e)
        {
            Close();
        } //bClose_Click


        private void PopulateRegionalSettings()
        {
            int[] regionalSettings = null;
            switch (_selectedVersion)
            {
                case SharePointVersion.SharePoint_2013_On_Premise:
                    regionalSettings = SP2013OP.GetRegionalSettingProperty();
                    break;
                case SharePointVersion.SharePoint_2016_On_Premise:

                    break;
                case SharePointVersion.SharePoint_2016_OnLine:
                    break;
            }

            cbTimeZone.Items.Clear();
            TimeZoneCollection timeZoneCollection = new TimeZoneCollection();
            cbTimeZone.DisplayMember = "TimeZoneName";
            cbTimeZone.ValueMember = "TimeZoneId";
            cbTimeZone.DataSource = timeZoneCollection.TimeZones;

            int selectedIdx = -1;
            int tmpIdx = regionalSettings[RegionalSettingProperty.TimeZone.GetHashCode()];
            for (int i = 0; i < cbTimeZone.Items.Count; i++)
            {
                TimeZone timeZone = cbTimeZone.Items[i] as TimeZone;
                if (timeZone.TimeZoneId == tmpIdx)
                {
                    selectedIdx = i;
                    break;
                }
            }
            cbTimeZone.SelectedIndex = selectedIdx;

            cbLocale.Items.Clear();
            LocaleCollection localeCollection = new LocaleCollection();
            cbLocale.DisplayMember = "LocaleName";
            cbLocale.ValueMember = "LocaleId";
            cbLocale.DataSource = localeCollection.Locales;

            selectedIdx = -1;
            tmpIdx = regionalSettings[RegionalSettingProperty.LocaleId.GetHashCode()];
            for (int i = 0; i < cbLocale.Items.Count; i++)
            {
                Locale locale = cbLocale.Items[i] as Locale;
                if (locale.LocaleId == tmpIdx)
                {
                    selectedIdx = i;
                    break;
                }
            }
            cbLocale.SelectedIndex = selectedIdx;

            cbSortOrder.Items.Clear();
            SortOrderCollection sortOrderCollection = new SortOrderCollection();
            cbSortOrder.DisplayMember = "SortOrderName";
            cbSortOrder.ValueMember = "SortOrderId";
            cbSortOrder.DataSource = sortOrderCollection.SortOrders;

            selectedIdx = -1;
            tmpIdx = regionalSettings[RegionalSettingProperty.Collation.GetHashCode()];
            for (int i = 0; i < cbSortOrder.Items.Count; i++)
            {
                SortOrder sortOrder = cbSortOrder.Items[i] as SortOrder;
                if (sortOrder.SortOrderId == tmpIdx)
                {
                    selectedIdx = i;
                    break;
                }
            }
            cbSortOrder.SelectedIndex = selectedIdx;

            cbCalendar.Items.Clear();
            CalendarCollection calendarCollection = new CalendarCollection();
            cbCalendar.DisplayMember = "CalendarName";
            cbCalendar.ValueMember = "CalendarId";
            cbCalendar.DataSource = calendarCollection.Calendars.Where(p => p.CalendarId > 0).ToList();

            selectedIdx = -1;
            tmpIdx = regionalSettings[RegionalSettingProperty.CalendarType.GetHashCode()];
            bool isHijriSelected = tmpIdx == 6; //Hijri
            for (int i = 0; i < cbCalendar.Items.Count; i++)
            {
                Calendar calendar = cbCalendar.Items[i] as Calendar;
                if (calendar.CalendarId == tmpIdx)
                {
                    selectedIdx = i;
                    break;
                }
            }
            cbCalendar.SelectedIndex = selectedIdx;

            tmpIdx = regionalSettings[RegionalSettingProperty.ShowWeeks.GetHashCode()];
            if (tmpIdx > 0)
            {
                cbShowWeekNumbers.Checked = true;
            }
            else
            {
                cbShowWeekNumbers.Checked = false;
            }

            if (isHijriSelected)
            {
                lAdjustHijriDate.Visible = true;
                cbAdjustHijriDate.Visible = true;
                tmpIdx = 2 + regionalSettings[RegionalSettingProperty.AdjustHijriDays.GetHashCode()];
                cbAdjustHijriDate.SelectedIndex = tmpIdx;
            }
            else
            {
                lAdjustHijriDate.Visible = false;
                cbAdjustHijriDate.Visible = false;
                cbAdjustHijriDate.SelectedIndex = 2; // equal to 0 value
            }

            cbAlternateCalendar.Items.Clear();
            cbAlternateCalendar.DisplayMember = "CalendarName";
            cbAlternateCalendar.ValueMember = "CalendarId";
            cbAlternateCalendar.DataSource = calendarCollection.Calendars.ToList();

            selectedIdx = -1;
            tmpIdx = regionalSettings[RegionalSettingProperty.AlternateCalendarType.GetHashCode()];
            for (int i = 0; i < cbAlternateCalendar.Items.Count; i++)
            {
                Calendar calendar = cbAlternateCalendar.Items[i] as Calendar;
                if (calendar.CalendarId == tmpIdx)
                {
                    selectedIdx = i;
                    break;
                }
            }
            cbAlternateCalendar.SelectedIndex = selectedIdx;

            clbWorkDays.Items.Clear();
            tmpIdx = regionalSettings[RegionalSettingProperty.WorkDays.GetHashCode()];
            WeekDayCollletion weekDayCollection = new WeekDayCollletion();
            foreach (WeekDay weekDay in weekDayCollection.WeekDays)
            {
                clbWorkDays.Items.Add(weekDay.WeekDayShortName, ((tmpIdx & weekDay.WeekDayBit) == weekDay.WeekDayBit));
            }

            cbFirstDayOfWeek.Items.Clear();
            cbFirstDayOfWeek.DisplayMember = "WeekDayLongName";
            cbFirstDayOfWeek.ValueMember = "WeekDayId";
            cbFirstDayOfWeek.DataSource = weekDayCollection.WeekDays;
            cbFirstDayOfWeek.SelectedIndex = regionalSettings[RegionalSettingProperty.FirstDayOfWeek.GetHashCode()];

            cbFirstWeekOfYear.Items.Clear();
            FirstWeekCollection firstWeekCollection = new FirstWeekCollection();
            cbFirstWeekOfYear.DisplayMember = "FirstWeekName";
            cbFirstWeekOfYear.ValueMember = "FirstWeekId";
            cbFirstWeekOfYear.DataSource = firstWeekCollection.FirstWeeks;
            cbFirstWeekOfYear.SelectedIndex = regionalSettings[RegionalSettingProperty.FirstWeekOfYear.GetHashCode()];

            cbWorkDayStartTime.Items.Clear();
            DayHourCollection dayHourCollection = new DayHourCollection();
            cbWorkDayStartTime.DisplayMember = "DayHourName";
            cbWorkDayStartTime.ValueMember = "DayHourId";
            cbWorkDayStartTime.DataSource = dayHourCollection.DayHours.ToList();
            cbWorkDayStartTime.SelectedIndex = regionalSettings[RegionalSettingProperty.WorkDayStartHour.GetHashCode()];

            cbWorkDayEndTime.Items.Clear();
            cbWorkDayEndTime.DisplayMember = "DayHourName";
            cbWorkDayEndTime.ValueMember = "DayHourId";
            cbWorkDayEndTime.DataSource = dayHourCollection.DayHours.ToList();
            cbWorkDayEndTime.SelectedIndex = regionalSettings[RegionalSettingProperty.WorkDayEndHour.GetHashCode()];

            cbTimeFormat.Items.Clear();
            TimeFormatCollection timeFormatCollection = new TimeFormatCollection();
            cbTimeFormat.DisplayMember = "TimeFormatName";
            cbTimeFormat.ValueMember = "TimeFormatId";
            cbTimeFormat.DataSource = timeFormatCollection.TimeFormats;
            cbTimeFormat.SelectedIndex = regionalSettings[RegionalSettingProperty.Time24.GetHashCode()];

        } //PopulateRegionalSettings

        private void PopulateComposedLook()
        {
            string[] composedLook = null;
            switch (_selectedVersion)
            {
                case SharePointVersion.SharePoint_2013_On_Premise:
                    composedLook = SP2013OP.GetComposedLook();
                    break;
                case SharePointVersion.SharePoint_2016_On_Premise:

                    break;
                case SharePointVersion.SharePoint_2016_OnLine:
                    break;
            }

            tbComposedLookName.Text = composedLook[0];
            tbBackgroundFile.Text = composedLook[1];
            tbColorFile.Text = composedLook[2];
            tbFontFile.Text = composedLook[3];
            tbComposedLookVersion.Text = composedLook[4];

        } //PopulateComposedLook

        private void PopulateContentType(string contentTypeId)
        {
            string contentType = string.Empty;
            switch (_selectedVersion)
            {
                case SharePointVersion.SharePoint_2013_On_Premise:
                    contentType = SP2013OP.GetContentType(contentTypeId);
                    break;
                case SharePointVersion.SharePoint_2016_On_Premise:

                    break;
                case SharePointVersion.SharePoint_2016_OnLine:
                    break;
            }
            tbContentType.Text = contentType;

        } //PopulateContentType

        private void PopulateListInstance(string url)
        {
            string listInstance = string.Empty;
            switch (_selectedVersion)
            {
                case SharePointVersion.SharePoint_2013_On_Premise:
                    listInstance = SP2013OP.GetListInstance(url);
                    break;
                case SharePointVersion.SharePoint_2016_On_Premise:

                    break;
                case SharePointVersion.SharePoint_2016_OnLine:
                    break;
            }
            tbListInstance.Text = listInstance;

        } //PopulateListInstance

        private void HideActivePanel(string nodeFullPath)
        {
            if (!string.IsNullOrWhiteSpace(_activeNodePath))
            {
                if (_activeNodePath.Contains(@"\Regional Settings"))
                {
                    pRegionalSettings.Visible = false;
                    pRegionalSettings.Left = 1038;
                }
                else if (_activeNodePath.Contains(@"\Add-Ins"))
                {
                    if (!nodeFullPath.Contains(@"\Add-Ins\"))
                    {

                    }
                }
                else if (_activeNodePath.Contains(@"\Composed Look"))
                {
                    pComposedLook.Visible = false;
                    pComposedLook.Left = 1038;
                }
                else if (_activeNodePath.Contains(@"\Site Custom Actions"))
                {

                }
                else if (_activeNodePath.Contains(@"\Web Custom Actions"))
                {

                }
                else if (_activeNodePath.Contains(@"\Site Features"))
                {

                }
                else if (_activeNodePath.Contains(@"\Web Features"))
                {

                }
                else if (_activeNodePath.Contains(@"\Content Types"))
                {
                    if (!nodeFullPath.Contains(@"\Content Types\"))
                    {
                        pContentTypes.Visible = false;
                        pContentTypes.Left = 1038;
                    }
                }
                else if (_activeNodePath.Contains(@"\Site Fields"))
                {
                    if (!nodeFullPath.Contains(@"\Site Fields\"))
                    {
                        pSiteFields.Visible = false;
                        pSiteFields.Left = 1038;
                    }
                }
                else if (_activeNodePath.Contains(@"\Files"))
                {
                    if (!nodeFullPath.Contains(@"\Files\"))
                    {

                    }
                }
                else if (_activeNodePath.Contains(@"\Lists"))
                {
                    if (!nodeFullPath.Contains(@"\Lists\"))
                    {
                        pLists.Visible = false;
                        pLists.Left = 1038;
                    }
                    else if (!nodeFullPath.Contains(@"\Views\"))
                    {
                        pLists.Visible = false;
                        pLists.Left = 1038;
                        pViews.Visible = false;
                        pViews.Left = 1038;
                    }
                    else if (nodeFullPath.Contains(@"\Views\"))
                    {
                        pLists.Visible = false;
                        pLists.Left = 1038;
                    }
                }
                else if (_activeNodePath.Contains(@"\Localizations"))
                {
                    if (!nodeFullPath.Contains(@"\Localizations\"))
                    {

                    }
                }
                else if (_activeNodePath.Contains(@"\Pages"))
                {
                    if (!nodeFullPath.Contains(@"\Pages\"))
                    {

                    }
                }
                else if (_activeNodePath.Contains(@"\Properties"))
                {

                }
                else if (_activeNodePath.Contains(@"\Property Bag Entries"))
                {

                }
                else if (_activeNodePath.Contains(@"\Publishing"))
                {

                }
                else if (_activeNodePath.Contains(@"\Supported UI Languages"))
                {

                }
                else if (_activeNodePath.Contains(@"\Term Groups"))
                {
                    if (!nodeFullPath.Contains(@"\Term Groups\"))
                    {

                    }
                }
                else if (_activeNodePath.Contains(@"\Web Settings"))
                {

                }
                else if (_activeNodePath.Contains(@"\Workflow Definitions"))
                {
                    if (!nodeFullPath.Contains(@"\Workflow Definitions\"))
                    {

                    }
                }
                else if (_activeNodePath.Contains(@"\Workflow Subscriptions"))
                {
                    if (!nodeFullPath.Contains(@"\Workflow Subscriptions\"))
                    {

                    }
                }


            } //if

        } // HideActivePanel

        private void NodeSelected(object sender, TreeViewEventArgs e)
        {
            TreeNode node = e.Node;
            if (node != null)
            {
                if (!node.FullPath.Equals(_activeNodePath, StringComparison.OrdinalIgnoreCase))
                {
                    HideActivePanel(node.FullPath);
                }

                _activeNodePath = node.FullPath;
                if (_activeNodePath.Contains(@"\Regional Settings"))
                {
                    if (cbTimeZone.Items.Count <= 0)
                    {
                        PopulateRegionalSettings();
                    }
                    pRegionalSettings.Left = 438;
                    pRegionalSettings.Visible = true;

                }
                else if (_activeNodePath.Contains(@"\Composed Look"))
                {
                    if (string.IsNullOrWhiteSpace(tbComposedLookName.Text))
                    {
                        PopulateComposedLook();
                    }
                    pComposedLook.Left = 438;
                    pComposedLook.Visible = true;

                }
                else if (_activeNodePath.Contains(@"\Content Types\"))
                {
                    string tagCT = node.Tag.ToString();
                    if (tagCT.StartsWith("{"))
                    {
                        tbContentType.Text = tagCT;
                    }
                    else
                    {
                        PopulateContentType(node.Tag.ToString());
                        node.Tag = tbContentType.Text;
                    }
                    pContentTypes.Left = 438;
                    pContentTypes.Visible = true;

                }
                else if (_activeNodePath.Contains(@"\Site Fields\"))
                {
                    string tagSF = node.Tag.ToString();
                    tbSiteField.Text = tagSF;
                    pSiteFields.Left = 438;
                    pSiteFields.Visible = true;

                }
                else if (_activeNodePath.Contains(@"\Lists\"))
                {
                    if (_activeNodePath.Contains(@"\Views"))
                    {
                        if (_activeNodePath.Contains(@"\Views\"))
                        {
                            string tagLV = node.Tag.ToString();
                            tbView.Text = tagLV;
                            pViews.Left = 438;
                            pViews.Visible = true;

                        }

                    }
                    else
                    {
                        string tagLI = node.Tag.ToString();
                        if (tagLI.StartsWith("{"))
                        {
                            tbListInstance.Text = tagLI;
                        }
                        else
                        {
                            PopulateListInstance(node.Tag.ToString());
                            node.Tag = tbListInstance.Text;
                        }
                        pLists.Left = 438;
                        pLists.Visible = true;

                    }

                }

            }

        } //NodeSelected

        private void CalendarItemSelected(object sender, EventArgs e)
        {
            Calendar calendar = cbCalendar.SelectedItem as Calendar;
            if (calendar.CalendarId == 6) //Hijri
            {
                lAdjustHijriDate.Visible = true;
                cbAdjustHijriDate.Visible = true;
            }
            else
            {
                cbAdjustHijriDate.Visible = false;
                lAdjustHijriDate.Visible = false;
            }

        } //CalendarItemSelected

    } //EditWin

}
