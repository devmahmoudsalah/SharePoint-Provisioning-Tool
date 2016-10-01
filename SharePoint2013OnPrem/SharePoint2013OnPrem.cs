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
using SPClient = Microsoft.SharePoint.Client;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Xml;
using System.Xml.Linq;
using System.Web.Script.Serialization;

namespace Karabina.SharePoint.Provisioning
{
    public class SharePoint2013OnPrem
    {
        public SharePoint2013OnPrem()
        {
            //do nothing
        }

        private ProvisioningTemplate _editingTemplate = null;
        private ListBox _lbOutput = null;

        public ProvisioningTemplate EditingTemplate
        {
            get { return _editingTemplate; }
            set { _editingTemplate = value; }
        }

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

        private Dictionary<string, string> GetItemFieldValues(ListItem item, ProvisioningFieldCollection fieldCollection, SPClient.FieldCollection fields)
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
                                                sb.Append("#;");
                                            }
                                            sb.Append($"{lookupValues[i].LookupId}#;{lookupValues[i].LookupValue}");
                                        }
                                        data.Add(field.Name, sb.ToString());
                                    }
                                    else
                                    {
                                        //No, get the field id and value
                                        FieldLookupValue lookupValue = value as FieldLookupValue;
                                        data.Add(field.Name, $"{lookupValue.LookupId}#;{lookupValue.LookupValue}");
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
                                                sb.Append("#;");
                                            }
                                            sb.Append($"{userValues[i].LookupId}#;{userValues[i].LookupValue}");
                                        }
                                        data.Add(field.Name, sb.ToString());
                                    }
                                    else
                                    {
                                        //No, get the user id and value
                                        FieldUserValue userValue = value as FieldUserValue;
                                        data.Add(field.Name, $"{userValue.LookupId}#;{userValue.LookupValue}");
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
                                data.Add(field.Name, guid.ToString("N"));
                                break;
                            case ProvisioningFieldType.DateTime:
                                //Field is date time, save in ISO format
                                DateTime dateTime = Convert.ToDateTime(value);
                                data.Add(field.Name, dateTime.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"));
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
        }


        private void GetItemFieldValues(ListItem item, ProvisioningFieldCollection fieldCollection, SPClient.FieldCollection fields, Dictionary<string, string> properties)
        {
            Dictionary<string, string> data = GetItemFieldValues(item, fieldCollection, fields);
            if (data.Count > 0)
            {
                foreach (KeyValuePair<string, string> keyValue in data)
                {
                    properties.Add(keyValue.Key, keyValue.Value);
                }
            }
        }


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

            WriteMessage($"Info: Saving items from {listInstance.Title}");

            int itemCount = 0;

            if (listItems.Count > 0)
            {
                ProvisioningFieldCollection fieldCollection = new ProvisioningFieldCollection();

                //Get only the fields we need.
                foreach (Microsoft.SharePoint.Client.Field field in fields)
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

            WriteMessage($"Info: {itemCount} items saved");
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

        private void CleanupTemplate(ProvisioningOptions provisioningOptions,
                                     ProvisioningTemplate template,
                                     ProvisioningTemplate baseTemplate)
        {
            int total = 0;
            WriteMessage($"Info: Start performing {baseTemplate.BaseSiteTemplate} template clean up");

            if (provisioningOptions.IncludeCustomActions)
            {
                if ((baseTemplate.CustomActions != null) &&
                    (template.CustomActions != null))
                {
                    if ((baseTemplate.CustomActions.SiteCustomActions != null) &&
                        (template.CustomActions.SiteCustomActions != null))
                    {
                        total = template.CustomActions.SiteCustomActions.Count;
                        WriteMessage("Cleanup: Cleaning site collection custom actions from template");
                        foreach (var customAction in baseTemplate.CustomActions.SiteCustomActions)
                        {
                            template.CustomActions.SiteCustomActions.RemoveAll(p => p.Title.Equals(customAction.Title,
                                                                                                   StringComparison.OrdinalIgnoreCase));
                        }

                        total -= template.CustomActions.SiteCustomActions.Count;
                        WriteMessage($"Cleanup: {total} site collection custom actions cleaned from template");
                    }

                    if ((baseTemplate.CustomActions.WebCustomActions != null) &&
                       (template.CustomActions.WebCustomActions != null))
                    {
                        total = template.CustomActions.WebCustomActions.Count;
                        WriteMessage("Cleanup: Cleaning site custom actions from template");
                        foreach (var customAction in baseTemplate.CustomActions.WebCustomActions)
                        {
                            template.CustomActions.WebCustomActions.RemoveAll(p => p.Title.Equals(customAction.Title,
                                                                                                  StringComparison.OrdinalIgnoreCase));
                        }

                        total -= template.CustomActions.WebCustomActions.Count;
                        WriteMessage($"Cleanup: {total} site custom actions cleaned from template");
                    }

                }

            }
            if (provisioningOptions.IncludeFeatures)
            {
                if ((baseTemplate.Features != null) &&
                    (template.Features != null))
                {
                    if ((baseTemplate.Features.SiteFeatures != null) &&
                        (template.Features.SiteFeatures != null))
                    {
                        total = template.Features.SiteFeatures.Count;
                        WriteMessage("Cleanup: Cleaning site collection features from template");
                        foreach (var feature in baseTemplate.Features.SiteFeatures)
                        {
                            template.Features.SiteFeatures.RemoveAll(p => (p.Id.CompareTo(feature.Id) == 0));
                        }

                        total -= template.Features.SiteFeatures.Count;
                        WriteMessage($"Cleanup: {total} site collection features cleaned from template");
                    }

                    if ((baseTemplate.Features.WebFeatures != null) &&
                        (template.Features.WebFeatures != null))
                    {
                        total = template.Features.WebFeatures.Count;
                        WriteMessage("Cleanup: Cleaning site features from template");
                        foreach (var feature in baseTemplate.Features.WebFeatures)
                        {
                            template.Features.WebFeatures.RemoveAll(p => (p.Id.CompareTo(feature.Id) == 0));
                        }

                        total -= template.Features.WebFeatures.Count;
                        WriteMessage($"Cleanup: {total} site features cleaned from template");
                    }
                }
            }
            if (provisioningOptions.IncludeFields)
            {
                if ((baseTemplate.SiteFields != null) &&
                    (template.SiteFields != null))
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

                    WriteMessage($"Cleanup: {fieldsToDelete} site collection fields cleaned from template");
                }

            }

            if (provisioningOptions.IncludeFiles)
            {
                if ((baseTemplate.Files != null) &&
                    (template.Files != null))
                {
                    total = template.Files.Count;
                    WriteMessage("Cleanup: Cleaning files from template");
                    foreach (var file in baseTemplate.Files)
                    {
                        template.Files.RemoveAll(p => p.Src.Equals(file.Src, StringComparison.OrdinalIgnoreCase));
                    }

                    total -= template.Files.Count;
                    WriteMessage($"Cleanup: {total} files cleaned from template");
                }

            }

            if (provisioningOptions.IncludeListInstances)
            {
                if ((baseTemplate.Lists != null) &&
                    (template.Lists != null))
                {
                    total = template.Lists.Count;
                    WriteMessage("Cleanup: Cleaning lists from template");
                    foreach (var listInstance in baseTemplate.Lists)
                    {
                        template.Lists.RemoveAll(p => p.Title.Equals(listInstance.Title, StringComparison.OrdinalIgnoreCase));
                    }

                    total -= template.Lists.Count;
                    WriteMessage($"Cleanup: {total} lists cleaned from template");
                }

            }

            if (provisioningOptions.IncludePages)
            {
                if ((baseTemplate.Pages != null) &&
                    (template.Pages != null))
                {
                    total = template.Pages.Count;
                    WriteMessage("Cleanup: Cleaning pages from template");
                    foreach (var page in baseTemplate.Pages)
                    {
                        template.Pages.RemoveAll(p => p.Url.Equals(page.Url, StringComparison.OrdinalIgnoreCase));
                    }

                    total -= template.Pages.Count;
                    WriteMessage($"Cleanup: {total} pages cleaned from template");

                }

            }

            if (provisioningOptions.IncludePublishing)
            {
                if ((baseTemplate.Publishing != null) &&
                    (template.Publishing != null))
                {
                    if ((baseTemplate.Publishing.AvailableWebTemplates != null) &&
                        (template.Publishing.AvailableWebTemplates != null))
                    {
                        total = template.Publishing.AvailableWebTemplates.Count;
                        WriteMessage("Cleanup: Cleaning avaiable web templates from template");
                        foreach (var availableWebTemplate in baseTemplate.Publishing.AvailableWebTemplates)
                        {
                            template.Publishing.AvailableWebTemplates.RemoveAll(p => p.TemplateName.Equals(availableWebTemplate.TemplateName,
                                                                                                           StringComparison.OrdinalIgnoreCase));
                        }

                        total -= template.Publishing.AvailableWebTemplates.Count;
                        WriteMessage($"Cleanup: {total} avaiable web templates cleaned from template");
                    }

                    if ((baseTemplate.Publishing.PageLayouts != null) &&
                        (template.Publishing.PageLayouts != null))
                    {
                        total = template.Publishing.PageLayouts.Count;
                        WriteMessage("Cleanup: Cleaning page layouts from template");
                        foreach (var pageLayout in baseTemplate.Publishing.PageLayouts)
                        {
                            template.Publishing.PageLayouts.RemoveAll(p => p.Path.Equals(pageLayout.Path,
                                                                                         StringComparison.OrdinalIgnoreCase));
                        }

                        total -= template.Publishing.PageLayouts.Count;
                        WriteMessage($"Cleanup: {total} page layouts cleaned from template");
                    }

                }

            }

            if (provisioningOptions.IncludeSupportedUILanguages)
            {
                if ((baseTemplate.SupportedUILanguages != null) &&
                    (template.SupportedUILanguages != null))
                {
                    total = template.SupportedUILanguages.Count;
                    WriteMessage("Cleanup: Cleaning supported UI languages from template");
                    foreach (var supportedUILanguage in baseTemplate.SupportedUILanguages)
                    {
                        template.SupportedUILanguages.RemoveAll(p => (p.LCID == supportedUILanguage.LCID));
                    }

                    total -= template.SupportedUILanguages.Count;
                    WriteMessage($"Cleanup: {total} supported UI languages cleaned from template");

                }

            }

            if (provisioningOptions.IncludeTermGroups)
            {
                if ((baseTemplate.TermGroups != null) &&
                    (template.TermGroups != null))
                {
                    total = template.TermGroups.Count;
                    WriteMessage("Cleanup: Cleaning term groups from template");
                    foreach (var termGroup in baseTemplate.TermGroups)
                    {
                        template.TermGroups.RemoveAll(p => (p.Id.CompareTo(termGroup.Id) == 0));
                    }

                    total -= template.TermGroups.Count;
                    WriteMessage($"Cleanup: {total} term groups cleaned from template");

                }

            }

            if (provisioningOptions.IncludeWorkflows)
            {
                if ((baseTemplate.Workflows != null) &&
                    (template.Workflows != null))
                {
                    if ((baseTemplate.Workflows.WorkflowSubscriptions != null) &&
                        (template.Workflows.WorkflowSubscriptions != null))
                    {
                        total = template.Workflows.WorkflowSubscriptions.Count;
                        WriteMessage("Cleanup: Cleaning workflow subscriptions from template");
                        foreach (var workflowSubscription in baseTemplate.Workflows.WorkflowSubscriptions)
                        {
                            template.Workflows.WorkflowSubscriptions.RemoveAll(p =>
                            (p.DefinitionId.CompareTo(workflowSubscription.DefinitionId) == 0));
                        }

                        total -= template.Workflows.WorkflowSubscriptions.Count;
                        WriteMessage($"Cleanup: {total} workflow subscriptions cleaned from template");

                    }


                    if ((baseTemplate.Workflows.WorkflowDefinitions != null) &&
                        (template.Workflows.WorkflowDefinitions != null))
                    {
                        total = template.Workflows.WorkflowDefinitions.Count;
                        WriteMessage("Cleanup: Cleaning workflow definitions from template");
                        foreach (var workflowDefinition in baseTemplate.Workflows.WorkflowDefinitions)
                        {
                            template.Workflows.WorkflowDefinitions.RemoveAll(p =>
                            (p.Id.CompareTo(workflowDefinition.Id) == 0));
                        }

                        total -= template.Workflows.WorkflowDefinitions.Count;
                        WriteMessage($"Cleanup: {total} workflow definitions cleaned from template");

                    }

                }

            }

            if (provisioningOptions.IncludeContentTypes)
            {
                if ((baseTemplate.ContentTypes != null) &&
                    (template.ContentTypes != null))
                {
                    total = template.ContentTypes.Count;
                    WriteMessage("Cleanup: Cleaning content types from template");
                    foreach (var contentType in baseTemplate.ContentTypes)
                    {
                        template.ContentTypes.RemoveAll(p => p.Id.Equals(contentType.Id, StringComparison.OrdinalIgnoreCase));
                    }

                    total -= template.ContentTypes.Count;
                    WriteMessage($"Cleanup: {total} content types cleaned from template");

                }

            }

            if (provisioningOptions.IncludePropertyBagEntries)
            {
                if ((baseTemplate.PropertyBagEntries != null) &&
                    (template.PropertyBagEntries != null))
                {
                    total = template.PropertyBagEntries.Count;
                    WriteMessage("Cleanup: Cleaning property bag entries from template");
                    foreach (var propertyBagEntry in baseTemplate.PropertyBagEntries)
                    {
                        template.PropertyBagEntries.RemoveAll(p => p.Key.Equals(propertyBagEntry.Key, StringComparison.OrdinalIgnoreCase));
                    }

                    total -= template.PropertyBagEntries.Count;
                    WriteMessage($"Cleanup: {total} property bag entries cleaned from template");

                }

            }

            WriteMessage($"Info: Performed {baseTemplate.BaseSiteTemplate} template clean up");

        }

        private string TokenizeWebPartXml(Web web, string xml)
        {
            var lists = web.Lists;
            web.Context.Load(web, w => w.ServerRelativeUrl, w => w.Id);
            web.Context.Load(lists, ls => ls.Include(l => l.Id, l => l.Title));
            web.Context.ExecuteQueryRetry();

            foreach (var list in lists)
            {
                xml = Regex.Replace(xml, list.Id.ToString(), string.Format("{{listid:{0}}}", list.Title), RegexOptions.IgnoreCase);
            }
            xml = Regex.Replace(xml, web.Id.ToString(), "{siteid}", RegexOptions.IgnoreCase);
            xml = Regex.Replace(xml, "(\"" + web.ServerRelativeUrl + ")(?!&)", "\"{site}", RegexOptions.IgnoreCase);
            xml = Regex.Replace(xml, "'" + web.ServerRelativeUrl, "'{site}", RegexOptions.IgnoreCase);
            xml = Regex.Replace(xml, ">" + web.ServerRelativeUrl, ">{site}", RegexOptions.IgnoreCase);
            return xml;
        }

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

                WriteMessage($"Info: Saving items from {listInstance.Title} to template");

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
                        int fileIndex = template.Files.FindIndex(p => ((p.Folder.Equals(fileDirectory, StringComparison.OrdinalIgnoreCase)) &&
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
                                            Contents = webPartxml
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
                        int directoryIndex = template.Directories.FindIndex(p => ((p.Folder.Equals(fileDirectory, StringComparison.OrdinalIgnoreCase)) &&
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
                WriteMessage($"Info: {itemCount} items saved");

            }

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

                    // Load the web with all fields we will need.
                    Web web = ctx.Web;
                    ctx.Load(web, w => w.Title,
                                  w => w.Url,
                                  w => w.WebTemplate,
                                  w => w.Configuration,
                                  w => w.AllProperties,
                                  w => w.ServerRelativeUrl);
                    ctx.ExecuteQueryRetry();

                    WriteMessage($"Creating provisioning template from {web.Title} ( {web.Url} )");
                    WriteMessage($"Base template is {web.WebTemplate}#{web.Configuration}");


                    string fileNamePNP = provisioningOptions.TemplateName + ".pnp";
                    string fileNameXML = provisioningOptions.TemplateName + ".xml";

                    ProvisioningTemplateCreationInformation ptci = new ProvisioningTemplateCreationInformation(web);

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
                                case "site collection documents":
                                    if (provisioningOptions.IncludeDocumentLibraryFiles)
                                    {
                                        SaveFilesToTemplate(ctx, web, listInstance, template);
                                    }
                                    break;
                                case "images":
                                case "site collection images":
                                    if (provisioningOptions.IncludeImageFiles)
                                    {
                                        SaveFilesToTemplate(ctx, web, listInstance, template);
                                    }
                                    break;
                                case "pages":
                                case "site pages":
                                    if (provisioningOptions.IncludePages)
                                    {
                                        SaveFilesToTemplate(ctx, web, listInstance, template);
                                    }
                                    break;
                                case "site assets":
                                    if ((provisioningOptions.IncludeDocumentLibraryFiles) ||
                                        (provisioningOptions.IncludeImageFiles) ||
                                        (provisioningOptions.IncludeJavaScriptFiles))
                                    {
                                        SaveFilesToTemplate(ctx, web, listInstance, template);
                                    }
                                    break;
                                case "style library":
                                    if ((provisioningOptions.IncludeJavaScriptFiles) ||
                                        (provisioningOptions.IncludeXSLStyleSheetFiles) ||
                                        (provisioningOptions.IncludeImageFiles))
                                    {
                                        SaveFilesToTemplate(ctx, web, listInstance, template);
                                    }
                                    break;
                                default:
                                    if (listInstance.TemplateType == 100) //100 = Custom list
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
                                    else if ((listInstance.TemplateType == 101) && //101 = Document Library
                                             (provisioningOptions.IncludeDocumentLibraryFiles))
                                    {
                                        SaveFilesToTemplate(ctx, web, listInstance, template);
                                    }
                                    else if ((listInstance.TemplateType == 109) && //109 = Picture Library
                                             (provisioningOptions.IncludeImageFiles))
                                    {
                                        SaveFilesToTemplate(ctx, web, listInstance, template);
                                    }
                                    break;
                            }
                        }
                    }

                    //if exclude base template from template
                    if (provisioningOptions.ExcludeBaseTemplate)
                    {
                        ProvisioningTemplate baseTemplate = null;
                        WriteMessage($"Info: Loading base template {web.WebTemplate}#{web.Configuration}");

                        baseTemplate = web.GetBaseTemplate(web.WebTemplate, web.Configuration);

                        WriteMessage("Info: Base template loaded");

                        //perform the clean up
                        CleanupTemplate(provisioningOptions, template, baseTemplate);

                        //if not publishing site and publishing feature is activated, then clean publishing features from template
                        if (!baseTemplate.BaseSiteTemplate.Equals(Constants.Enterprise_Wiki_TemplateId, StringComparison.OrdinalIgnoreCase))
                        {
                            if (web.IsPublishingWeb())
                            {
                                WriteMessage("Info: Publishing feature actived on site");
                                WriteMessage($"Info: Loading {Constants.Enterprise_Wiki_TemplateId} base template");

                                string[] enterWikiArr = Constants.Enterprise_Wiki_TemplateId.Split(new char[] { '#' });

                                short config = Convert.ToInt16(enterWikiArr[1]);

                                baseTemplate = web.GetBaseTemplate(enterWikiArr[0], config);

                                WriteMessage($"Info: Done loading {Constants.Enterprise_Wiki_TemplateId} base template");

                                //perform the clean up
                                CleanupTemplate(provisioningOptions, template, baseTemplate);
                            }
                        }
                    }

                    XMLTemplateProvider provider = new XMLOpenXMLTemplateProvider(ptci.FileConnector as OpenXMLConnector);
                    provider.SaveAs(template, fileNameXML);

                    WriteMessage($"Template saved to {provisioningOptions.TemplatePath}\\{provisioningOptions.TemplateName}.pnp");

                    WriteMessage($"Done creating provisioning template from {web.Title} ( {web.Url} )");

                    result = true;
                }
            }
            catch (Exception ex)
            {
                if (!lbOutput.HorizontalScrollbar)
                {
                    lbOutput.HorizontalScrollbar = true;
                }
                WriteMessage("Error: " + ex.Message.Replace("\r\n", " "));
                if (ex.InnerException != null)
                {
                    WriteMessage("Error: Start of inner exception");
                    WriteMessage("Error: " + ex.InnerException.Message.Replace("\r\n", " "));
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
                WriteMessage("Error: " + ex.Message.Replace("\r\n", " "));
                if (ex.InnerException != null)
                {
                    WriteMessage("Error: Start of inner exception");
                    WriteMessage("Error: " + ex.InnerException.Message.Replace("\r\n", " "));
                    WriteMessageRange(ex.InnerException.StackTrace.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries));
                    WriteMessage("Error: End of inner exception");
                }
                WriteMessageRange(ex.StackTrace.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries));
                result = false;
            }
            return result;
        }

        public void OpenTemplateForEdit(string templatePath, string templateName, TreeView treeView)
        {
            string fileNamePNP = templateName + ".pnp";

            FileConnectorBase fileConnector = new FileSystemConnector(templatePath, "");

            XMLTemplateProvider provider = new XMLOpenXMLTemplateProvider(new OpenXMLConnector(fileNamePNP, fileConnector));

            List<ProvisioningTemplate> templates = provider.GetTemplates();

            ProvisioningTemplate template = templates[0];
            _editingTemplate = template;

            treeView.Nodes.Clear();

            KeyValueList templateList = new KeyValueList();

            TreeNode rootNode = new TreeNode($"Template - ( {templateName} )");
            rootNode.Name = "TemplateNode";

            if (template.RegionalSettings != null)
            {
                TreeNode rsNode = new TreeNode("Regional Settings");
                rsNode.Name = "RegionalSettings";
                rsNode.Tag = template.RegionalSettings;

                rootNode.Nodes.Add(rsNode);
                templateList.AddKeyValue(rsNode.Text, rsNode.Name);

            }

            if (template.AddIns?.Count > 0)
            {
                TreeNode aiNodes = new TreeNode("Add-Ins");
                aiNodes.Name = "AddIns";

                foreach (var addIn in template.AddIns)
                {
                    TreeNode aiNode = new TreeNode(addIn.PackagePath);
                    aiNode.Tag = addIn;

                    aiNodes.Nodes.Add(aiNode);
                }

                rootNode.Nodes.Add(aiNodes);
                templateList.AddKeyValue(aiNodes.Text, aiNodes.Name);

            }

            if (template.ComposedLook?.Name != null)
            {
                TreeNode clNode = new TreeNode("Composed Look");
                clNode.Name = "ComposedLook";
                clNode.Tag = template.ComposedLook;

                rootNode.Nodes.Add(clNode);
                templateList.AddKeyValue(clNode.Text, clNode.Name);

            }

            if (template.CustomActions?.SiteCustomActions?.Count > 0)
            {
                TreeNode scaNodes = new TreeNode("Site Custom Actions");
                scaNodes.Name = "SiteCustomActions";

                KeyValueList siteCustomActionsList = new KeyValueList();

                foreach (var siteCustomAction in template.CustomActions.SiteCustomActions)
                {
                    TreeNode scaNode = new TreeNode(siteCustomAction.Name);
                    scaNode.Name = siteCustomAction.RegistrationId;
                    scaNode.Tag = siteCustomAction;

                    scaNodes.Nodes.Add(scaNode);

                    siteCustomActionsList.AddKeyValue(siteCustomAction.Name, siteCustomAction.RegistrationId);
                }

                scaNodes.Tag = siteCustomActionsList;
                rootNode.Nodes.Add(scaNodes);
                templateList.AddKeyValue(scaNodes.Text, scaNodes.Name);

            }

            if (template.CustomActions?.WebCustomActions?.Count > 0)
            {
                TreeNode wcaNodes = new TreeNode("Web Custom Actions");
                wcaNodes.Name = "WebCustomActions";

                KeyValueList webCustomActionsList = new KeyValueList();

                foreach (var webCustomAction in template.CustomActions.WebCustomActions)
                {
                    TreeNode wcaNode = new TreeNode(webCustomAction.Name);
                    wcaNode.Name = webCustomAction.RegistrationId;
                    wcaNode.Tag = webCustomAction;

                    wcaNodes.Nodes.Add(wcaNode);

                    webCustomActionsList.AddKeyValue(webCustomAction.Name, webCustomAction.RegistrationId);

                }

                wcaNodes.Tag = webCustomActionsList;
                rootNode.Nodes.Add(wcaNodes);
                templateList.AddKeyValue(wcaNodes.Text, wcaNodes.Name);

            }

            if (template.Features?.SiteFeatures?.Count > 0)
            {
                TreeNode sfNodes = new TreeNode("Site Features");
                sfNodes.Name = "SiteFeatures";

                KeyValueList siteFeaturesList = new KeyValueList();

                foreach (var siteFeature in template.Features.SiteFeatures)
                {
                    siteFeaturesList.AddKeyValue(siteFeature.Id.ToString("N"), siteFeature.Id.ToString("N"));
                }

                sfNodes.Tag = siteFeaturesList;

                rootNode.Nodes.Add(sfNodes);
                templateList.AddKeyValue(sfNodes.Text, sfNodes.Name);

            }

            if (template.Features?.WebFeatures?.Count > 0)
            {
                TreeNode wfNodes = new TreeNode("Web Features");
                wfNodes.Name = "WebFeatures";

                KeyValueList webFeaturesList = new KeyValueList();

                foreach (var webFeature in template.Features.WebFeatures)
                {
                    webFeaturesList.AddKeyValue(webFeature.Id.ToString("N"), webFeature.Id.ToString("N"));
                }

                wfNodes.Tag = webFeaturesList;

                rootNode.Nodes.Add(wfNodes);
                templateList.AddKeyValue(wfNodes.Text, wfNodes.Name);

            }

            if (template.ContentTypes?.Count > 0)
            {
                TreeNode ctNodes = new TreeNode("Content Types");
                ctNodes.Name = "ContentTypes";

                KeyValueList contentTypeList = new KeyValueList();

                foreach (var contentType in template.ContentTypes)
                {
                    TreeNode ctNode = new TreeNode(contentType.Name);
                    ctNode.Name = contentType.Id;
                    ctNode.Tag = contentType.Id;

                    ctNodes.Nodes.Add(ctNode);

                    contentTypeList.AddKeyValue(contentType.Name, contentType.Id);

                }
                ctNodes.Tag = contentTypeList;

                rootNode.Nodes.Add(ctNodes);
                templateList.AddKeyValue(ctNodes.Text, ctNodes.Name);

            }

            if (template.SiteFields?.Count > 0)
            {
                TreeNode sfNodes = new TreeNode("Site Fields");
                sfNodes.Name = "SiteFields";

                KeyValueList siteFieldsList = new KeyValueList();

                foreach (var siteField in template.SiteFields)
                {
                    XElement fieldElement = XElement.Parse(siteField.SchemaXml);
                    string fieldID = fieldElement.Attribute("ID").Value;
                    string fieldName = fieldElement.Attribute("Name").Value;
                    TreeNode sfNode = new TreeNode(fieldName);

                    string fieldXml = fieldElement.ToString(SaveOptions.None);
                    int gtFirst = fieldXml.IndexOf('>', 0);
                    string fieldText = fieldXml.Substring(0, gtFirst).Replace("\" ", "\"\r\n       ") +
                                       fieldXml.Substring(gtFirst);

                    sfNode.Name = fieldID;
                    sfNode.Tag = fieldText;

                    sfNodes.Nodes.Add(sfNode);

                    siteFieldsList.AddKeyValue(fieldName, fieldID);

                }

                sfNodes.Tag = siteFieldsList;

                rootNode.Nodes.Add(sfNodes);
                templateList.AddKeyValue(sfNodes.Text, sfNodes.Name);

            }

            if (template.Files?.Count > 0)
            {
                TreeNode fNodes = new TreeNode("Files");
                fNodes.Name = "Files";

                KeyValueList filesList = new KeyValueList();

                foreach (var file in template.Files)
                {
                    TreeNode fNode = new TreeNode(file.Src);
                    fNode.Name = file.Src;
                    fNode.Tag = file.Src;

                    fNodes.Nodes.Add(fNode);

                    filesList.AddKeyValue(file.Src, file.Src);
                }

                fNodes.Tag = filesList;

                rootNode.Nodes.Add(fNodes);
                templateList.AddKeyValue(fNodes.Text, fNodes.Name);
            }

            if (template.Lists?.Count > 0)
            {
                TreeNode lNodes = new TreeNode("Lists");
                lNodes.Name = "Lists";

                KeyValueList listsList = new KeyValueList();

                foreach (var list in template.Lists)
                {
                    TreeNode lNode = new TreeNode(list.Title);
                    lNode.Name = list.Url;
                    lNode.Tag = list.Url;

                    if (list.Fields?.Count > 0)
                    {
                        TreeNode fNodes = new TreeNode("Fields");

                        KeyValueList fieldsList = new KeyValueList();

                        foreach (var field in list.Fields)
                        {
                            XElement fieldElement = XElement.Parse(field.SchemaXml);
                            string fieldID = fieldElement.Attribute("ID").Value;
                            string fieldName = fieldElement.Attribute("Name").Value;

                            TreeNode fNode = new TreeNode(fieldName);

                            string fieldXml = fieldElement.ToString(SaveOptions.None);
                            int gtFirst = fieldXml.IndexOf('>', 0);
                            string fieldText = fieldXml.Substring(0, gtFirst).Replace("\" ", "\"\r\n       ") + fieldXml.Substring(gtFirst);

                            fNode.Name = fieldID;
                            fNode.Tag = fieldText;

                            fNodes.Nodes.Add(fNode);
                            fieldsList.AddKeyValue(fieldName, fieldID);

                        }

                        fNodes.Tag = fieldsList;
                        lNode.Nodes.Add(fNodes);

                    }

                    if (list.Views?.Count > 0)
                    {
                        TreeNode vNodes = new TreeNode("Views");

                        KeyValueList viewsList = new KeyValueList();

                        foreach (var view in list.Views)
                        {
                            XElement viewElement = XElement.Parse(view.SchemaXml);
                            string viewName = viewElement.Attribute("Name").Value;
                            string displayName = viewElement.Attribute("DisplayName").Value;

                            TreeNode vNode = new TreeNode(displayName);

                            string viewXml = viewElement.ToString(SaveOptions.None);
                            int gtFirst = viewXml.IndexOf('>', 0);
                            string viewText = viewXml.Substring(0, gtFirst).Replace("\" ", "\"\r\n      ") + viewXml.Substring(gtFirst);

                            vNode.Name = viewName;
                            vNode.Tag = viewText;

                            vNodes.Nodes.Add(vNode);

                            viewsList.AddKeyValue(displayName, viewName);

                        }

                        vNodes.Tag = viewsList;
                        lNode.Nodes.Add(vNodes);

                    }

                    listsList.AddKeyValue(list.Title, list.Url);

                    lNodes.Nodes.Add(lNode);

                }

                lNodes.Tag = listsList;
                rootNode.Nodes.Add(lNodes);
                templateList.AddKeyValue(lNodes.Text, lNodes.Name);

            }

            if (template.Localizations?.Count > 0)
            {
                TreeNode glNodes = new TreeNode("Localizations");
                glNodes.Name = "Localizations";

                KeyValueList localizationsList = new KeyValueList();

                foreach (var localization in template.Localizations)
                {
                    TreeNode glNode = new TreeNode(localization.Name);
                    glNode.Name = localization.LCID.ToString();
                    glNode.Tag = localization.LCID;

                    glNodes.Nodes.Add(glNode);

                    localizationsList.AddKeyValue(localization.Name, localization.LCID.ToString());

                }

                glNodes.Tag = localizationsList;

                rootNode.Nodes.Add(glNodes);
                templateList.AddKeyValue(glNodes.Text, glNodes.Name);

            }

            //Navigation to do

            if (template.Pages?.Count > 0)
            {
                TreeNode pNodes = new TreeNode("Pages");
                pNodes.Name = "Pages";

                foreach (var page in template.Pages)
                {
                    TreeNode pNode = new TreeNode(page.Url);
                    pNode.Tag = page.Url;

                    pNodes.Nodes.Add(pNode);

                }

                rootNode.Nodes.Add(pNodes);
                templateList.AddKeyValue(pNodes.Text, pNodes.Name);

            }

            if (template.Properties?.Count > 0)
            {
                TreeNode pNode = new TreeNode("Properties");
                pNode.Name = "Properties";

                KeyValueList propertiesList = new KeyValueList();

                foreach (var property in template.Properties)
                {
                    propertiesList.AddKeyValue(property.Key, property.Value);
                }

                pNode.Tag = propertiesList;

                rootNode.Nodes.Add(pNode);
                templateList.AddKeyValue(pNode.Text, pNode.Name);

            }

            if (template.PropertyBagEntries?.Count > 0)
            {
                TreeNode pbeNodes = new TreeNode("Property Bag Entries");
                pbeNodes.Name = "PropertyBagEntries";

                KeyValueList propertyBagEntriesList = new KeyValueList();

                foreach (var propertyBagEntry in template.PropertyBagEntries)
                {
                    propertyBagEntriesList.AddKeyValue(propertyBagEntry.Key, propertyBagEntry.Value);
                }

                pbeNodes.Tag = propertyBagEntriesList;

                rootNode.Nodes.Add(pbeNodes);
                templateList.AddKeyValue(pbeNodes.Text, pbeNodes.Name);

            }

            if (template.Publishing != null)
            {
                TreeNode pNode = new TreeNode("Publishing");
                pNode.Name = "Publishing";

                pNode.Tag = template.Publishing;

                rootNode.Nodes.Add(pNode);
                templateList.AddKeyValue(pNode.Text, pNode.Name);

            }

            if (template.SupportedUILanguages?.Count > 0)
            {
                TreeNode suilNode = new TreeNode("Supported UI Languages");
                suilNode.Name = "SupportedUILanguages";


                suilNode.Tag = template.SupportedUILanguages;

                rootNode.Nodes.Add(suilNode);
                templateList.AddKeyValue(suilNode.Text, suilNode.Name);

            }

            if (template.TermGroups?.Count > 0)
            {
                TreeNode tgNodes = new TreeNode("Term Groups");
                tgNodes.Name = "TermGroups";

                KeyValueList termGroupsList = new KeyValueList();

                foreach (var termGroup in template.TermGroups)
                {
                    TreeNode tgNode = new TreeNode(termGroup.Name);
                    tgNode.Name = termGroup.Id.ToString("N");
                    tgNode.Tag = termGroup.Id;

                    if (termGroup.TermSets?.Count > 0)
                    {
                        TreeNode tsNodes = new TreeNode("Term Sets");

                        KeyValueList termSetsList = new KeyValueList();

                        foreach (var termSet in termGroup.TermSets)
                        {
                            TreeNode tsNode = new TreeNode(termSet.Name);
                            tsNode.Name = termSet.Id.ToString("N");
                            tsNode.Tag = termSet.Id;

                            tsNodes.Nodes.Add(tsNode);

                            termSetsList.AddKeyValue(termSet.Name, termSet.Id.ToString("N"));

                        }

                        tsNodes.Tag = termSetsList;

                        tgNode.Nodes.Add(tsNodes);

                    }

                    tgNodes.Nodes.Add(tgNode);

                    termGroupsList.AddKeyValue(termGroup.Name, termGroup.Id.ToString("N"));

                }

                tgNodes.Tag = termGroupsList;

                rootNode.Nodes.Add(tgNodes);
                templateList.AddKeyValue(tgNodes.Text, tgNodes.Name);

            }

            if (template.WebSettings != null)
            {
                TreeNode wsNode = new TreeNode("Web Settings");
                wsNode.Name = "WebSettings";

                WebSettings ws = template.WebSettings;
                wsNode.Tag = new WebSettings()
                {
                    AlternateCSS = ws.AlternateCSS,
                    CustomMasterPageUrl = ws.CustomMasterPageUrl,
                    Description = ws.Description,
                    MasterPageUrl = ws.MasterPageUrl,
                    NoCrawl = ws.NoCrawl,
                    RequestAccessEmail = ws.RequestAccessEmail,
                    SiteLogo = ws.SiteLogo,
                    Title = ws.Title,
                    WelcomePage = ws.WelcomePage
                };

                rootNode.Nodes.Add(wsNode);
                templateList.AddKeyValue(wsNode.Text, wsNode.Name);

            }

            if (template.Workflows?.WorkflowDefinitions?.Count > 0)
            {
                TreeNode wwdNodes = new TreeNode("Workflow Definitions");
                wwdNodes.Name = "WorkflowDefinitions";

                KeyValueList workflowDefinitionsList = new KeyValueList();

                foreach (var workflowDefinition in template.Workflows.WorkflowDefinitions)
                {
                    TreeNode wwdNode = new TreeNode(workflowDefinition.DisplayName);
                    wwdNode.Name = workflowDefinition.Id.ToString("N");
                    wwdNode.Tag = workflowDefinition.Id.ToString("D"); //do not want curly brackets {}

                    wwdNodes.Nodes.Add(wwdNode);

                    workflowDefinitionsList.AddKeyValue(workflowDefinition.DisplayName, workflowDefinition.Id.ToString("N"));

                }

                wwdNodes.Tag = workflowDefinitionsList;

                rootNode.Nodes.Add(wwdNodes);
                templateList.AddKeyValue(wwdNodes.Text, wwdNodes.Name);

            }

            if (template.Workflows?.WorkflowSubscriptions?.Count > 0)
            {
                TreeNode wwsNodes = new TreeNode("Workflow Subscriptions");
                wwsNodes.Name = "WorkflowSubscriptions";

                KeyValueList workflowSubscriptionsList = new KeyValueList();

                foreach (var workflowSubscription in template.Workflows.WorkflowSubscriptions)
                {
                    TreeNode wwsNode = new TreeNode(workflowSubscription.Name);
                    wwsNode.Name = workflowSubscription.Name;
                    wwsNode.Tag = workflowSubscription.Name;

                    wwsNodes.Nodes.Add(wwsNode);

                    workflowSubscriptionsList.AddKeyValue(workflowSubscription.Name, workflowSubscription.Name);

                }

                wwsNodes.Tag = workflowSubscriptionsList;

                rootNode.Nodes.Add(wwsNodes);
                templateList.AddKeyValue(wwsNodes.Text, wwsNodes.Name);

            }

            rootNode.Tag = templateList;
            rootNode.Expand();

            treeView.Nodes.Add(rootNode);

        } //OpenTemplateForEdit

        public int[] GetRegionalSettings()
        {
            int[] result = new int[]
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

            return result;

        } //GetRegionalSettingProperty

        public string[] GetComposedLook()
        {
            string[] result = new string[]
            {
                EditingTemplate.ComposedLook.Name,
                EditingTemplate.ComposedLook.BackgroundFile,
                EditingTemplate.ComposedLook.ColorFile,
                EditingTemplate.ComposedLook.FontFile,
                EditingTemplate.ComposedLook.Version.ToString()
            };

            return result;
        } //GetComposedLook

        public string GetContentType(string contentTypeId)
        {
            string result = string.Empty;
            PnPModel.ContentType contentType = EditingTemplate.ContentTypes.Find(p => p.Id.Equals(contentTypeId, StringComparison.OrdinalIgnoreCase));
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
                var serializer = new JavaScriptSerializer();
                result = serializer.Serialize(newCT).Replace(",\"ParentTemplate\":null", "")
                                                    .Replace("{", "{\r\n").Replace(":{", ":\r\n{").Replace(",", ",\r\n")
                                                    .Replace("[", "[\r\n").Replace("]", "\r\n]").Replace("}", "\r\n}");

            }
            return result;

        } //GetContentType

        public string GetListInstance(string url)
        {
            string result = string.Empty;
            ListInstance listInstance = EditingTemplate.Lists.Find(p => p.Url.Equals(url, StringComparison.OrdinalIgnoreCase));
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

                var serializer = new JavaScriptSerializer();
                result = serializer.Serialize(newLI).Replace(",\"ParentTemplate\":null", "")
                                                    .Replace(",\"Fields\":[]", "")
                                                    .Replace(",\"Views\":[]", "")
                                                    .Replace("{", "{\r\n").Replace(":{", ":\r\n{").Replace(",", ",\r\n")
                                                    .Replace("[", "[\r\n").Replace("]", "\r\n]").Replace("}", "\r\n}");

            }
            return result;

        } //GetListInstance

        public string[] GetWebSettings(object webSettings)
        {
            WebSettings ws = webSettings as WebSettings;
            string[] result = new string[]
            {
                ws.AlternateCSS,
                ws.CustomMasterPageUrl,
                ws.Description,
                ws.MasterPageUrl,
                (ws.NoCrawl ? "1" : "0"),
                ws.RequestAccessEmail,
                ws.SiteLogo,
                ws.Title,
                ws.WelcomePage
            };

            //Note: Ensure that the above are added in the order as defined in the WebSettingProperties enum

            return result;

        } //GetWebSettings

        public string GetWorkflowDefinition(Guid WorkflowDefinitionId)
        {
            string result = string.Empty;

            WorkflowDefinition workflowDefinition = EditingTemplate.Workflows.WorkflowDefinitions.Find(p => p.Id.Equals(WorkflowDefinitionId));
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

                var serializer = new JavaScriptSerializer();
                result = serializer.Serialize(newWD).Replace(",\"ParentTemplate\":null", "")
                                                    .Replace("{", "{\r\n").Replace(":{", ":\r\n{").Replace(",", ",\r\n")
                                                    .Replace("[", "[\r\n").Replace("]", "\r\n]").Replace("}", "\r\n}");

            }

            return result;

        } //GetWorkflowDefinition

        public string GetWorkflowSubscription(string workflowSubscriptionName)
        {
            string result = string.Empty;

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

                var serializer = new JavaScriptSerializer();
                result = serializer.Serialize(newWS).Replace(",\"ParentTemplate\":null", "")
                                                    .Replace("{", "{\r\n").Replace(":{", ":\r\n{").Replace(",", ",\r\n")
                                                    .Replace("[", "[\r\n").Replace("]", "\r\n]").Replace("}", "\r\n}");

            }

            return result;

        } //GetWorkflowSubscription

    } //SharePoint2013OnPrem

} //namespace
