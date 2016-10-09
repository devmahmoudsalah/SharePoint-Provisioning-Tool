using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Karabina.SharePoint.Provisioning
{
    public class TemplateItem
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public bool IsChanged { get; set; }

        public TemplateControlType ControlType { get; set; }

        public TemplateItemType ItemType { get; set; }

        public object Content { get; set; }

        public TemplateItem()
        {
            Id = Guid.NewGuid().ToString("N").ToUpperInvariant();
            IsChanged = false;
        }

        public TemplateItem(string name, TemplateControlType controlType,
                            TemplateItemType itemType, object content)
        {
            Id = Guid.NewGuid().ToString("N").ToUpperInvariant();
            IsChanged = false;
            Name = name;
            ControlType = controlType;
            ItemType = ItemType;
            Content = content;
        }

    } //TemplateItem

    public class TemplateItems : List<TemplateItem>
    {

        public string AddItem(string name, TemplateControlType controlType,
                            TemplateItemType itemType, object content)
        {
            TemplateItem item = new TemplateItem(name, controlType, itemType, content);

            Add(item);

            return item.Id;

        } //AddItem

        public TemplateItem GetItem(string id)
        {
            TemplateItem item = Find(p => p.Id.Equals(id, StringComparison.Ordinal));

            return item;

        } //GetItem

        public TemplateItem GetItem(string name, TemplateItemType itemType)
        {
            TemplateItem item = Find(p => (p.Name.Equals(name, StringComparison.OrdinalIgnoreCase) && 
                                          (p.ItemType == itemType)));

            return item;

        } //GetItem
        
        public object GetContent(string id)
        {
            object result = null;

            TemplateItem item = Find(p => p.Id.Equals(id, StringComparison.Ordinal));
            if (item != null)
            {
                result = item.Content;

            }

            return result;

        } //GetContent

        public void SetContent(string id, object content)
        {
            TemplateItem item = Find(p => p.Id.Equals(id, StringComparison.Ordinal));
            if (item != null)
            {
                if (item.Content != null)
                {
                    if (!item.Content.Equals(content))
                    {
                        item.IsChanged = true;

                    }

                }

                item.Content = content;

            }

        } //SetContent

    } //TemplateItems

} //Namespace
