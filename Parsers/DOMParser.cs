using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XMLator.Parsers
{
    internal class DOMParser : IParser
    {
        public Dictionary<string, List<string>> GetAllAttributes(Stream xmlStream)
        {
            throw new NotImplementedException();
        }

        public List<string> Search(Stream xmlStream, Dictionary<string, string> keyword)
        {
            throw new NotImplementedException();
        }
    }
}
