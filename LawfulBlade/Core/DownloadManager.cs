using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Text.Json;

namespace LawfulBlade.Core
{
    internal static class DownloadManager
    {
        /// <summary>
        /// Static instance of our HttpClient, which we use to download files.
        /// </summary>
        static HttpClient httpClient;

        /// <summary>
        /// Static Constructor<br/>
        /// Responsible for initializing anything we need for downloading
        /// </summary>
        static DownloadManager()
        {
            httpClient = new HttpClient()
            {
                Timeout = TimeSpan.FromMilliseconds(1000),
            };
        }

        /// <summary>
        /// Downloads a file into a temporary location.
        /// </summary>
        /// <param name="source">Uri of the source file</param>
        /// <param name="destination">Target path to download to</param>
        public static void DownloadSync(Uri source, string destination)
        {
            // If this URI is pointing to a file, we can just copy it to the destination
            if (source.IsFile)
            {
                File.Copy(source.LocalPath, destination, true);
                return;
            }

            // Otherwise, we need to check network connectivity before continuing
            if (!NetworkInterface.GetIsNetworkAvailable())
                throw new Exception($"No network connection!");

            // Now lets attempt to download that file...
            using Task<Stream> httpStream = httpClient.GetStreamAsync(source);
            httpStream.Wait();  // Wait for it to complete...

            // Wait for the stream task to complete
            using FileStream fileStream = new(destination, FileMode.OpenOrCreate);
            httpStream.Result.CopyTo(fileStream);
        }

        /// <summary>
        /// Sends a HTTP request to see if the download exists
        /// </summary>
        /// <param name="source">Uri of the source file</param>
        /// <returns>True if the file exists, false otherwise</returns>
        public static bool DownloadExists(Uri source)
        {
            // Make sure we're connected to the internet...
            if (!NetworkInterface.GetIsNetworkAvailable())
                throw new Exception("No internet connection!");

            // Poll for the file by doing a web request
            using HttpRequestMessage request = new(HttpMethod.Head, source);
            using HttpResponseMessage response = httpClient.Send(request);

            return (response.StatusCode == HttpStatusCode.OK);
        }

        /// <summary>
        /// Sends a HTTP request to recover the newest version...
        /// </summary>
        /// <param name="source"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static bool RequestVersion(Uri source, out UpdateInfo version)
        {
            // Make sure we're connected to the internet...
            if (!NetworkInterface.GetIsNetworkAvailable())
                throw new Exception("No internet connection!");

            // Poll for the file by doing a web request
            using HttpRequestMessage request = new(HttpMethod.Get, source);
            using HttpResponseMessage response = httpClient.Send(request);

            // Decode the response...
            using StreamReader sr = new StreamReader(response.Content.ReadAsStream());
            version = JsonSerializer.Deserialize<UpdateInfo>(sr.ReadToEnd());

            return (response.StatusCode == HttpStatusCode.OK);
        }
    }
}
