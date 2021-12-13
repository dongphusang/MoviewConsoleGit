using System;
using System.Collections.Generic;
using System.Linq;
using MoviewConsole.DataClasses;

namespace MoviewConsole
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ReportData report = new();
            report.Update();

            Console.WriteLine("Current Selection: Tomato");
            Console.WriteLine("Specialized Selection: Grape Tomato");
            Console.WriteLine($"Height: {report.Height}");
            Console.WriteLine($"Soil PH: {report.SoilPh}");
            Console.WriteLine($"Soil Moisture: {report.SoilMoisture}");
            Console.WriteLine($"Humidity: {report.Humidity}");
            Console.WriteLine($"Temperature: {report.SurroundTemperature}");
            Console.WriteLine($"CO2: {report.CO2Content}");
            Console.WriteLine($"Lux: {report.LuxContent}");
            Console.WriteLine($"Nitrogen: {report.NitrogenContent}");
            Console.WriteLine($"Phosphorus: {report.PhosphorusContent}"); 
            Console.WriteLine($"Potassium: {report.PotassiumContent}");       

            Console.Read();
        }
    }
}