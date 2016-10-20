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

        public TemplateItem GetParent(string id)
        {
            TemplateItem templateItem = Find(p => p.Id.Equals(id, StringComparison.Ordinal));
            if (templateItem != null)
            {
                if (!string.IsNullOrWhiteSpace(templateItem.ParentId))
                {
                    templateItem = Find(p => p.Id.Equals(templateItem.ParentId, StringComparison.Ordinal));

                }
                else
                {
                    templateItem = null;

                }

            }

            return templateItem;

        } //GetParent

        public TemplateItem GetParent(TemplateItem templateItem)
        {
            TemplateItem result = null;
            if (templateItem != null)
            {
                if (!string.IsNullOrWhiteSpace(templateItem.ParentId))
                {
                    result = Find(p => p.Id.Equals(templateItem.ParentId, StringComparison.Ordinal));

                }

            }

            return result;

        } //GetParent

        public TemplateItem GetParent(string id, TemplateItemType upToItemType)
        {
            TemplateItem templateItem = Find(p => p.Id.Equals(id, StringComparison.Ordinal));

            while (templateItem?.ItemType != upToItemType)
            {
                if (!string.IsNullOrWhiteSpace(templateItem.ParentId))
                {
                    templateItem = Find(p => p.Id.Equals(templateItem.ParentId, StringComparison.Ordinal));

                }
                else
                {
                    templateItem = null;
                    break;

                }

            }

            return templateItem;

        } //GetParent

        public TemplateItem GetParent(TemplateItem templateItem, TemplateItemType upToItemType)
        {
            TemplateItem result = null;
            if ((templateItem != null) && (!string.IsNullOrWhiteSpace(templateItem.ParentId)))
            {
                result = Find(p => p.Id.Equals(templateItem.ParentId, StringComparison.Ordinal));

                while (result?.ItemType != upToItemType)
                {
                    if (!string.IsNullOrWhiteSpace(result.ParentId))
                    {
                        result = Find(p => p.Id.Equals(result.ParentId, StringComparison.Ordinal));

                    }
                    else
                    {
                        templateItem = null;
                        break;

                    }

                }

            }

            return result;

        } //GetParent

        public List<TemplateItem> GetChildren(string parentId)
        {
            return FindAll(p => p.ParentId.Equals(parentId, StringComparison.Ordinal));

        } //GetChildren

        public List<TemplateItem> GetChildren(string parentId, bool allGenerations)
        {
            List<TemplateItem> children = FindAll(p => p.ParentId.Equals(parentId, StringComparison.Ordinal));
            if (allGenerations)
            {
                if (children?.Count > 0)
                {
                    foreach (var item in children)
                    {
                        List<TemplateItem> grandChildren = GetChildren(item.Id, true);
                        if (grandChildren?.Count > 0)
                        {
                            children.AddRange(grandChildren);

                        }

                    }

                }

            }

            return children;

        } //GetChildren

        public void SetDeleted(string id)
        {
            TemplateItem templateItem = Find(p => p.Id.Equals(id, StringComparison.Ordinal));
            if (templateItem != null)
            {
                templateItem.IsChanged = false;
                templateItem.IsDeleted = true;

            }

        } //SetDeleted

        public void SetDeleted(TemplateItem templateItem)
        {
            if (templateItem != null)
            {
                templateItem.IsChanged = false;
                templateItem.IsDeleted = true;

            }

        } //SetDeleted

        public void SetChildrenDeleted(string parentId)
        {
            FindAll(p => p.ParentId.Equals(parentId, StringComparison.Ordinal)).ForEach(
                delegate (TemplateItem item)
                {
                    SetChildrenDeleted(item.Id);
                    item.IsChanged = false;
                    item.IsDeleted = true;

                });

        } //SetChildrenDeleted

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

        public List<TemplateItem> GetDeletedItems()
        {
            return FindAll(p => p.IsDeleted);

        } //GetDeletedItems

        public List<TemplateItem> GetDeletedItems(TemplateItemType itemType)
        {
            return FindAll(p => ((p.IsDeleted) && (p.ItemType == itemType)));

        } //GetDeletedItems

        public List<TemplateItem> GetDeletedItems(TemplateItemType[] itemTypes)
        {
            return FindAll(p => ((p.IsDeleted) && (Array.IndexOf(itemTypes, p.ItemType) >= 0)));

        } //GetDeletedItems

        public List<TemplateItem> GetChangedItems()
        {
            return FindAll(p => p.IsChanged);

        } //GetChangedItems

        public List<TemplateItem> GetChangedItems(TemplateItemType itemType)
        {
            return FindAll(p => ((p.IsChanged) && (p.ItemType == itemType)));

        } //GetChangedItems

        public List<TemplateItem> GetChangedItems(TemplateItemType itemType, bool returnChildren)
        {
            List<TemplateItem> changedItems = FindAll(p => ((p.IsChanged) && (p.ItemType == itemType)));
            if (returnChildren)
            {
                foreach(var item in changedItems)
                {
                    List<TemplateItem> children = GetChildren(item.Id, true);
                    if (children?.Count > 0)
                    {
                        changedItems.AddRange(children);

                    }

                }

            }

            return changedItems;

        } //GetChangedItems

        public List<TemplateItem> GetChangedItems(TemplateItemType[] itemTypes)
        {
            return FindAll(p => ((p.IsChanged) && (Array.IndexOf(itemTypes, p.ItemType) >= 0)));

        } //GetChangedItems

        public void RemoveItem(TemplateItem templateItem)
        {
            if (templateItem != null)
            {
                List<TemplateItem> items = GetChildren(templateItem.Id);
                if (items?.Count > 0)
                {
                    foreach (var item in items)
                    {
                        RemoveItem(item);
                        Remove(item);

                    }

                }

                Remove(templateItem);

            }

        } //RemoveItem

        public void CommitItem(TemplateItem templateItem)
        {
            if (templateItem != null)
            {
                List<TemplateItem> items = GetChildren(templateItem.Id);
                if (items?.Count > 0)
                {
                    foreach (var item in items)
                    {
                        CommitItem(item);                        

                    }

                }

                templateItem.IsChanged = false;

            }

        } //CommitItem


    } //TemplateItems

} //Namespace
