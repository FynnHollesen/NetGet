using System.IO.Compression;
using NetGet.Core.Contracts.Services;

namespace NetGet.Core.Services;

public class ExtractService : IExtractService
{
    /// <summary>
    /// Extracts a file from a zip archive.
    /// </summary>
    /// <param name="originPath">The path to the zip archive.</param>
    /// <param name="fileName">The name of the file to extract.</param>
    /// <param name="destinationPath">The path where the extracted file will be saved.</param>
    public async Task ExtractFileAsync(string originPath, string fileName, string destinationPath)
    {
        using var stream = new FileStream(originPath, FileMode.Open);
        using var archive = new ZipArchive(stream);
        var entry = archive.GetEntry(fileName);

        if (entry != null)
        {
            using var entryStream = entry.Open();
            using var fileStream = new FileStream(destinationPath, FileMode.Create);
            var buffer = new byte[8192];
            int bytesRead;

            while ((bytesRead = await entryStream.ReadAsync(buffer)) != 0)
            {
                await fileStream.WriteAsync(buffer.AsMemory(0, bytesRead));
            }
        }
    }

    /// <summary>
    /// Extracts a file from a zip archive.
    /// </summary>
    /// <param name="originPath">The path to the zip archive.</param>
    /// <param name="fileName">The name of the file to extract.</param>
    /// <param name="destinationPath">The path where the extracted file will be saved.</param>
    /// <param name="progress">The progress of the extraction.</param>
    public async Task ExtractFileAsync(string originPath, string fileName, string destinationPath, IProgress<double> progress)
    {
        using var stream = new FileStream(originPath, FileMode.Open);
        using var archive = new ZipArchive(stream);
        var entry = archive.GetEntry(fileName);

        if (entry != null)
        {
            using var entryStream = entry.Open();
            using var fileStream = new FileStream(destinationPath, FileMode.Create);
            var buffer = new byte[8192];
            int bytesRead;
            long totalRead = 0;

            while ((bytesRead = await entryStream.ReadAsync(buffer)) != 0)
            {
                await fileStream.WriteAsync(buffer.AsMemory(0, bytesRead));
                totalRead += bytesRead;
                progress.Report((double)totalRead / entry.Length);
            }
        }
    }
}