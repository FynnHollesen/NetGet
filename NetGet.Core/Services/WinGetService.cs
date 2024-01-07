using NetGet.Core.Contracts.DbContexts;
using NetGet.Core.Contracts.Services;
using NetGet.Core.DbContexts;
using NetGet.Core.Models;

namespace NetGet.Core.Services;
public class WinGetService : IWinGetService
{
    private readonly IConfigurationService _configurationService;
    private readonly IDownloadService _downloadService;
    private readonly IExtractService _extractService;
    private readonly IWinGetContext _winGetContext;
    private Task<IEnumerable<WinGetItem>> _getWinGetItemsTask;

    public IEnumerable<WinGetItem> WinGetItems { get; private set; } = new List<WinGetItem>();

    public WinGetService(IConfigurationService configurationService, IDownloadService downloadService, IExtractService extractService)
    {
        _configurationService = configurationService;
        _downloadService = downloadService;
        _extractService = extractService;
        _winGetContext = new WinGetContext(_configurationService);
    }

    /// <summary>
    /// Retrieves the WinGet items asynchronously.
    /// </summary>
    /// <returns>The collection of WinGet items.</returns>
    public async Task<IEnumerable<WinGetItem>> GetWinGetItemsAsync()
    {
        if (_getWinGetItemsTask != null && !_getWinGetItemsTask.IsCompleted)
        {
            return await _getWinGetItemsTask;
        }

        _getWinGetItemsTask = GetWinGetItemsImplementationAsync();
        return await _getWinGetItemsTask;
    }

    /// <summary>
    /// Retrieves the versions of a specific WinGet item.
    /// </summary>
    /// <param name="winGetItem">The WinGet item to retrieve versions for.</param>
    public async Task<IEnumerable<string>> GetWinGetItemVersionsAsync(WinGetItem winGetItem)
    {
        
        return await _winGetContext.QueryWinGetItemVersionsAsync(winGetItem);
    }

    private async Task<IEnumerable<WinGetItem>> GetWinGetItemsImplementationAsync()
    {
        if (WinGetItems.Any())
        {
            return WinGetItems;
        }

        if (!File.Exists(_configurationService.WinGetDatabasePath))
        {
            await UpdateWinGetDatabaseAsync();
        }

        return await ReadWinGetItemsFromDatabaseAsync();
    }
    private async Task<IEnumerable<WinGetItem>> ReadWinGetItemsFromDatabaseAsync()
    {
        return WinGetItems = await _winGetContext.QueryWinGetItemsAsync();
    }
    private async Task UpdateWinGetDatabaseAsync()
    {
        await _downloadService.DownloadFileAsync(_configurationService.WinGetSourceUrl, _configurationService.WinGetSourcePath);
        await _extractService.ExtractFileAsync(_configurationService.WinGetSourcePath, "Public/index.db", _configurationService.WinGetDatabasePath);
    }
}
