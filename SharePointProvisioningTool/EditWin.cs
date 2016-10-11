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
        private TreeNode _selectedNode = null;
        private TemplateItems _templateItems = null;

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
            //panels are moved offscreen during design, now move them back to where they will appear.
            int left = 438;
            pComposedLook.Left = left;
            pTextControl.Left = left;
            pViewControl.Left = left;
            pRegionalSettings.Left = left;
            pWebSettings.Left = left;
            pListControl.Left = left;

        }

        private void BrowseForTemplate(object sender, EventArgs e)
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
                            _templateItems = SP2013OP.OpenTemplateForEdit(path, name, tvTemplate);
                            break;
                        case SharePointVersion.SharePoint_2016_On_Premise:
                            break;
                        case SharePointVersion.SharePoint_2016_OnLine:
                            break;
                    } // switch

                } // try
                catch (Exception ex)
                {
                    string msg = "Error:\r\n" + ex.Message + "\r\n\r\nPlease try again.";
                    MessageBox.Show(msg, "Error Loading Template", MessageBoxButtons.OK, MessageBoxIcon.Error);

                } //catch
                finally
                {
                    if (tvTemplate.Nodes?.Count > 0)
                    {
                        tvTemplate.SelectedNode = tvTemplate.Nodes["TemplateNode"];
                        bSave.Visible = false;

                    }

                    Cursor = Cursors.Default;

                } //finally

            }

        } //BrowseForTemplate

        private void bClose_Click(object sender, EventArgs e)
        {
            Close();

        } //bClose_Click


        private void PopulateRegionalSettings(TemplateItem templateItem)
        {
            int[] regionalSettings = (int[])templateItem.Content;
            if (regionalSettings?.Length > 0)
            {
                cbRSTimeZone.DataSource = null;
                cbRSTimeZone.Items.Clear();
                TimeZoneCollection timeZoneCollection = new TimeZoneCollection();
                cbRSTimeZone.DisplayMember = "TimeZoneName";
                cbRSTimeZone.ValueMember = "TimeZoneId";
                cbRSTimeZone.DataSource = timeZoneCollection.TimeZones;

                int selectedIdx = -1;
                int tmpIdx = regionalSettings[(int)RegionalSettingProperties.TimeZone];
                for (int i = 0; i < cbRSTimeZone.Items.Count; i++)
                {
                    TimeZone timeZone = cbRSTimeZone.Items[i] as TimeZone;
                    if (timeZone.TimeZoneId == tmpIdx)
                    {
                        selectedIdx = i;
                        break;

                    }

                }

                cbRSTimeZone.SelectedIndex = selectedIdx;

                cbRSLocale.DataSource = null;
                cbRSLocale.Items.Clear();
                LocaleCollection localeCollection = new LocaleCollection();
                cbRSLocale.DisplayMember = "LocaleName";
                cbRSLocale.ValueMember = "LocaleId";
                cbRSLocale.DataSource = localeCollection.Locales;

                selectedIdx = -1;
                tmpIdx = regionalSettings[(int)RegionalSettingProperties.LocaleId];
                for (int i = 0; i < cbRSLocale.Items.Count; i++)
                {
                    Locale locale = cbRSLocale.Items[i] as Locale;
                    if (locale.LocaleId == tmpIdx)
                    {
                        selectedIdx = i;
                        break;

                    }

                }

                cbRSLocale.SelectedIndex = selectedIdx;

                cbRSSortOrder.DataSource = null;
                cbRSSortOrder.Items.Clear();
                SortOrderCollection sortOrderCollection = new SortOrderCollection();
                cbRSSortOrder.DisplayMember = "SortOrderName";
                cbRSSortOrder.ValueMember = "SortOrderId";
                cbRSSortOrder.DataSource = sortOrderCollection.SortOrders;

                selectedIdx = -1;
                tmpIdx = regionalSettings[(int)RegionalSettingProperties.Collation];
                for (int i = 0; i < cbRSSortOrder.Items.Count; i++)
                {
                    SortOrder sortOrder = cbRSSortOrder.Items[i] as SortOrder;
                    if (sortOrder.SortOrderId == tmpIdx)
                    {
                        selectedIdx = i;
                        break;

                    }

                }

                cbRSSortOrder.SelectedIndex = selectedIdx;

                cbRSCalendar.DataSource = null;
                cbRSCalendar.Items.Clear();
                CalendarCollection calendarCollection = new CalendarCollection();
                cbRSCalendar.DisplayMember = "CalendarName";
                cbRSCalendar.ValueMember = "CalendarId";
                cbRSCalendar.DataSource = calendarCollection.Calendars.Where(p => p.CalendarId > 0).ToList();

                selectedIdx = -1;
                tmpIdx = regionalSettings[(int)RegionalSettingProperties.CalendarType];
                bool isHijriSelected = tmpIdx == 6; //Hijri
                for (int i = 0; i < cbRSCalendar.Items.Count; i++)
                {
                    Calendar calendar = cbRSCalendar.Items[i] as Calendar;
                    if (calendar.CalendarId == tmpIdx)
                    {
                        selectedIdx = i;
                        break;

                    }

                }

                cbRSCalendar.SelectedIndex = selectedIdx;

                tmpIdx = regionalSettings[(int)RegionalSettingProperties.ShowWeeks];
                if (tmpIdx > 0)
                {
                    cbRSShowWeekNumbers.Checked = true;

                }
                else
                {
                    cbRSShowWeekNumbers.Checked = false;

                }

                if (isHijriSelected)
                {
                    lAdjustHijriDate.Visible = true;
                    cbRSAdjustHijriDate.Visible = true;
                    tmpIdx = 2 + regionalSettings[(int)RegionalSettingProperties.AdjustHijriDays];
                    cbRSAdjustHijriDate.SelectedIndex = tmpIdx;

                }
                else
                {
                    lAdjustHijriDate.Visible = false;
                    cbRSAdjustHijriDate.Visible = false;
                    cbRSAdjustHijriDate.SelectedIndex = 2; // equal to 0 value

                }

                cbRSAlternateCalendar.DataSource = null;
                cbRSAlternateCalendar.Items.Clear();
                cbRSAlternateCalendar.DisplayMember = "CalendarName";
                cbRSAlternateCalendar.ValueMember = "CalendarId";
                cbRSAlternateCalendar.DataSource = calendarCollection.Calendars.ToList();

                selectedIdx = -1;
                tmpIdx = regionalSettings[(int)RegionalSettingProperties.AlternateCalendarType];
                for (int i = 0; i < cbRSAlternateCalendar.Items.Count; i++)
                {
                    Calendar calendar = cbRSAlternateCalendar.Items[i] as Calendar;
                    if (calendar.CalendarId == tmpIdx)
                    {
                        selectedIdx = i;
                        break;

                    }

                }

                cbRSAlternateCalendar.SelectedIndex = selectedIdx;

                clbRSWorkDays.Items.Clear();
                tmpIdx = regionalSettings[(int)RegionalSettingProperties.WorkDays];
                WeekDayColllection weekDayCollection = new WeekDayColllection();
                foreach (WeekDay weekDay in weekDayCollection.WeekDays)
                {
                    clbRSWorkDays.Items.Add(weekDay.WeekDayShortName, ((tmpIdx & weekDay.WeekDayBit) == weekDay.WeekDayBit));

                }

                cbRSFirstDayOfWeek.DataSource = null;
                cbRSFirstDayOfWeek.Items.Clear();
                cbRSFirstDayOfWeek.DisplayMember = "WeekDayLongName";
                cbRSFirstDayOfWeek.ValueMember = "WeekDayId";
                cbRSFirstDayOfWeek.DataSource = weekDayCollection.WeekDays;
                cbRSFirstDayOfWeek.SelectedIndex = regionalSettings[(int)RegionalSettingProperties.FirstDayOfWeek];

                cbRSFirstWeekOfYear.DataSource = null;
                cbRSFirstWeekOfYear.Items.Clear();
                FirstWeekCollection firstWeekCollection = new FirstWeekCollection();
                cbRSFirstWeekOfYear.DisplayMember = "FirstWeekName";
                cbRSFirstWeekOfYear.ValueMember = "FirstWeekId";
                cbRSFirstWeekOfYear.DataSource = firstWeekCollection.FirstWeeks;
                cbRSFirstWeekOfYear.SelectedIndex = regionalSettings[(int)RegionalSettingProperties.FirstWeekOfYear];

                cbRSWorkDayStartTime.DataSource = null;
                cbRSWorkDayStartTime.Items.Clear();
                DayHourCollection dayHourCollection = new DayHourCollection();
                cbRSWorkDayStartTime.DisplayMember = "DayHourName";
                cbRSWorkDayStartTime.ValueMember = "DayHourId";
                cbRSWorkDayStartTime.DataSource = dayHourCollection.DayHours.ToList();
                cbRSWorkDayStartTime.SelectedIndex = regionalSettings[(int)RegionalSettingProperties.WorkDayStartHour];

                cbRSWorkDayEndTime.DataSource = null;
                cbRSWorkDayEndTime.Items.Clear();
                cbRSWorkDayEndTime.DisplayMember = "DayHourName";
                cbRSWorkDayEndTime.ValueMember = "DayHourId";
                cbRSWorkDayEndTime.DataSource = dayHourCollection.DayHours.ToList();
                cbRSWorkDayEndTime.SelectedIndex = regionalSettings[(int)RegionalSettingProperties.WorkDayEndHour];

                cbRSTimeFormat.DataSource = null;
                cbRSTimeFormat.Items.Clear();
                TimeFormatCollection timeFormatCollection = new TimeFormatCollection();
                cbRSTimeFormat.DisplayMember = "TimeFormatName";
                cbRSTimeFormat.ValueMember = "TimeFormatId";
                cbRSTimeFormat.DataSource = timeFormatCollection.TimeFormats;
                cbRSTimeFormat.SelectedIndex = regionalSettings[(int)RegionalSettingProperties.Time24];

            }

        } //PopulateRegionalSettings

        private void PopulateComposedLook(TemplateItem templateItem)
        {
            string[] composedLook = (string[])templateItem.Content;
            if (composedLook?.Length > 0)
            {
                tbCLName.Text = composedLook[(int)ComposedLookProperties.Name];
                tbCLBackgroundFile.Text = composedLook[(int)ComposedLookProperties.BackgroundFile];
                tbCLColorFile.Text = composedLook[(int)ComposedLookProperties.ColorFile];
                tbCLFontFile.Text = composedLook[(int)ComposedLookProperties.FontFile];
                tbCLVersion.Text = composedLook[(int)ComposedLookProperties.Version];

            }

        } //PopulateComposedLook

        private void PopulateWebSettings(TemplateItem templateItem)
        {
            string[] ws = (string[])templateItem.Content;

            if (ws?.Length > 0)
            {
                tbWSTitle.Text = ws[(int)WebSettingProperties.Title];
                tbWSDescription.Text = ws[(int)WebSettingProperties.Description];
                tbWSSiteLogo.Text = ws[(int)WebSettingProperties.SiteLogo];
                tbWSMasterPageUrl.Text = ws[(int)WebSettingProperties.MasterPageUrl];
                tbWSCustomMasterPageUrl.Text = ws[(int)WebSettingProperties.CustomMasterPageUrl];
                tbWSAlternateCSS.Text = ws[(int)WebSettingProperties.AlternateCSS];
                tbWSWelcomePage.Text = ws[(int)WebSettingProperties.WelcomePage];
                tbWSRequestAccessEmail.Text = ws[(int)WebSettingProperties.RequestAccessEmail];
                cbWSNoCrawl.Checked = (ws[(int)WebSettingProperties.NoCrawl].Equals("1"));

            }

        } //PopulateWebSettings

        private void PopulateControlText(TemplateItem templateItem)
        {
            tbTextControl.Text = (string)templateItem.Content;

        } //PopulateControlText

        private void PopulateControlListView(TemplateItem templateItem)
        {
            lvViewControl.Items.Clear();

            KeyValueList keyValueList = (KeyValueList)templateItem.Content;

            if (keyValueList?.Count > 0)
            {
                foreach (var keyValue in keyValueList)
                {
                    var listViewItem = lvViewControl.Items.Add(keyValue.Key);
                    listViewItem.SubItems.Add(keyValue.Value);

                }
            }

        } //PopulatePropertyBagEntries

        private void PopulateControlList(TemplateItem templateItem)
        {
            lbListControl.DataSource = null;
            lbListControl.Items.Clear();

            KeyValueList keyValueList = (KeyValueList)templateItem.Content;

            if (keyValueList?.Count > 0)
            {
                lbListControl.DisplayMember = "Key";
                lbListControl.ValueMember = "Value";
                lbListControl.DataSource = keyValueList;

            }

        } //PopulateControlList

        private void HidePanels()
        {
            pComposedLook.Hide();
            pListControl.Hide();
            pViewControl.Hide();
            pRegionalSettings.Hide();
            pTextControl.Hide();
            pWebSettings.Hide();

        } // HideActivePanel

        private void NodeSelected(object sender, TreeViewEventArgs e)
        {
            TreeNode node = e.Node;
            if (node != null)
            {
                HidePanels();

                //Ensure save button is not triggered
                _selectedNode = null;

                TemplateItem templateItem = _templateItems.GetItem((string)node.Tag);

                if (templateItem != null)
                {
                    switch (templateItem.ControlType)
                    {
                        case TemplateControlType.Form:
                            switch (templateItem.ItemType)
                            {
                                case TemplateItemType.RegionalSetting:
                                    PopulateRegionalSettings(templateItem);
                                    pRegionalSettings.Show();

                                    break;

                                case TemplateItemType.ComposedLook:
                                    PopulateComposedLook(templateItem);
                                    pComposedLook.Show();

                                    break;

                                case TemplateItemType.WebSetting:
                                    PopulateWebSettings(templateItem);
                                    pWebSettings.Show();

                                    break;

                                default:
                                    break;

                            }

                            break;

                        case TemplateControlType.ListBox:
                            lListControl.Text = node.Text;
                            PopulateControlList(templateItem);
                            pListControl.Show();

                            break;

                        case TemplateControlType.ListView:
                            lViewControl.Text = node.Text;
                            PopulateControlListView(templateItem);
                            pViewControl.Show();

                            break;

                        case TemplateControlType.TextBox:
                            lTextControl.Text = node.Text;
                            PopulateControlText(templateItem);
                            pTextControl.Show();

                            break;

                        default:
                            break;

                    }

                }

                _selectedNode = node;

            }

        } //NodeSelected

        private void CalendarItemSelected(object sender, EventArgs e)
        {
            int value = (int)cbRSCalendar.SelectedValue;
            if (value == Constants.Hijri_CalendarId)
            {
                lAdjustHijriDate.Visible = true;
                cbRSAdjustHijriDate.Visible = true;

            }
            else
            {
                lAdjustHijriDate.Visible = false;
                cbRSAdjustHijriDate.Visible = false;

            }

        } //CalendarItemSelected

        private void ResizeControls(object sender, EventArgs e)
        {
            int newPanelHeight = (Height - 61 - 51);

            int newControlHeight = (newPanelHeight - 48);

            int newPanelWidth = (Width - 438 - 36);

            int newControlWidth = (newPanelWidth - 24);

            tvTemplate.Height = newPanelHeight;

            Size panelSize = new Size(newPanelWidth, newPanelHeight);
            Size controlSize = new Size(newControlWidth, newControlHeight);

            pRegionalSettings.Size = panelSize;

            pComposedLook.Size = panelSize;

            pTextControl.Size = panelSize;
            tbTextControl.Size = controlSize;

            pWebSettings.Size = panelSize;

            pViewControl.Size = panelSize;
            lvViewControl.Size = controlSize;

            pListControl.Size = panelSize;
            lbListControl.Size = controlSize;

        } //ResizeControls

        private void DeleteTemplateItem(object sender, EventArgs e)
        {
            if (tvTemplate.SelectedNode != null)
            {
                TreeNode node = tvTemplate.SelectedNode;
                if (!node.Name.Equals("TemplateNode"))
                {
                    /*
                    if (!_deletedNodes.Exists(p => p.Key.Equals(node.Name, StringComparison.OrdinalIgnoreCase)))
                    {
                        _deletedNodes.AddKeyValue(node.Name, node.Parent.Name);

                    }

                    if (_changedNodes.Exists(p => p.Key.Equals(node.Name, StringComparison.OrdinalIgnoreCase)))
                    {
                        _changedNodes.RemoveAll(p => p.Key.Equals(node.Name, StringComparison.OrdinalIgnoreCase));

                    }
                    */

                    /*
                    TreeNode prevNode = node.PrevNode;
                    TreeNode parentNode = node.Parent;

                    KeyValueList tagList = parentNode.Tag as KeyValueList;

                    tagList.RemoveAll(p => p.Value.Equals(node.Name, StringComparison.OrdinalIgnoreCase));

                    _selectedNode = null; //ensure NodeSelecting will not throw an exception

                    node.Remove();

                    tvTemplate.SelectedNode = prevNode != null ? prevNode : parentNode;

                    bSave.Visible = true;
                    */

                }
                else
                {
                    MessageBox.Show("It will be easier to delete the provisioning file\r\n than to delete all the nodes.",
                                    "Delete Information",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                }

            }

        } //DeleteTemplateItem

        private void DisplayActiveNode(object sender, EventArgs e)
        {
            ListBox listBox = sender as ListBox;
            if (listBox.SelectedItem != null)
            {
                KeyValue listItem = listBox.SelectedItem as KeyValue;
                TreeNode node = tvTemplate.SelectedNode;
                if (node?.Nodes?.Count > 0)
                {
                    TreeNode[] nodes = node.Nodes.Find(listItem.Value, true);
                    if (nodes?.Length > 0)
                    {
                        tvTemplate.SelectedNode = nodes.First(p => p.Name.Equals(listItem.Value, StringComparison.OrdinalIgnoreCase));

                    }

                }

            }

        } //DisplayActiveNode

        private void SaveChanges(object sender, EventArgs e)
        {
            //to do

        } //SaveChanges

        private void ListControlSelectAll(object sender, EventArgs e)
        {
            int topIdx = lbListControl.TopIndex;
            lbListControl.BeginUpdate();
            for (var i = 0; i < lbListControl.Items.Count; i++)
            {
                lbListControl.SetSelected(i, true);
            }
            lbListControl.EndUpdate();
            lbListControl.TopIndex = topIdx;

        } //ListControlSelectAll

        private void DeleteTemplateItemFromList(object sender, EventArgs e)
        {
            /*
            TreeNode selectedNode = tvTemplate.SelectedNode;
            if (!selectedNode.Name.Equals("TemplateNode"))
            {
                if (lbListControl.SelectedItems?.Count > 0)
                {
                    KeyValueList itemsToDelete = new KeyValueList();
                    foreach (var item in lbListControl.SelectedItems)
                    {
                        itemsToDelete.Add((item as KeyValue));

                    }

                    TreeNode parentNode = null;

                    foreach (var keyValue in itemsToDelete)
                    {
                        TreeNode[] nodes = selectedNode.Nodes.Find(keyValue.Value, true);

                        TreeNode node = nodes.First(p => p.Name.Equals(keyValue.Value, StringComparison.OrdinalIgnoreCase));


                        parentNode = node.Parent;

                        if (parentNode != null)
                        {
                            KeyValueList tagList = parentNode.Tag as KeyValueList;

                            tagList.RemoveAll(p => p.Value.Equals(node.Name, StringComparison.OrdinalIgnoreCase));

                        }

                        node.Remove();

                    }

                    if (parentNode != null)
                    {
                        TemplateItem item = _templateItems.GetItem((string)parentNode.Tag);

                        PopulateControlList(item);

                    }

                    bSave.Visible = true;

                }

            }
            */

        } //DeleteTemplateItemFromList

        private void ListControlKeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                DeleteTemplateItemFromList(sender, e);

            }

        } //ListControlKeyUp

        private void TreeViewKeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                DeleteTemplateItem(sender, e);

            }

        } //TreeViewKeyUp

        private void RegionalSettingChanged(object sender, EventArgs e)
        {
            if (_selectedNode != null)
            {
                if (sender is ComboBox)
                {
                    if ((sender as ComboBox).Name.Equals("cbRSCalendar", StringComparison.Ordinal))
                    {
                        CalendarItemSelected(sender, e);

                    }

                }

                TemplateItem templateItem = _templateItems.GetItem((string)_selectedNode.Tag);
                if (templateItem != null)
                {
                    int[] rsValues = new int[13];
                    rsValues[(int)RegionalSettingProperties.AdjustHijriDays] = Convert.ToInt32(cbRSAdjustHijriDate.SelectedItem);
                    rsValues[(int)RegionalSettingProperties.AlternateCalendarType] = (int)cbRSAlternateCalendar.SelectedValue;
                    rsValues[(int)RegionalSettingProperties.CalendarType] = (int)cbRSCalendar.SelectedValue;
                    rsValues[(int)RegionalSettingProperties.Collation] = (int)cbRSSortOrder.SelectedValue;
                    rsValues[(int)RegionalSettingProperties.FirstDayOfWeek] = (int)cbRSFirstDayOfWeek.SelectedValue;
                    rsValues[(int)RegionalSettingProperties.FirstWeekOfYear] = (int)cbRSFirstWeekOfYear.SelectedValue;
                    rsValues[(int)RegionalSettingProperties.LocaleId] = (int)cbRSLocale.SelectedValue;
                    rsValues[(int)RegionalSettingProperties.ShowWeeks] = (cbRSShowWeekNumbers.Checked ? 1 : 0);
                    rsValues[(int)RegionalSettingProperties.Time24] = (int)cbRSTimeFormat.SelectedValue;
                    rsValues[(int)RegionalSettingProperties.TimeZone] = (int)cbRSTimeZone.SelectedValue;
                    rsValues[(int)RegionalSettingProperties.WorkDayEndHour] = (int)cbRSWorkDayEndTime.SelectedValue;

                    int workDays = 0;

                    WeekDayColllection weekDayColllection = new WeekDayColllection();

                    for (int i = 0; i < clbRSWorkDays.Items.Count; i++)
                    {
                        if (clbRSWorkDays.GetItemChecked(i))
                        {
                            WeekDay weekDay = weekDayColllection.WeekDays.Find(p => (p.WeekDayId == i));
                            workDays = workDays | weekDay.WeekDayBit;

                        }

                    }

                    rsValues[(int)RegionalSettingProperties.WorkDays] = workDays;
                    rsValues[(int)RegionalSettingProperties.WorkDayStartHour] = (int)cbRSWorkDayStartTime.SelectedValue;

                    templateItem.Content = rsValues;

                    if (templateItem.IsChanged)
                    {
                        bSave.Visible = true;

                    }

                } //if templateItem

            } //if _selectedNode

        } //RegionalSettingChanged

        private void ComposedLookChanged(object sender, EventArgs e)
        {
            if (_selectedNode != null)
            {
                TemplateItem templateItem = _templateItems.GetItem((string)_selectedNode.Tag);
                if (templateItem != null)
                {
                    string[] clValues = new string[]
                     {
                        tbCLName.Text,
                        tbCLBackgroundFile.Text,
                        tbCLColorFile.Text,
                        tbCLFontFile.Text,
                        tbCLVersion.Text

                     };
                    //Ensure above is in the order of ComposedLookProperties enum

                    templateItem.Content = clValues;

                    if (templateItem.IsChanged)
                    {
                        bSave.Visible = true;

                    }

                } //if templateItem

            } //if _selectedNode

        } //ComposedLookChanged

        private void WebSettingChanged(object sender, EventArgs e)
        {
            if (_selectedNode != null)
            {
                TemplateItem templateItem = _templateItems.GetItem((string)_selectedNode.Tag);
                if (templateItem != null)
                {
                    string[] webSettings = new string[] {
                        tbWSAlternateCSS.Text,
                        tbWSCustomMasterPageUrl.Text,
                        tbWSDescription.Text,
                        tbWSMasterPageUrl.Text,
                        (cbWSNoCrawl.Checked ? "1" : "0"),
                        tbWSRequestAccessEmail.Text,
                        tbWSSiteLogo.Text,
                        tbWSTitle.Text,
                        tbWSWelcomePage.Text

                    };

                    //Ensure above is in the order of WebSettingProperties enum

                    templateItem.Content = webSettings;

                    if (templateItem.IsChanged)
                    {
                        bSave.Visible = true;

                    }

                } //if templateItem

            } //if _selectedNode

        } //WebSettingChanged

        private void ControlTextChanged(object sender, EventArgs e)
        {
            if (_selectedNode != null)
            {
                TemplateItem templateItem = _templateItems.GetItem((string)_selectedNode.Tag);
                if (templateItem != null)
                {
                    templateItem.Content = tbTextControl.Text;

                    if (templateItem.IsChanged)
                    {
                        bSave.Visible = true;

                    }

                } //if templateItem

            } //if _selectedNode

        } //ControlTextChanged

        private void ShowViewItem(object sender, EventArgs e)
        {
            if (_selectedNode != null)
            {
                TemplateItem templateItem = _templateItems.GetItem((string)_selectedNode.Tag);
                if (templateItem != null)
                {
                    if (lvViewControl.SelectedItems.Count > 0)
                    {
                        ListViewItem viewItem = lvViewControl.SelectedItems[0];

                        string key = viewItem.Text;
                        string value = viewItem.SubItems[1].Text;

                        ViewItem dialog = new ViewItem();

                        dialog.SetTitle(_selectedNode.Text);

                        dialog.SetKeyValue(key, value);

                        DialogResult result = dialog.ShowDialog(this);

                        if (result == DialogResult.OK)
                        {
                            string newVal = dialog.GetValue();
                            if (!value.Equals(newVal, StringComparison.Ordinal))
                            {
                                viewItem.SubItems[1].Text = newVal;

                                KeyValueList keyValues = new KeyValueList();

                                foreach (ListViewItem item in lvViewControl.Items)
                                {
                                    keyValues.AddKeyValue(item.SubItems[0].Text, item.SubItems[1].Text);

                                }

                                templateItem.Content = keyValues;

                                if (templateItem.IsChanged)
                                {
                                    bSave.Visible = true;

                                }

                            }

                        }

                    }

                } //if templateItem

            } //if _selectedNode

        } //ShowViewItem

    } //EditWin

} //Namespace
