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
        /// Retrieves blob from the cloud from given day sector.
        /// Goes back one sector if blob of that particular time frame isn't in the cloud. Goes back
        /// until there is an available blob in the cloud, however, retrieves a "{sector}null.txt" if there are none
        /// </summary>
        /// <DEBUG>
        /// fix block 54-59
        /// </DEBUG>
        /// <param name="sector">represents a specific time frame in a day</param>
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
                    // this for most cases is fine. Although, if there are none targeted blob availble, the program will stuck in an infinite loop
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

        /// <summary>
        /// Modify BlobName based on given sector.
        /// </summary>
        /// <param name="sector">representing the current time frame</param>
        /// <param name="fileName">the local file name representing the downloaded blob</param>
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
