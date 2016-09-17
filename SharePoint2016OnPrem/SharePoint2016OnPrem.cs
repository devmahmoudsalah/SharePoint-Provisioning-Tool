using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.SharePoint.Client;
using OfficeDevPnP.Core.Framework.Provisioning.Connectors;
using OfficeDevPnP.Core.Framework.Provisioning.Model;
using OfficeDevPnP.Core.Framework.Provisioning.ObjectHandlers;
using OfficeDevPnP.Core.Framework.Provisioning.Providers.Xml;
using System.Net;
using System.Security;
using System.Threading;

namespace Karabina.SharePoint.Provisioning
{
    public class SharePoint2016OnPrem
    {

        public SharePoint2016OnPrem()
        {
            //do nothing
        }

        public bool CreateProvisioningTemplate(ListBox lbOutput, ProvisioningOptions provisioningOptions)
        {
            bool result = false;
            try
            {
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

                    lbOutput.Items.Add($"Connecting to: {provisioningOptions.WebAddress}");
                    Application.DoEvents();

                    // Just to output the site details 
                    Web web = ctx.Web;
                    ctx.Load(web, w => w.Title, w => w.Url, w => w.WebTemplate, w => w.Configuration);
                    ctx.ExecuteQueryRetry();

                    lbOutput.Items.Add($"Creating provisioning template from {web.Title} ( {web.Url} )");
                    lbOutput.Items.Add($"Base template is {web.WebTemplate}#{web.Configuration}");
                    Application.DoEvents();

                    ProvisioningTemplate baseTemplate = null;
                    if (provisioningOptions.ExcludeBaseTemplate)
                    {
                        lbOutput.Items.Add("Loading base template");
                        Application.DoEvents();

                        baseTemplate = web.GetBaseTemplate(web.WebTemplate, web.Configuration);

                        lbOutput.Items.Add("Base template loaded");
                        Application.DoEvents();
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

                    ptci.MessagesDelegate = delegate (string message, ProvisioningMessageType messageType)
                    {
                        switch (messageType)
                        {
                            case ProvisioningMessageType.Error:
                                lbOutput.Items.Add("Error: " + message);
                                break;
                            case ProvisioningMessageType.Progress:
                                lbOutput.Items.Add("Progress: " + message);
                                break;
                            case ProvisioningMessageType.Warning:
                                lbOutput.Items.Add("Warning: " + message);
                                break;
                            case ProvisioningMessageType.EasterEgg:
                                lbOutput.Items.Add("EasterEgg: " + message);
                                break;
                            default:
                                lbOutput.Items.Add("Unknown: " + message);
                                break;
                        }
                        if (!lbOutput.HorizontalScrollbar)
                        {
                            lbOutput.HorizontalScrollbar = true;
                        }
                        lbOutput.TopIndex = (lbOutput.Items.Count - 1);
                        Application.DoEvents();
                    };

                    ptci.ProgressDelegate = delegate (string message, int progress, int total)
                    {
                        // Only to output progress
                        lbOutput.Items.Add(string.Format("{0:00}/{1:00} - {2}", progress, total, message));
                        lbOutput.TopIndex = (lbOutput.Items.Count - 1);
                        Application.DoEvents();
                    };

                    // Create FileSystemConnector, so that we can store composed files temporarely somewhere  
                    var fileSystemConnector = new FileSystemConnector(provisioningOptions.TemplatePath, "");

                    ptci.FileConnector = new OpenXMLConnector(fileNamePNP, fileSystemConnector, "SharePoint Team");

                    // Execute actual extraction of the tepmplate 
                    ProvisioningTemplate template = web.GetProvisioningTemplate(ptci);

                    XMLTemplateProvider provider = new XMLOpenXMLTemplateProvider(ptci.FileConnector as OpenXMLConnector);
                    provider.SaveAs(template, fileNameXML);

                    lbOutput.Items.Add($"Base site template is {template.BaseSiteTemplate}");

                    lbOutput.Items.Add($"Done creating provisioning template from {web.Title}");
                    lbOutput.TopIndex = (lbOutput.Items.Count - 1);
                    result = true;
                }
            }
            catch (Exception ex)
            {
                if (!lbOutput.HorizontalScrollbar)
                {
                    lbOutput.HorizontalScrollbar = true;
                }
                lbOutput.Items.Add("Error: " + ex.Message);
                if (ex.InnerException != null)
                {
                    lbOutput.Items.Add(ex.InnerException.Message);
                    lbOutput.Items.AddRange(ex.InnerException.StackTrace.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries));
                }
                lbOutput.Items.AddRange(ex.StackTrace.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries));
                lbOutput.TopIndex = (lbOutput.Items.Count - 1);
                result = false;
            }
            return result;
        }

        public bool ApplyProvisioningTemplate(ListBox lbOutput, ProvisioningOptions provisioningOptions)
        {
            bool result = false;
            try
            {
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

                    lbOutput.Items.Add($"Connecting to: {provisioningOptions.WebAddress}");
                    Application.DoEvents();

                    // Just to output the site details 
                    Web web = ctx.Web;
                    ctx.Load(web, w => w.Title, w => w.Url);
                    ctx.ExecuteQueryRetry();

                    webTitle = web.Title;

                    lbOutput.Items.Add($"Applying provisioning template to {webTitle} ( {web.Url} )");
                    Application.DoEvents();

                    string fileNamePNP = provisioningOptions.TemplateName + ".pnp";

                    FileConnectorBase fileConnector = new FileSystemConnector(provisioningOptions.TemplatePath, "");

                    XMLTemplateProvider provider = new XMLOpenXMLTemplateProvider(new OpenXMLConnector(fileNamePNP, fileConnector));

                    List<ProvisioningTemplate> templates = provider.GetTemplates();

                    ProvisioningTemplate template = templates[0];

                    lbOutput.Items.Add($"Base site template in provisioning template is {template.BaseSiteTemplate}");
                    Application.DoEvents();

                    template.WebSettings.Title = webTitle;

                    template.Connector = provider.Connector;

                    ProvisioningTemplateApplyingInformation ptai = new ProvisioningTemplateApplyingInformation();

                    ptai.MessagesDelegate = delegate (string message, ProvisioningMessageType messageType)
                    {
                        switch (messageType)
                        {
                            case ProvisioningMessageType.Error:
                                lbOutput.Items.Add("Error: " + message);
                                break;
                            case ProvisioningMessageType.Progress:
                                lbOutput.Items.Add("Progress: " + message);
                                break;
                            case ProvisioningMessageType.Warning:
                                lbOutput.Items.Add("Warning: " + message);
                                break;
                            case ProvisioningMessageType.EasterEgg:
                                lbOutput.Items.Add("EasterEgg: " + message);
                                break;
                            default:
                                lbOutput.Items.Add("Unknown: " + message);
                                break;
                        }
                        if (!lbOutput.HorizontalScrollbar)
                        {
                            lbOutput.HorizontalScrollbar = true;
                        }
                        lbOutput.TopIndex = (lbOutput.Items.Count - 1);
                        Application.DoEvents();
                    };

                    ptai.ProgressDelegate = delegate (string message, int progress, int total)
                    {
                        lbOutput.Items.Add(string.Format("{0:00}/{1:00} - {2}", progress, total, message));
                        lbOutput.TopIndex = (lbOutput.Items.Count - 1);
                        Application.DoEvents();
                    };

                    web.ApplyProvisioningTemplate(template, ptai);

                    lbOutput.Items.Add($"Done applying provisioning template to {web.Title}");
                    lbOutput.TopIndex = (lbOutput.Items.Count - 1);
                    result = true;
                }
            }
            catch (Exception ex)
            {
                if (!lbOutput.HorizontalScrollbar)
                {
                    lbOutput.HorizontalScrollbar = true;
                }
                lbOutput.Items.Add("Error: " + ex.Message);
                if (ex.InnerException != null)
                {
                    lbOutput.Items.Add(ex.InnerException.Message);
                    lbOutput.Items.AddRange(ex.InnerException.StackTrace.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries));
                }
                lbOutput.Items.AddRange(ex.StackTrace.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries));
                lbOutput.TopIndex = (lbOutput.Items.Count - 1);
                result = false;
            }
            return result;
        }

    }

}
