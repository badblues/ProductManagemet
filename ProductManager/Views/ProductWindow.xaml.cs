using System.Windows;
using ProductManager.ViewModels;

namespace ProductManager.Views;

public partial class ProductWindow : Window
{
    public ProductWindow(ProductViewModel productViewModel)
    {
        InitializeComponent();
        DataContext = productViewModel;
        productViewModel.DeleteProductEvent += (sender, args) => Close();
    }
}
