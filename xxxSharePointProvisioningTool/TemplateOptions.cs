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
    public partial class TemplateOptions : Form
    {
        private Color KarabinaRed = Color.FromArgb(Constants.Karabina_Red, Constants.Karabina_Green, Constants.Karabina_Blue);

        private bool _isCreating = false;

        private ProvisioningOptions _provisioningOptions = null;

        public delegate void SetStatusTextDelegate(string message);

        public SetStatusTextDelegate SetStatusBarText;

        private string[] _contentItems = new string[] {
                                                       "# lookup list items","# custom list items","# document library items",
                                                       "# survey list items","# links list items","# announcements list items",
                                                       "# contacts list items","# events list items","# tasks list items",
                                                       "# discussion board items","# picture library items","# wiki page library items",
                                                       "# gantt tasks list items","# meeting series items","# blog posts list items",
                                                       "# blog comments list items","# blog categories list items","# issue tracking items"
                                                      };

        public ProvisioningOptions ProvisioningOptions
        {
            get { return _provisioningOptions; }
            set
            {
                _provisioningOptions = value;
                if (_provisioningOptions != null)
                {
                    cbRegionalSettings.Checked = _provisioningOptions.RegionalSettings;
                    cbSupportedUILanguages.Checked = _provisioningOptions.SupportedUILanguages;
                    cbAuditSettings.Checked = _provisioningOptions.AuditSettings;
                    cbSitePolicy.Checked = _provisioningOptions.SitePolicy;
                    cbSiteSecurity.Checked = _provisioningOptions.SiteSecurity;
                    cbFields.Checked = _provisioningOptions.Fields;
                    cbContentTypes.Checked = _provisioningOptions.ContentTypes;
                    cbListInstances.Checked = _provisioningOptions.ListInstances;
                    cbCustomActions.Checked = _provisioningOptions.CustomActions;
                    cbFeatures.Checked = _provisioningOptions.Features;
                    cbComposedLook.Checked = _provisioningOptions.ComposedLook;
                    cbPageContents.Checked = _provisioningOptions.PageContents;
                    cbPropertyBagEntries.Checked = _provisioningOptions.PropertyBagEntries;
                    cbPublishing.Checked = _provisioningOptions.Publishing;
                    cbWorkflows.Checked = _provisioningOptions.Workflows;
                    cbWebSettings.Checked = _provisioningOptions.WebSettings;
                    cbNavigation.Checked = _provisioningOptions.Navigation;
                    cbAllTermGroups.Checked = _provisioningOptions.AllTermGroups;
                    cbNativePublishingFiles.Checked = _provisioningOptions.NativePublishingFiles;
                    cbSearchConfiguration.Checked = _provisioningOptions.SearchConfiguration;
                    cbSiteCollectionTermGroup.Checked = _provisioningOptions.SiteCollectionTermGroup;
                    cbSiteGroups.Checked = _provisioningOptions.SiteGroups;
                    cbTermGroupsSecurity.Checked = _provisioningOptions.TermGroupsSecurity;
                    cbBrandingFiles.Checked = _provisioningOptions.BrandingFiles;
                    cbMultiLanguageResources.Checked = _provisioningOptions.MultiLanguageResources;
                    cbPublishingFiles.Checked = _provisioningOptions.PublishingFiles;
                    cbExcludeBaseTemplate.Checked = _provisioningOptions.ExcludeBaseTemplate;

                    clbContentOptions.SetItemChecked(0, _provisioningOptions.LookupListItems);
                    clbContentOptions.SetItemChecked(1, _provisioningOptions.GenericList);
                    clbContentOptions.SetItemChecked(2, _provisioningOptions.DocumentLibrary);
                    clbContentOptions.SetItemChecked(3, _provisioningOptions.SurveyList);
                    clbContentOptions.SetItemChecked(4, _provisioningOptions.LinksList);
                    clbContentOptions.SetItemChecked(5, _provisioningOptions.AnnouncementsList);
                    clbContentOptions.SetItemChecked(6, _provisioningOptions.ContactsList);
                    clbContentOptions.SetItemChecked(7, _provisioningOptions.EventsList);
                    clbContentOptions.SetItemChecked(8, _provisioningOptions.TasksList);
                    clbContentOptions.SetItemChecked(9, _provisioningOptions.DiscussionBoard);
                    clbContentOptions.SetItemChecked(10, _provisioningOptions.PictureLibrary);
                    clbContentOptions.SetItemChecked(11, _provisioningOptions.WikiPageLibrary);
                    clbContentOptions.SetItemChecked(12, _provisioningOptions.GanttTasksList);
                    clbContentOptions.SetItemChecked(13, _provisioningOptions.MeetingSeriesList);
                    clbContentOptions.SetItemChecked(14, _provisioningOptions.BlogPostsList);
                    clbContentOptions.SetItemChecked(15, _provisioningOptions.BlogCommentsList);
                    clbContentOptions.SetItemChecked(16, _provisioningOptions.BlogCategoriesList);
                    clbContentOptions.SetItemChecked(17, _provisioningOptions.IssueTrackingList);


                }

            } //set

        } //ProvisioningOptions

        public TemplateOptions(string title, bool isCreating)
        {
            InitializeComponent();
            _isCreating = isCreating;
            Text = title;
            string hashText = isCreating ? "Include" : "Apply";
            string atText = isCreating ? "Persist" : "Apply";
            IEnumerable<GroupBox> groupBoxes = Controls.OfType<GroupBox>();
            foreach (var groupbox in groupBoxes)
            {
                IEnumerable<CheckBox> checkBoxes = groupbox.Controls.OfType<CheckBox>();
                foreach (var checkbox in checkBoxes)
                {
                    checkbox.Text = checkbox.Text.Replace("#", hashText).Replace("@", atText);

                }

            }

            foreach (string item in _contentItems)
            {
                string updatedSting = item.Replace("#", hashText).Replace("@", atText);
                clbContentOptions.Items.Add(updatedSting);

            }

            cbExcludeBaseTemplate.Enabled = isCreating;

            lToolTip.Text = Properties.Resources.ResourceManager.GetString(Constants.Option00);

            _provisioningOptions = new ProvisioningOptions();

        } //TemplateOptions

        private void bSave_Click(object sender, EventArgs e)
        {
            _provisioningOptions.RegionalSettings = cbRegionalSettings.Checked;
            _provisioningOptions.SupportedUILanguages = cbSupportedUILanguages.Checked;
            _provisioningOptions.AuditSettings = cbAuditSettings.Checked;
            _provisioningOptions.SitePolicy = cbSitePolicy.Checked;
            _provisioningOptions.SiteSecurity = cbSiteSecurity.Checked;
            _provisioningOptions.Fields = cbFields.Checked;
            _provisioningOptions.ContentTypes = cbContentTypes.Checked;
            _provisioningOptions.ListInstances = cbListInstances.Checked;
            _provisioningOptions.CustomActions = cbCustomActions.Checked;
            _provisioningOptions.Features = cbFeatures.Checked;
            _provisioningOptions.ComposedLook = cbComposedLook.Checked;
            _provisioningOptions.PageContents = cbPageContents.Checked;
            _provisioningOptions.PropertyBagEntries = cbPropertyBagEntries.Checked;
            _provisioningOptions.Publishing = cbPublishing.Checked;
            _provisioningOptions.Workflows = cbWorkflows.Checked;
            _provisioningOptions.WebSettings = cbWebSettings.Checked;
            _provisioningOptions.Navigation = cbNavigation.Checked;
            _provisioningOptions.AllTermGroups = cbAllTermGroups.Checked;
            _provisioningOptions.NativePublishingFiles = cbNativePublishingFiles.Checked;
            _provisioningOptions.SearchConfiguration = cbSearchConfiguration.Checked;
            _provisioningOptions.SiteCollectionTermGroup = cbSiteCollectionTermGroup.Checked;
            _provisioningOptions.SiteGroups = cbSiteGroups.Checked;
            _provisioningOptions.TermGroupsSecurity = cbTermGroupsSecurity.Checked;
            _provisioningOptions.BrandingFiles = cbBrandingFiles.Checked;
            _provisioningOptions.MultiLanguageResources = cbMultiLanguageResources.Checked;
            _provisioningOptions.PublishingFiles = cbPublishingFiles.Checked;
            _provisioningOptions.ExcludeBaseTemplate = cbExcludeBaseTemplate.Checked;

            _provisioningOptions.LookupListItems = clbContentOptions.GetItemChecked(0);
            _provisioningOptions.GenericList = clbContentOptions.GetItemChecked(1);
            _provisioningOptions.DocumentLibrary = clbContentOptions.GetItemChecked(2);
            _provisioningOptions.SurveyList = clbContentOptions.GetItemChecked(3);
            _provisioningOptions.LinksList = clbContentOptions.GetItemChecked(4);
            _provisioningOptions.AnnouncementsList = clbContentOptions.GetItemChecked(5);
            _provisioningOptions.ContactsList = clbContentOptions.GetItemChecked(6);
            _provisioningOptions.EventsList = clbContentOptions.GetItemChecked(7);
            _provisioningOptions.TasksList = clbContentOptions.GetItemChecked(8);
            _provisioningOptions.DiscussionBoard = clbContentOptions.GetItemChecked(9);
            _provisioningOptions.PictureLibrary = clbContentOptions.GetItemChecked(10);
            _provisioningOptions.WikiPageLibrary = clbContentOptions.GetItemChecked(11);
            _provisioningOptions.GanttTasksList = clbContentOptions.GetItemChecked(12);
            _provisioningOptions.MeetingSeriesList = clbContentOptions.GetItemChecked(13);
            _provisioningOptions.BlogPostsList = clbContentOptions.GetItemChecked(14);
            _provisioningOptions.BlogCommentsList = clbContentOptions.GetItemChecked(15);
            _provisioningOptions.BlogCategoriesList = clbContentOptions.GetItemChecked(16);
            _provisioningOptions.IssueTrackingList = clbContentOptions.GetItemChecked(17);

            if (_isCreating)
            {
                if (_provisioningOptions.ExcludeBaseTemplate)
                {
                    if (!_provisioningOptions.MoreThanOneOfTemplateOptions)
                    {
                        lToolTip.ForeColor = KarabinaRed;
                        lToolTip.Text = "Exclude Base Template requires more than one of the options under \"Template Options\" to be checked";
                        return;

                    }

                }

            }

            if (_provisioningOptions.OneOfContentOptions)
            {
                _provisioningOptions.ListInstances = true;
                cbListInstances.Checked = true;

            }

            if (_provisioningOptions.LookupListItems)
            {
                _provisioningOptions.Fields = true;
                cbFields.Checked = true;
                _provisioningOptions.ListInstances = true;
                cbListInstances.Checked = true;

            }

            if (!_provisioningOptions.AtLeastOneOfAllOptions)
            {
                lToolTip.ForeColor = KarabinaRed;
                lToolTip.Text = "Please check at least one option";

            }
            else
            {
                lToolTip.ForeColor = SystemColors.ControlText;
                lToolTip.Text = "";
                Close();

            }

        } //bSave_Click

        private void bClose_Click(object sender, EventArgs e)
        {
            Close();

        } //bClose_Click

        private void ShowToolTip(object sender, EventArgs e)
        {
            string tag = "Option"+ (sender as Control).Tag.ToString();

            lToolTip.Text = Properties.Resources.ResourceManager.GetString(tag);

        } //ShowToolTip

        private void HideToolTip(object sender, EventArgs e)
        {
            lToolTip.Text = Properties.Resources.ResourceManager.GetString(Constants.Option00);

        } //HideToolTip

    } //TemplateOptions

}
