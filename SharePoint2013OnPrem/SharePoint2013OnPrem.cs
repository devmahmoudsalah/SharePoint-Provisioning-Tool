using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.SharePoint.Client;
using OfficeDevPnP.Core.Framework.Provisioning.Connectors;
using OfficeDevPnP.Core.Framework.Provisioning.Model;
using OfficeDevPnP.Core.Framework.Provisioning.ObjectHandlers;
using OfficeDevPnP.Core.Framework.Provisioning.Providers.Xml;
using PnPModel = OfficeDevPnP.Core.Framework.Provisioning.Model;
using System.Net;
using System.Text;
using System.Threading;
using System.Xml;
using System.Xml.Linq;

namespace Karabina.SharePoint.Provisioning
{
    public class SharePoint2013OnPrem
    {
        public SharePoint2013OnPrem()
        {
            //do nothing
        }

        private ListBox _lbOutput = null;

        public ListBox OutputBox
        {
            get { return _lbOutput; }
            set { _lbOutput = value; }
        }

        private void WriteMessage(string message)
        {
            _lbOutput.Items.Add(message);
            _lbOutput.TopIndex = (_lbOutput.Items.Count - 1);
            Application.DoEvents();
        }

        private void WriteMessageRange(string[] message)
        {
            _lbOutput.Items.AddRange(message);
            _lbOutput.TopIndex = (_lbOutput.Items.Count - 1);
            Application.DoEvents();
        }

