using NetGet.Core.Models;

namespace NetGet.Core.Contracts.DbContexts;
public interface INetGetContext
{
    Task CreateDatabaseAndTableIfNotExistsAsync();
    Task UpsertNetGetDatabaseAsync(IEnumerable<WinGetItem> winGetItems);
    Task<IEnumerable<NetGetItem>> QueryNetGetItemsAsync();
}
