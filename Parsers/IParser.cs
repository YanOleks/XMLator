using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XMLator.Parsers
{
    internal interface IParser
    {
        List<string> Search(Stream xmlStream, Dictionary<string, string> keyword);
        Dictionary<string, List<string>> GetAllAttributes(Stream xmlStream);
    }
}
