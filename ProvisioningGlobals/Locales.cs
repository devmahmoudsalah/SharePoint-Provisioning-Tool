using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Karabina.SharePoint.Provisioning
{
    public class Locale
    {
        public int LocaleId { get; set; }
        public string LocaleName { get; set; }

        public Locale() { }

    }

    public class LocaleCollection
    {
        private List<Locale> _locales = null;
        public List<Locale> Locales
        {
            get { return _locales; }
            private set { _locales = value; }
        }

        public string DisplayMember
        {
            get { return "LocaleName"; }

        } //DisplayMember

        public string ValueMember
        {
            get { return "LocaleId"; }

        } //ValueMember

        public LocaleCollection()
        {
            _locales = new List<Locale>();

            _locales.Add(new Locale() { LocaleId = 1078, LocaleName = "Afrikaans" });
            _locales.Add(new Locale() { LocaleId = 1052, LocaleName = "Albanian" });
            _locales.Add(new Locale() { LocaleId = 1156, LocaleName = "Alsatian" });
            _locales.Add(new Locale() { LocaleId = 1118, LocaleName = "Amharic" });
            _locales.Add(new Locale() { LocaleId = 5121, LocaleName = "Arabic (Algeria)" });
            _locales.Add(new Locale() { LocaleId = 15361, LocaleName = "Arabic (Bahrain)" });
            _locales.Add(new Locale() { LocaleId = 3073, LocaleName = "Arabic (Egypt)" });
            _locales.Add(new Locale() { LocaleId = 2049, LocaleName = "Arabic (Iraq)" });
            _locales.Add(new Locale() { LocaleId = 11265, LocaleName = "Arabic (Jordan)" });
            _locales.Add(new Locale() { LocaleId = 13313, LocaleName = "Arabic (Kuwait)" });
            _locales.Add(new Locale() { LocaleId = 12289, LocaleName = "Arabic (Lebanon)" });
            _locales.Add(new Locale() { LocaleId = 4097, LocaleName = "Arabic (Libya)" });
            _locales.Add(new Locale() { LocaleId = 6145, LocaleName = "Arabic (Morocco)" });
            _locales.Add(new Locale() { LocaleId = 8193, LocaleName = "Arabic (Oman)" });
            _locales.Add(new Locale() { LocaleId = 16385, LocaleName = "Arabic (Qatar)" });
            _locales.Add(new Locale() { LocaleId = 1025, LocaleName = "Arabic (Saudi Arabia)" });
            _locales.Add(new Locale() { LocaleId = 10241, LocaleName = "Arabic (Syria)" });
            _locales.Add(new Locale() { LocaleId = 7169, LocaleName = "Arabic (Tunisia)" });
            _locales.Add(new Locale() { LocaleId = 14337, LocaleName = "Arabic (U.A.E.)" });
            _locales.Add(new Locale() { LocaleId = 9217, LocaleName = "Arabic (Yemen)" });
            _locales.Add(new Locale() { LocaleId = 1067, LocaleName = "Armenian" });
            _locales.Add(new Locale() { LocaleId = 1101, LocaleName = "Assamese" });
            _locales.Add(new Locale() { LocaleId = 2092, LocaleName = "Azerbaijani (Cyrillic)" });
            _locales.Add(new Locale() { LocaleId = 1068, LocaleName = "Azerbaijani (Latin)" });
            _locales.Add(new Locale() { LocaleId = 1133, LocaleName = "Bashkir" });
            _locales.Add(new Locale() { LocaleId = 1069, LocaleName = "Basque" });
            _locales.Add(new Locale() { LocaleId = 1059, LocaleName = "Belarusian" });
            _locales.Add(new Locale() { LocaleId = 2117, LocaleName = "Bangla (Bangladesh)" });
            _locales.Add(new Locale() { LocaleId = 1093, LocaleName = "Bangla" });
            _locales.Add(new Locale() { LocaleId = 8218, LocaleName = "Bosnian (Cyrillic)" });
            _locales.Add(new Locale() { LocaleId = 5146, LocaleName = "Bosnian (Latin)" });
            _locales.Add(new Locale() { LocaleId = 1150, LocaleName = "Breton" });
            _locales.Add(new Locale() { LocaleId = 1026, LocaleName = "Bulgarian" });
            _locales.Add(new Locale() { LocaleId = 1027, LocaleName = "Catalan" });
            _locales.Add(new Locale() { LocaleId = 3076, LocaleName = "Chinese (Hong Kong S.A.R.)" });
            _locales.Add(new Locale() { LocaleId = 5124, LocaleName = "Chinese (Macao S.A.R.)" });
            _locales.Add(new Locale() { LocaleId = 2052, LocaleName = "Chinese (PRC)" });
            _locales.Add(new Locale() { LocaleId = 4100, LocaleName = "Chinese (Singapore)" });
            _locales.Add(new Locale() { LocaleId = 1028, LocaleName = "Chinese (Taiwan)" });
            _locales.Add(new Locale() { LocaleId = 1155, LocaleName = "Corsican" });
            _locales.Add(new Locale() { LocaleId = 4122, LocaleName = "Croatian (Bosnia and Herzegovina)" });
            _locales.Add(new Locale() { LocaleId = 1050, LocaleName = "Croatian (Croatia)" });
            _locales.Add(new Locale() { LocaleId = 1029, LocaleName = "Czech" });
            _locales.Add(new Locale() { LocaleId = 1030, LocaleName = "Danish" });
            _locales.Add(new Locale() { LocaleId = 1164, LocaleName = "Dari" });
            _locales.Add(new Locale() { LocaleId = 1125, LocaleName = "Divehi" });
            _locales.Add(new Locale() { LocaleId = 2067, LocaleName = "Dutch (Belgium)" });
            _locales.Add(new Locale() { LocaleId = 1043, LocaleName = "Dutch (Netherlands)" });
            _locales.Add(new Locale() { LocaleId = 3081, LocaleName = "English (Australia)" });
            _locales.Add(new Locale() { LocaleId = 10249, LocaleName = "English (Belize)" });
            _locales.Add(new Locale() { LocaleId = 4105, LocaleName = "English(Canada)" });
            _locales.Add(new Locale() { LocaleId = 9225, LocaleName = "English (Caribbean)" });
            _locales.Add(new Locale() { LocaleId = 16393, LocaleName = "English (India)" });
            _locales.Add(new Locale() { LocaleId = 6153, LocaleName = "English (Ireland)" });
            _locales.Add(new Locale() { LocaleId = 8201, LocaleName = "English (Jamaica)" });
            _locales.Add(new Locale() { LocaleId = 17417, LocaleName = "English (Malaysia)" });
            _locales.Add(new Locale() { LocaleId = 5129, LocaleName = "English (New Zealand)" });
            _locales.Add(new Locale() { LocaleId = 13321, LocaleName = "English (Philippines)" });
            _locales.Add(new Locale() { LocaleId = 18441, LocaleName = "English (Singapore)" });
            _locales.Add(new Locale() { LocaleId = 7177, LocaleName = "English (South Africa)" });
            _locales.Add(new Locale() { LocaleId = 11273, LocaleName = "English (Trinidad and Tobago)" });
            _locales.Add(new Locale() { LocaleId = 2057, LocaleName = "English (United Kingdom)" });
            _locales.Add(new Locale() { LocaleId = 1033, LocaleName = "English (United States)" });
            _locales.Add(new Locale() { LocaleId = 12297, LocaleName = "English (Zimbabwe)" });
            _locales.Add(new Locale() { LocaleId = 1061, LocaleName = "Estonian" });
            _locales.Add(new Locale() { LocaleId = 1080, LocaleName = "Faroese" });
            _locales.Add(new Locale() { LocaleId = 1124, LocaleName = "Filipino" });
            _locales.Add(new Locale() { LocaleId = 1035, LocaleName = "Finnish" });
            _locales.Add(new Locale() { LocaleId = 2060, LocaleName = "French (Belgium)" });
            _locales.Add(new Locale() { LocaleId = 3084, LocaleName = "French (Canada)" });
            _locales.Add(new Locale() { LocaleId = 1036, LocaleName = "French (France)" });
            _locales.Add(new Locale() { LocaleId = 5132, LocaleName = "French (Luxembourg)" });
            _locales.Add(new Locale() { LocaleId = 6156, LocaleName = "French (Monaco)" });
            _locales.Add(new Locale() { LocaleId = 4108, LocaleName = "French (Switzerland)" });
            _locales.Add(new Locale() { LocaleId = 1122, LocaleName = "Frisian" });
            _locales.Add(new Locale() { LocaleId = 1110, LocaleName = "Galician" });
            _locales.Add(new Locale() { LocaleId = 1079, LocaleName = "Georgian" });
            _locales.Add(new Locale() { LocaleId = 3079, LocaleName = "German (Austria)" });
            _locales.Add(new Locale() { LocaleId = 1031, LocaleName = "German (Germany)" });
            _locales.Add(new Locale() { LocaleId = 5127, LocaleName = "German (Liechtenstein)" });
            _locales.Add(new Locale() { LocaleId = 4103, LocaleName = "German (Luxembourg)" });
            _locales.Add(new Locale() { LocaleId = 2055, LocaleName = "German (Switzerland)" });
            _locales.Add(new Locale() { LocaleId = 1032, LocaleName = "Greek" });
            _locales.Add(new Locale() { LocaleId = 1135, LocaleName = "Greenlandic" });
            _locales.Add(new Locale() { LocaleId = 1095, LocaleName = "Gujarati" });
            _locales.Add(new Locale() { LocaleId = 1128, LocaleName = "Hausa (Latin)" });
            _locales.Add(new Locale() { LocaleId = 1037, LocaleName = "Hebrew" });
            _locales.Add(new Locale() { LocaleId = 1081, LocaleName = "Hindi" });
            _locales.Add(new Locale() { LocaleId = 1038, LocaleName = "Hungarian" });
            _locales.Add(new Locale() { LocaleId = 1039, LocaleName = "Icelandic" });
            _locales.Add(new Locale() { LocaleId = 1136, LocaleName = "Igbo" });
            _locales.Add(new Locale() { LocaleId = 9275, LocaleName = "Inari Sami" });
            _locales.Add(new Locale() { LocaleId = 1057, LocaleName = "Indonesian" });
            _locales.Add(new Locale() { LocaleId = 2141, LocaleName = "Inuktitut (Latin)" });
            _locales.Add(new Locale() { LocaleId = 1117, LocaleName = "Inuktitut (Syllabics)" });
            _locales.Add(new Locale() { LocaleId = 2108, LocaleName = "Irish" });
            _locales.Add(new Locale() { LocaleId = 1076, LocaleName = "isiXhosa" });
            _locales.Add(new Locale() { LocaleId = 1077, LocaleName = "isiZulu" });
            _locales.Add(new Locale() { LocaleId = 1040, LocaleName = "Italian (Italy)" });
            _locales.Add(new Locale() { LocaleId = 2064, LocaleName = "Italian (Switzerland)" });
            _locales.Add(new Locale() { LocaleId = 1041, LocaleName = "Japanese" });
            _locales.Add(new Locale() { LocaleId = 1099, LocaleName = "Kannada" });
            _locales.Add(new Locale() { LocaleId = 1087, LocaleName = "Kazakh" });
            _locales.Add(new Locale() { LocaleId = 1107, LocaleName = "Khmer" });
            _locales.Add(new Locale() { LocaleId = 1158, LocaleName = "K'iche'" });
            _locales.Add(new Locale() { LocaleId = 1159, LocaleName = "Kinyarwanda" });
            _locales.Add(new Locale() { LocaleId = 1089, LocaleName = "Kiswahili" });
            _locales.Add(new Locale() { LocaleId = 1111, LocaleName = "Konkani" });
            _locales.Add(new Locale() { LocaleId = 1042, LocaleName = "Korean" });
            _locales.Add(new Locale() { LocaleId = 1088, LocaleName = "Kyrgyz" });
            _locales.Add(new Locale() { LocaleId = 1108, LocaleName = "Lao" });
            _locales.Add(new Locale() { LocaleId = 1062, LocaleName = "Latvian" });
            _locales.Add(new Locale() { LocaleId = 1063, LocaleName = "Lithuanian" });
            _locales.Add(new Locale() { LocaleId = 2094, LocaleName = "Lower Sorbian" });
            _locales.Add(new Locale() { LocaleId = 4155, LocaleName = "Lule Sami (Norway)" });
            _locales.Add(new Locale() { LocaleId = 5179, LocaleName = "Lule Sami (Sweden)" });
            _locales.Add(new Locale() { LocaleId = 1134, LocaleName = "Luxembourgish" });
            _locales.Add(new Locale() { LocaleId = 1071, LocaleName = "Macedonian" });
            _locales.Add(new Locale() { LocaleId = 1086, LocaleName = "Malay (Malaysia)" });
            _locales.Add(new Locale() { LocaleId = 2110, LocaleName = "Malay (Brunei Darussalam)" });
            _locales.Add(new Locale() { LocaleId = 1100, LocaleName = "Malayalam" });
            _locales.Add(new Locale() { LocaleId = 1082, LocaleName = "Maltese" });
            _locales.Add(new Locale() { LocaleId = 1153, LocaleName = "Maori" });
            _locales.Add(new Locale() { LocaleId = 1146, LocaleName = "Mapudungun" });
            _locales.Add(new Locale() { LocaleId = 1102, LocaleName = "Marathi" });
            _locales.Add(new Locale() { LocaleId = 1148, LocaleName = "Mohawk" });
            _locales.Add(new Locale() { LocaleId = 1104, LocaleName = "Mongolian (Cyrillic)" });
            _locales.Add(new Locale() { LocaleId = 2128, LocaleName = "Mongolian (Traditional Mongolian)" });
            _locales.Add(new Locale() { LocaleId = 1121, LocaleName = "Nepali" });
            _locales.Add(new Locale() { LocaleId = 3131, LocaleName = "Northern Sami (Finland)" });
            _locales.Add(new Locale() { LocaleId = 1083, LocaleName = "Northern Sami (Norway)" });
            _locales.Add(new Locale() { LocaleId = 2107, LocaleName = "Northern Sami (Sweden)" });
            _locales.Add(new Locale() { LocaleId = 1044, LocaleName = "Norwegian (Bokmål)" });
            _locales.Add(new Locale() { LocaleId = 2068, LocaleName = "Norwegian (Nynorsk)" });
            _locales.Add(new Locale() { LocaleId = 1154, LocaleName = "Occitan" });
            _locales.Add(new Locale() { LocaleId = 1096, LocaleName = "Odia" });
            _locales.Add(new Locale() { LocaleId = 1123, LocaleName = "Pashto" });
            _locales.Add(new Locale() { LocaleId = 1065, LocaleName = "Persian" });
            _locales.Add(new Locale() { LocaleId = 1045, LocaleName = "Polish" });
            _locales.Add(new Locale() { LocaleId = 1046, LocaleName = "Portuguese (Brazil)" });
            _locales.Add(new Locale() { LocaleId = 2070, LocaleName = "Portuguese (Portugal)" });
            _locales.Add(new Locale() { LocaleId = 1094, LocaleName = "Punjabi" });
            _locales.Add(new Locale() { LocaleId = 1131, LocaleName = "Quechua (Bolivia)" });
            _locales.Add(new Locale() { LocaleId = 2155, LocaleName = "Quechua (Ecuador)" });
            _locales.Add(new Locale() { LocaleId = 3179, LocaleName = "Quechua (Peru)" });
            _locales.Add(new Locale() { LocaleId = 1048, LocaleName = "Romanian" });
            _locales.Add(new Locale() { LocaleId = 1047, LocaleName = "Romansh" });
            _locales.Add(new Locale() { LocaleId = 1049, LocaleName = "Russian" });
            _locales.Add(new Locale() { LocaleId = 1157, LocaleName = "Sakha" });
            _locales.Add(new Locale() { LocaleId = 1103, LocaleName = "Sanskrit" });
            _locales.Add(new Locale() { LocaleId = 7194, LocaleName = "Serbian (Cyrillic, Bosnia and Herzegovina)" });
            _locales.Add(new Locale() { LocaleId = 12314, LocaleName = "Serbian (Cyrillic, Montenegro)" });
            _locales.Add(new Locale() { LocaleId = 10266, LocaleName = "Serbian (Cyrillic, Serbia)" });
            _locales.Add(new Locale() { LocaleId = 6170, LocaleName = "Serbian (Latin, Bosnia and Herzegovina)" });
            _locales.Add(new Locale() { LocaleId = 11290, LocaleName = "Serbian (Latin, Montenegro)" });
            _locales.Add(new Locale() { LocaleId = 9242, LocaleName = "Serbian (Latin, Serbia)" });
            _locales.Add(new Locale() { LocaleId = 1132, LocaleName = "Sesotho sa Leboa" });
            _locales.Add(new Locale() { LocaleId = 1074, LocaleName = "Setswana" });
            _locales.Add(new Locale() { LocaleId = 1169, LocaleName = "Scottish Gaelic" });
            _locales.Add(new Locale() { LocaleId = 1115, LocaleName = "Sinhala" });
            _locales.Add(new Locale() { LocaleId = 8251, LocaleName = "Skolt Sami" });
            _locales.Add(new Locale() { LocaleId = 1051, LocaleName = "Slovak" });
            _locales.Add(new Locale() { LocaleId = 1060, LocaleName = "Slovenian" });
            _locales.Add(new Locale() { LocaleId = 6203, LocaleName = "Southern Sami (Norway)" });
            _locales.Add(new Locale() { LocaleId = 7227, LocaleName = "Southern Sami (Sweden)" });
            _locales.Add(new Locale() { LocaleId = 11274, LocaleName = "Spanish (Argentina)" });
            _locales.Add(new Locale() { LocaleId = 16394, LocaleName = "Spanish (Bolivia)" });
            _locales.Add(new Locale() { LocaleId = 13322, LocaleName = "Spanish (Chile)" });
            _locales.Add(new Locale() { LocaleId = 9226, LocaleName = "Spanish (Colombia)" });
            _locales.Add(new Locale() { LocaleId = 5130, LocaleName = "Spanish (Costa Rica)" });
            _locales.Add(new Locale() { LocaleId = 7178, LocaleName = "Spanish (Dominican Republic)" });
            _locales.Add(new Locale() { LocaleId = 12298, LocaleName = "Spanish (Ecuador)" });
            _locales.Add(new Locale() { LocaleId = 17418, LocaleName = "Spanish (El Salvador)" });
            _locales.Add(new Locale() { LocaleId = 4106, LocaleName = "Spanish (Guatemala)" });
            _locales.Add(new Locale() { LocaleId = 18442, LocaleName = "Spanish (Honduras)" });
            _locales.Add(new Locale() { LocaleId = 2058, LocaleName = "Spanish (Mexico)" });
            _locales.Add(new Locale() { LocaleId = 19466, LocaleName = "Spanish (Nicaragua)" });
            _locales.Add(new Locale() { LocaleId = 6154, LocaleName = "Spanish (Panama)" });
            _locales.Add(new Locale() { LocaleId = 15370, LocaleName = "Spanish (Paraguay)" });
            _locales.Add(new Locale() { LocaleId = 10250, LocaleName = "Spanish (Peru)" });
            _locales.Add(new Locale() { LocaleId = 20490, LocaleName = "Spanish (Puerto Rico)" });
            _locales.Add(new Locale() { LocaleId = 1034, LocaleName = "Spanish (Spain, Traditional)" });
            _locales.Add(new Locale() { LocaleId = 3082, LocaleName = "Spanish (Spain)" });
            _locales.Add(new Locale() { LocaleId = 21514, LocaleName = "Spanish (United States)" });
            _locales.Add(new Locale() { LocaleId = 14346, LocaleName = "Spanish (Uruguay)" });
            _locales.Add(new Locale() { LocaleId = 8202, LocaleName = "Spanish (Venezuela)" });
            _locales.Add(new Locale() { LocaleId = 1053, LocaleName = "Swedish (Sweden)" });
            _locales.Add(new Locale() { LocaleId = 2077, LocaleName = "Swedish (Finland)" });
            _locales.Add(new Locale() { LocaleId = 1114, LocaleName = "Syriac" });
            _locales.Add(new Locale() { LocaleId = 1064, LocaleName = "Tajik" });
            _locales.Add(new Locale() { LocaleId = 2143, LocaleName = "Tamazight (Latin)" });
            _locales.Add(new Locale() { LocaleId = 1097, LocaleName = "Tamil" });
            _locales.Add(new Locale() { LocaleId = 1092, LocaleName = "Tatar" });
            _locales.Add(new Locale() { LocaleId = 1098, LocaleName = "Telugu" });
            _locales.Add(new Locale() { LocaleId = 1054, LocaleName = "Thai" });
            _locales.Add(new Locale() { LocaleId = 1105, LocaleName = "Tibetan" });
            _locales.Add(new Locale() { LocaleId = 1055, LocaleName = "Turkish" });
            _locales.Add(new Locale() { LocaleId = 1090, LocaleName = "Turkmen" });
            _locales.Add(new Locale() { LocaleId = 1058, LocaleName = "Ukrainian" });
            _locales.Add(new Locale() { LocaleId = 1070, LocaleName = "Upper Sorbian" });
            _locales.Add(new Locale() { LocaleId = 1056, LocaleName = "Urdu" });
            _locales.Add(new Locale() { LocaleId = 1152, LocaleName = "Uyghur" });
            _locales.Add(new Locale() { LocaleId = 2115, LocaleName = "Uzbek (Cyrillic)" });
            _locales.Add(new Locale() { LocaleId = 1091, LocaleName = "Uzbek (Latin)" });
            _locales.Add(new Locale() { LocaleId = 1066, LocaleName = "Vietnamese" });
            _locales.Add(new Locale() { LocaleId = 1106, LocaleName = "Welsh" });
            _locales.Add(new Locale() { LocaleId = 1160, LocaleName = "Wolof" });
            _locales.Add(new Locale() { LocaleId = 1144, LocaleName = "Yi" });
            _locales.Add(new Locale() { LocaleId = 1130, LocaleName = "Yoruba" });

        }

    }

}
