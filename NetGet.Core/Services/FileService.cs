using System.Text;

using NetGet.Core.Contracts.Services;

using Newtonsoft.Json;

namespace NetGet.Core.Services;

public class FileService : IFileService
{
    /// <summary>
    /// Reads the content of a file and deserializes it into an object of type T.
    /// </summary>
    /// <typeparam name="T">The type of the object to deserialize.</typeparam>
    /// <param name="folderPath">The path to the folder containing the file.</param>
    /// <param name="fileName">The name of the file to read.</param>
    /// <returns>The deserialized object of type T.</returns>
    public T Read<T>(string folderPath, string fileName)
    {
        var path = Path.Combine(folderPath, fileName);
        if (File.Exists(path))
        {
            var json = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<T>(json);
        }

        return default;
    }

    /// <summary>
    /// Saves the content of an object as a file.
    /// </summary>
    /// <typeparam name="T">The type of the object to serialize.</typeparam>
    /// <param name="folderPath">The path to the folder where the file will be saved.</param>
    /// <param name="fileName">The name of the file to save.</param>
    /// <param name="content">The object to serialize and save as a file.</param>
    public void Save<T>(string folderPath, string fileName, T content)
    {
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        var fileContent = JsonConvert.SerializeObject(content);
        File.WriteAllText(Path.Combine(folderPath, fileName), fileContent, Encoding.UTF8);
    }

    /// <summary>
    /// Deletes a file.
    /// </summary>
    /// <param name="folderPath">The path to the folder containing the file.</param>
    /// <param name="fileName">The name of the file to delete.</param>
    public void Delete(string folderPath, string fileName)
    {
        if (fileName != null && File.Exists(Path.Combine(folderPath, fileName)))
        {
            File.Delete(Path.Combine(folderPath, fileName));
        }
    }
}
