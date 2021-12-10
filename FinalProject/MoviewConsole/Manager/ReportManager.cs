using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoviewConsole.Importer;
using MoviewConsole.Decoder;

namespace MoviewConsole.Manager
{
    class ReportManager
    {
        public int Sector { get; private set; } // represents specific time frame in a day
        public string RawContent { get; private set; } // content extracted from blob

        private List<string> processedImport;
        private readonly ReportImporter importer;
        private readonly ReportDecoder decoder;
        private DateTime date; // manipulated to get sector 
        private string fileName;
        private string filePath;
        

        public ReportManager()
        {
            importer = new();
            decoder = new();
            processedImport = new List<string>();
            fileName = "not specified";
            filePath = "not specified";
            RawContent = "non retrieved";
        }

        /// <summary>
        /// Gets the saved blob and extracts raw content
        /// </summary>
        public async void ExtractContent()
        {
            date = DateTime.Now;
            Sector = DetermineSector();

            await importer.RetrieveFile(Sector); // temporary. this line's existence serves only for debugging
            Console.WriteLine(Sector);
            fileName = DetermineFileName();
            Console.WriteLine(fileName);
            filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), fileName); // path to %appdata%

            RawContent = File.ReadAllText(filePath);
            //Console.WriteLine("ReportManager.ExtractContent().RawContent: "+RawContent);
        }

        /// <summary>
        /// -Sub Method-
        /// If there is the target blob downloaded to the folder, its name will not include the word null in it
        /// </summary>
        /// <returns>string indication of whether raw data retrieved or not retrieved</returns>
        private string DetermineFileName()
        {
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), $"{Sector}.txt");
            if (File.Exists(path))
            {
                return $"{Sector}.txt";
            }
            else
                return $"{Sector}null.txt";
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

        public void RetrieveProcessedData()
        {
            decoder.ProcessImport(out processedImport);
        }

    }
}
