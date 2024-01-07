using System.Collections.ObjectModel;
using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace NetGet.Core.Models;

public class WinGetItem : ObservableObject
{
    public WinGetItem()
    {
        Versions = new List<string>();
    }

    public string Id
    {
        get; set;
    }
    public string Publisher
    {
        get; set;
    }
    public string Name
    {
        get; set;
    }
    public List<string> Versions
    {
        get; set;
    }
}
