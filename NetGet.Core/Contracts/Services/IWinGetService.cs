using System.Collections.ObjectModel;
using NetGet.Core.Models;

namespace NetGet.Core.Contracts.Services;
public interface IWinGetService
{
    Task<IEnumerable<WinGetItem>> GetWinGetItemsAsync();
    Task<IEnumerable<string>> GetWinGetItemVersionsAsync(WinGetItem winGetItem);
}
