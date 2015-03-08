using System;
using System.Collections.Generic;
using System.Configuration;
using System.Dynamic;
using System.Linq;
using System.Xml.Linq;

namespace ExecutableSpecifications.Core
{
    public static class DataManager
    {
        private static IEnumerable<dynamic> _testData;

        internal static string Environment
        {
            get { return ConfigurationManager.AppSettings["environment"]; }
        }

        private static string DataFileName
        {
            get { return String.Format(".\\Data\\{0}.Data.xml", Environment); }
        }

        public static IEnumerable<dynamic> TestData
        {
            get { return _testData ?? (_testData = GetExpandoFromXml(DataFileName)); }
        }

        private static IEnumerable<dynamic> GetExpandoFromXml(String file)
        {
            var items = new List<dynamic>();

            var doc = XDocument.Load(file);
            var root = doc.Root;
            if (root == null)
            {
                return new List<dynamic>();
            }
            foreach (var n in root.Elements())
            {
                dynamic item = new ExpandoObject();
                item.XName = n.Name;
                var itemHasData = false;
                foreach (var child in n.Elements())
                {
                    item.XName = n.Name;
                    var p = item as IDictionary<string, object>;
                    p[child.Name.ToString()] = child.Value.Trim();
                    itemHasData = true;
                }
                if (itemHasData)
                {
                    items.Add(item);
                }
            }

            return items;
        }

        public static dynamic GetDynamicByLocalName(string name)
        {
            return TestData.FirstOrDefault(item =>
            {
                var xName = item.XName as XName;
                return xName != null && xName.LocalName == name;
            });
        }
    }
}