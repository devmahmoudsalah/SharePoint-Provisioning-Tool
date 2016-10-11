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

        public string ParentId { get; set; }

        public bool IsChanged { get; set; }

        public bool IsDeleted { get; set; }

        public TemplateControlType ControlType { get; set; }

        public TemplateItemType ItemType { get; set; }

        private object _content = null;
        public object Content
        {
            get { return _content; }
            set
            {
                if (_content != null)
                {
                    if (!_content.Equals(value))
                    {
                        IsChanged = true;

                    }

                }

                _content = value;

            }

        }

        public TemplateItem()
        {
            Id = Guid.NewGuid().ToString("N").ToUpperInvariant();
            IsChanged = false;
        }

        public TemplateItem(string name, TemplateControlType controlType,
                            TemplateItemType itemType, object content, string parentId)
        {
            Id = Guid.NewGuid().ToString("N").ToUpperInvariant();
            IsChanged = false;
            Name = name;
            ParentId = parentId;
            ControlType = controlType;
            ItemType = itemType;
            Content = content;
        }

    } //TemplateItem

    public class TemplateItems : List<TemplateItem>
    {

        public string AddItem(string name, TemplateControlType controlType,
                            TemplateItemType itemType, object content, string parentId)
        {
            TemplateItem item = new TemplateItem(name, controlType, itemType, content, parentId);

            Add(item);

            return item.Id;

        } //AddItem

        public TemplateItem GetItem(string id)
        {
            return Find(p => p.Id.Equals(id, StringComparison.Ordinal));

        } //GetItem

        public TemplateItem GetItem(string id, TemplateItemType itemType)
        {
            return Find(p => (p.Id.Equals(id, StringComparison.Ordinal) && (p.ItemType == itemType)));

        } //GetItem

        public List<TemplateItem> GetItems(TemplateItemType itemType)
        {
            return FindAll(p => p.ItemType == itemType);

        } //GetItems

        public List<TemplateItem> GetChildren(string parentId)
        {
            return FindAll(p => p.ParentId.Equals(parentId, StringComparison.Ordinal));

        } //GetChildren

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
                item.Content = content;

            }

        } //SetContent

    } //TemplateItems

} //Namespace
