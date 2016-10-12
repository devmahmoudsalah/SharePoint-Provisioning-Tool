using System.Collections.Generic;
using System.Security;

namespace Karabina.SharePoint.Provisioning
{
    public class ProvisioningOptions
    {
        private bool _regionalSettings = true;
        private bool _supportedUILanguages = false;
        private bool _auditSettings = true;
        private bool _sitePolicy = true;
        private bool _siteSecurity = true;
        //private bool _TermGroups = false; //value set by _SiteCollectionTermGroup || _AllTermGroups
        private bool _fields = true;
        private bool _contentTypes = true;
        private bool _listInstances = true;
        private bool _customActions = true;
        private bool _features = true;
        private bool _composedLook = true;
        //private bool _searchSettings = false; //value set by _searchConfiguration
        //private bool _files = false; //value from _documentLibraryFiles || _imageFiles || _javaScriptFiles || _publishingPages || _xslStyleSheetFiles
        //private bool _pages = false; //value from _publishingPages || _xslStyleSheetFiles;
        private bool _pageContents = true; //home page only
        private bool _propertyBagEntries = false;
        private bool _publishing = true;
        private bool _workflows = true;
        private bool _webSettings = true;
        private bool _navigation = false;

        private bool _brandingFiles = true;
        private bool _multiLanguageResourceFiles = false;
        private bool _allTermGroups = false;
        private bool _siteCollectionTermGroup = false;
        private bool _siteGroups = false;
        private bool _termGroupsSecurity = false;
        private bool _searchConfiguration = false;
        private bool _publishingFiles = true;
        private bool _nativePublishingFiles = false;
        private bool _lookupListItems = false;
        private bool _genericListItems = false;
        private bool _documentLibraryFiles = false;
        private bool _javaScriptFiles = false;
        private bool _publishingPages = false;
        private bool _excludeBaseTemplate = false;
        private bool _xslStyleSheetFiles = false;
        private bool _imageFiles = false;

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

        public bool RegionalSettings
        {
            get { return _regionalSettings; }
            set { _regionalSettings = value; }
        }

        public bool SupportedUILanguages
        {
            get { return _supportedUILanguages; }
            set { _supportedUILanguages = value; }
        }

        public bool AuditSettings
        {
            get { return _auditSettings; }
            set { _auditSettings = value; }
        }

        public bool SitePolicy
        {
            get { return _sitePolicy; }
            set { _sitePolicy = value; }
        }

        public bool SiteSecurity
        {
            get { return _siteSecurity; }
            set { _siteSecurity = value; }
        }

        public bool TermGroups
        {
            get
            {
                return _siteCollectionTermGroup || _allTermGroups;
            }
        }

        public bool Fields
        {
            get { return _fields; }
            set { _fields = value; }
        }

        public bool ContentTypes
        {
            get { return _contentTypes; }
            set { _contentTypes = value; }
        }

        public bool ListInstances
        {
            get { return _listInstances; }
            set { _listInstances = value; }
        }

        public bool CustomActions
        {
            get { return _customActions; }
            set { _customActions = value; }
        }

        public bool Features
        {
            get { return _features; }
            set { _features = value; }
        }

        public bool ComposedLook
        {
            get { return _composedLook; }
            set { _composedLook = value; }
        }

        public bool SearchSettings
        {
            get { return _searchConfiguration; }
        }

        public bool Files
        {
            get { return _documentLibraryFiles || _imageFiles || _javaScriptFiles || _publishingPages || _xslStyleSheetFiles; }
        }

        public bool Pages
        {
            get { return _publishingPages || _xslStyleSheetFiles; }
        }

        public bool PageContents
        {
            get { return _pageContents; }
            set { _pageContents = value; }
        }

        public bool PropertyBagEntries
        {
            get { return _propertyBagEntries; }
            set { _propertyBagEntries = value; }
        }

        public bool Publishing
        {
            get { return _publishing; }
            set { _publishing = value; }
        }

        public bool Workflows
        {
            get { return _workflows; }
            set { _workflows = value; }
        }

        public bool WebSettings
        {
            get { return _webSettings; }
            set { _webSettings = value; }
        }

        public bool Navigation
        {
            get { return _navigation; }
            set { _navigation = value; }
        }

        public bool ExtensibilityProviders
        {
            get { return false; }
        }

        /// <summary>
        /// Will create resource files named "PnP_Resource_[LCID].resx for every supported language.
        /// </summary>
        public bool MultiLanguageResources
        {
            get { return _multiLanguageResourceFiles; }
            set { _multiLanguageResourceFiles = value; }
        }

        /// <summary>
        /// Do composed look files (theme files, site logo, alternate css)
        /// </summary>
        public bool BrandingFiles
        {
            get { return _brandingFiles; }
            set { _brandingFiles = value; }
        }

        /// <summary>
        /// Defines whether to extract or apply publishing files (MasterPages and PageLayouts)
        /// </summary>
        public bool PublishingFiles
        {
            get { return _publishingFiles; }
            set { _publishingFiles = value; }
        }

