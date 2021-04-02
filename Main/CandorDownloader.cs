using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Threading.Tasks;
using CandorUpdater.Utils;
using Serilog;
using static System.Net.HttpWebRequest;

namespace CandorUpdater.Main
{
    public class CandorDownloader
    {
        public static async Task DownloadCandor()
        {
            if (CheckDownloaded())
            {
                Log.Information("Candor already downloaded, skipping.");
                return;
            }
            
            Log.Information("We are now downloading the latest version of candor");
            const string candorUrl = "http://localhost:8080/download/candor/latest";
            var request = (HttpWebRequest)WebRequest.Create(candorUrl);
            string fileName;
            var destinationPath = Path.Combine(Environment.CurrentDirectory, "download");
            if (!Directory.Exists(destinationPath))
            {
                Directory.CreateDirectory(destinationPath);
            }

            using var response = (HttpWebResponse)request.GetResponseAsync().Result;
            var path = response.Headers["Content-Disposition"];
            if (string.IsNullOrWhiteSpace(path))
            {
                var uri = new Uri(candorUrl);
                fileName = Path.GetFileName(uri.LocalPath);
            }
            else
            {
                var contentDisposition = new ContentDisposition(path);
                fileName = contentDisposition.FileName;
            }

            var responseStream = response.GetResponseStream();
                
            var finalPath = Path.Combine(destinationPath, fileName);
            await using var fileStream = File.Create(finalPath);
            await responseStream.CopyToAsync(fileStream);
            
            response.Close();
            fileStream.Close();
            
            await UnzipCandor(finalPath);
        }

        private static bool CheckDownloaded()
        {
            return File.Exists(Path.Combine(Path.Combine(Environment.CurrentDirectory, "download"), "CandorModManager.zip"));
        }

        private static async Task UnzipCandor(string candorPath)
        {
            Log.Information(
                $"Attempting to extract file: {Path.GetFileName(candorPath)} to location {Environment.CurrentDirectory}");
            await Task.Run(() =>
            {
                ZipFile.ExtractToDirectory(candorPath, Environment.CurrentDirectory, true);
            });
            Log.Information("Extracted Successfully");
        }
    }
}