using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoviewConsole.Manager;

namespace MoviewConsole.DataClasses
{
    class ReportData
    {
        public string Height { get; private set; }
        public string SoilPh { get; private set; }
        public string SoilMoisture { get; private set; }
        public string SurroundTemperature { get; private set; }
        public string Humidity { get; private set; }
        public string CO2Content { get; private set; }
        public string LuxContent { get; private set; }
        public string NitrogenContent { get; private set; }
        public string PhosphorusContent { get; private set; }
        public string PotassiumContent { get; private set; }
        public string Evaluation { get; private set; }
        public List<string> DataList { get; private set; }
        private ReportManager manager;

        public ReportData()
        {
            Height = "null";
            SoilPh = "null";
            SoilMoisture = "null";
            SurroundTemperature = "null";
            Humidity = "null";
            CO2Content = "null";
            LuxContent = "null";
            NitrogenContent = "null";
            PhosphorusContent = "null";
            PotassiumContent = "null";
            Evaluation = "null";
            DataList = new();
            manager = new();
        }

        public void Evaluate()
        {
            throw new NotImplementedException();
        }

        public async void Update()
        {
            await manager.ProcessData();
            manager.SyncCollectedData(DataList);           
        }
    }
}
