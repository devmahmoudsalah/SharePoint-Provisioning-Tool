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

        } //SelectedVersion

        public delegate string OpenTemplateDelegate();

        public OpenTemplateDelegate OpenTemplate;

        public delegate void SetStatusTextDelegate(string message);

        public SetStatusTextDelegate SetStatusBarText;

        public EditWin()
        {
            InitializeComponent();
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
                        case SharePointVersion.SharePoint_2013_On_Premises:
                            //_templateItems = SP2013OP.OpenTemplateForEdit(path, name, tvTemplate);

                            break;

                        case SharePointVersion.SharePoint_2016_On_Premises:
                            //_templateItems = SP2016OP.OpenTemplateForEdit(path, name, tvTemplate);

                            break;

                        case SharePointVersion.SharePoint_2016_OnLine:
                            //_templateItems = SP2016OL.OpenTemplateForEdit(path, name, tvTemplate);

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
                        tvTemplate.Visible = true;
                        tvTemplate.SelectedNode = tvTemplate.Nodes[0];
                        bSave.Visible = false;

                    }

                    Cursor = Cursors.Default;

                } //finally

            }

        } //BrowseForTemplate
               
        private void PopulateRegionalSettings(TemplateItem templateItem)
        {
            int[] regionalSettings = (int[])templateItem.Content;
            if (regionalSettings?.Length > 0)
            {
                cbRSTimeZone.DataSource = null;
                cbRSTimeZone.Items.Clear();
                TimeZoneCollection timeZoneCollection = new TimeZoneCollection();
                cbRSTimeZone.DisplayMember = timeZoneCollection.DisplayMember;
                cbRSTimeZone.ValueMember = timeZoneCollection.ValueMember;
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
                cbRSLocale.DisplayMember = localeCollection.DisplayMember;
                cbRSLocale.ValueMember = localeCollection.ValueMember;
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
                cbRSSortOrder.DisplayMember = sortOrderCollection.DisplayMember;
                cbRSSortOrder.ValueMember = sortOrderCollection.ValueMember;
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
                cbRSCalendar.DisplayMember = calendarCollection.DisplayMember;
                cbRSCalendar.ValueMember = calendarCollection.ValueMember;
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
                cbRSAlternateCalendar.DisplayMember = calendarCollection.DisplayMember;
                cbRSAlternateCalendar.ValueMember = calendarCollection.ValueMember;
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

                cbRSFirstDayOfWeek.DisplayMember = weekDayCollection.DisplayMember;
                cbRSFirstDayOfWeek.ValueMember = weekDayCollection.ValueMember;
                cbRSFirstDayOfWeek.DataSource = weekDayCollection.WeekDays;
                cbRSFirstDayOfWeek.SelectedIndex = regionalSettings[(int)RegionalSettingProperties.FirstDayOfWeek];

                cbRSFirstWeekOfYear.DataSource = null;
                cbRSFirstWeekOfYear.Items.Clear();

                FirstWeekCollection firstWeekCollection = new FirstWeekCollection();
                cbRSFirstWeekOfYear.DisplayMember = firstWeekCollection.DisplayMember;
                cbRSFirstWeekOfYear.ValueMember = firstWeekCollection.ValueMember;
                cbRSFirstWeekOfYear.DataSource = firstWeekCollection.FirstWeeks;
                cbRSFirstWeekOfYear.SelectedIndex = regionalSettings[(int)RegionalSettingProperties.FirstWeekOfYear];

                cbRSWorkDayStartTime.DataSource = null;
                cbRSWorkDayStartTime.Items.Clear();

                DayHourCollection dayHourCollection = new DayHourCollection();
                cbRSWorkDayStartTime.DisplayMember = dayHourCollection.DisplayMember;
                cbRSWorkDayStartTime.ValueMember = dayHourCollection.ValueMember;
                cbRSWorkDayStartTime.DataSource = dayHourCollection.DayHours.ToList();
                cbRSWorkDayStartTime.SelectedIndex = regionalSettings[(int)RegionalSettingProperties.WorkDayStartHour];

                cbRSWorkDayEndTime.DataSource = null;
                cbRSWorkDayEndTime.Items.Clear();

                cbRSWorkDayEndTime.DisplayMember = dayHourCollection.DisplayMember;
                cbRSWorkDayEndTime.ValueMember = dayHourCollection.ValueMember;
                cbRSWorkDayEndTime.DataSource = dayHourCollection.DayHours.ToList();
                cbRSWorkDayEndTime.SelectedIndex = regionalSettings[(int)RegionalSettingProperties.WorkDayEndHour];

                cbRSTimeFormat.DataSource = null;
                cbRSTimeFormat.Items.Clear();

                TimeFormatCollection timeFormatCollection = new TimeFormatCollection();
                cbRSTimeFormat.DisplayMember = timeFormatCollection.DisplayMember;
                cbRSTimeFormat.ValueMember = timeFormatCollection.ValueMember;
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
                if (SelectedVersion == SharePointVersion.SharePoint_2016_OnLine)
                {
                    tbWSRequestAccessEmail.Text = ws[(int)WebSettingProperties.RequestAccessEmail];
                    cbWSNoCrawl.Checked = (ws[(int)WebSettingProperties.NoCrawl].Equals("1"));

                }
                else
                {
                    tbWSRequestAccessEmail.Enabled = false;
                    cbWSNoCrawl.Enabled = false;

                }

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
                lvViewControl.Items.Add(Constants.Add_New);
            }

        } //PopulatePropertyBagEntries

        private void PopulateControlList(TemplateItem templateItem)
        {
            lbListControl.DataSource = null;
            lbListControl.Items.Clear();

            KeyValueList keyValueList = (KeyValueList)templateItem.Content;

            if (keyValueList?.Count > 0)
            {
                lbListControl.DisplayMember = keyValueList.DisplayMember;
                lbListControl.ValueMember = keyValueList.ValueMember;
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

                TemplateItem templateItem = _templateItems.GetItem(node.Name);

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
                            PopulateControlList(templateItem);
                            pListControl.Show();

                            break;

                        case TemplateControlType.ListView:
                            PopulateControlListView(templateItem);
                            pViewControl.Show();

                            break;

                        case TemplateControlType.TextBox:
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
            int splitterWidth = scEditTemplate.SplitterDistance;

            int newHeight = (Height - 69 - 51);
            int newWidth = (Width - 48);

            Size newSize = new Size(newWidth, newHeight);

            scEditTemplate.Size = newSize;

            scEditTemplate.SplitterDistance = splitterWidth;

        } //ResizeControls

        private void DeleteTemplateItem(object sender, EventArgs e)
        {
            if (_selectedNode != null)
            {
                TemplateItem templateItem = _templateItems.GetItem(_selectedNode.Name);

                if (templateItem != null)
                {
                    _templateItems.SetChildrenDeleted(templateItem.Id);
                    _templateItems.SetDeleted(templateItem);

                    if (!string.IsNullOrWhiteSpace(templateItem.ParentId))
                    {
                        TemplateItem parentItem = _templateItems.GetItem(templateItem.ParentId);
                        if (parentItem.ControlType == TemplateControlType.ListBox)
                        {
                            KeyValueList keyValueList = parentItem.Content as KeyValueList;
                            keyValueList.RemoveAll(p => p.ValueEquals(templateItem.Id));

                        }
                        else if (parentItem.ItemType == TemplateItemType.FileWebPartItem)
                        {
                            //Contents of webpart is deleted, delete the webpart
                            _selectedNode = _selectedNode.Parent;
                            _templateItems.SetChildrenDeleted(parentItem.Id);
                            _templateItems.SetDeleted(parentItem);
                            string webPartTitle = parentItem.Id;
                            parentItem = _templateItems.GetItem(parentItem.ParentId);
                            if (parentItem.ControlType == TemplateControlType.ListBox)
                            {
                                KeyValueList keyValueList = parentItem.Content as KeyValueList;
                                keyValueList.RemoveAll(p => p.ValueEquals(webPartTitle));

                            }

                        }

                    }

                }

                TreeNode prevNode = _selectedNode.PrevNode;
                TreeNode parentNode = _selectedNode.Parent;

                _selectedNode.Remove();

                _selectedNode = null;

                if (prevNode != null)
                {
                    tvTemplate.SelectedNode = prevNode;

                }
                else if (parentNode != null)
                {
                    tvTemplate.SelectedNode = parentNode;

                }
                else
                {
                    HidePanels();

                }

                bSave.Visible = true;

            }

        } //DeleteTemplateItem

        private void DisplayActiveNode(object sender, EventArgs e)
        {
            if (_selectedNode != null)
            {
                ListBox listBox = sender as ListBox;
                if (listBox.SelectedItem != null)
                {
                    KeyValue listItem = listBox.SelectedItem as KeyValue;
                    if (_selectedNode.Nodes?.Count > 0)
                    {
                        TreeNode[] nodes = _selectedNode.Nodes.Find(listItem.Value, true);
                        if (nodes?.Length > 0)
                        {
                            tvTemplate.SelectedNode = nodes.First(p => p.Name.Equals(listItem.Value, StringComparison.OrdinalIgnoreCase));

                        }

                    }

                }

            }

        } //DisplayActiveNode

        private void ProcessEmptyParent(TemplateItem parentItem, TemplateItem templateItem)
        {
            if (parentItem != null)
            {
                if (parentItem.ControlType == TemplateControlType.ListBox)
                {
                    KeyValueList keyValueList = parentItem.Content as KeyValueList;
                    keyValueList.RemoveAll(p => p.ValueEquals(templateItem.Id));

                    if (keyValueList.Count == 0)
                    {
                        TreeNode[] nodes = tvTemplate.Nodes.Find(parentItem.Id, true);
                        if (nodes?.Length > 0)
                        {
                            foreach (var node in nodes)
                            {
                                node.Remove();

                            }

                        }

                        _templateItems.SetChildrenDeleted(parentItem.Id);
                        _templateItems.SetDeleted(parentItem);

                        TemplateItem grandParent = _templateItems.GetParent(parentItem);
                        ProcessEmptyParent(grandParent, parentItem);

                    }

                }

            }

        } //ProcessEmptyParent

        private void RemoveEmptyNodes()
        {
            List<TemplateItem> emptyItems = _templateItems.GetEmptyItems(TemplateControlType.TextBox);
            if (emptyItems?.Count > 0)
            {
                foreach (var templateItem in emptyItems)
                {
                    TreeNode[] nodes = tvTemplate.Nodes.Find(templateItem.Id, true);
                    if (nodes?.Length > 0)
                    {
                        foreach(var node in nodes)
                        {
                            if (node != null)
                            {
                                node.Remove();

                            }
                            _templateItems.SetChildrenDeleted(templateItem.Id);
                            _templateItems.SetDeleted(templateItem);

                            TemplateItem parentItem = _templateItems.GetParent(templateItem);

                            ProcessEmptyParent(parentItem, templateItem);

                        }

                    }

                }

            } //if emptyItems

        } //RemoveEmptyNodes

        private void SaveChanges(object sender, EventArgs e)
        {
            try
            {
                RemoveEmptyNodes();

                switch (_selectedVersion)
                {
                    case SharePointVersion.SharePoint_2013_On_Premises:
                        //SP2013OP.SaveTemplateForEdit(_templateItems);

                        break;

                    case SharePointVersion.SharePoint_2016_On_Premises:
                        //SP2016OP.SaveTemplateForEdit(_templateItems);

                        break;

                    case SharePointVersion.SharePoint_2016_OnLine:
                        //SP2016OL.SaveTemplateForEdit(_templateItems);

                        break;

                } // switch

                bSave.Visible = false;
                MessageBox.Show("Template saved", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

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
            if (_selectedNode != null)
            {
                TemplateItem templateItem = _templateItems.GetItem(_selectedNode.Name);
                if (templateItem != null)
                {
                    KeyValueList keyValueList = new KeyValueList();
                    keyValueList.AddRange(templateItem.Content as KeyValueList);

                    foreach (var item in lbListControl.SelectedItems)
                    {
                        KeyValue keyValue = item as KeyValue;

                        switch (templateItem.ItemType)
                        {
                            case TemplateItemType.SiteFeatureList:
                            case TemplateItemType.SupportedUILanguagesList:
                            case TemplateItemType.WebFeatureList:
                                //An item in these lists contain properties and not pointers to nodes
                                //do nothing as the item will be removed from the list in below code

                                break;

                            default:
                                //Item is a pointer to a tree node.
                                //Delete it from the tree view
                                TreeNode[] nodes = _selectedNode.Nodes.Find(keyValue.Value, false);
                                if (nodes?.Length > 0)
                                {
                                    TreeNode node = nodes.First(p => p.Name.Equals(keyValue.Value, StringComparison.Ordinal));
                                    if (node != null)
                                    {
                                        TemplateItem nodeItem = _templateItems.GetItem(node.Name);
                                        if (nodeItem != null)
                                        {
                                            if (node.Nodes?.Count > 0)
                                            {
                                                _templateItems.SetChildrenDeleted(nodeItem.Id);

                                            }

                                            _templateItems.SetDeleted(nodeItem);

                                        }

                                        tvTemplate.Nodes.Remove(node);

                                    }

                                }

                                break;

                        } //switch

                        keyValueList.RemoveAll(p => p.KeyEquals(keyValue.Key));

                    }

                    if (keyValueList.Count > 0)
                    {
                        templateItem.Content = keyValueList;

                        if (templateItem.IsChanged)
                        {
                            bSave.Visible = true;

                            PopulateControlList(templateItem);

                        }

                    }
                    else
                    {
                        TreeNode prevNode = _selectedNode.PrevNode;
                        TreeNode parentNode = _selectedNode.Parent;

                        templateItem.IsDeleted = true;
                        bSave.Visible = true;

                        tvTemplate.Nodes.Remove(_selectedNode);
                        _selectedNode = null;

                        if (prevNode != null)
                        {
                            tvTemplate.SelectedNode = prevNode;

                        }
                        else if (parentNode != null)
                        {
                            tvTemplate.SelectedNode = parentNode;

                        }
                        else
                        {
                            HidePanels();

                        }

                    }

                }

            }

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

                TemplateItem templateItem = _templateItems.GetItem(_selectedNode.Name);
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
                TemplateItem templateItem = _templateItems.GetItem(_selectedNode.Name);
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
                TemplateItem templateItem = _templateItems.GetItem(_selectedNode.Name);
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
                TemplateItem templateItem = _templateItems.GetItem(_selectedNode.Name);
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
                TemplateItem templateItem = _templateItems.GetItem(_selectedNode.Name);
                if (templateItem != null)
                {
                    if (lvViewControl.SelectedItems.Count > 0)
                    {
                        ListViewItem viewItem = lvViewControl.SelectedItems[0];

                        KeyValue keyValue = new KeyValue(viewItem.Text, string.Empty);

                        bool addNew = keyValue.KeyEquals(Constants.Add_New);

                        if (!addNew)
                        {
                            keyValue.Value = viewItem.SubItems[1].Text;

                        }
                        else
                        {
                            keyValue.Key = string.Empty;

                        }

                        ViewItem dialog = new ViewItem();

                        dialog.SetTitle(_selectedNode.Text);

                        dialog.SetKeyValue(keyValue);

                        DialogResult result = dialog.ShowDialog(this);

                        if (result == DialogResult.OK)
                        {
                            KeyValue newKV = dialog.GetKeyValue();
                            if (!newKV.KeyIsEmpty())
                            {
                                if (!newKV.Equals(keyValue))
                                {
                                    viewItem.Text = newKV.Key;
                                    if (addNew)
                                    {
                                        viewItem.SubItems.Add(newKV.Value);

                                    }
                                    else
                                    {
                                        viewItem.SubItems[1].Text = newKV.Value;

                                    }

                                    KeyValueList keyValues = new KeyValueList();

                                    foreach (ListViewItem item in lvViewControl.Items)
                                    {
                                        if (item.SubItems.Count > 1)
                                        {
                                            keyValues.AddKeyValue(item.SubItems[0].Text, item.SubItems[1].Text);

                                        }

                                    }

                                    templateItem.Content = keyValues;

                                    if (templateItem.IsChanged)
                                    {
                                        bSave.Visible = true;

                                    }

                                    if (addNew)
                                    {
                                        lvViewControl.Items.Add(Constants.Add_New);

                                    }

                                }

                            }

                        }

                    }

                } //if templateItem

            } //if _selectedNode

        } //ShowViewItem

        private void ViewControlSelectAll(object sender, EventArgs e)
        {
            if (lvViewControl.Items?.Count > 0)
            {
                var topItem = lvViewControl.TopItem;
                lvViewControl.BeginUpdate();
                for (var i = 0; i < lvViewControl.Items.Count; i++)
                {
                    lvViewControl.Items[i].Selected = true;

                }

                lvViewControl.EndUpdate();
                lvViewControl.TopItem = topItem;

            }

        } //ViewControlSelectAll

        private void DeleteTemplateItemFromView(object sender, EventArgs e)
        {
            if (_selectedNode != null)
            {
                TemplateItem templateItem = _templateItems.GetItem(_selectedNode.Name);
                if (templateItem != null)
                {
                    int removeCount = 0;
                    KeyValueList keyValues = new KeyValueList();
                    keyValues.AddRange(templateItem.Content as KeyValueList);
                    foreach (ListViewItem item in lvViewControl.SelectedItems)
                    {
                        if (!item.Text.Equals(Constants.Add_New, StringComparison.OrdinalIgnoreCase))
                        {
                            keyValues.RemoveAll(p => p.KeyEquals(item.Text));
                            lvViewControl.Items.Remove(item);
                            removeCount++;

                        }

                    }

                    if (removeCount > 0)
                    {
                        if (lvViewControl.Items.Count > 0)
                        {
                            templateItem.Content = keyValues;
                            if (templateItem.IsChanged)
                            {
                                bSave.Visible = true;

                            }

                        }
                        else
                        {
                            templateItem.IsDeleted = true;
                            bSave.Visible = true;

                            TreeNode prevNode = _selectedNode.PrevNode;
                            TreeNode parentNode = _selectedNode.Parent;

                            tvTemplate.Nodes.Remove(_selectedNode);
                            _selectedNode = null;

                            if (prevNode != null)
                            {
                                tvTemplate.SelectedNode = prevNode;

                            }
                            else if (parentNode != null)
                            {
                                tvTemplate.SelectedNode = parentNode;

                            }
                            else
                            {
                                HidePanels();

                            }

                        }

                    }

                }

            }

        } //DeleteTemplateItemFromView

        private void ViewControlKeyUp(object sender, KeyEventArgs e)
        {
            if (lvViewControl.Items?.Count > 0)
            {
                if (e.KeyCode == Keys.Delete)
                {
                    DeleteTemplateItemFromView(sender, e);

                }
                else if (e.KeyCode == Keys.Insert)
                {
                    ShowViewItem(sender, e);

                }

            }

        } //ViewControlKeyUp

        private void SetStatusText(object sender, EventArgs e)
        {
            string tag = Constants.Edit00;
            tag = (sender as Control).Tag.ToString();
            SetStatusBarText(Properties.Resources.ResourceManager.GetString(tag));

        } //SetStatusText

        private void SetStatusDefault(object sender, EventArgs e)
        {
            SetStatusBarText(Properties.Resources.ResourceManager.GetString(Constants.Edit00));

        } //SetStatusDefault

    } //EditWin

} //Namespace
