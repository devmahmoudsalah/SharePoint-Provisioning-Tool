using System.Collections.Generic;
using System.Security;

namespace Karabina.SharePoint.Provisioning
{
    public class ProvisioningOptions
    {
        private bool _includeRegionalSettings = true;
        private bool _includeSupportedUILanguages = false;
        private bool _includeAuditSettings = true;
        private bool _includeSitePolicy = true;
        private bool _includeSiteSecurity = true;
        //private bool _includeTermGroups = false; //value set by _includeSiteCollectionTermGroup || _includeAllTermGroups
        private bool _includeFields = true;
        private bool _includeContentTypes = true;
        private bool _includeListInstances = true;
        private bool _includeCustomActions = true;
        private bool _includeFeatures = true;
        private bool _includeComposedLook = true;
        //private bool _includeSearchSettings = false; //value set by _includeSearchConfiguration
        //private bool _includeFiles = false; //value from _includeDocumentLibraryFiles || _includeImageFiles || _includeJavaScriptFiles || _includePublishingPages || _includeXSLStyleSheetFiles
        //private bool _includePages = false; //value from _includePublishingPages || _includeXSLStyleSheetFiles;
        private bool _includePageContents = true; //home page only
        private bool _includePropertyBagEntries = false;
        private bool _includePublishing = true;
        private bool _includeWorkflows = true;
        private bool _includeWebSettings = true;
        private bool _includeNavigation = false;

        private bool _persistBrandingFiles = true;
        private bool _persistMultiLanguageResourceFiles = false;
        private bool _includeAllTermGroups = false;
        private bool _includeSiteCollectionTermGroup = false;
        private bool _includeSiteGroups = false;
        private bool _includeTermGroupsSecurity = false;
        private bool _includeSearchConfiguration = false;
        private bool _persistPublishingFiles = true;
        private bool _includeNativePublishingFiles = false;
        private bool _includeLookupListItems = false;
        private bool _includeGenericListItems = false;
        private bool _includeDocumentLibraryFiles = false;
        private bool _includeJavaScriptFiles = false;
        private bool _includePublishingPages = false;
        private bool _excludeBaseTemplate = false;
        private bool _includeXSLStyleSheetFiles = false;
        private bool _includeImageFiles = false;

        private bool _authenticationRequired = true;
        private string _userNameOrEmail = string.Empty;
        private SecureString _userPassword = null;
        private string _userDomain = string.Empty;
        private string _templateName = string.Empty;
        private string _templatePath = string.Empty;
        private string _webAddress = string.Empty;



        public ProvisioningOptions()
        {
            //nothing currently
        }

        public bool IncludeRegionalSettings
        {
            get { return _includeRegionalSettings; }
            set { _includeRegionalSettings = value; }
        }

        public bool IncludeSupportedUILanguages
        {
            get { return _includeSupportedUILanguages; }
            set { _includeSupportedUILanguages = value; }
        }

        public bool IncludeAuditSettings
        {
            get { return _includeAuditSettings; }
            set { _includeAuditSettings = value; }
        }

        public bool IncludeSitePolicy
        {
            get { return _includeSitePolicy; }
            set { _includeSitePolicy = value; }
        }

        public bool IncludeSiteSecurity
        {
            get { return _includeSiteSecurity; }
            set { _includeSiteSecurity = value; }
        }

        public bool IncludeTermGroups
        {
            get
            {
                return _includeSiteCollectionTermGroup || _includeAllTermGroups;
            }
        }

        public bool IncludeFields
        {
            get { return _includeFields; }
            set { _includeFields = value; }
        }

        public bool IncludeContentTypes
        {
            get { return _includeContentTypes; }
            set { _includeContentTypes = value; }
        }

        public bool IncludeListInstances
        {
            get { return _includeListInstances; }
            set { _includeListInstances = value; }
        }

        public bool IncludeCustomActions
        {
            get { return _includeCustomActions; }
            set { _includeCustomActions = value; }
        }

        public bool IncludeFeatures
        {
            get { return _includeFeatures; }
            set { _includeFeatures = value; }
        }

        public bool IncludeComposedLook
        {
            get { return _includeComposedLook; }
            set { _includeComposedLook = value; }
        }

        public bool IncludeSearchSettings
        {
            get { return _includeSearchConfiguration; }
        }

