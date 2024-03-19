using System.ComponentModel;
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

    private void Window_Closing(object sender, CancelEventArgs e)
    {
        var viewModel = DataContext as ProductViewModel;
        viewModel?.SelectLinkCommand.Execute(null);
    }
}
