using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Karabina.SharePoint.Provisioning
{
    public enum ProvisioningFieldType
    {
        Invalid = 0,
        Integer = 1,
        Text = 2,
        Note = 3,
        DateTime = 4,
        Counter = 5,
        Choice = 6,
        Lookup = 7,
        Boolean = 8,
        Number = 9,
        Currency = 10,
        URL = 11,
        Computed = 12,
        Threading = 13,
        Guid = 14,
        MultiChoice = 15,
        GridChoice = 16,
        Calculated = 17,
        File = 18,
        Attachments = 19,
        User = 20,
        Recurrence = 21,
        CrossProjectLink = 22,
        ModStat = 23,
        Error = 24,
        ContentTypeId = 25,
        PageSeparator = 26,
        ThreadIndex = 27,
        WorkflowStatus = 28,
        AllDayEvent = 29,
        WorkflowEventType = 30,
        Geolocation = 31,
        OutcomeChoice = 32,
        MaxItems = 33

    } //ProvisioningFieldType

    public class ProvisioningField
    {
        public string Name { get; set; }

        public ProvisioningFieldType FieldType { get; set; }

        public ProvisioningField() { }

        public ProvisioningField(string name, ProvisioningFieldType fieldType)
        {
            Name = name;
            FieldType = fieldType;

        } //ProvisioningField

    } //ProvisioningField

    public class ProvisioningFieldCollection
    {
        private List<ProvisioningField> _fields = null;

        public List<ProvisioningField> Fields
        {
            get { return _fields; }
            set { _fields = value; }

        } //Fields

        public int Count
        {
            get
            {
                return _fields != null ? _fields.Count : 0;

            }

        } //Count

        public ProvisioningFieldCollection()
        {
            _fields = new List<ProvisioningField>();

        } //ProvisioningFieldCollection

        public void Add(string name, ProvisioningFieldType fieldType)
        {
            Add(new ProvisioningField(name, fieldType));

        } //Add

        public void Add(ProvisioningField item)
        {
            if (item == null) return;
            if (_fields.FindIndex(p => p.Name.Equals(item.Name, StringComparison.OrdinalIgnoreCase)) < 0)
            {
                _fields.Add(item);

            }

        } //Add

        public ProvisioningField Find(string name)
        {
            return _fields.Find(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

        } //Find

        public int FindIndex(string name)
        {
            return _fields.FindIndex(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

        } //FindIndex

    } //ProvisioningFieldCollection

}
