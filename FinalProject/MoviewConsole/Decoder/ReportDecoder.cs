using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoviewConsole.Interface;

namespace MoviewConsole.Decoder
{
    class ReportDecoder : IDecoder
    {
        public string RawImport { get; private set; }
        public StringBuilder SubString { get; private set; }

        public ReportDecoder()
        {
            RawImport = "Not Imported";
            SubString = new();
        }

        public void RegisterImport(string rawContent)
        {
            RawImport = rawContent;
        }

        public void ProcessImport(out List<string> processedImport)
        {
            processedImport = new List<string>();
            var tokens = "15.6,5,35,34,35,200,10000,100,100,100".Split(",");
            foreach (var token in tokens)
            {
                Console.Write(token +" ");
            }
        }


    }
}
