using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Karabina.SharePoint.Provisioning
{
    public static class Extentions
    {
        public static TemplateItem GetTemplateItemByName(this List<TemplateItem> templateItems, string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return null;

            return templateItems.Find(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

        } // GetItemByName



    } // Extentions

}
