using System;
using System.Collections.Generic;
using System.Linq;
using MoviewConsole.Manager;
using MoviewConsole.Importer;

namespace MoviewConsole
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ReportManager reportManager = new ReportManager();
            reportManager.ExtractContent();


            Console.Read();
        }
    }
}