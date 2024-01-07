using NetGet.Core.Contracts.Services;

namespace NetGet.Core.Services;
public class ConfigurationService : IConfigurationService
{
    public string ExecutableName
    {
        get;
    }
    public string DataDirectoryPath
    {
        get;
    }
    public string NetGetDatabasePath
    {
        get;
    }
    public string WinGetDatabasePath
    {
        get;
    }
    public string WinGetSourcePath
    {
        get;
    }
    public string WinGetSourceUrl
    {
        get;
    }

    public ConfigurationService()
    {
        ExecutableName = "NetGet.WinUI.exe";
        DataDirectoryPath = Environment.ProcessPath.Replace(ExecutableName, "Data");
        NetGetDatabasePath = Environment.ProcessPath.Replace(ExecutableName, "Data\\NetGetDatabase.db");
        WinGetDatabasePath = Environment.ProcessPath.Replace(ExecutableName, "Data\\WinGetDatabase.db");
        WinGetSourcePath = Environment.ProcessPath.Replace(ExecutableName, "Data\\WinGetSource.msix");
        WinGetSourceUrl = "https://cdn.winget.microsoft.com/cache/source.msix";
    }
}
