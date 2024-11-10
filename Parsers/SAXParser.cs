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
            XmlReaderSettings settings = new();
            List<string> result = [];
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
                        if (match) result.Add(reader.ReadContentAsString());
                    }
                }
            }
            return result;
        }
        public Dictionary<string, List<string>> GetAllAttributes(Stream stream)
        {
            XmlReaderSettings settings = new();
            Dictionary<string, List<string>> attributes = [];

            using (XmlReader reader = XmlReader.Create(stream, settings))
            {
                while (reader.MoveToElement())
                {
                    while (reader.MoveToNextAttribute())
                    {
                        (attributes[reader.Name]).Add(reader.Value);
                    }
                }
            }
            return attributes;
        }
    }
}
