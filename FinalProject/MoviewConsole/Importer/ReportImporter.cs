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
        private readonly BlobServiceClient blobServiceClient;
        private readonly BlobContainerClient blobContainerClient;
        private BlobClient blobClient;
        public string BlobName { get; private set; }
        public string ContainerName { get; private set; }

        public ReportImporter ()
        {
            blobServiceClient = new BlobServiceClient (IImporter.CONNECTION_STRING);
            blobContainerClient = blobServiceClient.GetBlobContainerClient(IImporter.CONTAINER_NAME);
            blobClient = blobContainerClient.GetBlobClient("Not Specified");
            BlobName = "Not specified";
            ContainerName = "Not specified";
        }

        /// <summary>
        /// Retrieves blob from the cloud from given day sector.
        /// Goes back one sector if blob of that particular time frame isn't in the cloud. Goes back
        /// until there is an available blob in the cloud, however, retrieves a "{sector}null.txt" if there are none
        /// </summary>
        /// <param name="sector">represents a specific time frame in a day</param>
        public async Task RetrieveFile(int sector)
        {
            BlobName = $"{sector}.txt";
            var fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), BlobName);
            try
            {
                blobClient = blobContainerClient.GetBlobClient(BlobName);
                if (await blobClient.ExistsAsync())
                {
                    //Console.WriteLine("Blob Exists and Downloaded"); 
                    await blobClient.DownloadToAsync(fileName);
                }
                else
                {
                    while(!blobClient.Exists())
                     {
                        Console.WriteLine("Blob doesn't exist, decrease sector by 1, blob of previous sector retrieved"); // debug
                        if (sector <= 7 && sector >= 0)
                        {
                            sector--;
                            BlobName = $"{sector}.txt";
                            fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), BlobName);
                            blobClient = blobContainerClient.GetBlobClient(BlobName);
                        }
                        else
                        {
                            BlobName = $"null.txt";
                            fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), BlobName);
                            blobClient = blobContainerClient.GetBlobClient(BlobName);
                        }
                        //Console.WriteLine("ReportImporter.RetrieveFile().fileName(60): " + fileName);
                    }
                       await blobClient.DownloadToAsync(fileName);      
                }
            }
            catch (RequestFailedException e)
            {
                Console.WriteLine($"HTTP request failed: {e.Message} {e.ErrorCode}");
            }
        }

        
    }
}
