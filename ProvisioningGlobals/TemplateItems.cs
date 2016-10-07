using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Karabina.SharePoint.Provisioning
{
    public class TemplateItem
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public TemplateControlType ControlType { get; set; }

        public TemplateItemType ItemType { get; set; }

        public object Content { get; set; }

        public TemplateItem() { }

        public TemplateItem(string name, TemplateControlType controlType,
                            TemplateItemType itemType, object content)
        {
            Id = 0;
            Name = name;
            ControlType = controlType;
            ItemType = ItemType;
            Content = content;
        }

    } //TemplateItem

    public class TemplateItems : List<TemplateItem>
    {
        private int GetNextId()
        {
            int result = 0;
            if (Count > 0)
            {
                TemplateItem item = this[Count - 1];
                if (item != null)
                {
                    result = item.Id + 1;

                }
                else
                {
                    int loopDown = Count - 1;
                    while (loopDown >= 0)
                    {
                        item = this[loopDown];
                        if (item != null)
                        {
                            result = item.Id + 1;
                            break;

                        }

                        loopDown--;

                    }

                    if (loopDown < 0)
                    {
                        result = 0;

                    }

                }

            }

            return result;

        } //GetNextId

        public new void Add(TemplateItem item)
        {
            item.Id = GetNextId();

            base.Add(item);

        } //Add

        public new void AddRange(IEnumerable<TemplateItem> collection)
        {
            int newId = GetNextId();
            foreach (var item in collection)
            {
                item.Id = newId;
                newId++;

            }

            base.AddRange(collection);

        } //AddRange

        public int AddItem(string name, TemplateControlType controlType,
                            TemplateItemType itemType, object content)
        {
            TemplateItem item = new TemplateItem(name, controlType, itemType, content);

            Add(item);

            return item.Id;

        } //AddItem

        public TemplateItem GetItem(int id)
        {
            TemplateItem item = Find(p => p.Id == id);

            return item;

        } //GetItem

        public TemplateItem GetItem(string name)
        {
            TemplateItem item = Find(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

            return item;

        } //GetItem

        public TemplateItem GetItem(string name, TemplateItemType itemType)
        {
            TemplateItem item = Find(p => (p.Name.Equals(name, StringComparison.OrdinalIgnoreCase) && 
                                          (p.ItemType == itemType)));

            return item;

        } //GetItem
        
        public object GetContent(int id)
        {
            object result = null;

            TemplateItem item = Find(p => p.Id == id);
            if (item != null)
            {
                result = item.Content;

            }

            return result;

        } //GetContent

        public void SetContent(int id, object content)
        {
            TemplateItem item = Find(p => p.Id == id);
            if (item != null)
            {
                item.Content = content;
            }

        } //SetContent

    } //TemplateItems

} //Namespace
