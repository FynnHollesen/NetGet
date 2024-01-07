using Microsoft.UI.Xaml.Controls;
using NetGet.WinUI.ViewModels;

namespace NetGet.WinUI.Views;

public sealed partial class ListDetailsPage : Page
{
    public ListDetailsViewModel ViewModel
    {
        get;
    }

    public ListDetailsPage()
    {
        ViewModel = App.GetService<ListDetailsViewModel>();
        InitializeComponent();
        Loaded += ViewModel.ListDetailsPage_Loaded;
    }
}
