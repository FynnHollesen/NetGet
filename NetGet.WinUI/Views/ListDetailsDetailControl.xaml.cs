using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using NetGet.Core.Models;
using NetGet.WinUI.ViewModels;

namespace NetGet.WinUI.Views;

public sealed partial class ListDetailsDetailControl : UserControl
{
    public ListDetailsViewModel ViewModel
    {
        get;
    }

    public NetGetItem? ListDetailsMenuItem
    {
        get => GetValue(ListDetailsMenuItemProperty) as NetGetItem;
        set => SetValue(ListDetailsMenuItemProperty, value);
    }

    public static readonly DependencyProperty ListDetailsMenuItemProperty = DependencyProperty.Register("ListDetailsMenuItem", typeof(NetGetItem), typeof(ListDetailsDetailControl), new PropertyMetadata(null, OnListDetailsMenuItemPropertyChanged));

    public ListDetailsDetailControl()
    {
        ViewModel = App.GetService<ListDetailsViewModel>();
        InitializeComponent();
        DataContextChanged += ViewModel.ListDetailsDetailContent_Loaded;
        DataContextChanged += OnDataContextChanged;
    }

    private static void OnListDetailsMenuItemPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is ListDetailsDetailControl control)
        {
            control.ForegroundElement.ChangeView(0, 0, 1);
        }
    }

    private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        VersionsDropDownButton.Content = VersionsListView.SelectedItem;
        VersionsDropDownButton.Flyout.Hide();
    }

    private void OnDataContextChanged(object sender, DataContextChangedEventArgs e)
    {
        VersionsListView.SelectedIndex = 0;
        VersionsListView.ScrollIntoView(VersionsListView.Items.FirstOrDefault());
    }
}
