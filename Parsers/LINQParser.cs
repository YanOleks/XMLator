using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace XMLator.Parsers
{
    internal class LINQParser : IParser
    {
        public Dictionary<string, List<string>> GetAllAttributes(Stream xmlStream)
        {
            XDocument doc = XDocument.Load(xmlStream);

            var attributes = doc.Descendants()
                .SelectMany(element => element.Attributes())  // отримуємо всі атрибути з кожного елемента
                .GroupBy(attr => attr.Name.LocalName)         // групуємо за назвою атрибута
                .ToDictionary(
                    group => group.Key,                       // назва атрибута
                    group => group.Select(attr => attr.Value) // значення атрибутів
                                   .Distinct()                // унікальні значення
                                   .ToList()
                );

            return attributes;
        }

        public List<string> Search(Stream xmlStream, Dictionary<string, string> keywords)
        {
            XDocument doc = XDocument.Load(xmlStream);

            var result = doc.Descendants()
                            .Where(element => keywords.All(keyword =>
                                element.Attribute(keyword.Key) != null &&
                                element.Attribute(keyword.Key).Value == keyword.Value))
                            .Select(element => element.ToString())
                            .ToList();

            return result;
        }
    }
}
