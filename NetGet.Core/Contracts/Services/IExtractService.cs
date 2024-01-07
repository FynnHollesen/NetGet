namespace NetGet.Core.Contracts.Services;
public interface IExtractService
{
    Task ExtractFileAsync(string originPath, string fileName, string destinationPath);
    Task ExtractFileAsync(string originPath, string fileName, string destinationPath, IProgress<double> progress);
}