        private void SaveListItemsToTemplate(ClientContext ctx, ListCollection lists, ListInstance listInstance)
        {
            Dictionary<string, string> values = new Dictionary<string, string>();
            List list = lists.GetByTitle(listInstance.Title);
            CamlQuery camlQuery = new CamlQuery();
            camlQuery.ViewXml = "<View/>";
            ListItemCollection listItems = list.GetItems(camlQuery);
            ctx.Load(listItems);
            Microsoft.SharePoint.Client.FieldCollection fields = list.Fields;
            ctx.Load(fields);
            ctx.ExecuteQuery();

            WriteMessage($"Info: Saving items from {listInstance.Title}");
            Application.DoEvents();

            int itemCount = 0;

            if (listItems.Count > 0)
            {
                ProvisioningFieldCollection fieldColl = new ProvisioningFieldCollection();

                foreach (Microsoft.SharePoint.Client.Field field in fields)
                {
                    if ((!field.ReadOnlyField) &&
                        (!field.Hidden) &&
                        (field.FieldTypeKind != FieldType.Attachments) &&
                        (field.FieldTypeKind != FieldType.Calculated) &&
                        (field.FieldTypeKind != FieldType.Computed) &&
                        (field.FieldTypeKind != FieldType.ContentTypeId))
                    {
                        fieldColl.Add(field.InternalName, (ProvisioningFieldType)field.FieldTypeKind);
                    }
                }

                foreach (ListItem item in listItems)
                {
                    itemCount++;
                    Dictionary<string, string> data = new Dictionary<string, string>();
                    Dictionary<string, object> fieldValues = item.FieldValues;

                    foreach (ProvisioningField field in fieldColl.Fields)
                    {
                        if (fieldValues.ContainsKey(field.Name))
                        {
                            object value = fieldValues[field.Name];
                            if (value != null)
                            {
                                switch (field.FieldType)
                                {
                                    case ProvisioningFieldType.Lookup:
                                        FieldLookup fieldLookup = fields.GetFieldByName<FieldLookup>(field.Name);
                                        if (fieldLookup != null)
                                        {
                                            if (fieldLookup.AllowMultipleValues)
                                            {
                                                FieldLookupValue[] lookupValues = value as FieldLookupValue[];
                                                StringBuilder sb = new StringBuilder();
                                                for (int i = 0; i < lookupValues.Length; i++)
                                                {
                                                    if (i > 0)
                                                    {
                                                        sb.Append("#;");
                                                    }
                                                    sb.Append($"{lookupValues[i].LookupId}#;{lookupValues[i].LookupValue}");
                                                }
                                                data.Add(field.Name, sb.ToString());
                                            }
                                            else
                                            {
                                                FieldLookupValue lookupValue = value as FieldLookupValue;
                                                data.Add(field.Name, $"{lookupValue.LookupId}#;{lookupValue.LookupValue}");
                                            }
                                        }
                                        break;
                                    case ProvisioningFieldType.User:
                                        FieldUser fieldUser = fields.GetFieldByName<FieldUser>(field.Name);
                                        if (fieldUser != null)
                                        {
                                            if (fieldUser.AllowMultipleValues)
                                            {
                                                FieldUserValue[] userValues = value as FieldUserValue[];
                                                StringBuilder sb = new StringBuilder();
                                                for (int i = 0; i < userValues.Length; i++)
                                                {
                                                    if (i > 0)
                                                    {
                                                        sb.Append("#;");
                                                    }
                                                    sb.Append($"{userValues[i].LookupId}#;{userValues[i].LookupValue}");
                                                }
                                                data.Add(field.Name, sb.ToString());
                                            }
                                            else
                                            {
                                                FieldUserValue userValue = value as FieldUserValue;
                                                data.Add(field.Name, $"{userValue.LookupId}#;{userValue.LookupValue}");
                                            }
                                        }
                                        break;
                                    case ProvisioningFieldType.URL:
                                        FieldUrlValue urlValue = value as FieldUrlValue;
                                        data.Add(field.Name, $"{urlValue.Url},{urlValue.Description}");
                                        break;
                                    case ProvisioningFieldType.Guid:
                                        Guid guid = Guid.Parse(value.ToString());
                                        data.Add(field.Name, guid.ToString("N"));
                                        break;
                                    case ProvisioningFieldType.DateTime:
                                        DateTime dateTime = Convert.ToDateTime(value);
                                        data.Add(field.Name, dateTime.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"));
                                        break;
                                    default:
                                        data.Add(field.Name, value.ToString());
                                        break;
                                }
                            }
                        }

                    }
                    DataRow dataRow = new DataRow(data);
                    listInstance.DataRows.Add(dataRow);
                }
            }

            WriteMessage($"Info: {itemCount} items saved");
            Application.DoEvents();
        }

        private void FixReferenceFields(ProvisioningTemplate template, List<string> lookupLists)
        {
            WriteMessage("Info: Start performing fix up of reference fields");
            Dictionary<string, int> indexFields = new Dictionary<string, int>();
            Dictionary<string, List<string>> referenceFields = new Dictionary<string, List<string>>();
            var fields = template.SiteFields;
            int totalFields = fields.Count;
            int index = 0;
            for (index = 0; index < totalFields; index++)
            {
                PnPModel.Field field = fields[index];

                XElement fieldElement = XElement.Parse(field.SchemaXml);
                string fieldName = fieldElement.Attribute("Name").Value;

                indexFields.Add(fieldName, index);

                if (lookupLists != null)
                {
                    XAttribute listAttribute = fieldElement.Attribute("List");
                    if (listAttribute != null)
                    {
                        string lookupListTitle = listAttribute.Value.Replace("{{listid:", "").Replace("}}", "");
                        if (lookupLists.IndexOf(lookupListTitle) < 0)
                        {
                            lookupLists.Add(lookupListTitle);
                        }
                    }
                }

                if (fieldElement.HasElements)
                {
                    IEnumerable<XElement> elements = fieldElement.Elements();
                    foreach (XElement element in elements)
                    {
                        string value = string.Empty;
                        switch (element.Name.LocalName)
                        {
                            case "Default":
                                value = element.Value;
                                break;
                            case "Formula":
                                value = element.Value;
                                break;
                            case "DefaultFormula":
                                value = element.Value;
                                break;
                            default:
                                break;
                        }
                        if (!string.IsNullOrWhiteSpace(value))
                        {
                            if (value.IndexOf("{fieldtitle:") >= 0)
                            {
                                string[] values = value.Split(new char[] { '{', '}' }, StringSplitOptions.RemoveEmptyEntries);
                                foreach (string val in values)
                                {
                                    if (val.StartsWith("fieldtitle:", StringComparison.OrdinalIgnoreCase))
                                    {
                                        string fieldTitle = val.Substring(11);

                                        if (!referenceFields.ContainsKey(fieldTitle))
                                        {
                                            List<string> keyValues = new List<string>();
                                            keyValues.Add(fieldName);
                                            referenceFields.Add(fieldTitle, keyValues);
                                        }
                                        else
                                        {
                                            List<string> keyValues = referenceFields[fieldTitle];

                                            if (keyValues == null)
                                            {
                                                keyValues = new List<string>();
                                            }
                                            if (!keyValues.Contains(fieldName))
                                            {
                                                keyValues.Add(fieldName);
                                            }
                                        }

                                    }

                                }

                            }
                        }

                    }
                }
            }

            if (referenceFields.Count > 0)
            {
                WriteMessage($"Info: Found {referenceFields.Count} fields to fix up");
                foreach (var referencedField in referenceFields)
                {
                    index = indexFields[referencedField.Key];
                    if (index >= 0)
                    {
                        int lowestIndex = int.MaxValue;
                        foreach (string keyValue in referencedField.Value)
                        {
                            int keyIndex = indexFields[keyValue];
                            if (keyIndex < lowestIndex)
                            {
                                lowestIndex = keyIndex;
                            }
                        }

                        if (index > lowestIndex)
                        {
                            //Swap lowest reference field with referenced field
                            PnPModel.Field tempField = fields[lowestIndex];

                            XElement tempElement = XElement.Parse(tempField.SchemaXml);
                            string tempTitle = tempElement.Attribute("Name").Value;

                            fields[lowestIndex] = fields[index];
                            fields[index] = tempField;

                            indexFields[referencedField.Key] = lowestIndex;
                            indexFields[tempTitle] = index;
                        }
                    }

                }

            }
            WriteMessage("Info: Done performing fix up of reference fields");
        }

        private void CleanupTemplate(ProvisioningOptions provisioningOptions, ProvisioningTemplate template, ProvisioningTemplate baseTemplate)
        {
            WriteMessage("Info: Start performing template clean up");
            if (provisioningOptions.IncludeCustomActions)
            {
                WriteMessage("Cleanup: Cleaning site collection custom actions from template");
                foreach (var customAction in baseTemplate.CustomActions.SiteCustomActions)
                {
                    template.CustomActions.SiteCustomActions.RemoveAll(p => p.Title.Equals(customAction.Title, StringComparison.OrdinalIgnoreCase));
                }
                WriteMessage("Cleanup: Cleaning site custom actions from template");
                foreach (var customAction in baseTemplate.CustomActions.WebCustomActions)
                {
                    template.CustomActions.WebCustomActions.RemoveAll(p => p.Title.Equals(customAction.Title, StringComparison.OrdinalIgnoreCase));
                }
            }
            if (provisioningOptions.IncludeFeatures)
            {
                WriteMessage("Cleanup: Cleaning site collection features from template");
                foreach (var feature in baseTemplate.Features.SiteFeatures)
                {
                    template.Features.SiteFeatures.RemoveAll(p => (p.Id.CompareTo(feature.Id) == 0));
                }
                WriteMessage("Cleanup: Cleaning site features from template");
                foreach (var feature in baseTemplate.Features.WebFeatures)
                {
                    template.Features.WebFeatures.RemoveAll(p => (p.Id.CompareTo(feature.Id) == 0));
                }
            }
            if (provisioningOptions.IncludeFields)
            {
                WriteMessage("Cleanup: Cleaning site collection fields from template");
                List<string> baseFieldKeys = new List<string>();
                Dictionary<string, int> fieldsIndex = new Dictionary<string, int>();
                var baseFields = baseTemplate.SiteFields;
                var fields = template.SiteFields;
                int baseCount = baseFields.Count;
                int count = fields.Count;
                int totalFields = ((baseCount > count) ? baseCount : count);
                for (int i = 0; i < totalFields; i++)
                {
                    if (i < baseCount)
                    {
                        PnPModel.Field baseField = baseFields[i];
                        XElement baseFieldElement = XElement.Parse(baseField.SchemaXml);
                        baseFieldKeys.Add(baseFieldElement.Attribute("Name").Value);
                    }
                    if (i < count)
                    {
                        PnPModel.Field field = fields[i];
                        XElement fieldElement = XElement.Parse(field.SchemaXml);
                        fieldsIndex.Add(fieldElement.Attribute("Name").Value, i);
                    }
                }
                int fieldsToDelete = 0;
                foreach (var baseFieldKey in baseFieldKeys)
                {
                    if (fieldsIndex.ContainsKey(baseFieldKey))
                    {
                        int idx = fieldsIndex[baseFieldKey];
                        fields[idx].SchemaXml = null;
                        fieldsToDelete++;
                    }
                }
                if (fieldsToDelete > 0)
                {
                    fields.RemoveAll(p => p.SchemaXml == null);
                }
            }
            if (provisioningOptions.IncludeFiles)
            {
                WriteMessage("Cleanup: Cleaning files from template");
                foreach (var file in baseTemplate.Files)
                {
                    template.Files.RemoveAll(p => p.Src.Equals(file.Src, StringComparison.OrdinalIgnoreCase));
                }
            }
            if (provisioningOptions.IncludeListInstances)
            {
                WriteMessage("Cleanup: Cleaning lists from template");
                foreach (var listInstance in baseTemplate.Lists)
                {
                    template.Lists.RemoveAll(p => p.Title.Equals(listInstance.Title, StringComparison.OrdinalIgnoreCase));
                }
            }
            if (provisioningOptions.IncludePages)
            {
                WriteMessage("Cleanup: Cleaning pages from template");
                foreach (var page in baseTemplate.Pages)
                {
                    template.Pages.RemoveAll(p => p.Url.Equals(page.Url, StringComparison.OrdinalIgnoreCase));
                }
            }
            if (provisioningOptions.IncludePublishing)
            {
                WriteMessage("Cleanup: Cleaning avaiable web templates from template");
                foreach (var availableWebTemplate in baseTemplate.Publishing.AvailableWebTemplates)
                {
                    template.Publishing.AvailableWebTemplates.RemoveAll(p => p.TemplateName.Equals(availableWebTemplate.TemplateName, StringComparison.OrdinalIgnoreCase));
                }
                WriteMessage("Cleanup: Cleaning page layouts from template");
                foreach (var pageLayout in baseTemplate.Publishing.PageLayouts)
                {
                    template.Publishing.PageLayouts.RemoveAll(p => p.Path.Equals(pageLayout.Path, StringComparison.OrdinalIgnoreCase));
                }
            }
            if (provisioningOptions.IncludeSupportedUILanguages)
            {
                WriteMessage("Cleanup: Cleaning supported UI languages from template");
                foreach (var supportedUILanguage in baseTemplate.SupportedUILanguages)
                {
                    template.SupportedUILanguages.RemoveAll(p => (p.LCID == supportedUILanguage.LCID));
                }
            }
            if (provisioningOptions.IncludeTermGroups)
            {
                WriteMessage("Cleanup: Cleaning term groups from template");
                foreach (var termGroup in baseTemplate.TermGroups)
                {
                    template.TermGroups.RemoveAll(p => (p.Id.CompareTo(termGroup.Id) == 0));
                }
            }
            if (provisioningOptions.IncludeWorkflows)
            {
                WriteMessage("Cleanup: Cleaning workflow subscriptions from template");
                foreach (var workflowSubscription in baseTemplate.Workflows.WorkflowSubscriptions)
                {
                    template.Workflows.WorkflowSubscriptions.RemoveAll(p => (p.DefinitionId.CompareTo(workflowSubscription.DefinitionId) == 0));
                }
                WriteMessage("Cleanup: Cleaning workflow definitions from template");
                foreach (var workflowDefinition in baseTemplate.Workflows.WorkflowDefinitions)
                {
                    template.Workflows.WorkflowDefinitions.RemoveAll(p => (p.Id.CompareTo(workflowDefinition.Id) == 0));
                }
            }
            if (provisioningOptions.IncludeContentTypes)
            {
                WriteMessage("Cleanup: Cleaning content types from template");
                foreach (var contentType in baseTemplate.ContentTypes)
                {
                    template.ContentTypes.RemoveAll(p => p.Id.Equals(contentType.Id, StringComparison.OrdinalIgnoreCase));
                }
            }
            if (provisioningOptions.IncludePropertyBagEntries)
            {
                WriteMessage("Cleanup: Cleaning property bag entries from template");
                foreach (var propertyBagEntry in baseTemplate.PropertyBagEntries)
                {
                    template.PropertyBagEntries.RemoveAll(p => p.Key.Equals(propertyBagEntry.Key, StringComparison.OrdinalIgnoreCase));
                }
            }
            WriteMessage("Info: Done performing template clean up");
        }

        public bool CreateProvisioningTemplate(ListBox lbOutput, ProvisioningOptions provisioningOptions)
        {
            bool result = false;
            try
            {
                _lbOutput = lbOutput;

                using (var ctx = new ClientContext(provisioningOptions.WebAddress))
                {
                    if (provisioningOptions.AuthenticationRequired)
                    {
                        if (!string.IsNullOrWhiteSpace(provisioningOptions.UserDomain))
                        {
                            ctx.Credentials = new NetworkCredential(provisioningOptions.UserNameOrEmail,
                                                                    provisioningOptions.UserPassword,
                                                                    provisioningOptions.UserDomain);

                        }
                        else
                        {
                            ctx.Credentials = new NetworkCredential(provisioningOptions.UserNameOrEmail,
                                                                    provisioningOptions.UserPassword);
                        }
                    }
                    ctx.RequestTimeout = Timeout.Infinite;

                    WriteMessage($"Connecting to {provisioningOptions.WebAddress}");

                    // Just to output the site details 
                    Web web = ctx.Web;
                    ctx.Load(web, w => w.Title, w => w.Url, w => w.WebTemplate, w => w.Configuration, w => w.AllProperties);
                    ctx.ExecuteQueryRetry();

                    WriteMessage($"Creating provisioning template from {web.Title} ( {web.Url} )");
                    WriteMessage($"Base template is {web.WebTemplate}#{web.Configuration}");

                    ProvisioningTemplate baseTemplate = null;
                    if (provisioningOptions.ExcludeBaseTemplate)
                    {
                        WriteMessage("Info: Loading base template for excluding option");

                        baseTemplate = web.GetBaseTemplate(web.WebTemplate, web.Configuration);

                        WriteMessage("Info: Base template loaded for excluding option");
                    }

                    string fileNamePNP = provisioningOptions.TemplateName + ".pnp";
                    string fileNameXML = provisioningOptions.TemplateName + ".xml";

                    ProvisioningTemplateCreationInformation ptci = new ProvisioningTemplateCreationInformation(web);

                    if (baseTemplate != null)
                    {
                        ptci.BaseTemplate = baseTemplate;
                    }

                    ptci.IncludeAllTermGroups = provisioningOptions.IncludeAllTermGroups;
                    ptci.IncludeNativePublishingFiles = provisioningOptions.IncludeNativePublishingFiles;
                    ptci.IncludeSearchConfiguration = provisioningOptions.IncludeSearchConfiguration;
                    ptci.IncludeSiteCollectionTermGroup = provisioningOptions.IncludeSiteCollectionTermGroup;
                    ptci.IncludeSiteGroups = provisioningOptions.IncludeSiteGroups;
                    ptci.IncludeTermGroupsSecurity = provisioningOptions.IncludeTermGroupsSecurity;
                    ptci.PersistBrandingFiles = provisioningOptions.PersistBrandingFiles;
                    ptci.PersistMultiLanguageResources = provisioningOptions.PersistMultiLanguageResources;
                    ptci.PersistPublishingFiles = provisioningOptions.PersistPublishingFiles;

                    ptci.HandlersToProcess = (provisioningOptions.IncludeAuditSettings ? Handlers.AuditSettings : 0) |
                                             (provisioningOptions.IncludeComposedLook ? Handlers.ComposedLook : 0) |
                                             (provisioningOptions.IncludeCustomActions ? Handlers.CustomActions : 0) |
                                             (provisioningOptions.ExtensibilityProviders ? Handlers.ExtensibilityProviders : 0) |
                                             (provisioningOptions.IncludeFeatures ? Handlers.Features : 0) |
                                             (provisioningOptions.IncludeFields ? Handlers.Fields : 0) |
                                             (provisioningOptions.IncludeFiles ? Handlers.Files : 0) |
                                             (provisioningOptions.IncludeListInstances ? Handlers.Lists : 0) |
                                             (provisioningOptions.IncludePages ? Handlers.Pages : 0) |
                                             (provisioningOptions.IncludePublishing ? Handlers.Publishing : 0) |
                                             (provisioningOptions.IncludeRegionalSettings ? Handlers.RegionalSettings : 0) |
                                             (provisioningOptions.IncludeSearchSettings ? Handlers.SearchSettings : 0) |
                                             (provisioningOptions.IncludeSitePolicy ? Handlers.SitePolicy : 0) |
                                             (provisioningOptions.IncludeSupportedUILanguages ? Handlers.SupportedUILanguages : 0) |
                                             (provisioningOptions.IncludeTermGroups ? Handlers.TermGroups : 0) |
                                             (provisioningOptions.IncludeWorkflows ? Handlers.Workflows : 0) |
                                             (provisioningOptions.IncludeSiteSecurity ? Handlers.SiteSecurity : 0) |
                                             (provisioningOptions.IncludeContentTypes ? Handlers.ContentTypes : 0) |
                                             (provisioningOptions.IncludePropertyBagEntries ? Handlers.PropertyBagEntries : 0) |
                                             (provisioningOptions.IncludePageContents ? Handlers.PageContents : 0) |
                                             (provisioningOptions.IncludeWebSettings ? Handlers.WebSettings : 0) |
                                             (provisioningOptions.IncludeNavigation ? Handlers.Navigation : 0);

                    ptci.MessagesDelegate = delegate (string message, ProvisioningMessageType messageType)
                    {
                        switch (messageType)
                        {
                            case ProvisioningMessageType.Error:
                                WriteMessage("Error: " + message);
                                break;
                            case ProvisioningMessageType.Progress:
                                WriteMessage("Progress: " + message);
                                break;
                            case ProvisioningMessageType.Warning:
                                WriteMessage("Warning: " + message);
                                break;
                            case ProvisioningMessageType.EasterEgg:
                                WriteMessage("EasterEgg: " + message);
                                break;
                            default:
                                WriteMessage("Unknown: " + message);
                                break;
                        }
                        if (!lbOutput.HorizontalScrollbar)
                        {
                            lbOutput.HorizontalScrollbar = true;
                        }
                    };

                    ptci.ProgressDelegate = delegate (string message, int progress, int total)
                    {
                        // Output progress
                        WriteMessage(string.Format("{0:00}/{1:00} - {2}", progress, total, message));
                    };

                    // Create FileSystemConnector, to be used by OpenXMLConnector
                    var fileSystemConnector = new FileSystemConnector(provisioningOptions.TemplatePath, "");

                    ptci.FileConnector = new OpenXMLConnector(fileNamePNP, fileSystemConnector, "SharePoint Team");

                    // Execute actual extraction of the tepmplate 
                    ProvisioningTemplate template = web.GetProvisioningTemplate(ptci);

                    //if exclude base template from template
                    if (provisioningOptions.ExcludeBaseTemplate)
                    {
                        //perform the clean up
                        CleanupTemplate(provisioningOptions, template, baseTemplate);

                        //if not publishing site and publishing feature is activated, then clean publishing features from template
                        if (!baseTemplate.BaseSiteTemplate.Equals("ENTERWIKI#0", StringComparison.OrdinalIgnoreCase))
                        {
                            if (web.AllProperties.FieldValues.ContainsKey("__PublishingFeatureActivated"))
                            {
                                var propBagEntry = web.AllProperties["__PublishingFeatureActivated"];
                                if (propBagEntry != null)
                                {
                                    if (propBagEntry.ToString().Equals("True", StringComparison.OrdinalIgnoreCase))
                                    {
                                        WriteMessage("Info: Publishing feature actived on site");
                                        WriteMessage("Info: Loading ENTERWIKI#0 base template");

                                        short config = 0;
                                        baseTemplate = web.GetBaseTemplate("ENTERWIKI", config);

                                        WriteMessage("Info: Done loading ENTERWIKI#0 base template");

                                        //perform the clean up
                                        CleanupTemplate(provisioningOptions, template, baseTemplate);
                                    }
                                }
                            }
                        }
                    }

                    //List to hold all the lookup list names
                    List<string> lookupListTitles = new List<string>();

                    if (provisioningOptions.IncludeFields)
                    {
                        //fix fields with default, formula or defaultformula elements so that the referenced fields have 
                        //a lower index than the fields that reference them in the SiteFields collection
                        //This prevents the "Invalid field found" error from occuring
                        FixReferenceFields(template, lookupListTitles);
                    }

                    //Check if we should do any content operations
                    if (provisioningOptions.IncludeDocumentLibraryFiles ||
                        provisioningOptions.IncludeLookupListItems ||
                        provisioningOptions.IncludeGenericListItems ||
                        provisioningOptions.IncludeJavaScriptFiles ||
                        provisioningOptions.IncludePublishingPages ||
                        provisioningOptions.IncludeXSLStyleSheetFiles ||
                        provisioningOptions.IncludeImageFiles)
                    {
                        ctx.Load(web.Lists);
                        ctx.ExecuteQuery();

                        foreach (ListInstance listInstance in template.Lists)
                        {
                            string listTitle = listInstance.Title.ToLowerInvariant();
                            switch (listTitle)
                            {
                                case "documents":
                                    break;
                                case "images":
                                case "site collection images":
                                    break;
                                case "pages":
                                case "site pages":
                                    break;
                                case "site assets":
                                    break;
                                case "style library":
                                    if (provisioningOptions.IncludeJavaScriptFiles)
                                    {

                                    }
                                    if (provisioningOptions.IncludeXSLStyleSheetFiles)
                                    {

                                    }
                                    break;
                                default:
                                    if (listInstance.TemplateType == 100)
                                    {
                                        if (provisioningOptions.IncludeGenericListItems)
                                        {
                                            SaveListItemsToTemplate(ctx, web.Lists, listInstance);
                                        }
                                        else if (provisioningOptions.IncludeLookupListItems)
                                        {
                                            if (lookupListTitles.IndexOf(listInstance.Title) >= 0)
                                            {
                                                SaveListItemsToTemplate(ctx, web.Lists, listInstance);
                                            }
                                        }
                                    }
                                    if ((listInstance.TemplateType == 101) && (provisioningOptions.IncludeDocumentLibraryFiles))
                                    {

                                    }
                                    break;
                            }
                        }
                    }

                    XMLTemplateProvider provider = new XMLOpenXMLTemplateProvider(ptci.FileConnector as OpenXMLConnector);
                    provider.SaveAs(template, fileNameXML);


                    WriteMessage($"Base site template is {template.BaseSiteTemplate}");

                    WriteMessage($"Done creating provisioning template from {web.Title}");

                    result = true;
                }
            }
            catch (Exception ex)
            {
                if (!lbOutput.HorizontalScrollbar)
                {
                    lbOutput.HorizontalScrollbar = true;
                }
                WriteMessage("Error: " + ex.Message);
                if (ex.InnerException != null)
                {
                    WriteMessage("Error: Start of inner exception");
                    WriteMessage(ex.InnerException.Message);
                    WriteMessageRange(ex.InnerException.StackTrace.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries));
                    WriteMessage("Error: End of inner exception");
                }
                WriteMessageRange(ex.StackTrace.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries));
                result = false;
            }
            return result;

        }

