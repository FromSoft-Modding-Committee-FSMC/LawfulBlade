using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace LawfulBladeManager.Networking
{
    public class DownloadManager
    {
        readonly HttpClient client;

        public DownloadManager()
        {
            // Initialize HTTP Client
            client = new HttpClient
            {
                Timeout = TimeSpan.FromMilliseconds(1000)
            };

            downloads = new List<Download>();
        }

        public async void StartASyncDownload(string sourceUri, string destinationFile)
        {
            // Is the computer connected to the internet?
            if(!NetworkInterface.GetIsNetworkAvailable())
                return;

            // Create a valid URI structure
            if (!Uri.TryCreate(sourceUri, UriKind.Absolute, out Uri? uri))
                throw new ArgumentException("URI is invalid.");

            // Create a temporary file
            string tempFile = GetTemporaryFilename();

            // Begin reading internet file
            using (HttpResponseMessage response = await client.GetAsync(uri, HttpCompletionOption.ResponseHeadersRead))
            {
                // Make sure that we've got a successful response
                response.EnsureSuccessStatusCode();

                using (FileStream fs = File.Open(tempFile, FileMode.Open))
                {
                    using (Stream ns = await response.Content.ReadAsStreamAsync())
                    {
                        await ns.CopyToAsync(fs);
                    }
                }
            }

            // Assuming that's all done, we can now copy the temporary file to our final
            File.Copy(tempFile, destinationFile);
        }

        public static string GetTemporaryFilename()
        {
            return Path.ChangeExtension(Path.GetTempFileName(), "lbd");
        }
    }
}
