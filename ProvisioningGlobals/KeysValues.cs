using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Karabina.SharePoint.Provisioning
{
    [Serializable]
    public class KeyValue
    {
        public string Key { get; set; }

        public string Value { get; set; }

        public KeyValue() { }

        public KeyValue(string key, string value)
        {
            Key = key;
            Value = value;

        } //KeyValue

        public bool KeyIsEmpty()
        {
            return string.IsNullOrWhiteSpace(Key);

        } //KeyIsEmpty

        public bool ValueIsEmpty()
        {
            return string.IsNullOrWhiteSpace(Value);

        } //ValueIsEmpty

        public bool Equals(KeyValue keyValue)
        {
            bool result = KeyEquals(keyValue.Key);
            if (result)
            {
                result = ValueEquals(keyValue.Value);

            }

            return result;

        } //Equals

        public bool KeyEquals(string key)
        {
            bool result = false;
            if (!string.IsNullOrWhiteSpace(Key))
            {
                result = Key.Equals(key, StringComparison.OrdinalIgnoreCase);

            }

            return result;

        } //KeyEquals

        public bool ValueEquals(string value)
        {
            bool result = false;
            if (!string.IsNullOrWhiteSpace(Value))
            {
                result = Value.Equals(value, StringComparison.OrdinalIgnoreCase);

            }

            return result;

        } //ValueEquals

    } //KeyValue

    [Serializable]
    public class KeyValueList : List<KeyValue>
    {
        public string DisplayMember
        {
            get { return "Key"; }

        } //DisplayMember

        public string ValueMember
        {
            get { return "Value"; }

        } //ValueMember

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
