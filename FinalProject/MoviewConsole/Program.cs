using System;
using System.Collections.Generic;
using System.Linq;
using MoviewConsole.Importer;

namespace MoviewConsole
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ReportImporter importer = new ReportImporter();
            importer.RetrieveFile(6);

            Console.WriteLine(importer.ToString());
            Console.Read();
        }
    }
}