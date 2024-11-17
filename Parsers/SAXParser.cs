using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace XMLator.Parsers
{
    internal class SAXParser : IParser
    {
        public List<string> Search(Stream stream, Dictionary<string, string> searchQuery)
        {
            XmlReaderSettings settings = new XmlReaderSettings
            {
                DtdProcessing = DtdProcessing.Ignore,
                IgnoreWhitespace = true
            };
            List<string> result = new List<string>();

            using (XmlReader reader = XmlReader.Create(stream, settings))
            {
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        bool match = true;
                        foreach (KeyValuePair<string, string> entry in searchQuery)
                        {
                            if (reader.GetAttribute(entry.Key) != entry.Value)
                            {
                                match = false;
                                break;
                            }
                        }

                        if (match)
                        {
                            result.Add(reader.ReadOuterXml());
                        }
                    }
                }
            }
            return result;
        }

        public Dictionary<string, List<string>> GetAllAttributes(Stream stream)
        {
            XmlReaderSettings settings = new XmlReaderSettings
            {
                DtdProcessing = DtdProcessing.Ignore,
                IgnoreWhitespace = true
            };
            Dictionary<string, List<string>> attributes = new Dictionary<string, List<string>>();

            using (XmlReader reader = XmlReader.Create(stream, settings))
            {
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        while (reader.MoveToNextAttribute())
                        {
                            string attributeName = reader.Name;
                            string attributeValue = reader.Value;
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
            }
            return attributes;
        }
    }

}
