namespace NetGet.Core.Contracts.Services;

public interface IDownloadService
{
    Task DownloadFileAsync(string url, string destinationPath);
    Task DownloadFileAsync(string url, string destinationPath, IProgress<double> progress);
}