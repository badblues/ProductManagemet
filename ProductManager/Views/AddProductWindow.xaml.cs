using System.Windows;
using ProductManager.ViewModels;

namespace ProductManager.Views;

public partial class AddProductWindow : Window
{
    public AddProductWindow(AddProductViewModel addProductViewModel)
    {
        InitializeComponent();
        DataContext = addProductViewModel;
    }
}
