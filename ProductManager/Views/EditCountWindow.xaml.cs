using System.Windows;
using ProductManager.ViewModels;

namespace ProductManager.Views;

public partial class EditCountWindow : Window
{
    public EditCountWindow(ProductViewModel productViewModel)
    {
        InitializeComponent();
        DataContext = productViewModel;
    }
}
