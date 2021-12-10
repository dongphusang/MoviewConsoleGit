using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviewConsole.Interface
{
    interface IDecoder
    {
        const string DELIMETER = "_";
        public void RegisterImport(string rawContent);
        public void ProcessImport(out List<string> processedImport);
    }
}
