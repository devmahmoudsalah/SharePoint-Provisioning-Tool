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
                    cbDocumentLibraryFiles.Checked = _provisioningOptions.DocumentLibraryFiles;
                    cbLookupListItems.Checked = _provisioningOptions.LookupListItems;
                    cbGenericListItems.Checked = _provisioningOptions.GenericListItems;
                    cbAllTermGroups.Checked = _provisioningOptions.AllTermGroups;
                    cbNativePublishingFiles.Checked = _provisioningOptions.NativePublishingFiles;
                    cbSearchConfiguration.Checked = _provisioningOptions.SearchConfiguration;
                    cbSiteCollectionTermGroup.Checked = _provisioningOptions.SiteCollectionTermGroup;
                    cbSiteGroups.Checked = _provisioningOptions.SiteGroups;
                    cbTermGroupsSecurity.Checked = _provisioningOptions.TermGroupsSecurity;
                    cbJavaScriptFiles.Checked = _provisioningOptions.JavaScriptFiles;
                    cbBrandingFiles.Checked = _provisioningOptions.BrandingFiles;
                    cbMultiLanguageResources.Checked = _provisioningOptions.MultiLanguageResources;
                    cbPublishingFiles.Checked = _provisioningOptions.PublishingFiles;
                    cbPublishingPages.Checked = _provisioningOptions.PublishingPages;
                    cbExcludeBaseTemplate.Checked = _provisioningOptions.ExcludeBaseTemplate;
                    cbXSLStyleSheetFiles.Checked = _provisioningOptions.XSLStyleSheetFiles;
                    cbImageFiles.Checked = _provisioningOptions.ImageFiles;

                }

            }

        }

        public TemplateOptions(string title, bool isCreating)
        {
            InitializeComponent();
            _isCreating = isCreating;
            Text = title;
            string hashText = isCreating ? "Include" : "Apply";
            string atText = isCreating ? "Persist" : "Apply";
            IEnumerable<GroupBox> groupBoxes = Controls.OfType<GroupBox>();
            foreach(var groupbox in groupBoxes)
            {
                IEnumerable<CheckBox> checkBoxes = groupbox.Controls.OfType<CheckBox>();
                foreach (var checkbox in checkBoxes)
                {
                    checkbox.Text = checkbox.Text.Replace("#", hashText).Replace("@", atText);

                }

            }

            cbExcludeBaseTemplate.Enabled = isCreating;            

            _provisioningOptions = new ProvisioningOptions();

        }

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
            _provisioningOptions.DocumentLibraryFiles = cbDocumentLibraryFiles.Checked;
            _provisioningOptions.LookupListItems = cbLookupListItems.Checked;
            _provisioningOptions.GenericListItems = cbGenericListItems.Checked;
            _provisioningOptions.AllTermGroups = cbAllTermGroups.Checked;
            _provisioningOptions.NativePublishingFiles = cbNativePublishingFiles.Checked;
            _provisioningOptions.SearchConfiguration = cbSearchConfiguration.Checked;
            _provisioningOptions.SiteCollectionTermGroup = cbSiteCollectionTermGroup.Checked;
            _provisioningOptions.SiteGroups = cbSiteGroups.Checked;
            _provisioningOptions.TermGroupsSecurity = cbTermGroupsSecurity.Checked;
            _provisioningOptions.JavaScriptFiles = cbJavaScriptFiles.Checked;
            _provisioningOptions.BrandingFiles = cbBrandingFiles.Checked;
            _provisioningOptions.MultiLanguageResources = cbMultiLanguageResources.Checked;
            _provisioningOptions.PublishingFiles = cbPublishingFiles.Checked;
            _provisioningOptions.PublishingPages = cbPublishingPages.Checked;
            _provisioningOptions.ExcludeBaseTemplate = cbExcludeBaseTemplate.Checked;
            _provisioningOptions.XSLStyleSheetFiles = cbXSLStyleSheetFiles.Checked;
            _provisioningOptions.ImageFiles = cbImageFiles.Checked;

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

            if (_provisioningOptions.GenericListItems)
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

        }
        
        private void bClose_Click(object sender, EventArgs e)
        {
            Close();

        }

        private void ShowToolTip(object sender, EventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            int tooltipId = Convert.ToInt32(cb.Tag);

        }

        private void HideToolTip(object sender, EventArgs e)
        {
            lToolTip.Text = "Move the mouse over the items to see the explanation of the option";

        }

    }

}
