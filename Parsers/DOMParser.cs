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

            // Проходимо по всіх елементах і збираємо атрибути
            foreach (XmlNode node in doc.DocumentElement.GetElementsByTagName("*"))
            {
                if (node.Attributes != null)
                {
                    foreach (XmlAttribute attribute in node.Attributes)
                    {
                        string attributeName = attribute.Name;
                        string attributeValue = attribute.Value;

                        // Якщо атрибут ще не існує в словнику, додаємо його
                        if (!attributes.ContainsKey(attributeName))
                        {
                            attributes[attributeName] = new List<string>();
                        }

                        // Додаємо значення атрибута, якщо його ще немає
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

            // Проходимо по всіх елементах і фільтруємо на основі відповідності ключовим словам
            foreach (XmlNode node in doc.DocumentElement.GetElementsByTagName("*"))
            {
                bool match = true;
                if (node.Attributes != null)
                {
                    foreach (KeyValuePair<string, string> entry in keyword)
                    {
                        // Перевіряємо наявність атрибута та його значення
                        if (node.Attributes[entry.Key]?.Value != entry.Value)
                        {
                            match = false;
                            break;
                        }
                    }
                }

                if (match)
                {
                    // Додаємо весь елемент у форматі XML
                    result.Add(node.OuterXml);
                }
            }

            return result;
        }
    }
}