        public bool ApplyProvisioningTemplate(ListBox lbOutput, ProvisioningOptions provisioningOptions)
        {
            bool result = false;
            try
            {
                _lbOutput = lbOutput;
                using (var ctx = new ClientContext(provisioningOptions.WebAddress))
                {
                    if (provisioningOptions.AuthenticationRequired)
                    {
                        if (!string.IsNullOrWhiteSpace(provisioningOptions.UserDomain))
                        {
                            ctx.Credentials = new NetworkCredential(provisioningOptions.UserNameOrEmail,
                                                                    provisioningOptions.UserPassword,
                                                                    provisioningOptions.UserDomain);

                        }
                        else
                        {
                            ctx.Credentials = new NetworkCredential(provisioningOptions.UserNameOrEmail,
                                                                    provisioningOptions.UserPassword);
                        }
                    }
                    ctx.RequestTimeout = Timeout.Infinite;

                    string webTitle = string.Empty;

                    WriteMessage($"Connecting to {provisioningOptions.WebAddress}");

                    // Just to output the site details 
                    Web web = ctx.Web;
                    ctx.Load(web, w => w.Title, w => w.Url);
                    ctx.ExecuteQueryRetry();

                    webTitle = web.Title;

                    WriteMessage($"Applying provisioning template to {webTitle} ( {web.Url} )");

                    string fileNamePNP = provisioningOptions.TemplateName + ".pnp";

                    FileConnectorBase fileConnector = new FileSystemConnector(provisioningOptions.TemplatePath, "");

                    XMLTemplateProvider provider = new XMLOpenXMLTemplateProvider(new OpenXMLConnector(fileNamePNP, fileConnector));

                    List<ProvisioningTemplate> templates = provider.GetTemplates();

                    ProvisioningTemplate template = templates[0];

                    WriteMessage($"Base site template in provisioning template is {template.BaseSiteTemplate}");

                    if (template.WebSettings != null)
                    {
                        template.WebSettings.Title = webTitle;
                    }

                    template.Connector = provider.Connector;

                    ProvisioningTemplateApplyingInformation ptai = new ProvisioningTemplateApplyingInformation();

                    ptai.MessagesDelegate = delegate (string message, ProvisioningMessageType messageType)
                    {
                        switch (messageType)
                        {
                            case ProvisioningMessageType.Error:
                                WriteMessage("Error: " + message);
                                break;
                            case ProvisioningMessageType.Progress:
                                WriteMessage("Progress: " + message);
                                break;
                            case ProvisioningMessageType.Warning:
                                WriteMessage("Warning: " + message);
                                break;
                            case ProvisioningMessageType.EasterEgg:
                                WriteMessage("EasterEgg: " + message);
                                break;
                            default:
                                WriteMessage("Unknown: " + message);
                                break;
                        }
                        if (!lbOutput.HorizontalScrollbar)
                        {
                            lbOutput.HorizontalScrollbar = true;
                        }
                    };

                    ptai.ProgressDelegate = delegate (string message, int progress, int total)
                    {
                        WriteMessage(string.Format("{0:00}/{1:00} - {2}", progress, total, message));
                    };

                    web.ApplyProvisioningTemplate(template, ptai);

                    WriteMessage($"Done applying provisioning template to {web.Title} ( {web.Url} )");

                    result = true;
                }
            }
            catch (Exception ex)
            {
                if (!lbOutput.HorizontalScrollbar)
                {
                    lbOutput.HorizontalScrollbar = true;
                }
                WriteMessage("Error: " + ex.Message);
                if (ex.InnerException != null)
                {
                    WriteMessage("Error: Start of inner exception");
                    WriteMessage(ex.InnerException.Message);
                    WriteMessageRange(ex.InnerException.StackTrace.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries));
                    WriteMessage("Error: End of inner exception");
                }
                WriteMessageRange(ex.StackTrace.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries));
                result = false;
            }
            return result;
        }

    }

}
