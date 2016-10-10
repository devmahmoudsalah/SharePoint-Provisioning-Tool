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
                cbTimeZone.DataSource = null;
                cbTimeZone.Items.Clear();
                TimeZoneCollection timeZoneCollection = new TimeZoneCollection();
                cbTimeZone.DisplayMember = "TimeZoneName";
                cbTimeZone.ValueMember = "TimeZoneId";
                cbTimeZone.DataSource = timeZoneCollection.TimeZones;

                int selectedIdx = -1;
                int tmpIdx = regionalSettings[(int)RegionalSettingProperties.TimeZone];
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

                cbLocale.DataSource = null;
                cbLocale.Items.Clear();
                LocaleCollection localeCollection = new LocaleCollection();
                cbLocale.DisplayMember = "LocaleName";
                cbLocale.ValueMember = "LocaleId";
                cbLocale.DataSource = localeCollection.Locales;

                selectedIdx = -1;
                tmpIdx = regionalSettings[(int)RegionalSettingProperties.LocaleId];
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

                cbSortOrder.DataSource = null;
                cbSortOrder.Items.Clear();
                SortOrderCollection sortOrderCollection = new SortOrderCollection();
                cbSortOrder.DisplayMember = "SortOrderName";
                cbSortOrder.ValueMember = "SortOrderId";
                cbSortOrder.DataSource = sortOrderCollection.SortOrders;

                selectedIdx = -1;
                tmpIdx = regionalSettings[(int)RegionalSettingProperties.Collation];
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

                cbCalendar.DataSource = null;
                cbCalendar.Items.Clear();
                CalendarCollection calendarCollection = new CalendarCollection();
                cbCalendar.DisplayMember = "CalendarName";
                cbCalendar.ValueMember = "CalendarId";
                cbCalendar.DataSource = calendarCollection.Calendars.Where(p => p.CalendarId > 0).ToList();

                selectedIdx = -1;
                tmpIdx = regionalSettings[(int)RegionalSettingProperties.CalendarType];
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

                tmpIdx = regionalSettings[(int)RegionalSettingProperties.ShowWeeks];
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
                    tmpIdx = 2 + regionalSettings[(int)RegionalSettingProperties.AdjustHijriDays];
                    cbAdjustHijriDate.SelectedIndex = tmpIdx;

                }
                else
                {
                    lAdjustHijriDate.Visible = false;
                    cbAdjustHijriDate.Visible = false;
                    cbAdjustHijriDate.SelectedIndex = 2; // equal to 0 value

                }

                cbAlternateCalendar.DataSource = null;
                cbAlternateCalendar.Items.Clear();
                cbAlternateCalendar.DisplayMember = "CalendarName";
                cbAlternateCalendar.ValueMember = "CalendarId";
                cbAlternateCalendar.DataSource = calendarCollection.Calendars.ToList();

                selectedIdx = -1;
                tmpIdx = regionalSettings[(int)RegionalSettingProperties.AlternateCalendarType];
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
                tmpIdx = regionalSettings[(int)RegionalSettingProperties.WorkDays];
                WeekDayCollletion weekDayCollection = new WeekDayCollletion();
                foreach (WeekDay weekDay in weekDayCollection.WeekDays)
                {
                    clbWorkDays.Items.Add(weekDay.WeekDayShortName, ((tmpIdx & weekDay.WeekDayBit) == weekDay.WeekDayBit));

                }

                cbFirstDayOfWeek.DataSource = null;
                cbFirstDayOfWeek.Items.Clear();
                cbFirstDayOfWeek.DisplayMember = "WeekDayLongName";
                cbFirstDayOfWeek.ValueMember = "WeekDayId";
                cbFirstDayOfWeek.DataSource = weekDayCollection.WeekDays;
                cbFirstDayOfWeek.SelectedIndex = regionalSettings[(int)RegionalSettingProperties.FirstDayOfWeek];

                cbFirstWeekOfYear.DataSource = null;
                cbFirstWeekOfYear.Items.Clear();
                FirstWeekCollection firstWeekCollection = new FirstWeekCollection();
                cbFirstWeekOfYear.DisplayMember = "FirstWeekName";
                cbFirstWeekOfYear.ValueMember = "FirstWeekId";
                cbFirstWeekOfYear.DataSource = firstWeekCollection.FirstWeeks;
                cbFirstWeekOfYear.SelectedIndex = regionalSettings[(int)RegionalSettingProperties.FirstWeekOfYear];

                cbWorkDayStartTime.DataSource = null;
                cbWorkDayStartTime.Items.Clear();
                DayHourCollection dayHourCollection = new DayHourCollection();
                cbWorkDayStartTime.DisplayMember = "DayHourName";
                cbWorkDayStartTime.ValueMember = "DayHourId";
                cbWorkDayStartTime.DataSource = dayHourCollection.DayHours.ToList();
                cbWorkDayStartTime.SelectedIndex = regionalSettings[(int)RegionalSettingProperties.WorkDayStartHour];

                cbWorkDayEndTime.DataSource = null;
                cbWorkDayEndTime.Items.Clear();
                cbWorkDayEndTime.DisplayMember = "DayHourName";
                cbWorkDayEndTime.ValueMember = "DayHourId";
                cbWorkDayEndTime.DataSource = dayHourCollection.DayHours.ToList();
                cbWorkDayEndTime.SelectedIndex = regionalSettings[(int)RegionalSettingProperties.WorkDayEndHour];

                cbTimeFormat.DataSource = null;
                cbTimeFormat.Items.Clear();
                TimeFormatCollection timeFormatCollection = new TimeFormatCollection();
                cbTimeFormat.DisplayMember = "TimeFormatName";
                cbTimeFormat.ValueMember = "TimeFormatId";
                cbTimeFormat.DataSource = timeFormatCollection.TimeFormats;
                cbTimeFormat.SelectedIndex = regionalSettings[(int)RegionalSettingProperties.Time24];

            }

        } //PopulateRegionalSettings

        private void PopulateComposedLook(TemplateItem templateItem)
        {
            string[] composedLook = (string[])templateItem.Content;
            if (composedLook?.Length > 0)
            {
                tbComposedLookName.Text = composedLook[(int)ComposedLookProperties.Name];
                tbBackgroundFile.Text = composedLook[(int)ComposedLookProperties.BackgroundFile];
                tbColorFile.Text = composedLook[(int)ComposedLookProperties.ColorFile];
                tbFontFile.Text = composedLook[(int)ComposedLookProperties.FontFile];
                tbComposedLookVersion.Text = composedLook[(int)ComposedLookProperties.Version];

            }

            pComposedLook.Tag = composedLook; //used to check if there were changes

        } //PopulateComposedLook

        private void PopulateControlText(TemplateItem templateItem)
        {
            string value = (string)templateItem.Content;

            tbTextControl.Text = value;

        }

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

        private void PopulateControlListView(TemplateItem templateItem)
        {
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

        private void HideActivePanel()
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
                HideActivePanel();

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

                /*
                if (!node.FullPath.Equals(_activeNodePath, StringComparison.OrdinalIgnoreCase))
                {
                }

                _activeNodePath = node.FullPath;
                _selectedNode = null;

                if (node.Name.Equals("TemplateNode", StringComparison.OrdinalIgnoreCase))
                {
                    lListControl.Text = node.Text;
                    PopulateControlList(node.Tag);
                    pListControl.Show();
                    return;

                }

                if (_activeNodePath.Contains(@"\Regional Settings"))
                {
                    if (cbTimeZone.Items.Count <= 0)
                    {
                        PopulateRegionalSettings();

                    }

                    pRegionalSettings.Show();

                }
                else if (_activeNodePath.Contains(@"\Composed Look"))
                {
                    if (string.IsNullOrWhiteSpace(tbComposedLookName.Text))
                    {
                        PopulateComposedLook();

                    }

                    pComposedLook.Show();

                }
                else if (_activeNodePath.Contains(@"\Content Types"))
                {
                    if (_activeNodePath.Contains(@"\Content Types\"))
                    {
                        lTextControl.Text = "Content type:";
                        string tagCT = node.Tag.ToString();
                        if (tagCT.StartsWith("{"))
                        {
                            tbTextControl.Text = tagCT;

                        }
                        else
                        {
                            PopulateContentType(node.Tag.ToString());
                            node.Tag = tbTextControl.Text;

                        }
                        pTextControl.Show();
                        _selectedNode = node;

                    }
                    else
                    {
                        lListControl.Text = "Content types:";
                        PopulateControlList(node.Tag);
                        pListControl.Show();

                    }

                }
                else if (_activeNodePath.Contains(@"\Site Fields"))
                {
                    if (_activeNodePath.Contains(@"\Site Fields\"))
                    {
                        lTextControl.Text = "Site field:";
                        string tagSF = node.Tag.ToString();
                        tbTextControl.Text = tagSF;
                        pTextControl.Show();
                        _selectedNode = node;

                    }
                    else
                    {
                        lListControl.Text = "Site fields:";
                        PopulateControlList(node.Tag);
                        pListControl.Show();

                    }

                }
                else if (_activeNodePath.Contains(@"\Fields")) //fields under lists
                {
                    if (_activeNodePath.Contains(@"\Fields\"))
                    {
                        lTextControl.Text = "List field:";
                        string tagF = node.Tag.ToString();
                        tbTextControl.Text = tagF;
                        pTextControl.Show();
                        _selectedNode = node;

                    }
                    else
                    {
                        lListControl.Text = "List fields:";
                        PopulateControlList(node.Tag);
                        pListControl.Show();

                    }

                }
                else if (_activeNodePath.Contains(@"\Views")) //views under lists
                {
                    if (_activeNodePath.Contains(@"\Views\"))
                    {
                        lTextControl.Text = "List view:";
                        string tagLV = node.Tag.ToString();
                        tbTextControl.Text = tagLV;
                        pTextControl.Show();
                        _selectedNode = node;

                    }
                    else
                    {
                        lListControl.Text = "List views:";
                        PopulateControlList(node.Tag);
                        pListControl.Show();

                    }

                }
                else if (_activeNodePath.Contains(@"\Lists")) //the lists
                {
                    if (_activeNodePath.Contains(@"\Lists\"))
                    {
                        lTextControl.Text = "List instance:";
                        string tagLI = node.Tag.ToString();
                        if (tagLI.StartsWith("{"))
                        {
                            tbTextControl.Text = tagLI;

                        }
                        else
                        {
                            PopulateListInstance(node.Tag.ToString());
                            node.Tag = tbTextControl.Text;

                        }
                        pTextControl.Show();
                        _selectedNode = node;

                    }
                    else
                    {
                        lListControl.Text = "Lists:";
                        PopulateControlList(node.Tag);
                        pListControl.Show();

                    }
                }
                else if (_activeNodePath.Contains(@"\Files"))
                {
                    if (_activeNodePath.Contains(@"\Files\"))
                    {
                        _selectedNode = node;

                    }
                    else
                    {
                        lListControl.Text = "Files:";
                        PopulateControlList(node.Tag);
                        pListControl.Show();

                    }
                }
                else if (_activeNodePath.Contains(@"\Web Settings"))
                {
                    if (string.IsNullOrWhiteSpace(tbWSMasterPageUrl.Text))
                    {
                        PopulateWebSettings(node.Tag);

                    }

                    pWebSettings.Show();

                }
                else if (_activeNodePath.Contains(@"\Property Bag Entries"))
                {
                    if (lvViewControl.Items.Count <= 0)
                    {
                        PopulatePropertyBagEntries(node.Tag);

                    }

                    pViewControl.Show();

                }
                else if (_activeNodePath.Contains(@"\Term Sets"))
                {
                    if (_activeNodePath.Contains(@"\Term Sets\"))
                    {
                        //to do
                        _selectedNode = node;

                    }
                    else
                    {
                        lListControl.Text = "Term sets:";
                        PopulateControlList(node.Tag);
                        pListControl.Show();

                    }

                }
                else if (_activeNodePath.Contains(@"\Term Groups"))
                {
                    if (_activeNodePath.Contains(@"\Term Groups\"))
                    {
                        //to do
                        _selectedNode = node;

                    }
                    else
                    {
                        lListControl.Text = "Term groups:";
                        PopulateControlList(node.Tag);
                        pListControl.Show();

                    }

                }
                else if (_activeNodePath.Contains(@"\Workflow Definitions"))
                {
                    if (_activeNodePath.Contains(@"\Workflow Definitions\"))
                    {
                        lTextControl.Text = "Workflow definition:";
                        string tagWD = node.Tag.ToString();
                        if (tagWD.StartsWith("{"))
                        {
                            tbTextControl.Text = tagWD.ToString();

                        }
                        else
                        {
                            PopulateWorkflowDefinition(Guid.Parse(node.Tag.ToString()));
                            node.Tag = tbTextControl.Text;

                        }

                        pTextControl.Show();
                        _selectedNode = node;

                    }
                    else
                    {
                        lListControl.Text = "Workflow definitions:";
                        PopulateControlList(node.Tag);
                        pListControl.Show();

                    }

                }
                else if (_activeNodePath.Contains(@"\Workflow Subscriptions"))
                {
                    if (_activeNodePath.Contains(@"\Workflow Subscriptions\"))
                    {
                        lTextControl.Text = "Workflow subscription:";
                        string tagWS = node.Tag.ToString();
                        if (tagWS.StartsWith("{"))
                        {
                            tbTextControl.Text = tagWS.ToString();

                        }
                        else
                        {
                            PopulateWorkflowSubscription(node.Tag.ToString());
                            node.Tag = tbTextControl.Text;

                        }

                        pTextControl.Show();
                        _selectedNode = node;

                    }
                    else
                    {
                        lListControl.Text = "Workflow subscriptions:";
                        PopulateControlList(node.Tag);
                        pListControl.Show();

                    }

                }

                */

            }

        } //NodeSelected

        private void CalendarItemSelected(object sender, EventArgs e)
        {
            if (cbCalendar.SelectedItem != null)
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

                    TreeNode prevNode = node.PrevNode;
                    TreeNode parentNode = node.Parent;

                    KeyValueList tagList = parentNode.Tag as KeyValueList;

                    tagList.RemoveAll(p => p.Value.Equals(node.Name, StringComparison.OrdinalIgnoreCase));

                    _selectedNode = null; //ensure NodeSelecting will not throw an exception

                    node.Remove();

                    tvTemplate.SelectedNode = prevNode != null ? prevNode : parentNode;

                    bSave.Visible = true;

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

        } //DeleteTemplateItemFromList

        private void NodeSelecting(object sender, TreeViewCancelEventArgs e)
        {
            if (_selectedNode != null)
            {
                //Check if an item was changed and mark it to be updated when Save button is clicked

                TemplateItem templateItem = _templateItems.GetItem((string)_selectedNode.Tag);

                if (templateItem != null)
                {
                    switch (templateItem.ControlType)
                    {
                        case TemplateControlType.Form:
                            //to do
                            break;
                        case TemplateControlType.ListBox:
                            //to do
                            break;
                        case TemplateControlType.ListView:
                            // to do
                            break;
                        case TemplateControlType.TextBox:
                            templateItem.Content = tbTextControl.Text;
                            if (templateItem.IsChanged)
                            {
                                bSave.Visible = true;

                            }

                            break;
                        default:
                            break;

                    }

                }

            }

        } //NodeSelecting

        private void ListControlKeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                DeleteTemplateItemFromList(sender, e);

            }

        } //ListControlKeyUp

        private void TreeViewKeyUp(object sender, KeyEventArgs e)
        {
            if(e.KeyCode== Keys.Delete)
            {
                DeleteTemplateItem(sender, e);

            }

        } //TreeViewKeyUp


    } //EditWin

} //Namespace