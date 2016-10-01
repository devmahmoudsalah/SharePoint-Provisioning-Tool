using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Karabina.SharePoint.Provisioning
{
    public class SortOrder
    {
        public int SortOrderId { get; set; }
        public string SortOrderName { get; set; }

        public SortOrder() { }
    }

    public class SortOrderCollection
    {
        private List<SortOrder> _sortOrders = null;

        public List<SortOrder> SortOrders
        {
            get { return _sortOrders; }
            private set { _sortOrders = value; }
        }

        public SortOrderCollection()
        {
            _sortOrders = new List<SortOrder>();

            _sortOrders.Add(new SortOrder() { SortOrderId = 25, SortOrderName = "General" });
            _sortOrders.Add(new SortOrder() { SortOrderId = 0, SortOrderName = "Albanian" });
            _sortOrders.Add(new SortOrder() { SortOrderId = 1, SortOrderName = "Arabic" });
            _sortOrders.Add(new SortOrder() { SortOrderId = 39, SortOrderName = "Azeri (Cyrillic)" });
            _sortOrders.Add(new SortOrder() { SortOrderId = 40, SortOrderName = "Azeri (Latin)" });
            _sortOrders.Add(new SortOrder() { SortOrderId = 4, SortOrderName = "Chinese (Traditional) Bopomofo" });
            _sortOrders.Add(new SortOrder() { SortOrderId = 5, SortOrderName = "Chinese (Traditional) Stroke Count" });
            _sortOrders.Add(new SortOrder() { SortOrderId = 2, SortOrderName = "Chinese Pronunciation" });
            _sortOrders.Add(new SortOrder() { SortOrderId = 3, SortOrderName = "Chinese Stroke Count" });
            _sortOrders.Add(new SortOrder() { SortOrderId = 6, SortOrderName = "Croatian" });
            _sortOrders.Add(new SortOrder() { SortOrderId = 7, SortOrderName = "Cyrillic" });
            _sortOrders.Add(new SortOrder() { SortOrderId = 8, SortOrderName = "Czech" });
            _sortOrders.Add(new SortOrder() { SortOrderId = 9, SortOrderName = "Danish/Norwegian" });
            _sortOrders.Add(new SortOrder() { SortOrderId = 42, SortOrderName = "Divehi" });
            _sortOrders.Add(new SortOrder() { SortOrderId = 10, SortOrderName = "Estonian" });
            _sortOrders.Add(new SortOrder() { SortOrderId = 11, SortOrderName = "Finnish/Swedish" });
            _sortOrders.Add(new SortOrder() { SortOrderId = 12, SortOrderName = "French" });
            _sortOrders.Add(new SortOrder() { SortOrderId = 13, SortOrderName = "Georgian Modern" });
            _sortOrders.Add(new SortOrder() { SortOrderId = 14, SortOrderName = "German Phone Book" });
            _sortOrders.Add(new SortOrder() { SortOrderId = 15, SortOrderName = "Greek" });
            _sortOrders.Add(new SortOrder() { SortOrderId = 16, SortOrderName = "Hebrew" });
            _sortOrders.Add(new SortOrder() { SortOrderId = 17, SortOrderName = "Hindi" });
            _sortOrders.Add(new SortOrder() { SortOrderId = 41, SortOrderName = "Hong Kong Stroke Order" });
            _sortOrders.Add(new SortOrder() { SortOrderId = 18, SortOrderName = "Hungarian" });
            _sortOrders.Add(new SortOrder() { SortOrderId = 19, SortOrderName = "Hungarian Technical" });
            _sortOrders.Add(new SortOrder() { SortOrderId = 20, SortOrderName = "Icelandic" });
            _sortOrders.Add(new SortOrder() { SortOrderId = 43, SortOrderName = "Indic (General)" });
            _sortOrders.Add(new SortOrder() { SortOrderId = 21, SortOrderName = "Japanese" });
            _sortOrders.Add(new SortOrder() { SortOrderId = 22, SortOrderName = "Japanese Unicode" });
            _sortOrders.Add(new SortOrder() { SortOrderId = 44, SortOrderName = "Kazakh" });
            _sortOrders.Add(new SortOrder() { SortOrderId = 23, SortOrderName = "Korean" });
            _sortOrders.Add(new SortOrder() { SortOrderId = 24, SortOrderName = "Korean Unicode" });
            _sortOrders.Add(new SortOrder() { SortOrderId = 26, SortOrderName = "Latvian" });
            _sortOrders.Add(new SortOrder() { SortOrderId = 27, SortOrderName = "Lithuanian" });
            _sortOrders.Add(new SortOrder() { SortOrderId = 28, SortOrderName = "Lithuanian Classic" });
            _sortOrders.Add(new SortOrder() { SortOrderId = 45, SortOrderName = "Macedonian (Macedonia, FYRO)" });
            _sortOrders.Add(new SortOrder() { SortOrderId = 31, SortOrderName = "Polish" });
            _sortOrders.Add(new SortOrder() { SortOrderId = 32, SortOrderName = "Romanian" });
            _sortOrders.Add(new SortOrder() { SortOrderId = 33, SortOrderName = "Slovak" });
            _sortOrders.Add(new SortOrder() { SortOrderId = 34, SortOrderName = "Slovenian" });
            _sortOrders.Add(new SortOrder() { SortOrderId = 30, SortOrderName = "Spanish (Modern Sort)" });
            _sortOrders.Add(new SortOrder() { SortOrderId = 29, SortOrderName = "Spanish (Traditional Sort)" });
            _sortOrders.Add(new SortOrder() { SortOrderId = 46, SortOrderName = "Syriac" });
            _sortOrders.Add(new SortOrder() { SortOrderId = 47, SortOrderName = "Tatar" });
            _sortOrders.Add(new SortOrder() { SortOrderId = 35, SortOrderName = "Thai" });
            _sortOrders.Add(new SortOrder() { SortOrderId = 36, SortOrderName = "Turkish" });
            _sortOrders.Add(new SortOrder() { SortOrderId = 37, SortOrderName = "Ukrainian" });
            _sortOrders.Add(new SortOrder() { SortOrderId = 48, SortOrderName = "Uzbek (Latin)" });
            _sortOrders.Add(new SortOrder() { SortOrderId = 38, SortOrderName = "Vietnamese" });

        }

    }

}
