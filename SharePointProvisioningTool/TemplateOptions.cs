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
                    cbIncludeRegionalSettings.Checked = _provisioningOptions.IncludeRegionalSettings;
                    cbIncludeSupportedUILanguages.Checked = _provisioningOptions.IncludeSupportedUILanguages;
                    cbIncludeAuditSettings.Checked = _provisioningOptions.IncludeAuditSettings;
                    cbIncludeSitePolicy.Checked = _provisioningOptions.IncludeSitePolicy;
                    cbIncludeSiteSecurity.Checked = _provisioningOptions.IncludeSiteSecurity;
                    cbIncludeFields.Checked = _provisioningOptions.IncludeFields;
                    cbIncludeContentTypes.Checked = _provisioningOptions.IncludeContentTypes;
                    cbIncludeListInstances.Checked = _provisioningOptions.IncludeListInstances;
                    cbIncludeCustomActions.Checked = _provisioningOptions.IncludeCustomActions;
                    cbIncludeFeatures.Checked = _provisioningOptions.IncludeFeatures;
                    cbIncludeComposedLook.Checked = _provisioningOptions.IncludeComposedLook;
                    cbIncludePageContents.Checked = _provisioningOptions.IncludePageContents;
                    cbIncludePropertyBagEntries.Checked = _provisioningOptions.IncludePropertyBagEntries;
                    cbIncludePublishing.Checked = _provisioningOptions.IncludePublishing;
                    cbIncludeWorkflows.Checked = _provisioningOptions.IncludeWorkflows;
                    cbIncludeWebSettings.Checked = _provisioningOptions.IncludeWebSettings;
                    cbIncludeNavigation.Checked = _provisioningOptions.IncludeNavigation;
                    cbDocumentLibraryFiles.Checked = _provisioningOptions.IncludeDocumentLibraryFiles;
                    cbIncludeLookupListItems.Checked = _provisioningOptions.IncludeLookupListItems;
                    cbGenericListItems.Checked = _provisioningOptions.IncludeGenericListItems;
                    cbIncludeAllTermGroups.Checked = _provisioningOptions.IncludeAllTermGroups;
                    cbIncludeNativePublishingFiles.Checked = _provisioningOptions.IncludeNativePublishingFiles;
                    cbIncludeSearchConfiguration.Checked = _provisioningOptions.IncludeSearchConfiguration;
                    cbIncludeSiteCollectionTermGroup.Checked = _provisioningOptions.IncludeSiteCollectionTermGroup;
                    cbIncludeSiteGroups.Checked = _provisioningOptions.IncludeSiteGroups;
                    cbIncludeTermGroupsSecurity.Checked = _provisioningOptions.IncludeTermGroupsSecurity;
                    cbJavaScriptFiles.Checked = _provisioningOptions.IncludeJavaScriptFiles;
                    cbPersistBrandingFiles.Checked = _provisioningOptions.PersistBrandingFiles;
                    cbPersistMultiLanguageResources.Checked = _provisioningOptions.PersistMultiLanguageResources;
                    cbPersistPublishingFiles.Checked = _provisioningOptions.PersistPublishingFiles;
                    cbPublishingPages.Checked = _provisioningOptions.IncludePublishingPages;
                    cbExcludeBaseTemplate.Checked = _provisioningOptions.ExcludeBaseTemplate;
                    cbXSLStyleSheetFiles.Checked = _provisioningOptions.IncludeXSLStyleSheetFiles;
                    cbIncludeImageFiles.Checked = _provisioningOptions.IncludeImageFiles;
                }
            }
        }

        public TemplateOptions()
        {
            InitializeComponent();
            _provisioningOptions = new ProvisioningOptions();
        }

        private void bSave_Click(object sender, EventArgs e)
        {
            _provisioningOptions.IncludeRegionalSettings = cbIncludeRegionalSettings.Checked;
            _provisioningOptions.IncludeSupportedUILanguages = cbIncludeSupportedUILanguages.Checked;
            _provisioningOptions.IncludeAuditSettings = cbIncludeAuditSettings.Checked;
            _provisioningOptions.IncludeSitePolicy = cbIncludeSitePolicy.Checked;
            _provisioningOptions.IncludeSiteSecurity = cbIncludeSiteSecurity.Checked;
            _provisioningOptions.IncludeFields = cbIncludeFields.Checked;
            _provisioningOptions.IncludeContentTypes = cbIncludeContentTypes.Checked;
            _provisioningOptions.IncludeListInstances = cbIncludeListInstances.Checked;
            _provisioningOptions.IncludeCustomActions = cbIncludeCustomActions.Checked;
            _provisioningOptions.IncludeFeatures = cbIncludeFeatures.Checked;
            _provisioningOptions.IncludeComposedLook = cbIncludeComposedLook.Checked;
            _provisioningOptions.IncludePageContents = cbIncludePageContents.Checked;
            _provisioningOptions.IncludePropertyBagEntries = cbIncludePropertyBagEntries.Checked;
            _provisioningOptions.IncludePublishing = cbIncludePublishing.Checked;
            _provisioningOptions.IncludeWorkflows = cbIncludeWorkflows.Checked;
            _provisioningOptions.IncludeWebSettings = cbIncludeWebSettings.Checked;
            _provisioningOptions.IncludeNavigation = cbIncludeNavigation.Checked;
            _provisioningOptions.IncludeDocumentLibraryFiles = cbDocumentLibraryFiles.Checked;
            _provisioningOptions.IncludeLookupListItems = cbIncludeLookupListItems.Checked;
            _provisioningOptions.IncludeGenericListItems = cbGenericListItems.Checked;
            _provisioningOptions.IncludeAllTermGroups = cbIncludeAllTermGroups.Checked;
            _provisioningOptions.IncludeNativePublishingFiles = cbIncludeNativePublishingFiles.Checked;
            _provisioningOptions.IncludeSearchConfiguration = cbIncludeSearchConfiguration.Checked;
            _provisioningOptions.IncludeSiteCollectionTermGroup = cbIncludeSiteCollectionTermGroup.Checked;
            _provisioningOptions.IncludeSiteGroups = cbIncludeSiteGroups.Checked;
            _provisioningOptions.IncludeTermGroupsSecurity = cbIncludeTermGroupsSecurity.Checked;
            _provisioningOptions.IncludeJavaScriptFiles = cbJavaScriptFiles.Checked;
            _provisioningOptions.PersistBrandingFiles = cbPersistBrandingFiles.Checked;
            _provisioningOptions.PersistMultiLanguageResources = cbPersistMultiLanguageResources.Checked;
            _provisioningOptions.PersistPublishingFiles = cbPersistPublishingFiles.Checked;
            _provisioningOptions.IncludePublishingPages = cbPublishingPages.Checked;
            _provisioningOptions.ExcludeBaseTemplate = cbExcludeBaseTemplate.Checked;
            _provisioningOptions.IncludeXSLStyleSheetFiles = cbXSLStyleSheetFiles.Checked;
            _provisioningOptions.IncludeImageFiles = cbIncludeImageFiles.Checked;

            if (_provisioningOptions.ExcludeBaseTemplate)
            {
                if (!_provisioningOptions.MoreThanOneOfTemplateOptions)
                {
                    lToolTip.ForeColor = Color.Red;
                    lToolTip.Text = "Exclude Base Template requires more than one of the options under \"Template Options\" to be checked";
                    return;
                }
            }

            if (_provisioningOptions.IncludeGenericListItems)
            {
                _provisioningOptions.IncludeListInstances = true;
                cbIncludeListInstances.Checked = true;
            }

            if (_provisioningOptions.IncludeLookupListItems)
            {
                _provisioningOptions.IncludeFields = true;
                cbIncludeFields.Checked = true;
                _provisioningOptions.IncludeListInstances = true;
                cbIncludeListInstances.Checked = true;
            }

            if (!_provisioningOptions.AtLeastOneOfAllOptions)
            {
                lToolTip.ForeColor = Color.Red;
                lToolTip.Text = "Please check at least one option";
            }
            else
            {
                lToolTip.ForeColor = Color.FromKnownColor(KnownColor.ControlText);
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
