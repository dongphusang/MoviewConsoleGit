using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoviewConsole.Importer;

namespace MoviewConsole.Manager
{
    class ReportManager
    {
        public int Sector { get; private set; } // represents a specific time frame in a day. Each time frame is 3 hours in length. Starting from 0:00 to 2:59 is sector 0 for example
        private string rawImport; // content extracted from blob

        private List<string> processedImport;
        private readonly ReportImporter importer;
        private DateTime date; // manipulated to get sector 
        private string fileName;
        private string filePath;
        

        public ReportManager()
        {
            importer = new();
            processedImport = new List<string>();
            fileName = "not specified";
            filePath = "not specified";
            rawImport = "non retrieved";
        }

        /// <summary>
        /// Gets the saved blob and extracts raw content
        /// </summary>
        /// <DEBUGGING NOTE>
        /// Can't separate line 51-55 from the method as processedImport might get incorrect/unexpected results. 
        /// This could potentially because of the block running ahead of time before file is retrieved in ReportImporter
        /// </DEBUGGING>
        public async void ProcessData()
        {
            date = DateTime.Now;
            Sector = DetermineSector();

            await importer.RetrieveFile(Sector); // temporary. this line's existence serves only for debugging
            Console.WriteLine(Sector); // debugging
            fileName = importer.BlobName;
            Console.WriteLine(fileName); // debugging
            filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), fileName); // path to %appdata%

            rawImport = File.ReadAllText(filePath);
            Console.WriteLine("ReportManager.ExtractContent().RawContent: "+rawImport); // debugging

            var tokens = rawImport.Split(",");
            foreach (var token in tokens)
            {
                Console.WriteLine("ReportManager.ProcessImport(): " + token); // debugging
                processedImport.Add(token);
            }
        }

        /// <summary>
        /// -Sub Method-
        /// Determine the value of sector based the current Hour and Minute components of date
        /// </summary>
        private int DetermineSector()
        {
            if (date.Hour <= 2 && date.Minute <= 59)
            {
                return 0;
            }
            else if (date.Hour <= 5 && date.Minute <= 59)
            {
                return 1;
            }
            else if (date.Hour <= 8 && date.Minute <= 59)
            {
                return 2;
            }
            else if (date.Hour <= 11 && date.Minute <= 59)
            {
                return 3;
            }
            else if (date.Hour <= 14 && date.Minute <= 59)
            {
                return 4;
            }
            else if (date.Hour <= 17 && date.Minute <= 59)
            {
                return 5;
            }
            else if (date.Hour <= 20 && date.Minute <= 59)
            {
                return 6;
            }
            else
            {
                Console.WriteLine("ReportManager.DetermineSector().ifelse_structure.else: " + date.Hour + " " + date.Minute); // debug. inappropriate hours and minutes might get considered
                return 7;
            }
        }


    }
}
