using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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

        /// <summary>
        /// Downloads a file from a URI, and returns a path to it.
        /// </summary>
        /// <param name="source">Uri to download the file from</param>
        /// <returns>If the URI points into the local file system, this path will be to the true file. If it was a network URI, it will be to a temporary file.</returns>
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

        public bool DownloadFileExists(Uri source)
        {
            // Make sure we're connected to the internet...
            if (!NetworkInterface.GetIsNetworkAvailable())
                throw new Exception("No internet connection!");

            // Poll for the file by doing a web request
            using HttpRequestMessage  request = new (HttpMethod.Head, source);
            using HttpResponseMessage response = httpClient.Send(request);

            return (response.StatusCode == HttpStatusCode.OK);
        }
    }
}
