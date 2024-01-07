using NetGet.Core.Contracts.Services;

namespace NetGet.Core.Services;

public class DownloadService : IDownloadService
{
    /// <summary>
    /// Downloads a file from the specified URL and saves it to the specified destination path.
    /// </summary>
    /// <param name="url">The URL of the file to download.</param>
    /// <param name="destinationPath">The destination path where the file will be saved.</param>
    public async Task DownloadFileAsync(string url, string destinationPath)
    {
        using var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36 Edg/91.0.864.48");
        using var response = await httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"The request returned with HTTP status code {response.StatusCode}");
        }

        var contentLength = response.Content.Headers.ContentLength ?? -1;
        using var contentStream = await response.Content.ReadAsStreamAsync();

        var destinationDirectory = Path.GetDirectoryName(destinationPath);
        if (!Directory.Exists(destinationDirectory))
        {
            Directory.CreateDirectory(destinationDirectory);
        }

        using var fileStream = new FileStream(destinationPath, FileMode.Create, FileAccess.Write, FileShare.None, 8192, true);
        var buffer = new byte[8192];
        int bytesRead;

        while ((bytesRead = await contentStream.ReadAsync(buffer)) != 0)
        {
            await fileStream.WriteAsync(buffer.AsMemory(0, bytesRead));
        }
    }

    /// <summary>
    /// Downloads a file from the specified URL and saves it to the specified destination path.
    /// <param name="url">The URL of the file to download.</param>
    /// <param name="destinationPath">The destination path where the file will be saved.</param>
    /// <param name="progress">The progress of the download.</param>
    /// </summary>
    public async Task DownloadFileAsync(string url, string destinationPath, IProgress<double> progress)
    {
        using var httpClient = new HttpClient();
        using var response = await httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"The request returned with HTTP status code {response.StatusCode}");
        }

        var contentLength = response.Content.Headers.ContentLength ?? -1;
        using var contentStream = await response.Content.ReadAsStreamAsync();

        var destinationDirectory = Path.GetDirectoryName(destinationPath);
        if (!Directory.Exists(destinationDirectory))
        {
            Directory.CreateDirectory(destinationDirectory);
        }

        using var fileStream = new FileStream(destinationPath, FileMode.Create, FileAccess.Write, FileShare.None, 8192, true);
        var buffer = new byte[8192];
        int bytesRead;
        long totalRead = 0;

        while ((bytesRead = await contentStream.ReadAsync(buffer)) != 0)
        {
            await fileStream.WriteAsync(buffer.AsMemory(0, bytesRead));
            totalRead += bytesRead;
            if (contentLength != -1)
            {
                progress.Report((double)totalRead / contentLength);
            }
        }
    }
}
