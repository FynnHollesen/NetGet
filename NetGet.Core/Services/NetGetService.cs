using System.Data.SQLite;
using NetGet.Core.Contracts.DbContexts;
using NetGet.Core.Contracts.Services;
using NetGet.Core.DbContexts;
using NetGet.Core.Models;

namespace NetGet.Core.Services;
public class NetGetService : INetGetService
{
    private readonly IConfigurationService _configurationService;
    private readonly INetGetContext _netGetContext;
    private readonly IWinGetService _winGetService;

    private Task<IEnumerable<NetGetItem>> _getNetGetItemsTask;

    public List<NetGetItem> NetGetItems { get; private set; } = new List<NetGetItem>();

    public NetGetService(IConfigurationService configurationService, IWinGetService winGetService)
    {
        _configurationService = configurationService;
        _netGetContext = new NetGetContext(configurationService);
        _winGetService = winGetService;
    }

    /// <summary>
    /// Retrieves the NetGetItems asynchronously.
    /// </summary>
    /// <returns>A collection of NetGetItems.</returns>
    public async Task<IEnumerable<NetGetItem>> GetNetGetItemsAsync()
    {
        if (_getNetGetItemsTask != null && !_getNetGetItemsTask.IsCompleted)
        {
            return await _getNetGetItemsTask;
        }

        _getNetGetItemsTask = GetNetGetItemsImplementationAsync();
        return await _getNetGetItemsTask;
    }

    /// <summary>
    /// Retrieves the versions of a NetGetItem from the WinGet service.
    /// </summary>
    /// <param name="netGetItem">The NetGetItem to retrieve versions for.</param>
    /// <returns>A collection of strings representing the versions of the NetGetItem.</returns>
    public async Task<IEnumerable<string>> GetNetGetItemVersionsAsync(NetGetItem netGetItem)
    {
        return (await _winGetService.GetWinGetItemVersionsAsync(netGetItem));
    }

    private async Task<IEnumerable<NetGetItem>> GetNetGetItemsImplementationAsync()
    {
        if (NetGetItems.Any())
        {
            return NetGetItems;
        }

        if (!File.Exists(_configurationService.WinGetDatabasePath))
        {
            await _netGetContext.CreateDatabaseAndTableIfNotExistsAsync();
            var winGetItems = await _winGetService.GetWinGetItemsAsync();
            await _netGetContext.UpsertNetGetDatabaseAsync(winGetItems);
        }

        NetGetItems = (await _netGetContext.QueryNetGetItemsAsync()).ToList();
        return NetGetItems;
    }
}
