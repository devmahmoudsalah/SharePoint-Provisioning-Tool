using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Karabina.SharePoint.Provisioning
{
    public interface IProvisioningTool
    {

        bool ApplyProvisioningTemplate(ProvisioningOptions provisioningOptions,
                                       Action<string> writeMessage,
                                       Action<string[]> writeMessageRange);

        bool CreateProvisioningTemplate(ProvisioningOptions provisioningOptions,
                                               Action<string> writeMessage,
                                               Action<string[]> writeMessageRange);

        TemplateItems OpenTemplateForEdit(string templatePath, string templateName);

        void SaveTemplateForEdit(TemplateItems templateItems);

    } //IProvisioningTool

} //namespace