        public bool IncludeFiles
        {
            get { return _includeDocumentLibraryFiles || _includeImageFiles || _includeJavaScriptFiles || _includePublishingPages || _includeXSLStyleSheetFiles; }
        }

        public bool IncludePages
        {
            get { return _includePublishingPages || _includeXSLStyleSheetFiles; }
        }

        public bool IncludePageContents
        {
            get { return _includePageContents; }
            set { _includePageContents = value; }
        }

        public bool IncludePropertyBagEntries
        {
            get { return _includePropertyBagEntries; }
            set { _includePropertyBagEntries = value; }
        }

        public bool IncludePublishing
        {
            get { return _includePublishing; }
            set { _includePublishing = value; }
        }

        public bool IncludeWorkflows
        {
            get { return _includeWorkflows; }
            set { _includeWorkflows = value; }
        }

        public bool IncludeWebSettings
        {
            get { return _includeWebSettings; }
            set { _includeWebSettings = value; }
        }

        public bool IncludeNavigation
        {
            get { return _includeNavigation; }
            set { _includeNavigation = value; }
        }

        public bool ExtensibilityProviders
        {
            get { return false; }
        }

        /// <summary>
        /// Will create resource files named "PnP_Resource_[LCID].resx for every supported language.
        /// </summary>
        public bool PersistMultiLanguageResources
        {
            get { return _persistMultiLanguageResourceFiles; }
            set { _persistMultiLanguageResourceFiles = value; }
        }

        /// <summary>
        /// Do composed look files (theme files, site logo, alternate css)
        /// </summary>
        public bool PersistBrandingFiles
        {
            get { return _persistBrandingFiles; }
            set { _persistBrandingFiles = value; }
        }

        /// <summary>
        /// Defines whether to persist publishing files (MasterPages and PageLayouts)
        /// </summary>
        public bool PersistPublishingFiles
        {
            get { return _persistPublishingFiles; }
            set { _persistPublishingFiles = value; }
        }

        /// <summary>
        /// Defines whether to extract native publishing files (MasterPages and PageLayouts)
        /// </summary>
        public bool IncludeNativePublishingFiles
        {
            get { return _includeNativePublishingFiles; }
            set { _includeNativePublishingFiles = value; }
        }

        public bool IncludeAllTermGroups
        {
            get { return _includeAllTermGroups; }
            set { _includeAllTermGroups = value; }
        }

        public bool IncludeSiteCollectionTermGroup
        {
            get { return _includeSiteCollectionTermGroup; }
            set { _includeSiteCollectionTermGroup = value; }
        }

        public bool IncludeTermGroupsSecurity
        {
            get { return _includeTermGroupsSecurity; }
            set { _includeTermGroupsSecurity = value; }
        }

        public bool IncludeSiteGroups
        {
            get { return _includeSiteGroups; }
            set { _includeSiteGroups = value; }
        }

        public bool IncludeSearchConfiguration
        {
            get { return _includeSearchConfiguration; }
            set { _includeSearchConfiguration = value; }
        }

        public bool IncludeLookupListItems
        {
            get { return _includeLookupListItems; }
            set { _includeLookupListItems = value; }
        }

        public bool IncludeGenericListItems
        {
            get { return _includeGenericListItems; }
            set { _includeGenericListItems = value; }
        }

        public bool IncludeDocumentLibraryFiles
        {
            get { return _includeDocumentLibraryFiles; }
            set { _includeDocumentLibraryFiles = value; }
        }

        public bool IncludeJavaScriptFiles
        {
            get { return _includeJavaScriptFiles; }
            set { _includeJavaScriptFiles = value; }
        }

        public bool IncludePublishingPages
        {
            get { return _includePublishingPages; }
            set { _includePublishingPages = value; }
        }

        public bool ExcludeBaseTemplate
        {
            get { return _excludeBaseTemplate; }
            set { _excludeBaseTemplate = value; }
        }

        public bool IncludeXSLStyleSheetFiles
        {
            get { return _includeXSLStyleSheetFiles; }
            set { _includeXSLStyleSheetFiles = value; }
        }

        public bool IncludeImageFiles
        {
            get { return _includeImageFiles; }
            set { _includeImageFiles = value; }
        }

        public bool AuthenticationRequired
        {
            get { return _authenticationRequired; }
            set { _authenticationRequired = value; }
        }

