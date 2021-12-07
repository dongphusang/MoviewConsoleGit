using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        /// Extract content from the file
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public void ExtractRawContent()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Retrieves blob from the cloud from given day sector
        /// Debug> Three cases to be tested !>
        /// Debug> First case: Blob Exists and Downloaded to destination
        /// Debug> Second case: Blob doesn't exist, decrease sector by 1
        /// Debug> Third case: Blob doesn't exist, sector = 0, null.txt will be downloaded
        /// </summary>
        /// <param name="sector"> represents a specific time frame in a day. Ex: sector 0 = 0:00 to 2:59 in the morning</param>
        public async void RetrieveFile(int sector)
        {
            await DownloadBlob(sector);
        }

        private async Task DownloadBlob(int sector)
        {
            BlobName = $"{sector}.txt";
            try
            {
                BlobClient = BlobContainerClient.GetBlobClient(BlobName); 
                if (await BlobClient.ExistsAsync())
                {
                    Console.WriteLine("Blob Exists and Downloaded"); // debug
                    await BlobClient.DownloadToAsync($"C:\\Users\\sangs\\Desktop\\{BlobName}");
                }
                else
                {
                    if (sector != 0)
                    {
                        Console.WriteLine("Blob doesn't exist, decrease sector by 1, blob retrieved"); // debug
                        sector = sector - 1;
                        BlobName = $"{sector}.txt";
                        await BlobClient.DownloadToAsync($"C:\\Users\\sangs\\Desktop\\{BlobName}");
                    }
                    else
                    {
                        Console.WriteLine("lmao your data isn't here yet bruh :)"); // debug
                        BlobName = "null.txt";
                        await BlobClient.DownloadToAsync($"C:\\Users\\sangs\\Desktop\\{BlobName}");
                    }                     
                }
            } catch (RequestFailedException e)
            {
                Console.WriteLine($"HTTP request failed: {e.Message} {e.ErrorCode}");
            }
        }
    }
}
