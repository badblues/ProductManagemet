using System.Windows;
using ProductManager.ViewModels;

namespace ProductManager.Views;

public partial class MainWindow : Window
{
    public MainWindow(MainViewModel mainViewModel)
    {
        DataContext = mainViewModel;
        InitializeComponent();
    }
}