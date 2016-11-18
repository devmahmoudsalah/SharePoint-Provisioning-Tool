using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Windows.Forms;

namespace Karabina.SharePoint.Provisioning
{
    public class SPLoader : MarshalByRefObject
    {
        private Assembly assembly = null;
        private IProvisioningTool type = null;
        private object sharepointObject = null;

        /*
        private MethodInfo applyProvisioningTemplate = null;
        private MethodInfo createProvisioningTemplate = null;
        private MethodInfo openTemplateForEdit = null;
        private MethodInfo saveTemplateForEdit = null;
        */

        public SPLoader() { }

        public void LoadProvisioningAssembly(string assemblyName)
        {
            assembly = Assembly.Load(assemblyName);
            Type[] types = assembly.GetTypes();
            if (types.Length > 0)
            {
                type = types[0] as IProvisioningTool;

                //applyProvisioningTemplate = type.GetMethod("ApplyProvisioningTemplate");
                //createProvisioningTemplate = type.GetMethod("CreateProvisioningTemplate");
                //openTemplateForEdit = type.GetMethod("OpenTemplateForEdit");
                //saveTemplateForEdit = type.GetMethod("SaveTemplateForEdit");

                sharepointObject = Activator.CreateInstance(type as Type);

            }

        }

        public bool ApplyProvisioningTemplate(ProvisioningOptions provisioningOptions,
                                              Action<string> writeMessage,
                                              Action<string[]> writeMessageRange)
        {
            return type.ApplyProvisioningTemplate(provisioningOptions, writeMessage, writeMessageRange);

            /*
            object[] args = new object[] { provisioningOptions, writeMessage, writeMessageRange };

            object result = applyProvisioningTemplate.Invoke(sharepointObject, args);

            return (bool)result;
            */
        }

        public bool CreateProvisioningTemplate(ProvisioningOptions provisioningOptions,
                                               Action<string> writeMessage,
                                               Action<string[]> writeMessageRange)
        {
            return type.CreateProvisioningTemplate(provisioningOptions, writeMessage, writeMessageRange);

            /*
            object[] args = new object[] { provisioningOptions, writeMessage, writeMessageRange };

            object result = createProvisioningTemplate.Invoke(sharepointObject, args);

            return (bool)result;
            */
        }

        public TemplateItems OpenTemplateForEdit(string templatePath, string templateName)
        {
            return type.OpenTemplateForEdit(templatePath, templateName);

            /*
            object[] args = new object[] { templatePath, templateName };

            object result = openTemplateForEdit.Invoke(sharepointObject, args);

            return (TemplateItems)result;
            */
        }

        public void SaveTemplateForEdit(TemplateItems templateItems)
        {
            type.SaveTemplateForEdit(templateItems);

            /*
            object[] args = new object[] { templateItems };

            saveTemplateForEdit.Invoke(sharepointObject, args);
            */

        }

    }

}
