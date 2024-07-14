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
        readonly HttpClient httpClient;

        public DownloadManager()
        {
            // Initialize HTTP Client
            httpClient = new HttpClient
            {
                Timeout = TimeSpan.FromMilliseconds(1000),
            };
        }

        public string DownloadFileSync(Uri source)
        {
            // If this is a file Uri we just return the local path (?)
            if (source.IsFile)
                return source.LocalPath;

            // It wasn't a file uri, so it's hosted somewhere. Make sure we have network connection.
            if (!NetworkInterface.GetIsNetworkAvailable())
                throw new Exception($"Cannot download file from '{source}'. No network connection!");

            // Lets download the file into a temporary file for now.
            string temporaryFile = Path.GetTempFileName();

            using(Task<Stream> httpStream = httpClient.GetStreamAsync(source))
            {
                // Wait for the stream task to complete
                using(FileStream fileStream = new (temporaryFile, FileMode.OpenOrCreate))
                    httpStream.Result.CopyTo(fileStream);

            }

            return temporaryFile;
        }
    }
}
