using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace XMLator.Parsers
{
    internal class DOMParser : IParser
    {
        public Dictionary<string, List<string>> GetAllAttributes(Stream xmlStream)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(xmlStream);

            Dictionary<string, List<string>> attributes = new Dictionary<string, List<string>>();

            foreach (XmlNode node in doc.DocumentElement.GetElementsByTagName("*"))
            {
                if (node.Attributes != null)
                {
                    foreach (XmlAttribute attribute in node.Attributes)
                    {
                        string attributeName = attribute.Name;
                        string attributeValue = attribute.Value;

                        if (!attributes.ContainsKey(attributeName))
                        {
                            attributes[attributeName] = new List<string>();
                        }

                        if (!attributes[attributeName].Contains(attributeValue))
                        {
                            attributes[attributeName].Add(attributeValue);
                        }
                    }
                }
            }

            return attributes;
        }

        public List<string> Search(Stream xmlStream, Dictionary<string, string> keyword)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(xmlStream);

            List<string> result = new List<string>();
            foreach (XmlNode node in doc.DocumentElement.GetElementsByTagName("*"))
            {
                bool match = true;
                if (node.Attributes != null)
                {
                    foreach (KeyValuePair<string, string> entry in keyword)
                    {
                        if (node.Attributes[entry.Key]?.Value != entry.Value)
                        {
                            match = false;
                            break;
                        }
                    }
                }

                if (match)
                {
                    result.Add(node.OuterXml);
                }
            }

            return result;
        }
    }
}
