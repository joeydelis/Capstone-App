using MauiApp1.Classes.Models;
namespace MauiApp1.Pages;

public partial class BluetoothConnectionPage : ContentPage
{
    private readonly BleViewModel viewModel = new();
    public BluetoothConnectionPage()
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}