        public string UserNameOrEmail
        {
            get { return _userNameOrEmail; }
            set
            {
                _userNameOrEmail = value;
                if (_userNameOrEmail.IndexOf('\\') > 0)
                {
                    string[] userNameDomainName = _userNameOrEmail.Split(new char[] { '\\' });
                    _userDomain = userNameDomainName[0];
                    _userNameOrEmail = userNameDomainName[1];
                }
            }
        }

        public SecureString UserPassword
        {
            get { return _userPassword; }
            set { _userPassword = value; }
        }

        public string UserDomain
        {
            get { return _userDomain; }
            private set { _userDomain = value; }
        }

        public string TemplateName
        {
            get { return _templateName; }
            set { _templateName = value; }
        }

        public string TemplatePath
        {
            get { return _templatePath; }
            set { _templatePath = value; }
        }

        public string WebAddress
        {
            get { return _webAddress; }
            set { _webAddress = value; }
        }

        public bool AtLeastOneOfTemplateOptions
        {
            get
            {
                if (_includeRegionalSettings) { return true; }
                if (_includeSupportedUILanguages) { return true; }
                if (_includeAuditSettings) { return true; }
                if (_includeSitePolicy) { return true; }
                if (_includeSiteSecurity) { return true; }
                if (_includeFields) { return true; }
                if (_includeContentTypes) { return true; }
                if (_includeListInstances) { return true; }
                if (_includeCustomActions) { return true; }
                if (_includeFeatures) { return true; }
                if (_includeComposedLook) { return true; }
                if (_includePageContents) { return true; }
                if (_includePropertyBagEntries) { return true; }
                if (_includePublishing) { return true; }
                if (_includeWorkflows) { return true; }
                if (_includeWebSettings) { return true; }
                if (_includeNavigation) { return true; }
                return false;
            }
        }

        public bool AtLeastOneOfAllOptions
        {
            get
            {
                if (_includeRegionalSettings) { return true; }
                if (_includeSupportedUILanguages) { return true; }
                if (_includeAuditSettings) { return true; }
                if (_includeSitePolicy) { return true; }
                if (_includeSiteSecurity) { return true; }
                if (_includeFields) { return true; }
                if (_includeContentTypes) { return true; }
                if (_includeListInstances) { return true; }
                if (_includeCustomActions) { return true; }
                if (_includeFeatures) { return true; }
                if (_includeComposedLook) { return true; }
                if (_includePageContents) { return true; }
                if (_includePropertyBagEntries) { return true; }
                if (_includePublishing) { return true; }
                if (_includeWorkflows) { return true; }
                if (_includeWebSettings) { return true; }
                if (_includeNavigation) { return true; }
                if (_persistBrandingFiles) { return true; }
                if (_persistMultiLanguageResourceFiles) { return true; }
                if (_includeAllTermGroups) { return true; }
                if (_includeSiteCollectionTermGroup) { return true; }
                if (_includeSiteGroups) { return true; }
                if (_includeTermGroupsSecurity) { return true; }
                if (_includeSearchConfiguration) { return true; }
                if (_persistPublishingFiles) { return true; }
                if (_includeNativePublishingFiles) { return true; }
                if (_includeLookupListItems) { return true; }
                if (_includeGenericListItems) { return true; }
                if (_includeDocumentLibraryFiles) { return true; }
                if (_includeJavaScriptFiles) { return true; }
                if (_includePublishingPages) { return true; }
                if (_excludeBaseTemplate) { return true; }
                if (_includeXSLStyleSheetFiles) { return true; }
                if (_includeImageFiles) { return true; }

                return false;
            }
        }

        public bool MoreThanOneOfTemplateOptions
        {
            get
            {
                int count = 0;

                if (_includeRegionalSettings) { count++; }
                if (_includeSupportedUILanguages) { count++; }
                if (_includeAuditSettings) { count++; }
                if (_includeSitePolicy) { count++; }
                if (_includeSiteSecurity) { count++; }
                if (_includeFields) { count++; }
                if (_includeContentTypes) { count++; }
                if (_includeListInstances) { count++; }
                if (_includeCustomActions) { count++; }
                if (_includeFeatures) { count++; }
                if (_includeComposedLook) { count++; }
                if (_includePageContents) { count++; }
                if (_includePropertyBagEntries) { count++; } 
                if (_includePublishing) { count++; }
                if (_includeWorkflows) { count++; }
                if (_includeWebSettings) { count++; }
                if (_includeNavigation) { count++; }

                return count > 1;
            }

        }

    }

}
