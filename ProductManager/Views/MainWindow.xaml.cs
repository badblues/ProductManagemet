using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;
using ProductManager.ViewModels;

namespace ProductManager.Views;

public partial class MainWindow : Window
{
    public MainWindow(MainViewModel mainViewModel)
    {
        DataContext = mainViewModel;
        InitializeComponent();
    }

    private void ExportFile_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new SaveFileDialog
        {
            Filter = "Excel Files (*.xlsx)|*xlsx",
            DefaultExt = ".xlsx",
            Title = "Export file"
        };

        if (dialog.ShowDialog() == true)
        {
            var viewModel = DataContext as MainViewModel;
            ICommand exportCommand = viewModel.ExportToExcelCommand;
            if (exportCommand.CanExecute(null))
            {
                exportCommand.Execute(dialog.FileName);
            }
        }
    }

    protected override void OnClosed(EventArgs e)
    {
        base.OnClosed(e);

        Application.Current.Shutdown();
    }
}