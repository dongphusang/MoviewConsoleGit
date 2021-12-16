using System;
using System.Collections.Generic;
using System.Linq;
using MoviewConsole.Manager;

namespace MoviewConsole
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ReportManager report = new();

            while(true)
            {
                Console.Clear();
                report.ProcessData();
                Thread.Sleep(18000000);
            }                        
        }
    }
}