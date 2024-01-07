using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetGet.Core.Contracts.Services;
public interface IConfigurationService
{
    string ExecutableName
    {
        get;
    }
    string DataDirectoryPath
    {
        get;
    }
    string NetGetDatabasePath
    {
        get;
    }
    string WinGetDatabasePath
    {
        get;
    }
    string WinGetSourcePath
    {
        get;
    }
    string WinGetSourceUrl
    {
        get;
    }
}
