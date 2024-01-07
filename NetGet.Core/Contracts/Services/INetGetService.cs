using NetGet.Core.Models;

namespace NetGet.Core.Contracts.Services;
public interface INetGetService
{
    Task<IEnumerable<NetGetItem>> GetNetGetItemsAsync();
    Task<IEnumerable<string>> GetNetGetItemVersionsAsync(NetGetItem netGetItem);
}
