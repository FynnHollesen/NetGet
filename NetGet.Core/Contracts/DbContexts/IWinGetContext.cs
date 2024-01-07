using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetGet.Core.Models;

namespace NetGet.Core.Contracts.DbContexts;
public interface IWinGetContext
{
    Task<IEnumerable<WinGetItem>> QueryWinGetItemsAsync();
    Task<IEnumerable<string>> QueryWinGetItemVersionsAsync(WinGetItem winGetItem);
}
