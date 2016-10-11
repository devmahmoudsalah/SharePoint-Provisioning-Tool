using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Karabina.SharePoint.Provisioning
{
    public class KeyValue
    {
        public string Key { get; set; }

        public string Value { get; set; }

        public KeyValue() { }

        public KeyValue(string key, string value)
        {
            Key = key;
            Value = value;
        }

    } //KeyValue

    public class KeyValueList : List<KeyValue>
    {
        public void AddKeyValue(string key, string value)
        {
            Add(new KeyValue(key, value));

        } //AddKeyValue

        public int ItemIndex(string key)
        {
            return FindIndex(p => p.Key.Equals(key, StringComparison.OrdinalIgnoreCase));

        } //ItemIndex

    } //KeyValueList

}
