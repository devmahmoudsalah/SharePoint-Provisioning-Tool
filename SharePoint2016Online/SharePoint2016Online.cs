using System;
using System.Collections.Generic;
using System.Drawing;
using Microsoft.SharePoint.Client;
using OfficeDevPnP.Core.Framework.Provisioning.Connectors;
using OfficeDevPnP.Core.Framework.Provisioning.Model;
using OfficeDevPnP.Core.Framework.Provisioning.ObjectHandlers;
using OfficeDevPnP.Core.Framework.Provisioning.Providers.Xml;
using PnPModel = OfficeDevPnP.Core.Framework.Provisioning.Model;
using SPClient = Microsoft.SharePoint.Client;
using System.Net;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Xml;
using System.Xml.Linq;
using System.IO;
using Newtonsoft.Json;

namespace Karabina.SharePoint.Provisioning
{
    public class SharePoint2016Online : MarshalByRefObject
    {

        public SharePoint2016Online()
        {
            //do nothing
        }

        private ProvisioningTemplate _editingTemplate = null;
        private XMLTemplateProvider _editingProvider = null;
        private Action<string> _writeMessage = null;
        private Action<string[]> _writeMessageRange = null;


        public ProvisioningTemplate EditingTemplate
        {
            get { return _editingTemplate; }
            set { _editingTemplate = value; }

        } //EditingTemplate

        public XMLTemplateProvider EditingProvider
        {
            get { return _editingProvider; }
            set { _editingProvider = value; }

        } //EditingProvider

        private void WriteMessage(string message)
        {
            //_lbOutput.Items.Add(message);
            //_lbOutput.TopIndex = (_lbOutput.Items.Count - 1);
            //Application.DoEvents();

        } //WriteMessage

        private void WriteMessageRange(string[] message)
        {
            //_lbOutput.Items.AddRange(message);
            //_lbOutput.TopIndex = (_lbOutput.Items.Count - 1);
            //Application.DoEvents();

        } //WriteMessageRange

        private Dictionary<string, string> GetItemFieldValues(ListItem item, ProvisioningFieldCollection fieldCollection,
                                                              SPClient.FieldCollection fields)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            Dictionary<string, object> fieldValues = item.FieldValues;

            foreach (ProvisioningField field in fieldCollection.Fields)
            {
                if (fieldValues.ContainsKey(field.Name))
                {
                    object value = fieldValues[field.Name];
                    if (value != null)
                    {
                        //Check what type of field we have.
                        switch (field.FieldType)
                        {
                            case ProvisioningFieldType.Lookup:
                                FieldLookup fieldLookup = fields.GetFieldByName<FieldLookup>(field.Name);
                                if (fieldLookup != null)
                                {
                                    //Check it allows multiple values
                                    if (fieldLookup.AllowMultipleValues)
                                    {
                                        //Yes, get the array of ids and values
                                        FieldLookupValue[] lookupValues = value as FieldLookupValue[];
                                        StringBuilder sb = new StringBuilder();
                                        for (int i = 0; i < lookupValues.Length; i++)
                                        {
                                            if (i > 0)
                                            {
                                                sb.Append(";#");

                                            }

                                            sb.Append($"{lookupValues[i].LookupId};#{lookupValues[i].LookupValue}");

                                        }

                                        data.Add(field.Name, sb.ToString());

                                    }
                                    else
                                    {
                                        //No, get the field id and value
                                        FieldLookupValue lookupValue = value as FieldLookupValue;
                                        data.Add(field.Name, $"{lookupValue.LookupId};#{lookupValue.LookupValue}");

                                    }

                                }

                                break;

                            case ProvisioningFieldType.User:
                                FieldUser fieldUser = fields.GetFieldByName<FieldUser>(field.Name);
                                if (fieldUser != null)
                                {
                                    //Check if it allows multiple users
                                    if (fieldUser.AllowMultipleValues)
                                    {
                                        //Yes, get the array of users
                                        FieldUserValue[] userValues = value as FieldUserValue[];
                                        StringBuilder sb = new StringBuilder();
                                        for (int i = 0; i < userValues.Length; i++)
                                        {
                                            if (i > 0)
                                            {
                                                sb.Append(";#");

                                            }

                                            sb.Append($"{userValues[i].LookupId};#{userValues[i].LookupValue}");

                                        }

                                        data.Add(field.Name, sb.ToString());

                                    }
                                    else
                                    {
                                        //No, get the user id and value
                                        FieldUserValue userValue = value as FieldUserValue;
                                        data.Add(field.Name, $"{userValue.LookupId};#{userValue.LookupValue}");

                                    }

                                }

                                break;

                            case ProvisioningFieldType.URL:
                                //Field is URL, save in url,description format.
                                FieldUrlValue urlValue = value as FieldUrlValue;
                                data.Add(field.Name, $"{urlValue.Url},{urlValue.Description}");

                                break;

                            case ProvisioningFieldType.Guid:
                                //Field is GUID, save full guid format.
                                Guid guid = Guid.Parse(value.ToString());
                                data.Add(field.Name, guid.ToString("B"));

                                break;

                            case ProvisioningFieldType.DateTime:
                                //Field is date time, save in ISO format
                                DateTime dateTime = Convert.ToDateTime(value);
                                data.Add(field.Name, dateTime.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")); //ISO format

                                break;

                            default:
                                //Field is text, number or one of the other types not checked above.
                                data.Add(field.Name, value.ToString());

                                break;

                        }

                    }

                }

            }

            return data;

        } //GetItemFieldValues


        private void GetItemFieldValues(ListItem item, ProvisioningFieldCollection fieldCollection, SPClient.FieldCollection fields,
                                        Dictionary<string, string> properties)
        {
            Dictionary<string, string> data = GetItemFieldValues(item, fieldCollection, fields);
            if (data.Count > 0)
            {
                foreach (KeyValuePair<string, string> keyValue in data)
                {
                    properties.Add(keyValue.Key, keyValue.Value);

                }

            }

        } //GetItemFieldValues


        private void SaveListItemsToTemplate(ClientContext ctx, ListCollection lists, ListInstance listInstance)
        {
            Dictionary<string, string> values = new Dictionary<string, string>();
            List list = lists.GetByTitle(listInstance.Title);
            CamlQuery camlQuery = new CamlQuery();
            camlQuery.ViewXml = "<View/>";
            ListItemCollection listItems = list.GetItems(camlQuery);
            ctx.Load(listItems);
            SPClient.FieldCollection fields = list.Fields;
            ctx.Load(fields);
            ctx.ExecuteQuery();

            _writeMessage($"Info: Saving items from {listInstance.Title}");

            int itemCount = 0;

            if (listItems.Count > 0)
            {
                ProvisioningFieldCollection fieldCollection = new ProvisioningFieldCollection();

                //Get only the fields we need.
                foreach (SPClient.Field field in fields)
                {
                    if ((!field.ReadOnlyField) &&
                        (!field.Hidden) &&
                        (field.FieldTypeKind != FieldType.Attachments) &&
                        (field.FieldTypeKind != FieldType.Calculated) &&
                        (field.FieldTypeKind != FieldType.Computed) &&
                        (field.FieldTypeKind != FieldType.ContentTypeId))
                    {
                        fieldCollection.Add(field.InternalName, (ProvisioningFieldType)field.FieldTypeKind);

                    }

                }

                //Now get this items with our fields.
                foreach (ListItem item in listItems)
                {
                    itemCount++;
                    Dictionary<string, string> data = GetItemFieldValues(item, fieldCollection, fields);
                    DataRow dataRow = new DataRow(data);
                    listInstance.DataRows.Add(dataRow);

                }

            }

            _writeMessage($"Info: {itemCount} items saved");

        } //SaveListItemsToTemplate

