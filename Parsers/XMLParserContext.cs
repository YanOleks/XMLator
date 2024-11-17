using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XMLator.Parsers
{
    internal class XmlParserContext
    {
        private IParser _parser;

        public XmlParserContext(IParser parser)
        {
            _parser = parser;
        }

        public void SetParser(IParser parser)
        {
            _parser = parser;
        }

        public Dictionary<string, List<string>> GetAllAttributes(Stream xmlStream)
        {
            if (_parser == null)
                throw new InvalidOperationException("Парсер не встановлено");

            return _parser.GetAllAttributes(xmlStream);
        }
        public List<string> Search(Stream xmlStream, Dictionary<string, string> searchQuery)
        {
            if (_parser == null)
                throw new InvalidOperationException("Парсер не встановлено");

            return _parser.Search(xmlStream, searchQuery);
        }
    }
}