        /// <summary>
        /// Defines whether to extract or applay native publishing files (MasterPages and PageLayouts)
        /// </summary>
        public bool NativePublishingFiles
        {
            get { return _nativePublishingFiles; }
            set { _nativePublishingFiles = value; }
        }

        public bool AllTermGroups
        {
            get { return _allTermGroups; }
            set { _allTermGroups = value; }
        }

        public bool SiteCollectionTermGroup
        {
            get { return _siteCollectionTermGroup; }
            set { _siteCollectionTermGroup = value; }
        }

        public bool TermGroupsSecurity
        {
            get { return _termGroupsSecurity; }
            set { _termGroupsSecurity = value; }
        }

        public bool SiteGroups
        {
            get { return _siteGroups; }
            set { _siteGroups = value; }
        }

        public bool SearchConfiguration
        {
            get { return _searchConfiguration; }
            set { _searchConfiguration = value; }
        }

        public bool LookupListItems
        {
            get { return _lookupListItems; }
            set { _lookupListItems = value; }
        }

        public bool GenericListItems
        {
            get { return _genericListItems; }
            set { _genericListItems = value; }
        }

        public bool DocumentLibraryFiles
        {
            get { return _documentLibraryFiles; }
            set { _documentLibraryFiles = value; }
        }

        public bool JavaScriptFiles
        {
            get { return _javaScriptFiles; }
            set { _javaScriptFiles = value; }
        }

        public bool PublishingPages
        {
            get { return _publishingPages; }
            set { _publishingPages = value; }
        }

        public bool ExcludeBaseTemplate
        {
            get { return _excludeBaseTemplate; }
            set { _excludeBaseTemplate = value; }
        }

        public bool XSLStyleSheetFiles
        {
            get { return _xslStyleSheetFiles; }
            set { _xslStyleSheetFiles = value; }
        }

        public bool ImageFiles
        {
            get { return _imageFiles; }
            set { _imageFiles = value; }
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
                if (_regionalSettings) { return true; }
                if (_supportedUILanguages) { return true; }
                if (_auditSettings) { return true; }
                if (_sitePolicy) { return true; }
                if (_siteSecurity) { return true; }
                if (_fields) { return true; }
                if (_contentTypes) { return true; }
                if (_listInstances) { return true; }
                if (_customActions) { return true; }
                if (_features) { return true; }
                if (_composedLook) { return true; }
                if (_pageContents) { return true; }
                if (_propertyBagEntries) { return true; }
                if (_publishing) { return true; }
                if (_workflows) { return true; }
                if (_webSettings) { return true; }
                if (_navigation) { return true; }
                return false;
            }
        }

        public bool AtLeastOneOfAllOptions
        {
            get
            {
                if (_regionalSettings) { return true; }
                if (_supportedUILanguages) { return true; }
                if (_auditSettings) { return true; }
                if (_sitePolicy) { return true; }
                if (_siteSecurity) { return true; }
                if (_fields) { return true; }
                if (_contentTypes) { return true; }
                if (_listInstances) { return true; }
                if (_customActions) { return true; }
                if (_features) { return true; }
                if (_composedLook) { return true; }
                if (_pageContents) { return true; }
                if (_propertyBagEntries) { return true; }
                if (_publishing) { return true; }
                if (_workflows) { return true; }
                if (_webSettings) { return true; }
                if (_navigation) { return true; }
                if (_brandingFiles) { return true; }
                if (_multiLanguageResourceFiles) { return true; }
                if (_allTermGroups) { return true; }
                if (_siteCollectionTermGroup) { return true; }
                if (_siteGroups) { return true; }
                if (_termGroupsSecurity) { return true; }
                if (_searchConfiguration) { return true; }
                if (_publishingFiles) { return true; }
                if (_nativePublishingFiles) { return true; }
                if (_lookupListItems) { return true; }
                if (_genericListItems) { return true; }
                if (_documentLibraryFiles) { return true; }
                if (_javaScriptFiles) { return true; }
                if (_publishingPages) { return true; }
                if (_excludeBaseTemplate) { return true; }
                if (_xslStyleSheetFiles) { return true; }
                if (_imageFiles) { return true; }

                return false;
            }
        }

        public bool MoreThanOneOfTemplateOptions
        {
            get
            {
                int count = 0;

                if (_regionalSettings) { count++; }
                if (_supportedUILanguages) { count++; }
                if (_auditSettings) { count++; }
                if (_sitePolicy) { count++; }
                if (_siteSecurity) { count++; }
                if (_fields) { count++; }
                if (_contentTypes) { count++; }
                if (_listInstances) { count++; }
                if (_customActions) { count++; }
                if (_features) { count++; }
                if (_composedLook) { count++; }
                if (_pageContents) { count++; }
                if (_propertyBagEntries) { count++; } 
                if (_publishing) { count++; }
                if (_workflows) { count++; }
                if (_webSettings) { count++; }
                if (_navigation) { count++; }

                return count > 1;
            }

        }

    }

}