        private void FixReferenceFields(ProvisioningTemplate template, List<string> lookupLists)
        {
            _writeMessage("Info: Start performing fix up of reference fields");
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
                                string[] values = value.Split(new char[] { '[', '{', '}', ']' },
                                                              StringSplitOptions.RemoveEmptyEntries);
                                foreach (string val in values)
                                {
                                    if (val.StartsWith("fieldtitle:", StringComparison.OrdinalIgnoreCase))
                                    {
                                        string fieldTitle = val.Substring(11);

                                        if (!string.IsNullOrWhiteSpace(fieldTitle))
                                        {
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

                                    } //if starts with fieldtitle

                                }

                            } //if fieldtitle

                        } //if value

                    }

                } //if fieldElement.HasElements

            } //for totalFields

            if (referenceFields.Count > 0)
            {
                _writeMessage($"Info: Found {referenceFields.Count} fields to fix up");
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

            _writeMessage("Info: Done performing fix up of reference fields");

        } //FixReferenceFields

        private void CleanupTemplate(ProvisioningOptions provisioningOptions,
                                     ProvisioningTemplate template,
                                     ProvisioningTemplate baseTemplate)
        {
            int total = 0;
            _writeMessage($"Info: Start performing {baseTemplate.BaseSiteTemplate} template clean up");

            if (provisioningOptions.CustomActions)
            {
                if ((baseTemplate.CustomActions != null) &&
                    (template.CustomActions != null))
                {
                    if ((baseTemplate.CustomActions.SiteCustomActions != null) &&
                        (template.CustomActions.SiteCustomActions != null))
                    {
                        total = template.CustomActions.SiteCustomActions.Count;
                        _writeMessage("Cleanup: Cleaning site collection custom actions from template");
                        foreach (var customAction in baseTemplate.CustomActions.SiteCustomActions)
                        {
                            template.CustomActions.SiteCustomActions.RemoveAll(p => p.Title.Equals(customAction.Title,
                                                                                                   StringComparison.OrdinalIgnoreCase));

                        }

                        total -= template.CustomActions.SiteCustomActions.Count;
                        _writeMessage($"Cleanup: {total} site collection custom actions cleaned from template");

                    }

                    if ((baseTemplate.CustomActions.WebCustomActions != null) &&
                       (template.CustomActions.WebCustomActions != null))
                    {
                        total = template.CustomActions.WebCustomActions.Count;
                        _writeMessage("Cleanup: Cleaning site custom actions from template");
                        foreach (var customAction in baseTemplate.CustomActions.WebCustomActions)
                        {
                            template.CustomActions.WebCustomActions.RemoveAll(p => p.Title.Equals(customAction.Title,
                                                                                                  StringComparison.OrdinalIgnoreCase));

                        }

                        total -= template.CustomActions.WebCustomActions.Count;
                        _writeMessage($"Cleanup: {total} site custom actions cleaned from template");

                    }

                }

            }

            if (provisioningOptions.Features)
            {
                if ((baseTemplate.Features != null) &&
                    (template.Features != null))
                {
                    if ((baseTemplate.Features.SiteFeatures != null) &&
                        (template.Features.SiteFeatures != null))
                    {
                        total = template.Features.SiteFeatures.Count;
                        _writeMessage("Cleanup: Cleaning site collection features from template");
                        foreach (var feature in baseTemplate.Features.SiteFeatures)
                        {
                            template.Features.SiteFeatures.RemoveAll(p => (p.Id.CompareTo(feature.Id) == 0));

                        }

                        total -= template.Features.SiteFeatures.Count;
                        _writeMessage($"Cleanup: {total} site collection features cleaned from template");

                    }

                    if ((baseTemplate.Features.WebFeatures != null) &&
                        (template.Features.WebFeatures != null))
                    {
                        total = template.Features.WebFeatures.Count;
                        _writeMessage("Cleanup: Cleaning site features from template");
                        foreach (var feature in baseTemplate.Features.WebFeatures)
                        {
                            template.Features.WebFeatures.RemoveAll(p => (p.Id.CompareTo(feature.Id) == 0));

                        }

                        total -= template.Features.WebFeatures.Count;
                        _writeMessage($"Cleanup: {total} site features cleaned from template");

                    }

                }

            }

            if (provisioningOptions.Fields)
            {
                if ((baseTemplate.SiteFields != null) &&
                    (template.SiteFields != null))
                {
                    _writeMessage("Cleanup: Cleaning site collection fields from template");
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

                    _writeMessage($"Cleanup: {fieldsToDelete} site collection fields cleaned from template");
                }

            }

            if (provisioningOptions.Files)
            {
                if ((baseTemplate.Files != null) &&
                    (template.Files != null))
                {
                    total = template.Files.Count;
                    _writeMessage("Cleanup: Cleaning files from template");
                    foreach (var file in baseTemplate.Files)
                    {
                        template.Files.RemoveAll(p => p.Src.Equals(file.Src, StringComparison.OrdinalIgnoreCase));

                    }

                    total -= template.Files.Count;
                    _writeMessage($"Cleanup: {total} files cleaned from template");

                }

            }

            if (provisioningOptions.ListInstances)
            {
                if ((baseTemplate.Lists != null) &&
                    (template.Lists != null))
                {
                    total = template.Lists.Count;
                    _writeMessage("Cleanup: Cleaning lists from template");
                    foreach (var listInstance in baseTemplate.Lists)
                    {
                        template.Lists.RemoveAll(p => p.Title.Equals(listInstance.Title, StringComparison.OrdinalIgnoreCase));

                    }

                    total -= template.Lists.Count;
                    _writeMessage($"Cleanup: {total} lists cleaned from template");

                }

            }

            if (provisioningOptions.Pages)
            {
                if ((baseTemplate.Pages != null) &&
                    (template.Pages != null))
                {
                    total = template.Pages.Count;
                    _writeMessage("Cleanup: Cleaning pages from template");
                    foreach (var page in baseTemplate.Pages)
                    {
                        template.Pages.RemoveAll(p => p.Url.Equals(page.Url, StringComparison.OrdinalIgnoreCase));

                    }

                    total -= template.Pages.Count;
                    _writeMessage($"Cleanup: {total} pages cleaned from template");

                }

            }

            if (provisioningOptions.Publishing)
            {
                if ((baseTemplate.Publishing != null) &&
                    (template.Publishing != null))
                {
                    if ((baseTemplate.Publishing.AvailableWebTemplates != null) &&
                        (template.Publishing.AvailableWebTemplates != null))
                    {
                        total = template.Publishing.AvailableWebTemplates.Count;
                        _writeMessage("Cleanup: Cleaning avaiable web templates from template");
                        foreach (var availableWebTemplate in baseTemplate.Publishing.AvailableWebTemplates)
                        {
                            template.Publishing.AvailableWebTemplates.RemoveAll(p =>
                                p.TemplateName.Equals(availableWebTemplate.TemplateName, StringComparison.OrdinalIgnoreCase));

                        }

                        total -= template.Publishing.AvailableWebTemplates.Count;
                        _writeMessage($"Cleanup: {total} avaiable web templates cleaned from template");

                    }

                    if ((baseTemplate.Publishing.PageLayouts != null) &&
                        (template.Publishing.PageLayouts != null))
                    {
                        total = template.Publishing.PageLayouts.Count;
                        _writeMessage("Cleanup: Cleaning page layouts from template");
                        foreach (var pageLayout in baseTemplate.Publishing.PageLayouts)
                        {
                            template.Publishing.PageLayouts.RemoveAll(p => p.Path.Equals(pageLayout.Path,
                                                                                         StringComparison.OrdinalIgnoreCase));

                        }

                        total -= template.Publishing.PageLayouts.Count;
                        _writeMessage($"Cleanup: {total} page layouts cleaned from template");

                    }

                }

            }

            if (provisioningOptions.SupportedUILanguages)
            {
                if ((baseTemplate.SupportedUILanguages != null) &&
                    (template.SupportedUILanguages != null))
                {
                    total = template.SupportedUILanguages.Count;
                    _writeMessage("Cleanup: Cleaning supported UI languages from template");
                    foreach (var supportedUILanguage in baseTemplate.SupportedUILanguages)
                    {
                        template.SupportedUILanguages.RemoveAll(p => (p.LCID == supportedUILanguage.LCID));

                    }

                    total -= template.SupportedUILanguages.Count;
                    _writeMessage($"Cleanup: {total} supported UI languages cleaned from template");

                }

            }

            if (provisioningOptions.TermGroups)
            {
                if ((baseTemplate.TermGroups != null) &&
                    (template.TermGroups != null))
                {
                    total = template.TermGroups.Count;
                    _writeMessage("Cleanup: Cleaning term groups from template");
                    foreach (var termGroup in baseTemplate.TermGroups)
                    {
                        template.TermGroups.RemoveAll(p => (p.Id.CompareTo(termGroup.Id) == 0));

                    }

                    total -= template.TermGroups.Count;
                    _writeMessage($"Cleanup: {total} term groups cleaned from template");

                }

            }

            if (provisioningOptions.Workflows)
            {
                if ((baseTemplate.Workflows != null) &&
                    (template.Workflows != null))
                {
                    if ((baseTemplate.Workflows.WorkflowSubscriptions != null) &&
                        (template.Workflows.WorkflowSubscriptions != null))
                    {
                        total = template.Workflows.WorkflowSubscriptions.Count;
                        _writeMessage("Cleanup: Cleaning workflow subscriptions from template");
                        foreach (var workflowSubscription in baseTemplate.Workflows.WorkflowSubscriptions)
                        {
                            template.Workflows.WorkflowSubscriptions.RemoveAll(p =>
                            (p.DefinitionId.CompareTo(workflowSubscription.DefinitionId) == 0));

                        }

                        total -= template.Workflows.WorkflowSubscriptions.Count;
                        _writeMessage($"Cleanup: {total} workflow subscriptions cleaned from template");

                    }


                    if ((baseTemplate.Workflows.WorkflowDefinitions != null) &&
                        (template.Workflows.WorkflowDefinitions != null))
                    {
                        total = template.Workflows.WorkflowDefinitions.Count;
                        _writeMessage("Cleanup: Cleaning workflow definitions from template");
                        foreach (var workflowDefinition in baseTemplate.Workflows.WorkflowDefinitions)
                        {
                            template.Workflows.WorkflowDefinitions.RemoveAll(p =>
                            (p.Id.CompareTo(workflowDefinition.Id) == 0));

                        }

                        total -= template.Workflows.WorkflowDefinitions.Count;
                        _writeMessage($"Cleanup: {total} workflow definitions cleaned from template");

                    }

                }

            }

            if (provisioningOptions.ContentTypes)
            {
                if ((baseTemplate.ContentTypes != null) &&
                    (template.ContentTypes != null))
                {
                    total = template.ContentTypes.Count;
                    _writeMessage("Cleanup: Cleaning content types from template");
                    foreach (var contentType in baseTemplate.ContentTypes)
                    {
                        template.ContentTypes.RemoveAll(p => p.Id.Equals(contentType.Id, StringComparison.OrdinalIgnoreCase));

                    }

                    total -= template.ContentTypes.Count;
                    _writeMessage($"Cleanup: {total} content types cleaned from template");

                }

            }

            if (provisioningOptions.PropertyBagEntries)
            {
                if ((baseTemplate.PropertyBagEntries != null) &&
                    (template.PropertyBagEntries != null))
                {
                    total = template.PropertyBagEntries.Count;
                    _writeMessage("Cleanup: Cleaning property bag entries from template");
                    foreach (var propertyBagEntry in baseTemplate.PropertyBagEntries)
                    {
                        template.PropertyBagEntries.RemoveAll(p => p.Key.Equals(propertyBagEntry.Key,
                                                                                StringComparison.OrdinalIgnoreCase));

                    }

                    total -= template.PropertyBagEntries.Count;
                    _writeMessage($"Cleanup: {total} property bag entries cleaned from template");

                }

            }

            _writeMessage($"Info: Performed {baseTemplate.BaseSiteTemplate} template clean up");

        } //CleanupTemplate

        private string TokenizeWebPartXml(Web web, string xml)
        {
            var lists = web.Lists;
            web.Context.Load(web, w => w.ServerRelativeUrl, w => w.Id);
            web.Context.Load(lists, ls => ls.Include(l => l.Id, l => l.Title));
            web.Context.ExecuteQueryRetry();

            foreach (var list in lists)
            {
                xml = Regex.Replace(xml, list.Id.ToString(),
                                    string.Format("{{listid:{0}}}", list.Title),
                                    RegexOptions.IgnoreCase);

            }
            xml = Regex.Replace(xml, web.Id.ToString(), "{siteid}", RegexOptions.IgnoreCase);
            xml = Regex.Replace(xml, "(\"" + web.ServerRelativeUrl + ")(?!&)", "\"{site}", RegexOptions.IgnoreCase);
            xml = Regex.Replace(xml, "'" + web.ServerRelativeUrl, "'{site}", RegexOptions.IgnoreCase);
            xml = Regex.Replace(xml, ">" + web.ServerRelativeUrl, ">{site}", RegexOptions.IgnoreCase);

            return xml;

        } //TokenizeWebPartXml

        private void SaveFilesToTemplate(ClientContext ctx, Web web, ListInstance listInstance, ProvisioningTemplate template)
        {
            List list = web.Lists.GetByTitle(listInstance.Title);
            CamlQuery camlQuery = new CamlQuery();
            camlQuery.ViewXml = "<View Scope='RecursiveAll'/>";
            ListItemCollection listItems = list.GetItems(camlQuery);
            ctx.Load(listItems);
            SPClient.FieldCollection fields = list.Fields;
            ctx.Load(fields);
            ctx.ExecuteQuery();

            if (listItems.Count > 0)
            {
                ProvisioningFieldCollection fieldCollection = new ProvisioningFieldCollection();

                //Get only the fields we need.
                foreach (SPClient.Field field in fields)
                {
                    if ((!field.ReadOnlyField) &&
                        (!field.Hidden) &&
                        (!field.InternalName.Equals("FileLeafRef", StringComparison.OrdinalIgnoreCase)) &&
                        (field.FieldTypeKind != FieldType.Attachments) &&
                        (field.FieldTypeKind != FieldType.Calculated) &&
                        (field.FieldTypeKind != FieldType.Computed) &&
                        (field.FieldTypeKind != FieldType.ContentTypeId) &&
                        (field.FieldTypeKind != FieldType.User) &&
                        (!field.TypeAsString.Contains("Taxonomy")))
                    {
                        fieldCollection.Add(field.InternalName, (ProvisioningFieldType)field.FieldTypeKind);

                    }

                }

                _writeMessage($"Info: Saving items from {listInstance.Title} to template");

                int itemCount = 0;

                string serverRelativeUrl = web.ServerRelativeUrl;
                string serverRelativeUrlForwardSlash = serverRelativeUrl + "/";

                foreach (ListItem item in listItems)
                {
                    itemCount++;

                    string filePathName = item["FileRef"].ToString();
                    string fileFullName = filePathName.Replace(serverRelativeUrl, "{site}");
                    string fileDirectory = item["FileDirRef"].ToString().Replace(serverRelativeUrl, "{site}");
                    string fileName = item["FileLeafRef"].ToString();
                    string fileStreamName = filePathName.Replace(serverRelativeUrlForwardSlash, "");

                    if (item.FileSystemObjectType == FileSystemObjectType.File)
                    {
                        //Make sure file is not already saved during template creation
                        int fileIndex = template.Files.FindIndex(p =>
                                            ((p.Folder.Equals(fileDirectory, StringComparison.OrdinalIgnoreCase)) &&
                                             (p.Src.Equals(fileStreamName, StringComparison.OrdinalIgnoreCase))));

                        if (fileIndex < 0)
                        {
                            SPClient.File file = item.File;
                            ctx.Load(file);
                            ctx.ExecuteQuery();

                            if (file.Exists)
                            {
                                PnPModel.File pnpFile = new PnPModel.File();
                                pnpFile.Folder = fileDirectory;
                                switch (file.Level)
                                {
                                    case SPClient.FileLevel.Draft:
                                        pnpFile.Level = PnPModel.FileLevel.Draft;

                                        break;

                                    case SPClient.FileLevel.Published:
                                        pnpFile.Level = PnPModel.FileLevel.Published;

                                        break;

                                    case SPClient.FileLevel.Checkout:
                                        pnpFile.Level = PnPModel.FileLevel.Checkout;

                                        break;

                                    default:

                                        break;

                                }

                                pnpFile.Overwrite = true;

                                pnpFile.Src = fileStreamName;

                                if (fieldCollection.Count > 0)
                                {
                                    GetItemFieldValues(item, fieldCollection, fields, pnpFile.Properties);

                                }

                                if (fileName.ToLowerInvariant().EndsWith(".aspx"))
                                {
                                    var webParts = web.GetWebParts(filePathName);
                                    foreach (var webPartDefinition in webParts)
                                    {
                                        string webPartXml = web.GetWebPartXml(webPartDefinition.Id, filePathName);
                                        var webPartxml = TokenizeWebPartXml(web, webPartXml);

                                        WebPart webPart = new WebPart()
                                        {
                                            Title = webPartDefinition.WebPart.Title,
                                            Row = (uint)webPartDefinition.WebPart.ZoneIndex,
                                            Order = (uint)webPartDefinition.WebPart.ZoneIndex,
                                            Contents = webPartxml,
                                            Zone = webPartDefinition.ZoneId

                                        };

                                        pnpFile.WebParts.Add(webPart);

                                    }

                                }

                                template.Files.Add(pnpFile);
                                FileInformation fileInfo = SPClient.File.OpenBinaryDirect(ctx, filePathName);
                                template.Connector.SaveFileStream(fileStreamName, string.Empty, fileInfo.Stream);

                            }

                        }

                    }
                    else if (item.FileSystemObjectType == FileSystemObjectType.Folder)
                    {
                        //Make sure the directory is not already stored during template creation
                        int directoryIndex = template.Directories.FindIndex(p =>
                                                ((p.Folder.Equals(fileDirectory, StringComparison.OrdinalIgnoreCase)) &&
                                                 (p.Src.Equals(fileStreamName, StringComparison.OrdinalIgnoreCase))));

                        if (directoryIndex < 0)
                        {
                            PnPModel.Directory pnpDirectory = new PnPModel.Directory();
                            pnpDirectory.Folder = fileDirectory;
                            pnpDirectory.Level = PnPModel.FileLevel.Published;
                            pnpDirectory.Overwrite = true;

                            pnpDirectory.Src = fileStreamName;

                            template.Directories.Add(pnpDirectory);

                        }

                    }

                }

                _writeMessage($"Info: {itemCount} items saved");

            }

        } //SaveFilesToTemplate

        public bool CreateProvisioningTemplate(ProvisioningOptions provisioningOptions,
                                               Action<string> writeMessage,
                                               Action<string[]> writeMessageRange)
        {
            bool result = false;
            try
            {
                if (writeMessage != null)
                {
                    _writeMessage = writeMessage;

                }
                else
                {
                    _writeMessage = WriteMessage;

                }

                if (writeMessageRange != null)
                {
                    _writeMessageRange = writeMessageRange;

                }
                else
                {
                    _writeMessageRange = WriteMessageRange;

                }

                using (var ctx = new ClientContext(provisioningOptions.WebAddress))
                {
                    SecureString pwdSecure = new SecureString();
                    foreach (char c in provisioningOptions.UserPassword.ToCharArray()) pwdSecure.AppendChar(c);

                    ctx.Credentials = new SharePointOnlineCredentials(provisioningOptions.UserNameOrEmail,
                                                                      pwdSecure);

                    ctx.RequestTimeout = Timeout.Infinite;

                    _writeMessage($"Connecting to {provisioningOptions.WebAddress}");

                    // Load the web with all fields we will need.
                    Web web = ctx.Web;
                    ctx.Load(web, w => w.Title,
                                  w => w.Url,
                                  w => w.WebTemplate,
                                  w => w.Configuration,
                                  w => w.AllProperties,
                                  w => w.ServerRelativeUrl);

                    ctx.ExecuteQueryRetry();

                    _writeMessage($"Creating provisioning template from {web.Title} ( {web.Url} )");
                    _writeMessage($"Base template is {web.WebTemplate}#{web.Configuration}");


                    string fileNamePNP = provisioningOptions.TemplateName + ".pnp";
                    string fileNameXML = provisioningOptions.TemplateName + ".xml";

                    ProvisioningTemplateCreationInformation ptci = new ProvisioningTemplateCreationInformation(web);

                    ptci.IncludeAllTermGroups = provisioningOptions.AllTermGroups;
                    ptci.IncludeNativePublishingFiles = provisioningOptions.NativePublishingFiles;
                    ptci.IncludeSearchConfiguration = provisioningOptions.SearchConfiguration;
                    ptci.IncludeSiteCollectionTermGroup = provisioningOptions.SiteCollectionTermGroup;
                    ptci.IncludeSiteGroups = provisioningOptions.SiteGroups;
                    ptci.IncludeTermGroupsSecurity = provisioningOptions.TermGroupsSecurity;
                    ptci.PersistBrandingFiles = provisioningOptions.BrandingFiles;
                    ptci.PersistMultiLanguageResources = provisioningOptions.MultiLanguageResources;
                    ptci.PersistPublishingFiles = provisioningOptions.PublishingFiles;

                    ptci.HandlersToProcess = (provisioningOptions.AuditSettings ? Handlers.AuditSettings : 0) |
                                             (provisioningOptions.ComposedLook ? Handlers.ComposedLook : 0) |
                                             (provisioningOptions.CustomActions ? Handlers.CustomActions : 0) |
                                             (provisioningOptions.ExtensibilityProviders ? Handlers.ExtensibilityProviders : 0) |
                                             (provisioningOptions.Features ? Handlers.Features : 0) |
                                             (provisioningOptions.Fields ? Handlers.Fields : 0) |
                                             (provisioningOptions.Files ? Handlers.Files : 0) |
                                             (provisioningOptions.ListInstances ? Handlers.Lists : 0) |
                                             (provisioningOptions.Pages ? Handlers.Pages : 0) |
                                             (provisioningOptions.Publishing ? Handlers.Publishing : 0) |
                                             (provisioningOptions.RegionalSettings ? Handlers.RegionalSettings : 0) |
                                             (provisioningOptions.SearchSettings ? Handlers.SearchSettings : 0) |
                                             (provisioningOptions.SitePolicy ? Handlers.SitePolicy : 0) |
                                             (provisioningOptions.SupportedUILanguages ? Handlers.SupportedUILanguages : 0) |
                                             (provisioningOptions.TermGroups ? Handlers.TermGroups : 0) |
                                             (provisioningOptions.Workflows ? Handlers.Workflows : 0) |
                                             (provisioningOptions.SiteSecurity ? Handlers.SiteSecurity : 0) |
                                             (provisioningOptions.ContentTypes ? Handlers.ContentTypes : 0) |
                                             (provisioningOptions.PropertyBagEntries ? Handlers.PropertyBagEntries : 0) |
                                             (provisioningOptions.PageContents ? Handlers.PageContents : 0) |
                                             (provisioningOptions.WebSettings ? Handlers.WebSettings : 0) |
                                             (provisioningOptions.Navigation ? Handlers.Navigation : 0);

                    ptci.MessagesDelegate = delegate (string message, ProvisioningMessageType messageType)
                    {
                        switch (messageType)
                        {
                            case ProvisioningMessageType.Error:
                                _writeMessage("Error: " + message);

                                break;

                            case ProvisioningMessageType.Progress:
                                _writeMessage("Progress: " + message);

                                break;

                            case ProvisioningMessageType.Warning:
                                _writeMessage("Warning: " + message);

                                break;

                            case ProvisioningMessageType.EasterEgg:
                                _writeMessage("EasterEgg: " + message);

                                break;

                            default:
                                _writeMessage("Unknown: " + message);

                                break;

                        }

                    };

                    ptci.ProgressDelegate = delegate (string message, int progress, int total)
                    {
                        // Output progress
                        _writeMessage(string.Format("{0:00}/{1:00} - {2}", progress, total, message));

                    };

                    // Create FileSystemConnector, to be used by OpenXMLConnector
                    var fileSystemConnector = new FileSystemConnector(provisioningOptions.TemplatePath, "");

                    ptci.FileConnector = new OpenXMLConnector(fileNamePNP, fileSystemConnector, "SharePoint Team");

                    // Execute actual extraction of the tepmplate 
                    ProvisioningTemplate template = web.GetProvisioningTemplate(ptci);

                    //Set properties for template web site
                    if (template.Properties?.Count > 0)
                    {
                        if (template.Properties.ContainsKey(Constants.PnP_Supports_SP2013_Platform))
                        {
                            template.Properties[Constants.PnP_Supports_SP2013_Platform] = ((provisioningOptions.SharePointVersion == SharePointVersion.SharePoint_2013_On_Premises) ? "true" : "false");

                        }
                        else
                        {
                            template.Properties.Add(Constants.PnP_Supports_SP2013_Platform, ((provisioningOptions.SharePointVersion == SharePointVersion.SharePoint_2013_On_Premises) ? "true" : "false"));

                        }

                        if (template.Properties.ContainsKey(Constants.PnP_Supports_SP2016_Platform))
                        {
                            template.Properties[Constants.PnP_Supports_SP2016_Platform] = ((provisioningOptions.SharePointVersion == SharePointVersion.SharePoint_2016_On_Premises) ? "true" : "false");

                        }
                        else
                        {
                            template.Properties.Add(Constants.PnP_Supports_SP2016_Platform, ((provisioningOptions.SharePointVersion == SharePointVersion.SharePoint_2016_On_Premises) ? "true" : "false"));

                        }

                        if (template.Properties.ContainsKey(Constants.PnP_Supports_SPO_Platform))
                        {
                            template.Properties[Constants.PnP_Supports_SPO_Platform] = ((provisioningOptions.SharePointVersion == SharePointVersion.SharePoint_2016_OnLine) ? "true" : "false");

                        }
                        else
                        {
                            template.Properties.Add(Constants.PnP_Supports_SPO_Platform, ((provisioningOptions.SharePointVersion == SharePointVersion.SharePoint_2016_OnLine) ? "true" : "false"));

                        }

                    }
                    else
                    {
                        template.Properties.Add(Constants.PnP_Supports_SP2013_Platform, ((provisioningOptions.SharePointVersion == SharePointVersion.SharePoint_2013_On_Premises) ? "true" : "false"));
                        template.Properties.Add(Constants.PnP_Supports_SP2016_Platform, ((provisioningOptions.SharePointVersion == SharePointVersion.SharePoint_2016_On_Premises) ? "true" : "false"));
                        template.Properties.Add(Constants.PnP_Supports_SPO_Platform, ((provisioningOptions.SharePointVersion == SharePointVersion.SharePoint_2016_OnLine) ? "true" : "false"));

                    }

                    _writeMessage($"Info: PnP platform support for {Constants.SharePoint_2013_On_Premises} set to {template.Properties[Constants.PnP_Supports_SP2013_Platform]}");
                    _writeMessage($"Info: PnP platform support for {Constants.SharePoint_2016_On_Premises} set to {template.Properties[Constants.PnP_Supports_SP2016_Platform]}");
                    _writeMessage($"Info: PnP platform support for {Constants.SharePoint_2016_Online} set to {template.Properties[Constants.PnP_Supports_SPO_Platform]}");

                    //List to hold all the lookup list names
                    List<string> lookupListTitles = new List<string>();

                    if (provisioningOptions.Fields)
                    {
                        //fix fields with default, formula or defaultformula elements so that the referenced fields have 
                        //a lower index than the fields that reference them in the SiteFields collection
                        //This prevents the "Invalid field found" error from occuring when applying the template to a site
                        FixReferenceFields(template, lookupListTitles);

                    }

                    //Check if we should do any content operations
                    if (provisioningOptions.LookupListItems ||
                        provisioningOptions.Files ||
                        provisioningOptions.Lists)
                    {
                        ctx.Load(web.Lists);
                        ctx.ExecuteQuery();

                        foreach (ListInstance listInstance in template.Lists)
                        {

                            if (provisioningOptions.LookupListItems)
                            {
                                if (lookupListTitles.IndexOf(listInstance.Title) >= 0)
                                {
                                    SaveListItemsToTemplate(ctx, web.Lists, listInstance);

                                }

                            }

                            switch (listInstance.TemplateType)
                            {
                                case 100: //  Generic list
                                    if (provisioningOptions.GenericList)
                                    {
                                        SaveListItemsToTemplate(ctx, web.Lists, listInstance);

                                    }

                                    break;

                                case 101: //   Document library
                                    if (provisioningOptions.DocumentLibrary)
                                    {
                                        SaveFilesToTemplate(ctx, web, listInstance, template);

                                    }

                                    break;

                                case 102: //   Survey
                                    if (provisioningOptions.SurveyList)
                                    {
                                        SaveListItemsToTemplate(ctx, web.Lists, listInstance);

                                    }

                                    break;

                                case 103: //   Links list
                                    if (provisioningOptions.LinksList)
                                    {
                                        SaveListItemsToTemplate(ctx, web.Lists, listInstance);

                                    }

                                    break;

                                case 104: //   Announcements list
                                    if (provisioningOptions.AnnouncementsList)
                                    {
                                        SaveListItemsToTemplate(ctx, web.Lists, listInstance);

                                    }

                                    break;

                                case 105: //   Contacts list
                                    if (provisioningOptions.ContactsList)
                                    {
                                        SaveListItemsToTemplate(ctx, web.Lists, listInstance);

                                    }

                                    break;

                                case 106: //   Events list
                                    if (provisioningOptions.EventsList)
                                    {
                                        SaveListItemsToTemplate(ctx, web.Lists, listInstance);

                                    }

                                    break;

                                case 107: //   Tasks list
                                    if (provisioningOptions.TasksList)
                                    {
                                        SaveListItemsToTemplate(ctx, web.Lists, listInstance);

                                    }

                                    break;

                                case 108: //   Discussion board
                                    if (provisioningOptions.DiscussionBoard)
                                    {
                                        SaveListItemsToTemplate(ctx, web.Lists, listInstance);

                                    }

                                    break;

                                case 109: //   Picture library
                                    if (provisioningOptions.PictureLibrary)
                                    {
                                        SaveFilesToTemplate(ctx, web, listInstance, template);

                                    }

                                    break;

                                case 119: //   Wiki Page library
                                    if (provisioningOptions.WikiPageLibrary)
                                    {
                                        SaveFilesToTemplate(ctx, web, listInstance, template);

                                    }

                                    break;

                                case 150: //   Gantt Tasks list
                                    if (provisioningOptions.GanttTasksList)
                                    {
                                        SaveListItemsToTemplate(ctx, web.Lists, listInstance);

                                    }

                                    break;

                                case 200: //   Meeting Series
                                    if (provisioningOptions.MeetingSeriesList)
                                    {
                                        SaveListItemsToTemplate(ctx, web.Lists, listInstance);

                                    }

                                    break;

                                case 301: //   Blog Posts list
                                    if (provisioningOptions.BlogPostsList)
                                    {
                                        SaveListItemsToTemplate(ctx, web.Lists, listInstance);

                                    }

                                    break;

                                case 302: //   Blog Comments list
                                    if (provisioningOptions.BlogCommentsList)
                                    {
                                        SaveListItemsToTemplate(ctx, web.Lists, listInstance);

                                    }

                                    break;

                                case 303: //   Blog Categories list
                                    if (provisioningOptions.BlogCategoriesList)
                                    {
                                        SaveListItemsToTemplate(ctx, web.Lists, listInstance);

                                    }

                                    break;

                                case 1100: //   Issue tracking
                                    if (provisioningOptions.IssueTrackingList)
                                    {
                                        SaveListItemsToTemplate(ctx, web.Lists, listInstance);

                                    }

                                    break;

                                default:

                                    break;

                            } //switch

                        }

                    }

                    //if exclude base template from template
                    if (provisioningOptions.ExcludeBaseTemplate)
                    {
                        ProvisioningTemplate baseTemplate = null;
                        _writeMessage($"Info: Loading base template {web.WebTemplate}#{web.Configuration}");

                        baseTemplate = web.GetBaseTemplate(web.WebTemplate, web.Configuration);

                        _writeMessage("Info: Base template loaded");

                        //perform the clean up
                        CleanupTemplate(provisioningOptions, template, baseTemplate);

                        //if not publishing site and publishing feature is activated, then clean publishing features from template
                        if (!baseTemplate.BaseSiteTemplate.Equals(Constants.Enterprise_Wiki_TemplateId,
                                                                  StringComparison.OrdinalIgnoreCase))
                        {
                            if (web.IsPublishingWeb())
                            {
                                _writeMessage("Info: Publishing feature actived on site");
                                _writeMessage($"Info: Loading {Constants.Enterprise_Wiki_TemplateId} base template");

                                string[] enterWikiArr = Constants.Enterprise_Wiki_TemplateId.Split(new char[] { '#' });

                                short config = Convert.ToInt16(enterWikiArr[1]);

                                baseTemplate = web.GetBaseTemplate(enterWikiArr[0], config);

                                _writeMessage($"Info: Done loading {Constants.Enterprise_Wiki_TemplateId} base template");

                                //perform the clean up
                                CleanupTemplate(provisioningOptions, template, baseTemplate);

                            }

                        }

                    }

                    XMLTemplateProvider provider = new XMLOpenXMLTemplateProvider(ptci.FileConnector as OpenXMLConnector);
                    provider.SaveAs(template, fileNameXML);


                    _writeMessage($"Template saved to {provisioningOptions.TemplatePath}\\{provisioningOptions.TemplateName}.pnp");

                    _writeMessage($"Done creating provisioning template from {web.Title} ( {web.Url} )");

                    result = true;

                }

            }
            catch (Exception ex)
            {
                _writeMessage("Error: " + ex.Message.Replace("\r\n", " "));
                if (ex.InnerException != null)
                {
                    _writeMessage("Error: Start of inner exception");
                    _writeMessage("Error: " + ex.InnerException.Message.Replace("\r\n", " "));
                    _writeMessageRange(ex.InnerException.StackTrace.Split(new char[] { '\n', '\r' },
                                                                         StringSplitOptions.RemoveEmptyEntries));
                    _writeMessage("Error: End of inner exception");

                }

                _writeMessageRange(ex.StackTrace.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries));
                result = false;

            }

            return result;

        } //CreateProvisioningTemplate

        public bool ApplyProvisioningTemplate(ProvisioningOptions provisioningOptions,
                                              Action<string> writeMessage,
                                              Action<string[]> writeMessageRange)
        {
            bool result = false;
            try
            {
                if (writeMessage != null)
                {
                    _writeMessage = writeMessage;

                }
                else
                {
                    _writeMessage = WriteMessage;

                }

                if (writeMessageRange != null)
                {
                    _writeMessageRange = writeMessageRange;

                }
                else
                {
                    _writeMessageRange = WriteMessageRange;

                }

                using (var ctx = new ClientContext(provisioningOptions.WebAddress))
                {
                    SecureString pwdSecure = new SecureString();
                    foreach (char c in provisioningOptions.UserPassword.ToCharArray()) pwdSecure.AppendChar(c);

                    ctx.Credentials = new SharePointOnlineCredentials(provisioningOptions.UserNameOrEmail,
                                                                      pwdSecure);

                    ctx.RequestTimeout = Timeout.Infinite;

                    string webTitle = string.Empty;

                    _writeMessage($"Connecting to {provisioningOptions.WebAddress}");

                    // Just to output the site details 
                    Web web = ctx.Web;
                    ctx.Load(web, w => w.Title, w => w.Url);
                    ctx.ExecuteQueryRetry();

                    webTitle = web.Title;

                    _writeMessage($"Applying provisioning template to {webTitle} ( {web.Url} )");

                    string fileNamePNP = provisioningOptions.TemplateName + ".pnp";

                    FileConnectorBase fileConnector = new FileSystemConnector(provisioningOptions.TemplatePath, "");

                    XMLTemplateProvider provider = new XMLOpenXMLTemplateProvider(new OpenXMLConnector(fileNamePNP, fileConnector));

                    List<ProvisioningTemplate> templates = provider.GetTemplates();

                    ProvisioningTemplate template = templates[0];

                    _writeMessage($"Base site template in provisioning template is {template.BaseSiteTemplate}");

                    //Check for template platform support
                    if (template.Properties?.Count > 0)
                    {
                        if (template.Properties.ContainsKey(Constants.PnP_Supports_SP2013_Platform))
                        {
                            _writeMessage($"Info: PnP platform support for {Constants.SharePoint_2013_On_Premises} set to {template.Properties[Constants.PnP_Supports_SP2013_Platform]}");

                        }

                        if (template.Properties.ContainsKey(Constants.PnP_Supports_SP2016_Platform))
                        {
                            _writeMessage($"Info: PnP platform support for {Constants.SharePoint_2016_On_Premises} set to {template.Properties[Constants.PnP_Supports_SP2016_Platform]}");

                        }

                        if (template.Properties.ContainsKey(Constants.PnP_Supports_SPO_Platform))
                        {
                            _writeMessage($"Info: PnP platform support for {Constants.SharePoint_2016_Online} set to {template.Properties[Constants.PnP_Supports_SPO_Platform]}");

                        }

                    }
                    else
                    {
                        _writeMessage("Warning: PnP platform support not defined");

                    }

                    //prevent web site title overwriting
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
                                _writeMessage("Error: " + message);

                                break;

                            case ProvisioningMessageType.Progress:
                                _writeMessage("Progress: " + message);

                                break;

                            case ProvisioningMessageType.Warning:
                                _writeMessage("Warning: " + message);

                                break;

                            case ProvisioningMessageType.EasterEgg:
                                _writeMessage("EasterEgg: " + message);

                                break;

                            default:
                                _writeMessage("Unknown: " + message);

                                break;

                        }

                    };

                    ptai.ProgressDelegate = delegate (string message, int progress, int total)
                    {
                        _writeMessage(string.Format("{0:00}/{1:00} - {2}", progress, total, message));

                    };

                    ptai.HandlersToProcess = (provisioningOptions.AuditSettings ? Handlers.AuditSettings : 0) |
                                             (provisioningOptions.ComposedLook ? Handlers.ComposedLook : 0) |
                                             (provisioningOptions.CustomActions ? Handlers.CustomActions : 0) |
                                             (provisioningOptions.ExtensibilityProviders ? Handlers.ExtensibilityProviders : 0) |
                                             (provisioningOptions.Features ? Handlers.Features : 0) |
                                             (provisioningOptions.Fields ? Handlers.Fields : 0) |
                                             (provisioningOptions.Files ? Handlers.Files : 0) |
                                             (provisioningOptions.ListInstances ? Handlers.Lists : 0) |
                                             (provisioningOptions.Pages ? Handlers.Pages : 0) |
                                             (provisioningOptions.Publishing ? Handlers.Publishing : 0) |
                                             (provisioningOptions.RegionalSettings ? Handlers.RegionalSettings : 0) |
                                             (provisioningOptions.SearchSettings ? Handlers.SearchSettings : 0) |
                                             (provisioningOptions.SitePolicy ? Handlers.SitePolicy : 0) |
                                             (provisioningOptions.SupportedUILanguages ? Handlers.SupportedUILanguages : 0) |
                                             (provisioningOptions.TermGroups ? Handlers.TermGroups : 0) |
                                             (provisioningOptions.Workflows ? Handlers.Workflows : 0) |
                                             (provisioningOptions.SiteSecurity ? Handlers.SiteSecurity : 0) |
                                             (provisioningOptions.ContentTypes ? Handlers.ContentTypes : 0) |
                                             (provisioningOptions.PropertyBagEntries ? Handlers.PropertyBagEntries : 0) |
                                             (provisioningOptions.PageContents ? Handlers.PageContents : 0) |
                                             (provisioningOptions.WebSettings ? Handlers.WebSettings : 0) |
                                             (provisioningOptions.Navigation ? Handlers.Navigation : 0);

                    web.ApplyProvisioningTemplate(template, ptai);

                    _writeMessage($"Done applying provisioning template to {web.Title} ( {web.Url} )");

                    result = true;

                }

            }
            catch (Exception ex)
            {
                _writeMessage("Error: " + ex.Message.Replace("\r\n", " "));
                if (ex.InnerException != null)
                {
                    _writeMessage("Error: Start of inner exception");
                    _writeMessage("Error: " + ex.InnerException.Message.Replace("\r\n", " "));
                    _writeMessageRange(ex.InnerException.StackTrace.Split(new char[] { '\r', '\n' },
                                                                         StringSplitOptions.RemoveEmptyEntries));
                    _writeMessage("Error: End of inner exception");

                }

                _writeMessageRange(ex.StackTrace.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries));

                result = false;

            }

            return result;

        } //ApplyProvisioningTemplate

        public TemplateItems OpenTemplateForEdit(string templatePath, string templateName)
        {
            TemplateItems templateItems = new TemplateItems();

            templateItems.TemplatePath = templatePath;

            string fileNamePNP = templateName;

            if (!templateName.EndsWith(".pnp", StringComparison.OrdinalIgnoreCase))
            {
                fileNamePNP += ".pnp";

            }

            templateItems.TemplateFilename = fileNamePNP;

            FileConnectorBase fileConnector = new FileSystemConnector(templatePath, "");

            EditingProvider = new XMLOpenXMLTemplateProvider(new OpenXMLConnector(fileNamePNP, fileConnector));

            XMLTemplateProvider provider = EditingProvider;

            List<ProvisioningTemplate> templates = provider.GetTemplates();
            ProvisioningTemplate template = templates[0]; //get only the first template

            template.Connector = provider.Connector;

            EditingTemplate = template;

            KeyValueList templateList = new KeyValueList();

            TemplateItem rootNode = templateItems.AddTemplateItem("TemplateNode", $"Template - ( {templateName} )",
                                                  TemplateControlType.ListBox, TemplateItemType.Template,
                                                  null, string.Empty);

            if (template.RegionalSettings != null)
            {
                TemplateItem rsNode = templateItems.AddTemplateItem("RegionalSettings", "Regional Settings",
                                                    TemplateControlType.Form, TemplateItemType.RegionalSetting,
                                                    GetRegionalSettings(), rootNode.Id);

                templateList.AddKeyValue(rsNode.Text, rsNode.Id);

            }

            if (template.AddIns?.Count > 0)
            {
                TemplateItem aiNodes = templateItems.AddTemplateItem("AddIns", "Add-Ins",
                                                     TemplateControlType.ListBox,
                                                     TemplateItemType.AddInList, null, rootNode.Id);

                KeyValueList addInsList = new KeyValueList();

                foreach (var addIn in template.AddIns)
                {
                    TemplateItem aiNode = templateItems.AddTemplateItem(addIn.PackagePath, addIn.PackagePath,
                                                        TemplateControlType.TextBox, TemplateItemType.AddInItem,
                                                        addIn.Source, aiNodes.Id);

                    addInsList.AddKeyValue(addIn.PackagePath, aiNode.Id);

                }

                aiNodes.Content = addInsList;

                templateList.AddKeyValue(aiNodes.Text, aiNodes.Id);

            }

            if (template.ComposedLook?.Name != null)
            {
                TemplateItem clNode = templateItems.AddTemplateItem("ComposedLook", "Composed Look",
                                                    TemplateControlType.Form, TemplateItemType.ComposedLook,
                                                    GetComposedLook(), rootNode.Id);

                templateList.AddKeyValue(clNode.Text, clNode.Id);

            }

            if (template.CustomActions?.SiteCustomActions?.Count > 0)
            {
                TemplateItem scaNodes = templateItems.AddTemplateItem("SiteCustomActions", "Site Custom Actions",
                                                      TemplateControlType.ListBox,
                                                      TemplateItemType.SiteCustomActionList, null,
                                                      rootNode.Id);

                KeyValueList siteCustomActionsList = new KeyValueList();

                foreach (var siteCustomAction in template.CustomActions.SiteCustomActions)
                {
                    TemplateItem scaNode = templateItems.AddTemplateItem(siteCustomAction.RegistrationId,
                                                         siteCustomAction.Name,
                                                         TemplateControlType.TextBox,
                                                         TemplateItemType.SiteCustomActionItem,
                                                         GetCustomAction(siteCustomAction),
                                                         scaNodes.Id);

                    siteCustomActionsList.AddKeyValue(siteCustomAction.Name, scaNode.Id);

                }

                scaNodes.Content = siteCustomActionsList;

                templateList.AddKeyValue(scaNodes.Text, scaNodes.Id);

            }

            if (template.CustomActions?.WebCustomActions?.Count > 0)
            {
                TemplateItem wcaNodes = templateItems.AddTemplateItem("WebCustomActions", "Web Custom Actions",
                                                      TemplateControlType.ListBox,
                                                      TemplateItemType.WebCustomActionList, null,
                                                      rootNode.Id);

                KeyValueList webCustomActionsList = new KeyValueList();

                foreach (var webCustomAction in template.CustomActions.WebCustomActions)
                {
                    TemplateItem wcaNode = templateItems.AddTemplateItem(webCustomAction.RegistrationId,
                                                         webCustomAction.Name,
                                                         TemplateControlType.TextBox,
                                                         TemplateItemType.WebCustomActionItem,
                                                         GetCustomAction(webCustomAction),
                                                         wcaNodes.Id);

                    webCustomActionsList.AddKeyValue(webCustomAction.Name, wcaNode.Id);

                }

                wcaNodes.Content = webCustomActionsList;

                templateList.AddKeyValue(wcaNodes.Text, wcaNodes.Id);

            }

            if (template.Features?.SiteFeatures?.Count > 0)
            {
                KeyValueList siteFeaturesList = new KeyValueList();

                foreach (var siteFeature in template.Features.SiteFeatures)
                {
                    siteFeaturesList.AddKeyValue(siteFeature.Id.ToString("B"), siteFeature.Id.ToString("B")); //B = {} format

                }

                TemplateItem sfNodes = templateItems.AddTemplateItem("SiteFeatures", "Site Features",
                                                     TemplateControlType.ListBox,
                                                     TemplateItemType.SiteFeatureList,
                                                     siteFeaturesList,
                                                     rootNode.Id);

                templateList.AddKeyValue(sfNodes.Text, sfNodes.Id);

            }

            if (template.Features?.WebFeatures?.Count > 0)
            {
                KeyValueList webFeaturesList = new KeyValueList();

                foreach (var webFeature in template.Features.WebFeatures)
                {
                    webFeaturesList.AddKeyValue(webFeature.Id.ToString("B"), webFeature.Id.ToString("B"));

                }

                TemplateItem wfNodes = templateItems.AddTemplateItem("WebFeatures", "Web Features",
                                                     TemplateControlType.ListBox,
                                                     TemplateItemType.WebFeatureList,
                                                     webFeaturesList,
                                                     rootNode.Id);

                templateList.AddKeyValue(wfNodes.Text, wfNodes.Id);

            }

            if (template.ContentTypes?.Count > 0)
            {
                TemplateItem ctNodes = templateItems.AddTemplateItem("ContentTypes", "Content Types",
                                                     TemplateControlType.ListBox,
                                                     TemplateItemType.ContentTypeList, null,
                                                     rootNode.Id);

                KeyValueList contentTypeList = new KeyValueList();

                foreach (var contentType in template.ContentTypes)
                {
                    KeyValueList contentTypeGroup = null;
                    TemplateItem groupItem = templateItems.GetItemByName(contentType.Group,
                                                                         TemplateItemType.ContentTypeGroup);
                    if (groupItem != null)
                    {
                        contentTypeGroup = groupItem.Content as KeyValueList;

                    }
                    else
                    {
                        groupItem = templateItems.AddTemplateItem(contentType.Group, contentType.Group,
                                                             TemplateControlType.ListBox,
                                                             TemplateItemType.ContentTypeGroup, null,
                                                             ctNodes.Id);

                        contentTypeGroup = new KeyValueList();

                    }

                    TemplateItem ctNode = templateItems.AddTemplateItem(contentType.Id, contentType.Name,
                                                                    TemplateControlType.TextBox,
                                                                    TemplateItemType.ContentTypeItem,
                                                                    GetContentType(contentType.Id),
                                                                    groupItem.Id);


                    contentTypeGroup.AddKeyValue(contentType.Name, ctNode.Id);

                    groupItem.Content = contentTypeGroup;

                    if (!contentTypeList.Exists(p => p.Key.Equals(contentType.Group,
                                                StringComparison.OrdinalIgnoreCase)))
                    {
                        contentTypeList.AddKeyValue(contentType.Group, groupItem.Id);

                    }

                }

                ctNodes.Content = contentTypeList;

                templateList.AddKeyValue(ctNodes.Text, ctNodes.Id);

            }

            if (template.SiteFields?.Count > 0)
            {
                TemplateItem sfNodes = templateItems.AddTemplateItem("SiteFields", "Site Fields",
                                                     TemplateControlType.ListBox,
                                                     TemplateItemType.SiteFieldList, null,
                                                     rootNode.Id);

                KeyValueList siteFieldsList = new KeyValueList();

                foreach (var siteField in template.SiteFields)
                {
                    XElement fieldElement = XElement.Parse(siteField.SchemaXml);
                    string fieldGroup = "Undefined Group";
                    if (fieldElement.Attribute("Group") != null)
                    {
                        fieldGroup = fieldElement.Attribute("Group").Value;

                    }

                    string fieldID = fieldElement.Attribute("ID").Value;
                    string fieldName = fieldElement.Attribute("Name").Value;

                    KeyValueList siteFieldsGroup = null;
                    TemplateItem groupItem = templateItems.GetItemByName(fieldGroup, TemplateItemType.SiteFieldGroup);
                    if (groupItem != null)
                    {
                        siteFieldsGroup = groupItem.Content as KeyValueList;

                    }
                    else
                    {
                        groupItem = templateItems.AddTemplateItem(fieldGroup, fieldGroup,
                                                             TemplateControlType.ListBox,
                                                             TemplateItemType.SiteFieldGroup, null,
                                                             sfNodes.Id);

                        siteFieldsGroup = new KeyValueList();

                    }

                    string fieldXml = fieldElement.ToString(SaveOptions.None);
                    int gtFirst = fieldXml.IndexOf('>', 0);
                    string fieldText = fieldXml.Substring(0, gtFirst).Replace("\" ", "\"\r\n       ") +
                                       fieldXml.Substring(gtFirst);

                    TemplateItem sfNode = templateItems.AddTemplateItem(fieldID, fieldName,
                                                        TemplateControlType.TextBox,
                                                        TemplateItemType.SiteFieldItem, fieldText,
                                                        groupItem.Id);

                    siteFieldsGroup.AddKeyValue(fieldName, sfNode.Id);

                    groupItem.Content = siteFieldsGroup;

                    if (!siteFieldsList.Exists(p => p.Key.Equals(fieldGroup,
                                               StringComparison.OrdinalIgnoreCase)))
                    {
                        siteFieldsList.AddKeyValue(fieldGroup, groupItem.Id);

                    }

                }

                sfNodes.Content = siteFieldsList;

                templateList.AddKeyValue(sfNodes.Text, sfNodes.Id);

            }

            if (template.Files?.Count > 0)
            {
                TemplateItem fNodes = templateItems.AddTemplateItem("Files", "Files",
                                                                    TemplateControlType.ListBox,
                                                                    TemplateItemType.FileList, null,
                                                                    rootNode.Id);

                KeyValueList filesList = new KeyValueList();

                foreach (var file in template.Files)
                {
                    string fileSrc = file.Src.Replace("%20", " ");
                    file.Src = fileSrc;

                    TemplateItem fNode = templateItems.AddTemplateItem(fileSrc, fileSrc,
                                                                       TemplateControlType.TextBox,
                                                                       TemplateItemType.FileItem,
                                                                       GetPNPFile(file),
                                                                       fNodes.Id);

                    if (file.WebParts?.Count > 0)
                    {
                        TemplateItem fwpNodes = templateItems.AddTemplateItem(fNode.Name + "_WebParts", "WebParts",
                                                              TemplateControlType.ListBox,
                                                              TemplateItemType.FileWebPartsList, null,
                                                              fNode.Id);

                        KeyValueList webPartList = new KeyValueList();

                        foreach (var webPart in file.WebParts)
                        {
                            WebPart newWP = new WebPart()
                            {
                                Column = webPart.Column,
                                Order = webPart.Order,
                                Row = webPart.Row,
                                Title = webPart.Title,
                                Zone = webPart.Zone

                            };

                            TemplateItem fwpNode = templateItems.AddTemplateItem(fwpNodes.Name + "_" + newWP.Title, newWP.Title,
                                                                 TemplateControlType.TextBox,
                                                                 TemplateItemType.FileWebPartItem,
                                                                 JsonConvert.SerializeObject(newWP, Newtonsoft.Json.Formatting.Indented),
                                                                 fwpNodes.Id);

                            webPartList.AddKeyValue(newWP.Title, fwpNode.Id);

                            XElement fwpcElement = XElement.Parse(webPart.Contents);

                            string fieldXml = fwpcElement.ToString(SaveOptions.None);

                            TemplateItem fwpcNode = templateItems.AddTemplateItem(fwpNode.Name + "_Contents", "Contents",
                                                                  TemplateControlType.TextBox,
                                                                  TemplateItemType.FileWebPartItemContent,
                                                                  fieldXml,
                                                                  fwpNode.Id);

                        }

                        fwpNodes.Content = webPartList;

                    }

                    filesList.AddKeyValue(fileSrc, fNode.Id);

                }

                fNodes.Content = filesList;

                templateList.AddKeyValue(fNodes.Text, fNodes.Id);

            }

            if (template.Lists?.Count > 0)
            {
                TemplateItem lNodes = templateItems.AddTemplateItem("Lists", "Lists",
                                                    TemplateControlType.ListBox,
                                                    TemplateItemType.ListList, null,
                                                    rootNode.Id);

                KeyValueList listsList = new KeyValueList();

                foreach (var list in template.Lists)
                {
                    TemplateItem lNode = templateItems.AddTemplateItem(list.Url, list.Title,
                                                                    TemplateControlType.TextBox,
                                                                    TemplateItemType.ListItem,
                                                                    GetListInstance(list.Url),
                                                                    lNodes.Id);

                    if (list.Fields?.Count > 0)
                    {
                        TemplateItem fNodes = templateItems.AddTemplateItem(lNode.Name + "_ListFields", "Fields", TemplateControlType.ListBox,
                                                            TemplateItemType.ListFieldList, null,
                                                            lNode.Id);

                        KeyValueList fieldsList = new KeyValueList();

                        foreach (var field in list.Fields)
                        {
                            XElement fieldElement = XElement.Parse(field.SchemaXml);
                            string fieldID = fieldElement.Attribute("ID").Value;
                            string fieldName = fieldElement.Attribute("Name").Value;

                            string fieldXml = fieldElement.ToString(SaveOptions.None);
                            //Arrange first element attributes in rows
                            int gtFirst = fieldXml.IndexOf('>', 0);
                            string fieldText = fieldXml.Substring(0, gtFirst).Replace("\" ", "\"\r\n       ") +
                                               fieldXml.Substring(gtFirst);

                            TemplateItem fNode = templateItems.AddTemplateItem(fieldID, fieldName, TemplateControlType.TextBox,
                                                               TemplateItemType.ListFieldItem, fieldText,
                                                               fNodes.Id);

                            fieldsList.AddKeyValue(fieldName, fNode.Id);

                        }

                        fNodes.Content = fieldsList;

                    }

                    if (list.Views?.Count > 0)
                    {
                        TemplateItem vNodes = templateItems.AddTemplateItem(lNode.Name + "_ListViews", "Views",
                                                            TemplateControlType.ListBox,
                                                            TemplateItemType.ListViewList, null,
                                                            lNode.Id);

                        KeyValueList viewsList = new KeyValueList();

                        foreach (var view in list.Views)
                        {
                            XElement viewElement = XElement.Parse(view.SchemaXml);
                            string viewName = viewElement.Attribute("Name").Value;
                            string displayName = viewElement.Attribute("DisplayName").Value;

                            string viewXml = viewElement.ToString(SaveOptions.None);
                            //Arrange first element attributes in rows
                            int gtFirst = viewXml.IndexOf('>', 0);
                            string viewText = viewXml.Substring(0, gtFirst).Replace("\" ", "\"\r\n      ") +
                                              viewXml.Substring(gtFirst);

                            TemplateItem vNode = templateItems.AddTemplateItem(viewName, displayName, TemplateControlType.TextBox,
                                                               TemplateItemType.ListViewItem, viewText,
                                                               vNodes.Id);


                            viewsList.AddKeyValue(displayName, vNode.Id);

                        }

                        vNodes.Content = viewsList;

                    }

                    listsList.AddKeyValue(list.Title, lNode.Id);

                }

                lNodes.Content = listsList;

                templateList.AddKeyValue(lNodes.Text, lNodes.Id);

            }

            if (template.Localizations?.Count > 0)
            {
                TemplateItem glNodes = templateItems.AddTemplateItem("Localizations", "Localizations", TemplateControlType.ListBox,
                                                    TemplateItemType.LocalizationsList, null,
                                                    rootNode.Id);

                KeyValueList localizationsList = new KeyValueList();

                foreach (var localization in template.Localizations)
                {
                    TemplateItem glNode = templateItems.AddTemplateItem(localization.LCID.ToString(), localization.Name,
                                                        TemplateControlType.TextBox,
                                                        TemplateItemType.LocalizationsItem,
                                                        GetLocalization(localization),
                                                        glNodes.Id);

                    localizationsList.AddKeyValue(localization.Name, glNode.Id);

                }

                glNodes.Content = localizationsList;

                templateList.AddKeyValue(glNodes.Text, glNodes.Id);

            }

            //Navigation to do

            if (template.Pages?.Count > 0)
            {
                TemplateItem pNodes = templateItems.AddTemplateItem("Pages", "Pages", TemplateControlType.ListBox,
                                                    TemplateItemType.PageList, null,
                                                    rootNode.Id);

                KeyValueList pageList = new KeyValueList();

                foreach (var page in template.Pages)
                {
                    TemplateItem pNode = templateItems.AddTemplateItem(page.Url, page.Url, TemplateControlType.TextBox,
                                                       TemplateItemType.PageItem,
                                                       GetPageContent(page),
                                                       pNodes.Id);

                    pageList.AddKeyValue(page.Url, pNode.Id);

                }

                pNodes.Content = pageList;

                templateList.AddKeyValue(pNodes.Text, pNodes.Id);

            }

            if (template.Properties?.Count > 0)
            {
                TemplateItem pNode = templateItems.AddTemplateItem("Properties", "Properties", TemplateControlType.ListView,
                                                   TemplateItemType.PropertiesList, null,
                                                   rootNode.Id);

                KeyValueList propertiesList = new KeyValueList();

                foreach (var property in template.Properties)
                {
                    propertiesList.AddKeyValue(property.Key, property.Value);

                }

                pNode.Content = propertiesList;

                templateList.AddKeyValue(pNode.Text, pNode.Id);

            }

            if (template.PropertyBagEntries?.Count > 0)
            {
                TemplateItem pbeNodes = templateItems.AddTemplateItem("PropertyBagEntries", "Property Bag Entries",
                                                      TemplateControlType.ListView,
                                                      TemplateItemType.PropertyBagEntriesList, null,
                                                      rootNode.Id);

                KeyValueList propertyBagEntriesList = new KeyValueList();

                foreach (var propertyBagEntry in template.PropertyBagEntries)
                {
                    propertyBagEntriesList.AddKeyValue(propertyBagEntry.Key, propertyBagEntry.Value);

                }

                pbeNodes.Content = propertyBagEntriesList;

                templateList.AddKeyValue(pbeNodes.Text, pbeNodes.Id);

            }

            if (template.Publishing != null)
            {
                TemplateItem pNode = templateItems.AddTemplateItem("Publishing", "Publishing", TemplateControlType.TextBox,
                                                   TemplateItemType.PublishingList,
                                                   GetPublishing(template.Publishing),
                                                   rootNode.Id);

                templateList.AddKeyValue(pNode.Text, pNode.Id);

            }

            if (template.Security != null)
            {
                SiteSecurity oldSecurity = template.Security;
                SiteSecurity newSecurity = new SiteSecurity()
                {
                    BreakRoleInheritance = oldSecurity.BreakRoleInheritance,
                    ClearSubscopes = oldSecurity.ClearSubscopes,
                    CopyRoleAssignments = oldSecurity.CopyRoleAssignments

                };
                newSecurity.AdditionalAdministrators.AddRange(oldSecurity.AdditionalAdministrators);
                newSecurity.AdditionalMembers.AddRange(oldSecurity.AdditionalMembers);
                newSecurity.AdditionalOwners.AddRange(oldSecurity.AdditionalOwners);
                newSecurity.AdditionalVisitors.AddRange(oldSecurity.AdditionalVisitors);
                newSecurity.SiteGroups.AddRange(oldSecurity.SiteGroups);
                newSecurity.SiteSecurityPermissions.RoleAssignments.AddRange(oldSecurity.SiteSecurityPermissions.RoleAssignments);
                newSecurity.SiteSecurityPermissions.RoleDefinitions.AddRange(oldSecurity.SiteSecurityPermissions.RoleDefinitions);

                string security = JsonConvert.SerializeObject(newSecurity, Newtonsoft.Json.Formatting.Indented);

                TemplateItem sNode = templateItems.AddTemplateItem("SiteSecurity", "Site Security", TemplateControlType.TextBox,
                                                   TemplateItemType.SecurityItem, security,
                                                   rootNode.Id);

                templateList.AddKeyValue(sNode.Text, sNode.Id);

            }

            if (template.SupportedUILanguages?.Count > 0)
            {
                TemplateItem suilNode = templateItems.AddTemplateItem("SupportedUILanguages", "Supported UI Languages",
                                                      TemplateControlType.ListBox,
                                                      TemplateItemType.SupportedUILanguagesList, null,
                                                      rootNode.Id);

                KeyValueList supportedUILanguages = new KeyValueList();

                foreach (var suil in template.SupportedUILanguages)
                {
                    supportedUILanguages.AddKeyValue(suil.LCID.ToString(), suil.LCID.ToString());

                }

                suilNode.Content = supportedUILanguages;

                templateList.AddKeyValue(suilNode.Text, suilNode.Id);

            }

            if (template.TermGroups?.Count > 0)
            {
                TemplateItem tgNodes = templateItems.AddTemplateItem("TermGroups", "Term Groups", TemplateControlType.ListBox,
                                                     TemplateItemType.TermGroupList, null,
                                                     rootNode.Id);

                KeyValueList termGroupsList = new KeyValueList();

                foreach (var termGroup in template.TermGroups)
                {
                    TemplateItem tgNode = templateItems.AddTemplateItem(termGroup.Id.ToString("N"), termGroup.Name,
                                                        TemplateControlType.TextBox,
                                                        TemplateItemType.TermGroupItem,
                                                        GetTermGroup(termGroup.Id),
                                                        tgNodes.Id);


                    if (termGroup.TermSets?.Count > 0)
                    {
                        TemplateItem tsNodes = templateItems.AddTemplateItem(tgNode.Name + "_TermSets", "Term Sets",
                                                             TemplateControlType.ListBox,
                                                             TemplateItemType.TermSetList, null,
                                                             tgNode.Id);

                        KeyValueList termSetsList = new KeyValueList();

                        foreach (var termSet in termGroup.TermSets)
                        {
                            TemplateItem tsNode = templateItems.AddTemplateItem(termSet.Id.ToString("N"), termSet.Name,
                                                                TemplateControlType.TextBox,
                                                                TemplateItemType.TermSetItem,
                                                                GetTermSet(termSet),
                                                                tsNodes.Id);

                            termSetsList.AddKeyValue(termSet.Name, tsNode.Id);

                        }

                        tsNodes.Content = termSetsList;

                    }

                    termGroupsList.AddKeyValue(termGroup.Name, tgNode.Id);

                }

                tgNodes.Content = termGroupsList;

                templateList.AddKeyValue(tgNodes.Text, tgNodes.Id);

            }

            if (template.WebSettings != null)
            {
                TemplateItem wsNode = templateItems.AddTemplateItem("WebSettings", "Web Settings", TemplateControlType.Form,
                                                    TemplateItemType.WebSetting,
                                                    GetWebSettings(template.WebSettings),
                                                    rootNode.Id);

                templateList.AddKeyValue(wsNode.Text, wsNode.Id);

            }

            if (template.Workflows?.WorkflowDefinitions?.Count > 0)
            {
                TemplateItem wwdNodes = templateItems.AddTemplateItem("WorkflowDefinitions", "Workflow Definitions",
                                                      TemplateControlType.ListBox,
                                                      TemplateItemType.WorkflowDefinitionList, null,
                                                      rootNode.Id);

                KeyValueList workflowDefinitionsList = new KeyValueList();

                foreach (var workflowDefinition in template.Workflows.WorkflowDefinitions)
                {
                    TemplateItem wwdNode = templateItems.AddTemplateItem(workflowDefinition.Id.ToString("N"),
                                                         workflowDefinition.DisplayName,
                                                         TemplateControlType.TextBox,
                                                         TemplateItemType.WorkflowDefinitionItem,
                                                         GetWorkflowDefinition(workflowDefinition.Id),
                                                         wwdNodes.Id);

                    workflowDefinitionsList.AddKeyValue(workflowDefinition.DisplayName, wwdNode.Id);

                }

                wwdNodes.Content = workflowDefinitionsList;

                templateList.AddKeyValue(wwdNodes.Text, wwdNodes.Id);

            }

            if (template.Workflows?.WorkflowSubscriptions?.Count > 0)
            {
                TemplateItem wwsNodes = templateItems.AddTemplateItem("WorkflowSubscriptions", "Workflow Subscriptions",
                                                      TemplateControlType.ListBox,
                                                      TemplateItemType.WorkflowSubscriptionList, null,
                                                      rootNode.Id);

                KeyValueList workflowSubscriptionsList = new KeyValueList();

                foreach (var workflowSubscription in template.Workflows.WorkflowSubscriptions)
                {
                    TemplateItem wwsNode = templateItems.AddTemplateItem(workflowSubscription.Name, workflowSubscription.Name,
                                                         TemplateControlType.TextBox,
                                                         TemplateItemType.WorkflowSubscriptionItem,
                                                         GetWorkflowSubscription(workflowSubscription.Name),
                                                         wwsNodes.Id);

                    workflowSubscriptionsList.AddKeyValue(workflowSubscription.Name, wwsNode.Id);

                }

                wwsNodes.Content = workflowSubscriptionsList;

                templateList.AddKeyValue(wwsNodes.Text, wwsNodes.Id);

            }

            rootNode.Content = templateList;

            return templateItems;

        } //OpenTemplateForEdit

        private int[] GetRegionalSettings()
        {
            int[] result = null;
            if (EditingTemplate?.RegionalSettings != null)
            {
                result = new int[]
                {
                    EditingTemplate.RegionalSettings.AdjustHijriDays,
                    (int)EditingTemplate.RegionalSettings.AlternateCalendarType,
                    (int)EditingTemplate.RegionalSettings.CalendarType,
                    EditingTemplate.RegionalSettings.Collation,
                    (int)EditingTemplate.RegionalSettings.FirstDayOfWeek,
                    EditingTemplate.RegionalSettings.FirstWeekOfYear,
                    EditingTemplate.RegionalSettings.LocaleId,
                    (EditingTemplate.RegionalSettings.ShowWeeks ? 1 : 0),
                    (EditingTemplate.RegionalSettings.Time24 ? 1 : 0),
                    EditingTemplate.RegionalSettings.TimeZone,
                    (int)EditingTemplate.RegionalSettings.WorkDayEndHour / 60,
                    EditingTemplate.RegionalSettings.WorkDays,
                    (int)EditingTemplate.RegionalSettings.WorkDayStartHour / 60

                };

                //Note: Ensure that the above array is populated in the order of the RegionalSettingProperties enum

            }

            return result;

        } //GetRegionalSettingProperty

        private string[] GetComposedLook()
        {
            string[] result = null;
            if (EditingTemplate?.ComposedLook != null)
            {
                result = new string[]
                {
                    EditingTemplate.ComposedLook.Name,
                    EditingTemplate.ComposedLook.BackgroundFile,
                    EditingTemplate.ComposedLook.ColorFile,
                    EditingTemplate.ComposedLook.FontFile,
                    EditingTemplate.ComposedLook.Version.ToString()

                };

                //Note: Ensure that the above array is populated in the order of the ComposedLookProperties enum

            }

            return result;

        } //GetComposedLook

        private string GetContentType(string contentTypeId)
        {
            string result = string.Empty;
            if (EditingTemplate?.ContentTypes != null)
            {
                PnPModel.ContentType contentType = EditingTemplate.ContentTypes.Find(p => p.Id.Equals(contentTypeId,
                                                                                     StringComparison.OrdinalIgnoreCase));
                if (contentType != null)
                {
                    PnPModel.ContentType newCT = new PnPModel.ContentType()
                    {
                        Id = contentType.Id,
                        Description = contentType.Description,
                        DisplayFormUrl = contentType.DisplayFormUrl,
                        DocumentSetTemplate = contentType.DocumentSetTemplate,
                        DocumentTemplate = contentType.DocumentTemplate,
                        EditFormUrl = contentType.EditFormUrl,
                        Group = contentType.Group,
                        Hidden = contentType.Hidden,
                        Name = contentType.Name,
                        NewFormUrl = contentType.NewFormUrl,
                        Overwrite = contentType.Overwrite,
                        ReadOnly = contentType.ReadOnly,
                        Sealed = contentType.Sealed

                    };

                    if (contentType.FieldRefs?.Count > 0)
                    {
                        newCT.FieldRefs.AddRange(contentType.FieldRefs);

                    }

                    result = JsonConvert.SerializeObject(newCT, Newtonsoft.Json.Formatting.Indented);

                }

            }

            return result;

        } //GetContentType

        private string GetListInstance(string url)
        {
            string result = string.Empty;
            if (EditingTemplate?.Lists != null)
            {
                ListInstance listInstance = EditingTemplate.Lists.Find(p => p.Url.Equals(url,
                                                                                         StringComparison.OrdinalIgnoreCase));
                if (listInstance != null)
                {
                    ListInstance newLI = new ListInstance()
                    {
                        ContentTypesEnabled = listInstance.ContentTypesEnabled,
                        Description = listInstance.Description,
                        DocumentTemplate = listInstance.DocumentTemplate,
                        DraftVersionVisibility = listInstance.DraftVersionVisibility,
                        EnableAttachments = listInstance.EnableAttachments,
                        EnableFolderCreation = listInstance.EnableFolderCreation,
                        EnableMinorVersions = listInstance.EnableMinorVersions,
                        EnableModeration = listInstance.EnableModeration,
                        EnableVersioning = listInstance.EnableVersioning,
                        ForceCheckout = listInstance.ForceCheckout,
                        Hidden = listInstance.Hidden,
                        MaxVersionLimit = listInstance.MaxVersionLimit,
                        MinorVersionLimit = listInstance.MinorVersionLimit,
                        OnQuickLaunch = listInstance.OnQuickLaunch,
                        RemoveExistingContentTypes = listInstance.RemoveExistingContentTypes,
                        RemoveExistingViews = listInstance.RemoveExistingViews,
                        TemplateFeatureID = listInstance.TemplateFeatureID,
                        TemplateType = listInstance.TemplateType,
                        Title = listInstance.Title,
                        Url = listInstance.Url

                    };

                    if (listInstance.ContentTypeBindings?.Count > 0)
                    {
                        newLI.ContentTypeBindings.AddRange(listInstance.ContentTypeBindings);

                    }

                    if (listInstance.DataRows?.Count > 0)
                    {
                        newLI.DataRows.AddRange(listInstance.DataRows);

                    }

                    if (listInstance.FieldDefaults?.Count > 0)
                    {
                        foreach (var kvp in listInstance.FieldDefaults)
                        {
                            newLI.FieldDefaults.Add(kvp.Key, kvp.Value);

                        }

                    }

                    if (listInstance.FieldRefs?.Count > 0)
                    {
                        newLI.FieldRefs.AddRange(listInstance.FieldRefs);

                    }

                    if (listInstance.Folders?.Count > 0)
                    {
                        newLI.Folders.AddRange(listInstance.Folders);

                    }

                    if (listInstance.Security != null)
                    {
                        newLI.Security = new ObjectSecurity()
                        {
                            ClearSubscopes = listInstance.Security.ClearSubscopes,
                            CopyRoleAssignments = listInstance.Security.CopyRoleAssignments

                        };

                        newLI.Security.RoleAssignments.AddRange(listInstance.Security.RoleAssignments);

                    }

                    if (listInstance.UserCustomActions?.Count > 0)
                    {
                        newLI.UserCustomActions.AddRange(listInstance.UserCustomActions);

                    }

                    //ensure fields are empty as they are handled elsewhere
                    newLI.Fields.Clear();
                    //ensure views are empty as they are handled elsewhere
                    newLI.Views.Clear();

                    result = JsonConvert.SerializeObject(newLI, Newtonsoft.Json.Formatting.Indented);

                }

            }

            return result;

        } //GetListInstance

        private string[] GetWebSettings(WebSettings webSettings)
        {
            string[] result = null;
            if (webSettings != null)
            {
                result = new string[]
               {
                   webSettings.AlternateCSS,
                   webSettings.CustomMasterPageUrl,
                   webSettings.Description,
                   webSettings.MasterPageUrl,
                   (webSettings.NoCrawl ? "1" : "0"),
                   webSettings.RequestAccessEmail,
                   webSettings.SiteLogo,
                   webSettings.Title,
                   webSettings.WelcomePage

               };

                //Note: Ensure that the above are added in the order as defined in the WebSettingProperties enum

            }

            return result;

        } //GetWebSettings

        private string GetWorkflowDefinition(Guid WorkflowDefinitionId)
        {
            string result = string.Empty;
            if (EditingTemplate?.Workflows?.WorkflowDefinitions != null)
            {
                WorkflowDefinition workflowDefinition = EditingTemplate.Workflows.WorkflowDefinitions.Find(p =>
                                                        p.Id.Equals(WorkflowDefinitionId));

                if (workflowDefinition != null)
                {
                    WorkflowDefinition newWD = new WorkflowDefinition()
                    {
                        AssociationUrl = workflowDefinition.AssociationUrl,
                        Description = workflowDefinition.Description,
                        DisplayName = workflowDefinition.DisplayName,
                        DraftVersion = workflowDefinition.DraftVersion,
                        FormField = workflowDefinition.FormField,
                        Id = workflowDefinition.Id,
                        InitiationUrl = workflowDefinition.InitiationUrl,
                        Published = workflowDefinition.Published,
                        RequiresAssociationForm = workflowDefinition.RequiresAssociationForm,
                        RequiresInitiationForm = workflowDefinition.RequiresInitiationForm,
                        RestrictToScope = workflowDefinition.RestrictToScope,
                        RestrictToType = workflowDefinition.RestrictToType,
                        XamlPath = workflowDefinition.XamlPath

                    };

                    if (workflowDefinition.Properties?.Count > 0)
                    {
                        foreach (var property in workflowDefinition.Properties)
                        {
                            newWD.Properties.Add(property.Key, property.Value);
                        }

                    }

                    result = JsonConvert.SerializeObject(newWD, Newtonsoft.Json.Formatting.Indented);

                }

            }

            return result;

        } //GetWorkflowDefinition

        private string GetWorkflowSubscription(string workflowSubscriptionName)
        {
            string result = string.Empty;
            if (EditingTemplate?.Workflows?.WorkflowSubscriptions != null)
            {
                WorkflowSubscription workflowSubscription = EditingTemplate.Workflows.WorkflowSubscriptions
                                                                           .Find(p => p.Name.Equals(workflowSubscriptionName,
                                                                                                    StringComparison.OrdinalIgnoreCase));
                if (workflowSubscription != null)
                {
                    WorkflowSubscription newWS = new WorkflowSubscription()
                    {
                        DefinitionId = workflowSubscription.DefinitionId,
                        Enabled = workflowSubscription.Enabled,
                        EventSourceId = workflowSubscription.EventSourceId,
                        EventTypes = workflowSubscription.EventTypes,
                        ListId = workflowSubscription.ListId,
                        ManualStartBypassesActivationLimit = workflowSubscription.ManualStartBypassesActivationLimit,
                        Name = workflowSubscription.Name,
                        ParentContentTypeId = workflowSubscription.ParentContentTypeId,
                        StatusFieldName = workflowSubscription.StatusFieldName

                    };

                    if (workflowSubscription.PropertyDefinitions?.Count > 0)
                    {
                        foreach (var propertyDefinition in workflowSubscription.PropertyDefinitions)
                        {
                            newWS.PropertyDefinitions.Add(propertyDefinition.Key, propertyDefinition.Value);

                        }

                    }

                    result = JsonConvert.SerializeObject(newWS, Newtonsoft.Json.Formatting.Indented);

                }

            }

            return result;

        } //GetWorkflowSubscription

        private string GetCustomAction(CustomAction customAction)
        {
            string result = string.Empty;
            if (customAction != null)
            {
                CustomAction newCA = new CustomAction()
                {
                    CommandUIExtension = customAction.CommandUIExtension,
                    Description = customAction.Description,
                    Enabled = customAction.Enabled,
                    Group = customAction.Group,
                    ImageUrl = customAction.ImageUrl,
                    Location = customAction.Location,
                    Name = customAction.Name,
                    RegistrationId = customAction.RegistrationId,
                    RegistrationType = customAction.RegistrationType,
                    Remove = customAction.Remove,
                    Rights = customAction.Rights,
                    ScriptBlock = customAction.ScriptBlock,
                    ScriptSrc = customAction.ScriptSrc,
                    Sequence = customAction.Sequence,
                    Title = customAction.Title,
                    Url = customAction.Url

                };

                result = JsonConvert.SerializeObject(newCA, Newtonsoft.Json.Formatting.Indented);

            }

            return result;

        } //GetCustomAction

        private string GetPNPFile(PnPModel.File file)
        {
            string result = string.Empty;
            if (file != null)
            {
                PnPModel.File newF = new PnPModel.File()
                {
                    Folder = file.Folder,
                    Level = file.Level,
                    Overwrite = file.Overwrite,
                    Security = file.Security,
                    Src = file.Src

                };

                if (file.Properties?.Count > 0)
                {
                    foreach (var property in file.Properties)
                    {
                        newF.Properties.Add(property.Key, property.Value);

                    }

                }

                //Do not add Webparts here as they are handled somewhere else

                result = JsonConvert.SerializeObject(newF, Newtonsoft.Json.Formatting.Indented);

            }

            return result;

        } //GetPNPFile

        private string GetLocalization(Localization localization)
        {
            string result = string.Empty;
            if (localization != null)
            {
                Localization newL = new Localization()
                {
                    LCID = localization.LCID,
                    Name = localization.Name,
                    ResourceFile = localization.ResourceFile

                };

                result = JsonConvert.SerializeObject(newL, Newtonsoft.Json.Formatting.Indented);

            }

            return result;

        } //GetLocalization

        private string GetPageContent(Page page)
        {
            string result = string.Empty;
            if (page != null)
            {
                Page newP = new Page()
                {
                    Layout = page.Layout,
                    Overwrite = page.Overwrite,
                    Url = page.Url

                };

                if (page.Fields?.Count > 0)
                {
                    foreach (var field in page.Fields)
                    {
                        newP.Fields.Add(field.Key, field.Value);

                    }

                }

                if (page.WebParts?.Count > 0)
                {
                    newP.WebParts.AddRange(page.WebParts);

                }

                result = JsonConvert.SerializeObject(newP, Newtonsoft.Json.Formatting.Indented);

            }

            return result;

        } //GetPageContent

        private string GetPublishing(Publishing publishing)
        {
            string result = string.Empty;
            if (publishing != null)
            {
                Publishing newP = new Publishing()
                {
                    AutoCheckRequirements = publishing.AutoCheckRequirements

                };

                if (publishing.AvailableWebTemplates?.Count > 0)
                {
                    newP.AvailableWebTemplates.AddRange(publishing.AvailableWebTemplates);

                }

                if (publishing.DesignPackage != null)
                {
                    newP.DesignPackage = new DesignPackage()
                    {
                        DesignPackagePath = publishing.DesignPackage.DesignPackagePath,
                        MajorVersion = publishing.DesignPackage.MajorVersion,
                        MinorVersion = publishing.DesignPackage.MinorVersion,
                        PackageGuid = publishing.DesignPackage.PackageGuid,
                        PackageName = publishing.DesignPackage.PackageName

                    };

                }

                if (publishing.PageLayouts?.Count > 0)
                {
                    foreach (var pageLayout in publishing.PageLayouts)
                    {
                        PageLayout newPL = new PageLayout()
                        {
                            IsDefault = pageLayout.IsDefault,
                            Path = pageLayout.Path

                        };

                        newP.PageLayouts.Add(newPL);

                    }

                }

                result = JsonConvert.SerializeObject(newP, Newtonsoft.Json.Formatting.Indented);

            }

            return result;

        } //GetPublishing

        private string GetTermGroup(Guid termGroupId)
        {
            string result = string.Empty;
            if (EditingTemplate?.TermGroups != null)
            {
                TermGroup termGroup = EditingTemplate.TermGroups.Find(p => p.Id.CompareTo(termGroupId) == 0);
                if (termGroup != null)
                {
                    TermGroup newTG = new TermGroup()
                    {
                        Description = termGroup.Description,
                        Id = termGroup.Id,
                        Name = termGroup.Name,
                        SiteCollectionTermGroup = termGroup.SiteCollectionTermGroup

                    };

                    if (termGroup.Contributors?.Count > 0)
                    {
                        foreach (var user in termGroup.Contributors)
                        {
                            PnPModel.User newU = new PnPModel.User()
                            {
                                Name = user.Name

                            };

                            newTG.Contributors.Add(newU);

                        }

                    }

                    if (termGroup.Managers?.Count > 0)
                    {

                        foreach (var user in termGroup.Managers)
                        {
                            PnPModel.User newU = new PnPModel.User()
                            {
                                Name = user.Name

                            };

                            newTG.Managers.Add(newU);

                        }

                    }

                    result = JsonConvert.SerializeObject(newTG, Newtonsoft.Json.Formatting.Indented);

                }

            }

            return result;

        } //GetTermGroup

        private void SetTermsIn(TermCollection here, TermCollection terms)
        {
            if (terms?.Count > 0)
            {
                foreach (var term in terms)
                {
                    Term newT = new Term()
                    {
                        CustomSortOrder = term.CustomSortOrder,
                        Description = term.Description,
                        Id = term.Id,
                        IsAvailableForTagging = term.IsAvailableForTagging,
                        IsDeprecated = term.IsDeprecated,
                        IsReused = term.IsReused,
                        IsSourceTerm = term.IsSourceTerm,
                        Language = term.Language,
                        Name = term.Name,
                        Owner = term.Owner,
                        SourceTermId = term.SourceTermId

                    };

                    if (term.Labels?.Count > 0)
                    {
                        foreach (var label in term.Labels)
                        {
                            TermLabel newL = new TermLabel()
                            {
                                IsDefaultForLanguage = label.IsDefaultForLanguage,
                                Language = label.Language,
                                Value = label.Value

                            };

                            newT.Labels.Add(newL);

                        }

                    }

                    if (term.LocalProperties?.Count > 0)
                    {
                        foreach (var localProperty in term.LocalProperties)
                        {
                            newT.LocalProperties.Add(localProperty.Key, localProperty.Value);

                        }

                    }

                    if (term.Properties?.Count > 0)
                    {
                        foreach (var property in term.Properties)
                        {
                            newT.Properties.Add(property.Key, property.Value);

                        }

                    }

                    if (term.Terms?.Count > 0)
                    {
                        SetTermsIn(newT.Terms, term.Terms);

                    }

                    here.Add(newT);
                }

            }

        }

        private string GetTermSet(TermSet termSet)
        {
            string result = string.Empty;
            if (termSet != null)
            {
                TermSet newTS = new TermSet()
                {
                    Description = termSet.Description,
                    Id = termSet.Id,
                    IsAvailableForTagging = termSet.IsAvailableForTagging,
                    IsOpenForTermCreation = termSet.IsOpenForTermCreation,
                    Language = termSet.Language,
                    Name = termSet.Name,
                    Owner = termSet.Owner

                };

                if (termSet.Properties?.Count > 0)
                {
                    foreach (var keyValue in termSet.Properties)
                    {
                        newTS.Properties.Add(keyValue.Key, keyValue.Value);

                    }

                }

                SetTermsIn(newTS.Terms, termSet.Terms);

                result = JsonConvert.SerializeObject(newTS, Newtonsoft.Json.Formatting.Indented);

            }

            return result;

        } //GetTermSet


        private void UpdateListWithNewList(ref ListInstance oldList, ListInstance newList,
                                           PnPModel.FieldCollection fields, PnPModel.ViewCollection views)
        {
            oldList.ContentTypeBindings.Clear();
            if (newList.ContentTypeBindings?.Count > 0)
            {
                oldList.ContentTypeBindings.AddRange(newList.ContentTypeBindings);

            }

            oldList.ContentTypesEnabled = newList.ContentTypesEnabled;
            oldList.DataRows.Clear();
            if (newList.DataRows?.Count > 0)
            {
                oldList.DataRows.AddRange(newList.DataRows);

            }

            oldList.Description = newList.Description;
            oldList.DocumentTemplate = newList.DocumentTemplate;
            oldList.DraftVersionVisibility = newList.DraftVersionVisibility;
            oldList.EnableAttachments = newList.EnableAttachments;
            oldList.EnableFolderCreation = newList.EnableFolderCreation;
            oldList.EnableMinorVersions = newList.EnableMinorVersions;
            oldList.EnableModeration = newList.EnableModeration;
            oldList.EnableVersioning = newList.EnableVersioning;
            oldList.FieldDefaults.Clear();
            if (newList.FieldDefaults?.Count > 0)
            {
                foreach (var keyValue in newList.FieldDefaults)
                {
                    oldList.FieldDefaults.Add(keyValue.Key, keyValue.Value);

                }

            }

            oldList.FieldRefs.Clear();
            if (newList.FieldRefs?.Count > 0)
            {
                oldList.FieldRefs.AddRange(newList.FieldRefs);

            }

            oldList.Fields.Clear();
            if (fields.Count > 0)
            {
                oldList.Fields.AddRange(newList.Fields);

            }

            oldList.Folders.Clear();
            if (newList.Folders?.Count > 0)
            {
                oldList.Folders.AddRange(newList.Folders);

            }

            oldList.ForceCheckout = newList.ForceCheckout;
            oldList.Hidden = newList.Hidden;
            oldList.MaxVersionLimit = newList.MaxVersionLimit;
            oldList.MinorVersionLimit = newList.MinorVersionLimit;
            oldList.OnQuickLaunch = newList.OnQuickLaunch;
            oldList.RemoveExistingContentTypes = newList.RemoveExistingContentTypes;
            oldList.RemoveExistingViews = newList.RemoveExistingViews;
            oldList.Security = newList.Security;
            oldList.TemplateFeatureID = newList.TemplateFeatureID;
            oldList.TemplateType = newList.TemplateType;
            oldList.Title = newList.Title;
            oldList.Url = newList.Url;
            oldList.UserCustomActions.Clear();
            if (newList.UserCustomActions?.Count > 0)
            {
                oldList.UserCustomActions.AddRange(newList.UserCustomActions);

            }

            oldList.Views.Clear();
            if (newList.Views?.Count > 0)
            {
                oldList.Views.AddRange(newList.Views);

            }

        } //UpdateListInstance

        public void SaveTemplateForEdit(TemplateItems templateItems)
        {
            if (EditingTemplate != null)
            {
                ProvisioningTemplate template = EditingTemplate;

                if (template.AddIns?.Count > 0)
                {
                    List<TemplateItem> deletedItems = templateItems.GetDeletedItems(TemplateItemType.AddInItem);
                    if (deletedItems?.Count > 0)
                    {
                        foreach (var templateItem in deletedItems)
                        {
                            template.AddIns.RemoveAll(p => p.PackagePath.Equals(templateItem.Name,
                                                                                StringComparison.OrdinalIgnoreCase));

                            templateItems.RemoveItem(templateItem);

                        }

                    }

                    List<TemplateItem> changedItems = templateItems.GetChangedItems(TemplateItemType.AddInItem);
                    if (changedItems?.Count > 0)
                    {
                        foreach (var templateItem in changedItems)
                        {
                            AddIn addIn = template.AddIns.Find(p => p.PackagePath.Equals(templateItem.Name,
                                                                                         StringComparison.OrdinalIgnoreCase));
                            if (addIn != null)
                            {
                                addIn.Source = (string)templateItem.Content;

                            }

                            templateItems.CommitItem(templateItem);

                        }

                    }

                } //if AddIns

                if (template.ComposedLook != null)
                {
                    List<TemplateItem> composedLookTemplateItems = templateItems.GetItems(TemplateItemType.ComposedLook);
                    if (composedLookTemplateItems?.Count > 0)
                    {
                        foreach (var templateItem in composedLookTemplateItems)
                        {
                            if (templateItem.IsDeleted)
                            {
                                template.ComposedLook = null;

                                templateItems.RemoveItem(templateItem);

                            }
                            else if (templateItem.IsChanged)
                            {
                                string[] values = templateItem.Content as string[];
                                template.ComposedLook.Name = values[(int)ComposedLookProperties.Name];
                                template.ComposedLook.BackgroundFile = values[(int)ComposedLookProperties.BackgroundFile];
                                template.ComposedLook.ColorFile = values[(int)ComposedLookProperties.ColorFile];
                                template.ComposedLook.FontFile = values[(int)ComposedLookProperties.FontFile];
                                template.ComposedLook.Version = Convert.ToInt32(values[(int)ComposedLookProperties.Version]);

                                templateItems.CommitItem(templateItem);

                            }

                        }

                    }

                } //if ComposedLook

                if (template.ContentTypes?.Count > 0)
                {
                    List<TemplateItem> deletedItems = templateItems.GetDeletedItems(TemplateItemType.ContentTypeItem);
                    if (deletedItems?.Count > 0)
                    {
                        foreach (var templateItem in deletedItems)
                        {
                            template.ContentTypes.RemoveAll(p => p.Id.Equals(templateItem.Name, StringComparison.OrdinalIgnoreCase));

                            templateItems.RemoveItem(templateItem);

                        }

                    }

                    List<TemplateItem> changedItems = templateItems.GetChangedItems(TemplateItemType.ContentTypeItem);
                    if (changedItems?.Count > 0)
                    {
                        foreach (var templateItem in changedItems)
                        {
                            PnPModel.ContentType oldCT = template.ContentTypes.Find(p =>
                                                            p.Id.Equals(templateItem.Name, StringComparison.OrdinalIgnoreCase));
                            if (oldCT != null)
                            {
                                string contentType = (string)templateItem.Content;
                                PnPModel.ContentType newCT = JsonConvert.DeserializeObject<PnPModel.ContentType>(contentType);
                                template.ContentTypes.Remove(oldCT);
                                template.ContentTypes.Add(newCT);

                            }

                            templateItems.CommitItem(templateItem);

                        }

                    }

                } //if ContentTypes

                if (template.CustomActions?.SiteCustomActions?.Count > 0)
                {
                    List<TemplateItem> deletedItems = templateItems.GetDeletedItems(TemplateItemType.SiteCustomActionItem);
                    if (deletedItems?.Count > 0)
                    {
                        foreach (var templateItem in deletedItems)
                        {
                            template.CustomActions.SiteCustomActions.RemoveAll(p =>
                                p.RegistrationId.Equals(templateItem.Name, StringComparison.OrdinalIgnoreCase));

                            templateItems.RemoveItem(templateItem);

                        }

                    }

                    List<TemplateItem> changedItems = templateItems.GetChangedItems(TemplateItemType.SiteCustomActionItem);
                    if (changedItems?.Count > 0)
                    {
                        foreach (var templateItem in changedItems)
                        {
                            CustomAction oldCA = template.CustomActions.SiteCustomActions.Find(p =>
                                                    p.RegistrationId.Equals(templateItem.Name, StringComparison.OrdinalIgnoreCase));
                            if (oldCA != null)
                            {
                                string customAction = (string)templateItem.Content;
                                CustomAction newCA = JsonConvert.DeserializeObject<CustomAction>(customAction);
                                template.CustomActions.SiteCustomActions.Remove(oldCA);
                                template.CustomActions.SiteCustomActions.Add(newCA);

                            }

                            templateItems.CommitItem(templateItem);

                        }

                    }

                } //if SiteCustomActions

                if (template.CustomActions?.WebCustomActions?.Count > 0)
                {
                    List<TemplateItem> deletedItems = templateItems.GetDeletedItems(TemplateItemType.WebCustomActionItem);
                    if (deletedItems?.Count > 0)
                    {
                        foreach (var templateItem in deletedItems)
                        {
                            template.CustomActions.WebCustomActions.RemoveAll(p =>
                                p.RegistrationId.Equals(templateItem.Name, StringComparison.OrdinalIgnoreCase));

                            templateItems.RemoveItem(templateItem);

                        }

                    }

                    List<TemplateItem> changedItems = templateItems.GetChangedItems(TemplateItemType.WebCustomActionItem);
                    if (changedItems?.Count > 0)
                    {
                        foreach (var templateItem in changedItems)
                        {
                            CustomAction oldCA = template.CustomActions.WebCustomActions.Find(p =>
                                                    p.RegistrationId.Equals(templateItem.Name, StringComparison.OrdinalIgnoreCase));
                            if (oldCA != null)
                            {
                                string customAction = (string)templateItem.Content;
                                CustomAction newCA = JsonConvert.DeserializeObject<CustomAction>(customAction);
                                template.CustomActions.WebCustomActions.Remove(oldCA);
                                template.CustomActions.WebCustomActions.Add(newCA);

                            }

                            templateItems.CommitItem(templateItem);

                        }

                    }

                } //if WebCustomActions

                if (template.Features != null)
                {

                    if (template.Features.SiteFeatures?.Count > 0)
                    {
                        List<TemplateItem> siteFeatureTemplateItems = templateItems.GetItems(TemplateItemType.SiteFeatureList);
                        if (siteFeatureTemplateItems?.Count > 0)
                        {
                            foreach (var templateItem in siteFeatureTemplateItems)
                            {
                                if (templateItem.IsDeleted)
                                {
                                    template.Features.SiteFeatures.Clear();

                                    templateItems.RemoveItem(templateItem);

                                }
                                else if (templateItem.IsChanged)
                                {
                                    KeyValueList keyValueList = templateItem.Content as KeyValueList;
                                    template.Features.SiteFeatures.Clear();
                                    foreach (var keyValue in keyValueList)
                                    {
                                        PnPModel.Feature feature = new PnPModel.Feature();
                                        feature.Id = new Guid(keyValue.Value);
                                        template.Features.SiteFeatures.Add(feature);
                                        templateItems.CommitItem(templateItem);

                                    }

                                }

                            }

                        }

                    } //if SiteFeatures

                    if (template.Features.WebFeatures?.Count > 0)
                    {
                        List<TemplateItem> webFeatureTemplateItems = templateItems.GetItems(TemplateItemType.WebFeatureList);
                        if (webFeatureTemplateItems?.Count > 0)
                        {
                            foreach (var templateItem in webFeatureTemplateItems)
                            {
                                if (templateItem.IsDeleted)
                                {
                                    template.Features.WebFeatures.Clear();

                                    templateItems.RemoveItem(templateItem);

                                }
                                else if (templateItem.IsChanged)
                                {
                                    template.Features.WebFeatures.Clear();
                                    KeyValueList keyValueList = templateItem.Content as KeyValueList;
                                    foreach (var keyValue in keyValueList)
                                    {
                                        PnPModel.Feature feature = new PnPModel.Feature();
                                        feature.Id = new Guid(keyValue.Value);
                                        template.Features.WebFeatures.Add(feature);
                                        templateItems.CommitItem(templateItem);

                                    }

                                }

                            }

                        }

                    } //if WebFeatures

                    if ((template.Features.SiteFeatures.Count == 0) &&
                        (template.Features.WebFeatures.Count == 0))
                    {
                        template.Features = null;

                    }

                }

                if (template.Files?.Count > 0)
                {

                    List<TemplateItem> deletedItems = templateItems.GetDeletedItems(TemplateItemType.FileItem);
                    if (deletedItems?.Count > 0)
                    {
                        List<string> files = template.Connector.GetFiles();
                        List<string> folders = template.Connector.GetFolders();

                        foreach (var templateItem in deletedItems)
                        {
                            PnPModel.File file = template.Files.Find(p => p.Src.Equals(templateItem.Name,
                                                                                       StringComparison.OrdinalIgnoreCase));
                            if (file != null)
                            {
                                string fileName = files.EndsWith(file.Src);
                                if (string.IsNullOrWhiteSpace(fileName))
                                {
                                    fileName = file.Src;

                                }

                                template.Connector.DeleteFile(fileName);
                                template.Files.Remove(file);

                            }

                            templateItems.RemoveItem(templateItem);

                        }

                    }

                    if (template.Files?.Count > 0)
                    {
                        deletedItems = templateItems.GetDeletedItems(TemplateItemType.FileWebPartItem);
                        if (deletedItems?.Count > 0)
                        {
                            foreach (var templateItem in deletedItems)
                            {
                                TemplateItem parentItem = templateItems.GetParent(templateItem, TemplateItemType.FileItem);
                                if (parentItem != null)
                                {
                                    PnPModel.File file = template.Files.Find(p => p.Src.Equals(parentItem.Name,
                                                                                               StringComparison.OrdinalIgnoreCase));
                                    if (file != null)
                                    {
                                        string[] titles = templateItem.Name.Split(new char[] { '_' });
                                        string webPartTitle = titles[titles.Length - 1];
                                        WebPart webPart = file.WebParts.Find(p => p.Title.Equals(webPartTitle,
                                                                                                 StringComparison.OrdinalIgnoreCase));
                                        if (webPart != null)
                                        {
                                            file.WebParts.Remove(webPart);
                                            templateItems.RemoveItem(templateItem);

                                        }

                                    }

                                }

                            }

                        }

                        deletedItems = templateItems.GetDeletedItems(TemplateItemType.FileWebPartItemContent);
                        if (deletedItems?.Count > 0)
                        {
                            foreach (var templateItem in deletedItems)
                            {
                                TemplateItem parentItem = templateItems.GetParent(templateItem, TemplateItemType.FileItem);
                                if (parentItem != null)
                                {
                                    PnPModel.File file = template.Files.Find(p => p.Src.Equals(parentItem.Name,
                                                                                               StringComparison.OrdinalIgnoreCase));
                                    if (file != null)
                                    {
                                        parentItem = templateItems.GetParent(templateItem);
                                        if (parentItem != null)
                                        {
                                            string[] titles = parentItem.Name.Split(new char[] { '_' });
                                            string webPartTitle = titles[titles.Length - 1];
                                            WebPart webPart = file.WebParts.Find(p => p.Title.Equals(webPartTitle,
                                                                                                     StringComparison.OrdinalIgnoreCase));
                                            if (webPart != null)
                                            {
                                                file.WebParts.Remove(webPart);
                                                templateItems.RemoveItem(parentItem);

                                            }

                                        }

                                    }

                                }

                            }

                        }

                    }

                    List<TemplateItem> changedItems = templateItems.GetChangedItems(TemplateItemType.FileItem);
                    if (changedItems?.Count > 0)
                    {
                        foreach (var templateItem in changedItems)
                        {
                            PnPModel.File oldFile = template.Files.Find(p => p.Src.Equals(templateItem.Name,
                                                                                       StringComparison.OrdinalIgnoreCase));
                            if (oldFile != null)
                            {
                                PnPModel.File newFile = JsonConvert.DeserializeObject<PnPModel.File>((string)templateItem.Content);

                                List<TemplateItem> children = templateItems.GetChildren(templateItem.Id); //Does this file have webparts?
                                if (children?.Count > 0)
                                {
                                    WebPartCollection webParts = new WebPartCollection(EditingTemplate);
                                    foreach (TemplateItem childItem in children)
                                    {
                                        List<TemplateItem> webPartItems = templateItems.GetChildren(childItem.Id);
                                        foreach (TemplateItem webPartItem in webPartItems)
                                        {
                                            WebPart webPart = JsonConvert.DeserializeObject<WebPart>((string)webPartItem.Content);
                                            List<TemplateItem> contentItems = templateItems.GetChildren(webPartItem.Id);
                                            foreach (TemplateItem contentItem in contentItems)
                                            {
                                                XElement element = XElement.Parse((string)contentItem.Content, LoadOptions.None);
                                                webPart.Contents = element.ToString(SaveOptions.DisableFormatting);

                                            }

                                            webParts.Add(webPart);

                                        }

                                    }

                                    if (webParts.Count > 0)
                                    {
                                        newFile.WebParts.AddRange(webParts);

                                    }

                                }

                                oldFile.Folder = newFile.Folder;
                                oldFile.Level = newFile.Level;
                                oldFile.Overwrite = newFile.Overwrite;
                                oldFile.Security = newFile.Security;
                                oldFile.Src = newFile.Src;
                                oldFile.WebParts.Clear();
                                oldFile.WebParts.AddRange(newFile.WebParts);
                                oldFile.Properties.Clear();
                                foreach (var keyValue in newFile.Properties)
                                {
                                    oldFile.Properties.Add(keyValue.Key, keyValue.Value);

                                }

                            }

                            templateItems.CommitItem(templateItem);

                        }

                    } //if changedItems - files

                    changedItems = templateItems.GetChangedItems(TemplateItemType.FileWebPartItem);
                    if (changedItems?.Count > 0)
                    {
                        foreach (var templateItem in changedItems)
                        {
                            if (templateItem.IsChanged)
                            {
                                TemplateItem parentItem = templateItems.GetParent(templateItem, TemplateItemType.FileItem);
                                if (parentItem != null)
                                {
                                    PnPModel.File file = template.Files.Find(p => p.Src.Equals(parentItem.Name));
                                    if (file != null)
                                    {
                                        List<TemplateItem> children = templateItems.GetChildren(parentItem.Id);
                                        if (children?.Count > 0)
                                        {
                                            WebPartCollection webParts = new WebPartCollection(EditingTemplate);
                                            foreach (TemplateItem childItem in children)
                                            {
                                                List<TemplateItem> webPartItems = templateItems.GetChildren(childItem.Id);
                                                foreach (TemplateItem webPartItem in webPartItems)
                                                {
                                                    WebPart webPart = JsonConvert.DeserializeObject<WebPart>((string)webPartItem.Content);
                                                    List<TemplateItem> contentItems = templateItems.GetChildren(webPartItem.Id);
                                                    foreach (TemplateItem contentItem in contentItems)
                                                    {
                                                        XElement element = XElement.Parse((string)contentItem.Content,
                                                                                          LoadOptions.None);
                                                        webPart.Contents = element.ToString(SaveOptions.DisableFormatting);

                                                    }

                                                    webParts.Add(webPart);

                                                }

                                            }

                                            if (webParts.Count > 0)
                                            {
                                                file.WebParts.Clear();
                                                file.WebParts.AddRange(webParts);

                                            }

                                        }

                                    }

                                    templateItems.CommitItem(parentItem);

                                }

                            }

                        }

                    } //if changedItems 2

                    changedItems = templateItems.GetChangedItems(TemplateItemType.FileWebPartItemContent);
                    if (changedItems?.Count > 0)
                    {
                        foreach (var templateItem in changedItems)
                        {
                            if (templateItem.IsChanged)
                            {
                                TemplateItem parentItem = templateItems.GetParent(templateItem, TemplateItemType.FileItem);
                                if (parentItem != null)
                                {
                                    PnPModel.File file = template.Files.Find(p => p.Src.Equals(parentItem.Name));
                                    if (file != null)
                                    {
                                        List<TemplateItem> children = templateItems.GetChildren(parentItem.Id);
                                        if (children?.Count > 0)
                                        {
                                            WebPartCollection webParts = new WebPartCollection(EditingTemplate);
                                            foreach (TemplateItem childItem in children)
                                            {
                                                List<TemplateItem> webPartItems = templateItems.GetChildren(childItem.Id);
                                                foreach (TemplateItem webPartItem in webPartItems)
                                                {
                                                    WebPart webPart = JsonConvert.DeserializeObject<WebPart>((string)webPartItem.Content);
                                                    List<TemplateItem> contentItems = templateItems.GetChildren(webPartItem.Id);
                                                    foreach (TemplateItem contentItem in contentItems)
                                                    {
                                                        XElement element = XElement.Parse((string)contentItem.Content,
                                                                                          LoadOptions.None);
                                                        webPart.Contents = element.ToString(SaveOptions.DisableFormatting);

                                                    }

                                                    webParts.Add(webPart);

                                                }

                                            }

                                            if (webParts.Count > 0)
                                            {
                                                file.WebParts.Clear();
                                                file.WebParts.AddRange(webParts);

                                            }

                                        }

                                    }

                                    templateItems.CommitItem(parentItem);

                                }

                            }

                        }

                    } //if changedItems 3

                } //if Files


                if (template.Lists?.Count > 0)
                {
                    List<TemplateItem> deletedItems = templateItems.GetDeletedItems(TemplateItemType.ListItem);
                    if (deletedItems?.Count > 0)
                    {
                        foreach (var templateItem in deletedItems)
                        {
                            ListInstance listInstance = template.Lists.Find(p => p.Url.Equals(templateItem.Name,
                                                                                              StringComparison.OrdinalIgnoreCase));
                            if (listInstance != null)
                            {
                                template.Lists.Remove(listInstance);

                                templateItems.RemoveItem(templateItem);

                            }

                        }

                    } //if deletedItems

                    deletedItems = templateItems.GetDeletedItems(TemplateItemType.ListFieldItem);
                    if (deletedItems?.Count > 0)
                    {
                        foreach (var templateItem in deletedItems)
                        {
                            TemplateItem parentItem = templateItems.GetParent(templateItem, TemplateItemType.ListItem);
                            if (parentItem != null)
                            {
                                ListInstance listInstance = template.Lists.Find(p => p.Url.Equals(parentItem.Name,
                                                                                                  StringComparison.OrdinalIgnoreCase));
                                if (listInstance != null)
                                {
                                    foreach (var field in listInstance.Fields)
                                    {
                                        XElement element = XElement.Parse(field.SchemaXml, LoadOptions.None);
                                        string fieldName = element.Attribute("Name").Value;
                                        if (templateItem.Name.Equals(fieldName, StringComparison.OrdinalIgnoreCase))
                                        {
                                            listInstance.Fields.Remove(field);
                                            templateItems.RemoveItem(templateItem);

                                            break;

                                        }

                                    }

                                }

                            }

                        }

                    } //if deletedItems - List Fields

                    deletedItems = templateItems.GetDeletedItems(TemplateItemType.ListViewItem);
                    if (deletedItems?.Count > 0)
                    {
                        foreach (var templateItem in deletedItems)
                        {
                            TemplateItem parentItem = templateItems.GetParent(templateItem, TemplateItemType.ListItem);
                            if (parentItem != null)
                            {
                                ListInstance listInstance = template.Lists.Find(p => p.Url.Equals(parentItem.Name,
                                                                                                  StringComparison.OrdinalIgnoreCase));
                                if (listInstance != null)
                                {
                                    foreach (var view in listInstance.Views)
                                    {
                                        XElement element = XElement.Parse(view.SchemaXml, LoadOptions.None);
                                        string viewName = element.Attribute("Name").Value;
                                        if (templateItem.Name.Equals(viewName, StringComparison.OrdinalIgnoreCase))
                                        {
                                            listInstance.Views.Remove(view);
                                            templateItems.RemoveItem(templateItem);

                                            break;

                                        }

                                    }

                                }

                            }

                        }

                    } //if deletedItems - List Views

                    List<TemplateItem> changedItems = templateItems.GetChangedItems(TemplateItemType.ListItem);
                    if (changedItems?.Count > 0)
                    {
                        foreach (var templateItem in changedItems)
                        {
                            ListInstance oldList = template.Lists.Find(p => p.Url.Equals(templateItem.Name,
                                                                                         StringComparison.OrdinalIgnoreCase));
                            if (oldList != null)
                            {
                                PnPModel.FieldCollection fields = new PnPModel.FieldCollection(EditingTemplate);
                                PnPModel.ViewCollection views = new PnPModel.ViewCollection(EditingTemplate);
                                List<TemplateItem> children = templateItems.GetChildren(templateItem.Id);
                                if (children?.Count > 0)
                                {
                                    foreach (var childItem in children)
                                    {
                                        if (childItem.ItemType == TemplateItemType.ListFieldList)
                                        {
                                            List<TemplateItem> fieldItems = templateItems.GetChildren(childItem.Id);
                                            foreach (var fieldItem in fieldItems)
                                            {
                                                XElement fieldElement = XElement.Parse((string)fieldItem.Content, LoadOptions.None);
                                                PnPModel.Field field = new PnPModel.Field();
                                                field.SchemaXml = fieldElement.ToString(SaveOptions.DisableFormatting);
                                                fields.Add(field);

                                            }

                                        }
                                        else if (childItem.ItemType == TemplateItemType.ListViewList)
                                        {
                                            List<TemplateItem> viewItems = templateItems.GetChildren(childItem.Id);
                                            foreach (var viewItem in viewItems)
                                            {
                                                XElement viewElement = XElement.Parse((string)viewItem.Content, LoadOptions.None);
                                                PnPModel.View view = new PnPModel.View();
                                                view.SchemaXml = viewElement.ToString(SaveOptions.DisableFormatting);
                                                views.Add(view);

                                            }

                                        }

                                    }

                                }

                                ListInstance newList = JsonConvert.DeserializeObject<ListInstance>((string)templateItem.Content);
                                UpdateListWithNewList(ref oldList, newList, fields, views);

                                templateItems.CommitItem(templateItem);

                            }

                        }

                    } //if changedItems - List

                    changedItems = templateItems.GetChangedItems(TemplateItemType.ListFieldItem);
                    if (changedItems?.Count > 0)
                    {
                        foreach (var templateItem in changedItems)
                        {
                            TemplateItem parentItem = templateItems.GetParent(templateItem, TemplateItemType.ListItem);
                            if (parentItem != null)
                            {
                                ListInstance oldList = template.Lists.Find(p => p.Url.Equals(parentItem.Name,
                                                                                             StringComparison.OrdinalIgnoreCase));
                                if (oldList != null)
                                {
                                    foreach (var field in oldList.Fields)
                                    {
                                        XElement fieldElement = XElement.Parse(field.SchemaXml, LoadOptions.None);
                                        string fieldID = fieldElement.Attribute("ID").Value;
                                        if (templateItem.Name.Equals(fieldID, StringComparison.OrdinalIgnoreCase))
                                        {
                                            XElement newElement = XElement.Parse((string)templateItem.Content, LoadOptions.None);
                                            field.SchemaXml = newElement.ToString(SaveOptions.DisableFormatting);

                                            break;

                                        }

                                    }

                                }

                            }

                            templateItems.CommitItem(templateItem);

                        }

                    } //if changedItems - List Fields

                    changedItems = templateItems.GetChangedItems(TemplateItemType.ListViewItem);
                    if (changedItems?.Count > 0)
                    {
                        foreach (var templateItem in changedItems)
                        {
                            TemplateItem parentItem = templateItems.GetParent(templateItem, TemplateItemType.ListItem);
                            if (parentItem != null)
                            {
                                ListInstance oldList = template.Lists.Find(p => p.Url.Equals(parentItem.Name,
                                                                                             StringComparison.OrdinalIgnoreCase));
                                if (oldList != null)
                                {
                                    foreach (var view in oldList.Views)
                                    {
                                        XElement viewElement = XElement.Parse(view.SchemaXml, LoadOptions.None);
                                        string viewName = viewElement.Attribute("Name").Value;
                                        if (templateItem.Name.Equals(viewName, StringComparison.OrdinalIgnoreCase))
                                        {
                                            XElement newElement = XElement.Parse((string)templateItem.Content, LoadOptions.None);
                                            view.SchemaXml = newElement.ToString(SaveOptions.DisableFormatting);

                                            break;

                                        }

                                    }

                                }

                            }

                            templateItems.CommitItem(templateItem);

                        }

                    } //if changedItems - List Views

                } //if Lists

                if (template.Localizations?.Count > 0)
                {
                    List<TemplateItem> deletedItems = templateItems.GetDeletedItems(TemplateItemType.LocalizationsItem);
                    if (deletedItems?.Count > 0)
                    {
                        foreach (var templateItem in deletedItems)
                        {
                            int lcid = int.Parse(templateItem.Name);
                            Localization oldLocalization = template.Localizations.Find(p => p.LCID == lcid);
                            if (oldLocalization != null)
                            {
                                template.Localizations.Remove(oldLocalization);

                            }

                            templateItems.RemoveItem(templateItem);

                        }

                    } //if deletedItems

                    List<TemplateItem> changedItems = templateItems.GetChangedItems(TemplateItemType.LocalizationsItem);
                    if (changedItems?.Count > 0)
                    {
                        foreach (var templateItem in changedItems)
                        {
                            int lcid = int.Parse(templateItem.Name);
                            Localization oldLocalization = template.Localizations.Find(p => p.LCID == lcid);
                            if (oldLocalization != null)
                            {
                                Localization newLocalization = JsonConvert.DeserializeObject<Localization>((string)templateItem.Content);
                                oldLocalization.LCID = newLocalization.LCID;
                                oldLocalization.Name = newLocalization.Name;
                                oldLocalization.ResourceFile = newLocalization.ResourceFile;

                            }

                            templateItems.CommitItem(templateItem);

                        }

                    } //if changedItems


                } //if Localizations

                if (template.Pages?.Count > 0)
                {
                    List<TemplateItem> deletedItems = templateItems.GetDeletedItems(TemplateItemType.PageItem);
                    if (deletedItems?.Count > 0)
                    {
                        foreach (var templateItem in deletedItems)
                        {
                            Page oldPage = template.Pages.Find(p => p.Url.Equals(templateItem.Name,
                                                                                 StringComparison.OrdinalIgnoreCase));
                            if (oldPage != null)
                            {
                                template.Pages.Remove(oldPage);

                                templateItems.RemoveItem(templateItem);

                            }

                        }

                    } //if deletedItems

                    List<TemplateItem> changedItems = templateItems.GetChangedItems(TemplateItemType.PageItem);
                    if (changedItems?.Count > 0)
                    {
                        foreach (var templateItem in changedItems)
                        {
                            Page oldPage = template.Pages.Find(p => p.Url.Equals(templateItem.Name,
                                                                                 StringComparison.OrdinalIgnoreCase));
                            if (oldPage != null)
                            {
                                Page newPage = JsonConvert.DeserializeObject<Page>((string)templateItem.Content);
                                oldPage.Fields.Clear();
                                if (newPage.Fields?.Count > 0)
                                {
                                    foreach (var field in newPage.Fields)
                                    {
                                        oldPage.Fields.Add(field.Key, field.Value);

                                    }

                                }

                                oldPage.Layout = newPage.Layout;
                                oldPage.Overwrite = newPage.Overwrite;
                                if (newPage.Security != null)
                                {
                                    if (oldPage.Security != null)
                                    {
                                        oldPage.Security.ClearSubscopes = newPage.Security.ClearSubscopes;
                                        oldPage.Security.CopyRoleAssignments = newPage.Security.CopyRoleAssignments;
                                        oldPage.Security.RoleAssignments.Clear();
                                        oldPage.Security.RoleAssignments.AddRange(newPage.Security.RoleAssignments);

                                    }

                                }

                                oldPage.Url = newPage.Url;
                                oldPage.WebParts.Clear();
                                if (newPage.WebParts?.Count > 0)
                                {
                                    oldPage.WebParts.AddRange(newPage.WebParts);

                                }

                            }

                            templateItems.CommitItem(templateItem);

                        }

                    } //if changedItems

                } //if Pages

                if (template.Properties?.Count > 0)
                {
                    List<TemplateItem> deletedItems = templateItems.GetDeletedItems(TemplateItemType.PropertiesList);
                    if (deletedItems?.Count > 0)
                    {
                        template.Properties.Clear();
                        foreach (var templateItem in deletedItems) //should only be one
                        {
                            templateItems.RemoveItem(templateItem);

                        }

                    } //if deletedItems

                    List<TemplateItem> changedItems = templateItems.GetChangedItems(TemplateItemType.PropertiesList);
                    if (changedItems?.Count > 0)
                    {
                        template.Properties.Clear();
                        foreach (var templateItem in changedItems)
                        {
                            KeyValueList keyValueList = templateItem.Content as KeyValueList;
                            foreach (var keyValue in keyValueList)
                            {
                                template.Properties.Add(keyValue.Key, keyValue.Value);

                            }

                            templateItems.CommitItem(templateItem);

                        }

                    } //if changedItems

                } //if Parameters

                if (template.PropertyBagEntries?.Count > 0)
                {
                    List<TemplateItem> deletedItems = templateItems.GetDeletedItems(TemplateItemType.PropertyBagEntriesList);
                    if (deletedItems?.Count > 0)
                    {
                        template.PropertyBagEntries.Clear();
                        foreach (var templateItem in deletedItems) //should only be one
                        {
                            templateItems.RemoveItem(templateItem);

                        }

                    } //if deletedItems

                    List<TemplateItem> changedItems = templateItems.GetChangedItems(TemplateItemType.PropertyBagEntriesList);
                    if (changedItems?.Count > 0)
                    {
                        template.PropertyBagEntries.Clear();
                        foreach (var templateItem in changedItems)
                        {
                            KeyValueList keyValueList = templateItem.Content as KeyValueList;
                            foreach (var keyValue in keyValueList)
                            {
                                PropertyBagEntry propertyBagEntry = new PropertyBagEntry()
                                {
                                    Key = keyValue.Key,
                                    Value = keyValue.Value

                                };

                                template.PropertyBagEntries.Add(propertyBagEntry);

                            }

                            templateItems.CommitItem(templateItem);

                        }

                    } //if changedItems

                } //if PropertyBagEntries

                if (template.Publishing != null)
                {
                    List<TemplateItem> deletedItems = templateItems.GetDeletedItems(TemplateItemType.PublishingList);
                    if (deletedItems?.Count > 0)
                    {
                        template.Publishing = null;
                        foreach (var templateItem in deletedItems) //should only be one
                        {
                            templateItems.RemoveItem(templateItem);

                        }

                    } //if deletedItems

                    List<TemplateItem> changedItems = templateItems.GetChangedItems(TemplateItemType.PublishingList);
                    if (changedItems?.Count > 0)
                    {
                        Publishing oldPublishing = template.Publishing;
                        foreach (var templateItem in changedItems) //should only be one
                        {
                            Publishing newPublishing = JsonConvert.DeserializeObject<Publishing>((string)templateItem.Content);
                            oldPublishing.AutoCheckRequirements = newPublishing.AutoCheckRequirements;
                            oldPublishing.AvailableWebTemplates.Clear();
                            if (newPublishing.AvailableWebTemplates?.Count > 0)
                            {
                                oldPublishing.AvailableWebTemplates.AddRange(newPublishing.AvailableWebTemplates);

                            }

                            oldPublishing.DesignPackage = newPublishing.DesignPackage;
                            oldPublishing.PageLayouts.Clear();
                            if (newPublishing.PageLayouts?.Count > 0)
                            {
                                oldPublishing.PageLayouts.AddRange(newPublishing.PageLayouts);

                            }

                            templateItems.CommitItem(templateItem);

                        }

                    } //if changedItems

                } //if Publishing


                if (template.Security != null)
                {
                    List<TemplateItem> securityItems = templateItems.GetItems(TemplateItemType.SecurityItem);
                    if (securityItems?.Count > 0)
                    {
                        foreach (var templateItem in securityItems)
                        {
                            if (templateItem.IsDeleted)
                            {
                                template.Security = null;
                                templateItems.RemoveItem(templateItem);

                            }
                            else if (templateItem.IsEmpty)
                            {
                                template.Security = null;
                                templateItems.RemoveItem(templateItem);

                            }
                            else if (templateItem.IsChanged)
                            {
                                SiteSecurity newSecurity = JsonConvert.DeserializeObject<SiteSecurity>((string)templateItem.Content);
                                if (newSecurity != null)
                                {
                                    template.Security.AdditionalAdministrators.Clear();
                                    if (newSecurity.AdditionalAdministrators?.Count > 0)
                                    {
                                        template.Security.AdditionalAdministrators.AddRange(newSecurity.AdditionalAdministrators);

                                    }

                                    template.Security.AdditionalMembers.Clear();
                                    if (newSecurity.AdditionalMembers?.Count > 0)
                                    {
                                        template.Security.AdditionalMembers.AddRange(newSecurity.AdditionalMembers);

                                    }

                                    template.Security.AdditionalOwners.Clear();
                                    if (newSecurity.AdditionalOwners?.Count > 0)
                                    {
                                        template.Security.AdditionalOwners.AddRange(newSecurity.AdditionalOwners);

                                    }

                                    template.Security.AdditionalVisitors.Clear();
                                    if (newSecurity.AdditionalVisitors?.Count > 0)
                                    {
                                        template.Security.AdditionalVisitors.AddRange(newSecurity.AdditionalVisitors);

                                    }

                                    template.Security.BreakRoleInheritance = newSecurity.BreakRoleInheritance;
                                    template.Security.ClearSubscopes = newSecurity.ClearSubscopes;
                                    template.Security.CopyRoleAssignments = newSecurity.CopyRoleAssignments;
                                    template.Security.SiteGroups.Clear();
                                    if (newSecurity.SiteGroups?.Count > 0)
                                    {
                                        template.Security.SiteGroups.AddRange(newSecurity.SiteGroups);

                                    }

                                    template.Security.SiteSecurityPermissions.RoleAssignments.Clear();
                                    if (newSecurity.SiteSecurityPermissions?.RoleAssignments?.Count > 0)
                                    {
                                        template.Security.SiteSecurityPermissions.RoleAssignments.AddRange(newSecurity.SiteSecurityPermissions.RoleAssignments);

                                    }

                                    template.Security.SiteSecurityPermissions.RoleDefinitions.Clear();
                                    if (newSecurity.SiteSecurityPermissions?.RoleDefinitions?.Count > 0)
                                    {
                                        template.Security.SiteSecurityPermissions.RoleDefinitions.AddRange(newSecurity.SiteSecurityPermissions.RoleDefinitions);

                                    }

                                    templateItems.CommitItem(templateItem);

                                } //if newSecurity

                            } //if IsChanged

                        }

                    }

                } //if Security

                if (template.SupportedUILanguages?.Count > 0)
                {
                    List<TemplateItem> deletedItems = templateItems.GetDeletedItems(TemplateItemType.SupportedUILanguagesList);
                    if (deletedItems?.Count > 0)
                    {
                        template.SupportedUILanguages.Clear();
                        foreach (var templateItem in deletedItems) //should only be one
                        {
                            templateItems.RemoveItem(templateItem);

                        }

                    } //if deletedItems

                    List<TemplateItem> changedItems = templateItems.GetChangedItems(TemplateItemType.SupportedUILanguagesList);
                    if (changedItems?.Count > 0)
                    {
                        template.SupportedUILanguages.Clear();
                        foreach (var templateItem in changedItems)
                        {
                            KeyValueList keyValueList = templateItem.Content as KeyValueList;
                            foreach (var keyValue in keyValueList)
                            {
                                SupportedUILanguage newSupportedUILanguage = new SupportedUILanguage()
                                {
                                    LCID = int.Parse(keyValue.Value)

                                };

                                template.SupportedUILanguages.Add(newSupportedUILanguage);

                            }

                            templateItems.CommitItem(templateItem);

                        }

                    } //if changedItems

                } //if SupportedUILanguages

                if (template.RegionalSettings != null)
                {
                    List<TemplateItem> deletedItems = templateItems.GetDeletedItems(TemplateItemType.RegionalSetting);
                    if (deletedItems?.Count > 0)
                    {
                        template.RegionalSettings = null;
                        foreach (var templateItem in deletedItems)
                        {
                            templateItems.RemoveItem(templateItem);

                        }

                    } //if deletedItems

                    List<TemplateItem> changedItems = templateItems.GetChangedItems(TemplateItemType.RegionalSetting);
                    if (changedItems?.Count > 0)
                    {
                        PnPModel.RegionalSettings oldRegionalSettings = template.RegionalSettings;
                        foreach (var templateItem in changedItems) //should only be one
                        {
                            int[] newRegionalSettings = (int[])templateItem.Content;
                            oldRegionalSettings.AdjustHijriDays = newRegionalSettings[(int)RegionalSettingProperties.AdjustHijriDays];
                            oldRegionalSettings.AlternateCalendarType = (CalendarType)newRegionalSettings[(int)RegionalSettingProperties.AlternateCalendarType];
                            oldRegionalSettings.CalendarType = (CalendarType)newRegionalSettings[(int)RegionalSettingProperties.CalendarType];
                            oldRegionalSettings.Collation = newRegionalSettings[(int)RegionalSettingProperties.Collation];
                            oldRegionalSettings.FirstDayOfWeek = (DayOfWeek)newRegionalSettings[(int)RegionalSettingProperties.FirstDayOfWeek];
                            oldRegionalSettings.FirstWeekOfYear = newRegionalSettings[(int)RegionalSettingProperties.FirstWeekOfYear];
                            oldRegionalSettings.LocaleId = newRegionalSettings[(int)RegionalSettingProperties.LocaleId];
                            oldRegionalSettings.ShowWeeks = (newRegionalSettings[(int)RegionalSettingProperties.ShowWeeks] == 1 ? true : false);
                            oldRegionalSettings.Time24 = (newRegionalSettings[(int)RegionalSettingProperties.Time24] == 1 ? true : false);
                            oldRegionalSettings.TimeZone = newRegionalSettings[(int)RegionalSettingProperties.TimeZone];
                            oldRegionalSettings.WorkDayEndHour = (WorkHour)newRegionalSettings[(int)RegionalSettingProperties.WorkDayEndHour];
                            oldRegionalSettings.WorkDays = newRegionalSettings[(int)RegionalSettingProperties.WorkDays];
                            oldRegionalSettings.WorkDayStartHour = (WorkHour)newRegionalSettings[(int)RegionalSettingProperties.WorkDayStartHour];

                            templateItems.CommitItem(templateItem);

                        }

                    } //if changedItems

                } //if RegionalSettings


                if (template.SiteFields?.Count > 0)
                {
                    List<TemplateItem> deletedItems = templateItems.GetDeletedItems(TemplateItemType.SiteFieldItem);
                    if (deletedItems?.Count > 0)
                    {
                        List<PnPModel.Field> fieldsToDelete = new List<PnPModel.Field>();
                        for (var i = 0; i < template.SiteFields.Count; i++)
                        {
                            var siteField = template.SiteFields[i];
                            XElement fieldElement = XElement.Parse(siteField.SchemaXml);
                            string fieldID = fieldElement.Attribute("ID").Value;
                            TemplateItem templateItem = deletedItems.GetTemplateItemByName(fieldID);
                            if (templateItem != null)
                            {
                                fieldsToDelete.Add(siteField);
                                templateItems.RemoveItem(templateItem);

                            }

                        }
                        if (fieldsToDelete.Count > 0)
                        {
                            foreach (var field in fieldsToDelete)
                            {
                                template.SiteFields.Remove(field);

                            }

                        }

                    } //if deletedItems

                    List<TemplateItem> changedItems = templateItems.GetChangedItems(TemplateItemType.SiteFieldItem);
                    if (changedItems?.Count > 0)
                    {
                        foreach (var siteField in template.SiteFields)
                        {
                            XElement oldElement = XElement.Parse(siteField.SchemaXml);
                            string fieldID = oldElement.Attribute("ID").Value;
                            TemplateItem templateItem = changedItems.GetTemplateItemByName(fieldID);
                            if (templateItem != null)
                            {
                                XElement newElement = XElement.Parse((string)templateItem.Content, LoadOptions.None);
                                siteField.SchemaXml = newElement.ToString(SaveOptions.DisableFormatting);
                                templateItems.CommitItem(templateItem);

                            }

                        }

                    } //if changedItems

                } //if SiteFields

                if (template.TermGroups?.Count > 0)
                {
                    List<TemplateItem> deletedItems = templateItems.GetDeletedItems(TemplateItemType.TermGroupItem);
                    if (deletedItems?.Count > 0)
                    {
                        foreach (var templateItem in deletedItems)
                        {
                            TermGroup termGroup = template.TermGroups.Find(p => p.Id.Equals(Guid.Parse(templateItem.Name)));
                            if (termGroup != null)
                            {
                                template.TermGroups.Remove(termGroup);

                            }

                            templateItems.RemoveItem(templateItem);

                        }

                    } //if deletedItems - TermGroups

                    List<TemplateItem> changedItems = templateItems.GetChangedItems(TemplateItemType.TermGroupItem);
                    if (changedItems?.Count > 0)
                    {
                        foreach (var templateItem in changedItems)
                        {
                            TermGroup oldTermGroup = template.TermGroups.Find(p => p.Id.Equals(Guid.Parse(templateItem.Name)));
                            if (oldTermGroup != null)
                            {
                                TermGroup newTermGroup = JsonConvert.DeserializeObject<TermGroup>((string)templateItem.Content);

                                if (oldTermGroup.TermSets?.Count > 0)
                                {
                                    List<TemplateItem> childTermGroup = templateItems.GetChildren(templateItem.Id);
                                    if (childTermGroup?.Count > 0)
                                    {
                                        foreach (var childItem in childTermGroup)
                                        {
                                            List<TemplateItem> termSets = templateItems.GetChildren(childItem.Id);
                                            if (termSets?.Count > 0)
                                            {
                                                foreach (var termSet in termSets)
                                                {
                                                    TermSet newTermSet = JsonConvert.DeserializeObject<TermSet>((string)termSet.Content);
                                                    oldTermGroup.TermSets.Clear();
                                                    oldTermGroup.TermSets.Add(newTermSet);

                                                }

                                            }

                                        }

                                    }

                                }

                                oldTermGroup.Contributors.Clear();
                                if (newTermGroup.Contributors?.Count > 0)
                                {
                                    oldTermGroup.Contributors.AddRange(newTermGroup.Contributors);

                                }

                                oldTermGroup.Description = newTermGroup.Description;
                                oldTermGroup.Id = newTermGroup.Id;
                                oldTermGroup.Managers.Clear();
                                if (newTermGroup.Managers?.Count > 0)
                                {
                                    oldTermGroup.Managers.AddRange(newTermGroup.Managers);

                                }

                                oldTermGroup.Name = newTermGroup.Name;
                                oldTermGroup.SiteCollectionTermGroup = newTermGroup.SiteCollectionTermGroup;

                            }

                            templateItems.CommitItem(templateItem);

                        }

                    } //if changedItems - TermGroups

                    deletedItems = templateItems.GetDeletedItems(TemplateItemType.TermSetItem);
                    if (deletedItems?.Count > 0)
                    {
                        foreach (var templateItem in deletedItems)
                        {
                            TemplateItem parentItem = templateItems.GetParent(templateItem, TemplateItemType.TermGroupItem);
                            if (parentItem != null)
                            {
                                TermGroup oldTermGroup = template.TermGroups.Find(p => p.Id.Equals(Guid.Parse(parentItem.Name)));
                                if (oldTermGroup != null)
                                {
                                    TermSet oldTermSet = oldTermGroup.TermSets.Find(p => p.Id.Equals(Guid.Parse(templateItem.Name)));
                                    if (oldTermSet != null)
                                    {
                                        oldTermGroup.TermSets.Remove(oldTermSet);

                                    }

                                }

                            }

                            templateItems.RemoveItem(templateItem);

                        }

                    } //if deletedItems - TermSets

                    changedItems = templateItems.GetChangedItems(TemplateItemType.TermSetItem);
                    if (changedItems?.Count > 0)
                    {
                        foreach (var templateItem in changedItems)
                        {
                            TemplateItem parentItem = templateItems.GetParent(templateItem, TemplateItemType.TermGroupItem);
                            if (parentItem != null)
                            {
                                TermGroup oldTermGroup = template.TermGroups.Find(p => p.Id.Equals(Guid.Parse(parentItem.Name)));
                                if (oldTermGroup != null)
                                {
                                    TermSet oldTermSet = oldTermGroup.TermSets.Find(p => p.Id.Equals(Guid.Parse(templateItem.Name)));
                                    if (oldTermSet != null)
                                    {
                                        TermSet newTermSet = JsonConvert.DeserializeObject<TermSet>((string)templateItem.Content);

                                        oldTermGroup.TermSets.Remove(oldTermSet);
                                        oldTermGroup.TermSets.Add(newTermSet);

                                    }

                                }

                            }

                            templateItems.CommitItem(templateItem);

                        }

                    } //if changedItems = TermSets

                } //if TermGroups

                if (template.WebSettings != null)
                {
                    List<TemplateItem> deletedItems = templateItems.GetDeletedItems(TemplateItemType.WebSetting);
                    if (deletedItems?.Count > 0)
                    {
                        template.WebSettings = null;
                        foreach (var templateItem in deletedItems) //should only be one
                        {
                            templateItems.RemoveItem(templateItem);

                        }

                    } //if deletedItems

                    List<TemplateItem> changedItems = templateItems.GetChangedItems(TemplateItemType.WebSetting);
                    if (changedItems?.Count > 0)
                    {
                        WebSettings oldWebSettings = template.WebSettings;
                        foreach (var templateItem in changedItems)
                        {
                            string[] newWebSettings = (string[])templateItem.Content;
                            if (newWebSettings?.Length > 0)
                            {
                                oldWebSettings.AlternateCSS = newWebSettings[(int)WebSettingProperties.AlternateCSS];
                                oldWebSettings.CustomMasterPageUrl = newWebSettings[(int)WebSettingProperties.CustomMasterPageUrl];
                                oldWebSettings.Description = newWebSettings[(int)WebSettingProperties.Description];
                                oldWebSettings.MasterPageUrl = newWebSettings[(int)WebSettingProperties.MasterPageUrl];
                                oldWebSettings.NoCrawl = (newWebSettings[(int)WebSettingProperties.NoCrawl].Equals("1", StringComparison.Ordinal) ? true : false);
                                oldWebSettings.RequestAccessEmail = newWebSettings[(int)WebSettingProperties.RequestAccessEmail];
                                oldWebSettings.SiteLogo = newWebSettings[(int)WebSettingProperties.SiteLogo];
                                oldWebSettings.Title = newWebSettings[(int)WebSettingProperties.Title];
                                oldWebSettings.WelcomePage = newWebSettings[(int)WebSettingProperties.WelcomePage];

                            }

                            templateItems.CommitItem(templateItem);

                        }

                    } //if changedItems

                } //if WebSettings

                if (template.Workflows?.WorkflowDefinitions?.Count > 0)
                {
                    List<TemplateItem> deletedItems = templateItems.GetDeletedItems(TemplateItemType.WorkflowDefinitionItem);
                    if (deletedItems?.Count > 0)
                    {
                        foreach (var templateItem in deletedItems)
                        {
                            WorkflowDefinition oldWorlkflowDefinition = template.Workflows.WorkflowDefinitions.Find(p => p.Id.Equals(Guid.Parse(templateItem.Name)));
                            if (oldWorlkflowDefinition != null)
                            {
                                template.Connector.DeleteFile(oldWorlkflowDefinition.XamlPath);
                                template.Workflows.WorkflowDefinitions.Remove(oldWorlkflowDefinition);
                            }

                            templateItems.RemoveItem(templateItem);

                        }

                    } //if deletedItems

                    List<TemplateItem> changedItems = templateItems.GetChangedItems(TemplateItemType.WorkflowDefinitionItem);
                    if (changedItems?.Count > 0)
                    {
                        foreach (var templateItem in changedItems)
                        {
                            WorkflowDefinition oldWorlkflowDefinition = template.Workflows.WorkflowDefinitions.Find(p => p.Id.Equals(Guid.Parse(templateItem.Name)));
                            if (oldWorlkflowDefinition != null)
                            {
                                WorkflowDefinition newWorkflowDefinition = JsonConvert.DeserializeObject<WorkflowDefinition>((string)templateItem.Content);
                                if (newWorkflowDefinition != null)
                                {
                                    oldWorlkflowDefinition.AssociationUrl = newWorkflowDefinition.AssociationUrl;
                                    oldWorlkflowDefinition.Description = newWorkflowDefinition.Description;
                                    oldWorlkflowDefinition.DisplayName = newWorkflowDefinition.DisplayName;
                                    oldWorlkflowDefinition.DraftVersion = newWorkflowDefinition.DraftVersion;
                                    oldWorlkflowDefinition.FormField = newWorkflowDefinition.FormField;
                                    oldWorlkflowDefinition.Id = newWorkflowDefinition.Id;
                                    oldWorlkflowDefinition.InitiationUrl = newWorkflowDefinition.InitiationUrl;
                                    oldWorlkflowDefinition.Properties.Clear();
                                    if (newWorkflowDefinition.Properties?.Count > 0)
                                    {
                                        foreach (var keyValue in newWorkflowDefinition.Properties)
                                        {
                                            oldWorlkflowDefinition.Properties.Add(keyValue.Key, keyValue.Value);

                                        }

                                    }

                                    oldWorlkflowDefinition.Published = newWorkflowDefinition.Published;
                                    oldWorlkflowDefinition.RequiresAssociationForm = newWorkflowDefinition.RequiresAssociationForm;
                                    oldWorlkflowDefinition.RequiresInitiationForm = newWorkflowDefinition.RequiresInitiationForm;
                                    oldWorlkflowDefinition.RestrictToScope = newWorkflowDefinition.RestrictToScope;
                                    oldWorlkflowDefinition.RestrictToType = newWorkflowDefinition.RestrictToType;

                                }

                            }

                            templateItems.CommitItem(templateItem);

                        }

                    } //if changedItems

                } //if WorkflowDefinitions

                if (template.Workflows?.WorkflowSubscriptions?.Count > 0)
                {
                    List<TemplateItem> deletedItems = templateItems.GetDeletedItems(TemplateItemType.WorkflowSubscriptionItem);
                    if (deletedItems?.Count > 0)
                    {
                        foreach (var templateItem in deletedItems)
                        {
                            WorkflowSubscription oldWorkflowSubscription = template.Workflows.WorkflowSubscriptions.Find(p =>
                                                                            p.Name.Equals(templateItem.Name, StringComparison.OrdinalIgnoreCase));
                            if (oldWorkflowSubscription != null)
                            {
                                template.Workflows.WorkflowSubscriptions.Remove(oldWorkflowSubscription);

                            }

                            templateItems.RemoveItem(templateItem);

                        }

                    } //if deletedItems

                    List<TemplateItem> changedItems = templateItems.GetChangedItems(TemplateItemType.WorkflowSubscriptionItem);
                    if (changedItems?.Count > 0)
                    {
                        foreach (var templateItem in deletedItems)
                        {
                            WorkflowSubscription oldWorkflowSubscription = template.Workflows.WorkflowSubscriptions.Find(p =>
                                                                            p.Name.Equals(templateItem.Name, StringComparison.OrdinalIgnoreCase));
                            if (oldWorkflowSubscription != null)
                            {
                                WorkflowSubscription newWorkflowSubscription = JsonConvert.DeserializeObject<WorkflowSubscription>((string)templateItem.Content);
                                if (newWorkflowSubscription != null)
                                {
                                    oldWorkflowSubscription.DefinitionId = newWorkflowSubscription.DefinitionId;
                                    oldWorkflowSubscription.Enabled = newWorkflowSubscription.Enabled;
                                    oldWorkflowSubscription.EventSourceId = newWorkflowSubscription.EventSourceId;
                                    oldWorkflowSubscription.EventTypes.Clear();
                                    if (newWorkflowSubscription.EventTypes?.Count > 0)
                                    {
                                        oldWorkflowSubscription.EventTypes.AddRange(newWorkflowSubscription.EventTypes);

                                    }

                                    oldWorkflowSubscription.ListId = newWorkflowSubscription.ListId;
                                    oldWorkflowSubscription.ManualStartBypassesActivationLimit = newWorkflowSubscription.ManualStartBypassesActivationLimit;
                                    oldWorkflowSubscription.Name = newWorkflowSubscription.Name;
                                    oldWorkflowSubscription.ParentContentTypeId = newWorkflowSubscription.ParentContentTypeId;
                                    oldWorkflowSubscription.PropertyDefinitions.Clear();
                                    if (newWorkflowSubscription.PropertyDefinitions?.Count > 0)
                                    {
                                        foreach (var keyValue in newWorkflowSubscription.PropertyDefinitions)
                                        {
                                            oldWorkflowSubscription.PropertyDefinitions.Add(keyValue.Key, keyValue.Value);

                                        }

                                    }

                                    oldWorkflowSubscription.StatusFieldName = newWorkflowSubscription.StatusFieldName;

                                }

                            }

                            templateItems.CommitItem(templateItem);

                        }

                    } //if changedItems

                } //if WorkflowSubscriptions

                XMLTemplateProvider provider = EditingProvider;

                if (provider == null)
                {

                    string xmlFileName = Path.GetFileName(templateItems.TemplateFilename);
                    xmlFileName = Path.ChangeExtension(xmlFileName, ".xml");

                    OpenXMLConnector xmlConnector = template.Connector as OpenXMLConnector;

                    provider = new XMLOpenXMLTemplateProvider(xmlConnector);
                    provider.Uri = xmlFileName;

                }

                provider.Save(template);

            } //if EditingTemplate

        } //SaveTemplateForEdit

    } //SharePoint2016Online

} //namespace
