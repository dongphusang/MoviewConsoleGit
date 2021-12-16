/*
 * Regulates imported blobs and extracts contents from them
 */ 



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
        private int sector; // represents a specific time frame in a day. Each time frame is 3 hours in length. Starting from 0:00 to 2:59 is sector 0 for example
        private string rawImport; // content extracted from blob

        private List<string> processedImport;
        private readonly ReportImporter importer;
        private DateTime date; // manipulated to get sector 
        private string fileName;
        private string filePath;

        private string height;
        private string soilpH;
        private string soilMoisture;
        private string humidity;
        private string temp;
        private string co2Content;
        private string luxContent;
        private string nitrogen;
        private string phosphorus;
        private string potassium;

        public ReportManager()
        {
            importer = new();
            processedImport = new();
            fileName = "not specified";
            filePath = "not specified";
            rawImport = "non retrieved";
            height = "null";
            soilpH = "null";
            soilMoisture = "null";
            humidity = "null";
            temp = "null";
            co2Content = "null";
            luxContent = "null";
            nitrogen = "null";
            phosphorus = "null";
            potassium = "null";
        }

        /// <summary>
        /// Extracts data from a downloaded blob
        /// </summary>
        /// <DEBUGGING NOTE>
        /// Can't separate line 51-55 from the method as processedImport might get incorrect/unexpected results. 
        /// This could potentially because of the block running ahead of time before file is retrieved in ReportImporter
        /// </DEBUGGING>
        public async void ProcessData()
        {
            date = DateTime.Now;
            sector = DetermineSector();

            await importer.RetrieveFile(sector); 

            fileName = importer.BlobName;
            filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), fileName); // path to %appdata%
            rawImport = File.ReadAllText(filePath);

            var tokens = rawImport.Split(",");
            foreach (var token in tokens)
            {
                processedImport.Add(token);
            }

            InitializeAttributes();
            OutputData();
        }

        /// <summary>
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
                return 7;
            }
        }

        /// <summary>
        /// Initialize attributes of a plant
        /// </summary>
        private void InitializeAttributes()
        {
            height = processedImport[0];
            soilpH = processedImport[1];
            soilMoisture = processedImport[2];
            humidity = processedImport[3];
            temp = processedImport[4];
            co2Content = processedImport[5];
            luxContent = processedImport[6];
            nitrogen = processedImport[7];
            phosphorus = processedImport[8];
            potassium = processedImport[9];
        }

        /// <summary>
        /// Output data
        /// </summary>
        private void OutputData()
        {
            Console.WriteLine("---Target Plant: Tomato---");
            Console.WriteLine("Target Specialized: Grape Tomato");

            Console.WriteLine("\n");
            Console.WriteLine("Size Attribute(s)\tSoil Attribute(s)\tSurroundings");
            Console.WriteLine("\n");

            Console.Write($"Height: {height} cm\t\t");
            Console.Write($"SoilMoisture: {soilMoisture}%\t");
            Console.WriteLine($"Humidity: {humidity}%");

            Console.Write("\t\t\t");
            Console.Write($"SoilpH: {soilpH}pH\t\t");
            Console.WriteLine($"Temperature: {temp}C");

            Console.Write("\t\t\t");
            Console.Write($"Nitrogen: {nitrogen} mg/kg\t");
            Console.WriteLine($"CO2: {co2Content} ppm");

            Console.Write("\t\t\t");
            Console.Write($"Phosphorus: {phosphorus} mg/kg\t");
            Console.WriteLine($"Lux: {luxContent} lux");

            Console.Write("\t\t\t");
            Console.WriteLine($"Potassium: {potassium} mg/kg");
        }

    }
}
