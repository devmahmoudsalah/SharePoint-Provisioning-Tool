using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Windows.Forms;

namespace Karabina.SharePoint.Provisioning
{
    public class SPReflection
    {
        private Assembly assembly = null;
        private Type type = null;
        private object sharepointObject = null;

        private MethodInfo applyProvisioningTemplate = null; //public bool ApplyProvisioningTemplate(ListBox lbOutput, ProvisioningOptions provisioningOptions)
        private MethodInfo createProvisioningTemplate = null; //public bool CreateProvisioningTemplate(ListBox lbOutput, ProvisioningOptions provisioningOptions)
        private MethodInfo openTemplateForEdit = null; //public TemplateItems OpenTemplateForEdit(string templatePath, string templateName, TreeView treeView)
        private MethodInfo saveTemplateForEdit = null; //public void SaveTemplateForEdit(TemplateItems templateItems)

        public SPReflection() { }

        public SPReflection(AppDomain appDomain, object spObject)
        {
            assembly = appDomain.GetAssemblies().FirstOrDefault();
            Type[] types = assembly.GetTypes();
            if (types.Length > 0)
            {
                type = types[0];

                applyProvisioningTemplate = type.GetMethod("ApplyProvisioningTemplate");
                createProvisioningTemplate = type.GetMethod("CreateProvisioningTemplate");
                openTemplateForEdit = type.GetMethod("OpenTemplateForEdit");
                saveTemplateForEdit = type.GetMethod("SaveTemplateForEdit");

                sharepointObject = spObject; // Activator.CreateInstance(type);

            }

        }

        public bool ApplyProvisioningTemplate(ListBox lbOutput, ProvisioningOptions provisioningOptions)
        {
            object[] args = new object[] { lbOutput, provisioningOptions };

            object result = applyProvisioningTemplate.Invoke(sharepointObject, args);

            return (bool)result;

        }

        public bool CreateProvisioningTemplate(ListBox lbOutput, ProvisioningOptions provisioningOptions)
        {
            object[] args = new object[] { lbOutput, provisioningOptions };

            object result = createProvisioningTemplate.Invoke(sharepointObject, args);

            return (bool)result;

        }

        public TemplateItems OpenTemplateForEdit(string templatePath, string templateName, TreeView treeView)
        {
            object[] args = new object[] { templatePath, templateName, treeView };

            object result = openTemplateForEdit.Invoke(sharepointObject, args);

            return (TemplateItems)result;

        }

        public void SaveTemplateForEdit(TemplateItems templateItems)
        {
            object[] args = new object[] { templateItems };

            saveTemplateForEdit.Invoke(sharepointObject, args);

        }

    }

}
