using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Azure;
using Azure.Storage.Blobs;
using MoviewConsole.Interface;

namespace MoviewConsole.Importer
{
    class ReportImporter : IImporter
    {
        public BlobServiceClient BlobServiceClient { get; private set; }
        public BlobContainerClient BlobContainerClient { get; private set; }
        public BlobClient BlobClient { get; private set; }
        public string BlobName { get; private set; }
        public string ContainerName { get; private set; }

        public ReportImporter ()
        {
            BlobServiceClient = new BlobServiceClient (IImporter.CONNECTION_STRING);
            BlobContainerClient = BlobServiceClient.GetBlobContainerClient(IImporter.CONTAINER_NAME);
            BlobClient = BlobContainerClient.GetBlobClient("Not Specified");
            BlobName = "Not specified";
            ContainerName = "Not specified";
        }

        /// <summary>
        /// Retrieves blob from the cloud from given day sector
        /// Go back one sector if blob of that particular time frame isn't in the cloud
        /// </summary>
        /// <debug>
        /// Debug> Three cases to be tested !>
        /// Debug> First case: Blob Exists and Downloaded to destination
        /// Debug> Second case: Blob doesn't exist, decrease sector by 1
        /// Debug> Third case: Blob doesn't exist, sector = 0, null.txt will be downloaded
        /// </debug>
        /// <param name="sector"> represents a specific time frame in a day. Ex: sector 0 = 0:00 to 2:59 in the morning</param>
        public async Task RetrieveFile(int sector)
        {
            BlobName = $"{sector}.txt";
            var fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), BlobName);
            try
            {
                BlobClient = BlobContainerClient.GetBlobClient(BlobName);
                if (await BlobClient.ExistsAsync())
                {
                    Console.WriteLine("Blob Exists and Downloaded"); // debug
                    await BlobClient.DownloadToAsync(fileName);
                }
                else
                {
                        while(!BlobClient.Exists())
                        {
                            Console.WriteLine("Blob doesn't exist, decrease sector by 1, blob of previous sector retrieved"); // debug
                            ModifyTargetBlob(sector, out fileName);
                            Console.WriteLine("ReportImporter.RetrieveFile().fileName(60): " + fileName);
                        }

                       await BlobClient.DownloadToAsync(fileName);      
                }
            }
            catch (RequestFailedException e)
            {
                Console.WriteLine($"HTTP request failed: {e.Message} {e.ErrorCode}");
            }

        }

        private void ModifyTargetBlob(int sector, out string fileName)
        {
            if (sector <= 7 && sector >= 0)
            {
                sector--;
                BlobName = $"{sector}.txt";
                fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), BlobName);
                BlobClient = BlobContainerClient.GetBlobClient(BlobName);
            }
            else
            {
                BlobName = $"{sector}null.txt";
                fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), BlobName);
                BlobClient = BlobContainerClient.GetBlobClient(BlobName);
            }
        }
    }
}
