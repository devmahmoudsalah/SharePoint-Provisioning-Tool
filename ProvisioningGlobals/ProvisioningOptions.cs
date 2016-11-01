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
        private bool _excludeBaseTemplate = false;

        private bool _lookupListItems = false;
        private bool _genericList = false;
        private bool _documentLibrary = false;
        private bool _surveyList = false;
        private bool _linksList = false;
        private bool _announcementsList = false;
        private bool _contactsList = false;
        private bool _eventsList = false;
        private bool _tasksList = false;
        private bool _discussionBoard = false;
        private bool _pictureLibrary = false;
        private bool _wikiPageLibrary = false;
        private bool _ganttTasksList = false;
        private bool _meetingSeriesList = false;
        private bool _blogPostsList = false;
        private bool _blogCommentsList = false;
        private bool _blogCategoriesList = false;
        private bool _issueTrackingList = false;

        //Security fields - used by the Create / Apply template methods
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

        } //ProvisioningOptions

        public bool RegionalSettings
        {
            get { return _regionalSettings; }
            set { _regionalSettings = value; }

        } //RegionalSettings

        public bool SupportedUILanguages
        {
            get { return _supportedUILanguages; }
            set { _supportedUILanguages = value; }

        } //SupportedUILanguages

        public bool AuditSettings
        {
            get { return _auditSettings; }
            set { _auditSettings = value; }

        } //AuditSettings

        public bool SitePolicy
        {
            get { return _sitePolicy; }
            set { _sitePolicy = value; }

        } //SitePolicy

        public bool SiteSecurity
        {
            get { return _siteSecurity; }
            set { _siteSecurity = value; }

        } //SiteSecurity

        public bool TermGroups
        {
            get
            {
                return _siteCollectionTermGroup || _allTermGroups;

            } //get

        } //TermGroups

        public bool Fields
        {
            get { return _fields; }
            set { _fields = value; }

        } //Fields

        public bool ContentTypes
        {
            get { return _contentTypes; }
            set { _contentTypes = value; }

        } //ContentTypes

        public bool ListInstances
        {
            get { return _listInstances; }
            set { _listInstances = value; }

        } //ListInstances

        public bool CustomActions
        {
            get { return _customActions; }
            set { _customActions = value; }

        } //CustomActions

        public bool Features
        {
            get { return _features; }
            set { _features = value; }

        } //Features

        public bool ComposedLook
        {
            get { return _composedLook; }
            set { _composedLook = value; }

        } //ComposedLook

        public bool SearchSettings
        {
            get { return _searchConfiguration; }

        } //SearchSettings

        public bool PageContents
        {
            get { return _pageContents; }
            set { _pageContents = value; }

        } //PageContents

        public bool PropertyBagEntries
        {
            get { return _propertyBagEntries; }
            set { _propertyBagEntries = value; }

        } //PropertyBagEntries

        public bool Publishing
        {
            get { return _publishing; }
            set { _publishing = value; }

        } //Publishing

        public bool Workflows
        {
            get { return _workflows; }
            set { _workflows = value; }

        } //Workflows

        public bool WebSettings
        {
            get { return _webSettings; }
            set { _webSettings = value; }

        } //WebSettings

        public bool Navigation
        {
            get { return _navigation; }
            set { _navigation = value; }

        } //Navigation

        public bool ExtensibilityProviders
        {
            get { return false; }

        } //ExtensibilityProviders

        /// <summary>
        /// Will create resource files named "PnP_Resource_[LCID].resx for every supported language.
        /// </summary>
        public bool MultiLanguageResources
        {
            get { return _multiLanguageResourceFiles; }
            set { _multiLanguageResourceFiles = value; }

        } //MultiLanguageResources

        /// <summary>
        /// Do composed look files (theme files, site logo, alternate css)
        /// </summary>
        public bool BrandingFiles
        {
            get { return _brandingFiles; }
            set { _brandingFiles = value; }

        } //BrandingFiles

        /// <summary>
        /// Defines whether to extract or apply publishing files (MasterPages and PageLayouts)
        /// </summary>
        public bool PublishingFiles
        {
            get { return _publishingFiles; }
            set { _publishingFiles = value; }

        } //PublishingFiles

        /// <summary>
        /// Defines whether to extract or applay native publishing files (MasterPages and PageLayouts)
        /// </summary>
        public bool NativePublishingFiles
        {
            get { return _nativePublishingFiles; }
            set { _nativePublishingFiles = value; }

        } //NativePublishingFiles

        public bool AllTermGroups
        {
            get { return _allTermGroups; }
            set { _allTermGroups = value; }

        } //AllTermGroups

        public bool SiteCollectionTermGroup
        {
            get { return _siteCollectionTermGroup; }
            set { _siteCollectionTermGroup = value; }

        } //SiteCollectionTermGroup

        public bool TermGroupsSecurity
        {
            get { return _termGroupsSecurity; }
            set { _termGroupsSecurity = value; }

        } //TermGroupsSecurity

        public bool SiteGroups
        {
            get { return _siteGroups; }
            set { _siteGroups = value; }

        } //SiteGroups

        public bool SearchConfiguration
        {
            get { return _searchConfiguration; }
            set { _searchConfiguration = value; }

        } //SearchConfiguration

        public bool ExcludeBaseTemplate
        {
            get { return _excludeBaseTemplate; }
            set { _excludeBaseTemplate = value; }

        } //ExcludeBaseTemplate

        public bool LookupListItems
        {
            get { return _lookupListItems; }
            set { _lookupListItems = value; }

        } //LookupListItems

        public bool GenericList
        {
            get { return _genericList; }
            set { _genericList = value; }

        } //GenericList

        public bool DocumentLibrary
        {
            get { return _documentLibrary; }
            set { _documentLibrary = value; }

        } //DocumentLibrary

        public bool SurveyList
        {
            get { return _surveyList; }
            set { _surveyList = value; }

        } //SurveyList

        public bool LinksList
        {
            get { return _linksList; }
            set { _linksList = value; }

        } //LinksList

        public bool AnnouncementsList
        {
            get { return _announcementsList; }
            set { _announcementsList = value; }

        } //AnnouncementsList

        public bool ContactsList
        {
            get { return _contactsList; }
            set { _contactsList = value; }

        } //ContactsList

        public bool EventsList
        {
            get { return _eventsList; }
            set { _eventsList = value; }

        } //EventsList

        public bool TasksList
        {
            get { return _tasksList; }
            set { _tasksList = value; }

        } //TasksList

        public bool DiscussionBoard
        {
            get { return _discussionBoard; }
            set { _discussionBoard = value; }

        } //DiscussionBoard

        public bool PictureLibrary
        {
            get { return _pictureLibrary; }
            set { _pictureLibrary = value; }

        } //PictureLibrary

        public bool WikiPageLibrary
        {
            get { return _wikiPageLibrary; }
            set { _wikiPageLibrary = value; }

        } //WikiPageLibrary

        public bool GanttTasksList
        {
            get { return _ganttTasksList; }
            set { _ganttTasksList = value; }

        } //GanttTasksList

        public bool MeetingSeriesList
        {
            get { return _meetingSeriesList; }
            set { _meetingSeriesList = value; }

        } //MeetingSeriesList

        public bool BlogPostsList
        {
            get { return _blogPostsList; }
            set { _blogPostsList = value; }

        } //BlogPostsList

        public bool BlogCommentsList
        {
            get { return _blogCommentsList; }
            set { _blogCommentsList = value; }

        } //BlogCommentsList

        public bool BlogCategoriesList
        {
            get { return _blogCategoriesList; }
            set { _blogCategoriesList = value; }

        } // BlogCategoriesList

        public bool IssueTrackingList
        {
            get { return _issueTrackingList; }
            set { _issueTrackingList = value; }

        } //IssueTrackingList

        public bool AuthenticationRequired
        {
            get { return _authenticationRequired; }
            set { _authenticationRequired = value; }

        } //AuthenticationRequired

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

            } //set

        } //UserNameOrEmail

        public SecureString UserPassword
        {
            get { return _userPassword; }
            set { _userPassword = value; }

        } //UserPassword

        public string UserDomain
        {
            get { return _userDomain; }
            private set { _userDomain = value; }

        } //UserDomain

        public string TemplateName
        {
            get { return _templateName; }
            set { _templateName = value; }

        } //TemplateName

        public string TemplatePath
        {
            get { return _templatePath; }
            set { _templatePath = value; }

        } //TemplatePath

        public string WebAddress
        {
            get { return _webAddress; }
            set { _webAddress = value; }

        } //WebAddress

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

        } //AtLeastOneOfTemplateOptions

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
                if (_genericList) { return true; }
                if (_documentLibrary) { return true; }
                if (_surveyList) { return true; }
                if (_linksList) { return true; }
                if (_announcementsList) { return true; }
                if (_contactsList) { return true; }
                if (_eventsList) { return true; }
                if (_tasksList) { return true; }
                if (_discussionBoard) { return true; }
                if (_pictureLibrary) { return true; }
                if (_wikiPageLibrary) { return true; }
                if (_ganttTasksList) { return true; }
                if (_meetingSeriesList) { return true; }
                if (_blogPostsList) { return true; }
                if (_blogCommentsList) { return true; }
                if (_blogCategoriesList) { return true; }
                if (_issueTrackingList) { return true; }

                return false;

            }

        } //AtLeastOneOfAllOptions

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

        } //MoreThanOneOfTemplateOptions

        public bool OneOfContentOptions
        {
            get
            {
                if (_genericList) { return true; }
                if (_documentLibrary) { return true; }
                if (_surveyList) { return true; }
                if (_linksList) { return true; }
                if (_announcementsList) { return true; }
                if (_contactsList) { return true; }
                if (_eventsList) { return true; }
                if (_tasksList) { return true; }
                if (_discussionBoard) { return true; }
                if (_pictureLibrary) { return true; }
                if (_wikiPageLibrary) { return true; }
                if (_ganttTasksList) { return true; }
                if (_meetingSeriesList) { return true; }
                if (_blogPostsList) { return true; }
                if (_blogCommentsList) { return true; }
                if (_blogCategoriesList) { return true; }
                if (_issueTrackingList) { return true; }

                return false;

            }

        } //AtLeastOneOfContentOptions

        public bool Files
        {
            get
            {
                if (_documentLibrary) { return true; }
                if (_pictureLibrary) { return true; }
                if (_wikiPageLibrary) { return true; }
                return false;

            }

        } //Files

        public bool Lists
        {
            get
            {
                if (_genericList) { return true; }
                if (_surveyList) { return true; }
                if (_linksList) { return true; }
                if (_announcementsList) { return true; }
                if (_contactsList) { return true; }
                if (_eventsList) { return true; }
                if (_tasksList) { return true; }
                if (_discussionBoard) { return true; }
                if (_ganttTasksList) { return true; }
                if (_meetingSeriesList) { return true; }
                if (_blogPostsList) { return true; }
                if (_blogCommentsList) { return true; }
                if (_blogCategoriesList) { return true; }
                if (_issueTrackingList) { return true; }

                return false;
            }

        } //Lists

        public bool Pages
        {
            get
            {
                if (_pageContents) { return true; }
                if (_publishing) { return true; }
                if (_publishingFiles) { return true; }

                return false;

            }

        } //Pages

    } //ProvisioningOptions

}
