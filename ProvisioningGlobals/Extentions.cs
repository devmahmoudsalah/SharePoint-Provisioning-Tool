using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Karabina.SharePoint.Provisioning
{
    public static class Extentions
    {
        /// <summary>
        /// Find the TemplateItem by its name property
        /// </summary>
        /// <param name="templateItems">List of TemplateItem items</param>
        /// <param name="name">The name to find</param>
        /// <returns>The found TemplateItem or null</returns>
        public static TemplateItem GetTemplateItemByName(this List<TemplateItem> templateItems, string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return null;

            return templateItems.Find(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

        } // GetItemByName

        /// <summary>
        /// Find first entry in the list that matches the specified value
        /// </summary>
        /// <param name="list">The list to search the value in</param>
        /// <param name="value">The value to search for</param>
        /// <returns>The found value or empty string</returns>
        public static string EndsWith(this List<string> list, string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return string.Empty;

            string result = string.Empty;
            if (list?.Count > 0)
            {
                for(int i = 0; i < list.Count; i++)
                {
                    if(list[i].EndsWith(value, StringComparison.OrdinalIgnoreCase))
                    {
                        result = list[i];
                        break;

                    }

                }

            }

            return result;

        } //EndsWith

    } // Extentions

}
