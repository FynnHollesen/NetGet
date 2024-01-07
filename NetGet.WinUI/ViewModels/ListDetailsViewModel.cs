using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using NetGet.Core.Contracts.Services;
using NetGet.Core.Models;
using Microsoft.UI.Xaml;
using System.Linq;

namespace NetGet.WinUI.ViewModels;

public partial class ListDetailsViewModel : ObservableRecipient
{
    private readonly INetGetService _netGetService;

    [ObservableProperty]
    private ObservableCollection<NetGetItem> _netGetItems;

    [ObservableProperty]
    private NetGetItem? _selected;

    [ObservableProperty]
    private ObservableCollection<string> _versions;

    [ObservableProperty]
    private string _selectedVersion;

    [ObservableProperty]
    private bool _isLoadingData;

    public ListDetailsViewModel(INetGetService netGetService)
    {
        _netGetService = netGetService;
        _netGetItems = new ObservableCollection<NetGetItem>();
        _versions = new ObservableCollection<string>();
        _isLoadingData = true;
        _selectedVersion = string.Empty;

    }

    public async void ListDetailsPage_Loaded(object sender, RoutedEventArgs e)
    {
        var netGetItems = await Task.Run(async () => await _netGetService.GetNetGetItemsAsync());

        IsLoadingData = false;

        NetGetItems.Clear();
        foreach (var netGetItem in netGetItems)
        {
                NetGetItems.Add(netGetItem);
        }
    }

    public async void ListDetailsDetailContent_Loaded(object sender, DataContextChangedEventArgs e)
    {
        if (e.NewValue is not NetGetItem selectedNetGetItem)
        {
            return;
        }

        var versions = await Task.Run(async () => await _netGetService.GetNetGetItemVersionsAsync(selectedNetGetItem));

        Versions.Clear();
        Versions.Add("Latest Version");
        SelectedVersion = "Latest Version";

        foreach (var version in versions)
        {
            Versions.Add(version);
        }
    }
}