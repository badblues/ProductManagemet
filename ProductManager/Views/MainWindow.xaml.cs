﻿using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;
using ProductManager.Core;
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
        InputDialog numberDialog = new InputDialog();

        var dialog = new SaveFileDialog
        {
            Filter = "Excel Files (*.xlsx)|*xlsx",
            DefaultExt = ".xlsx",
            Title = "Export file"
        };

        if (numberDialog.ShowDialog() == true)
        {
            if (dialog.ShowDialog() == true)
            {
                var viewModel = DataContext as MainViewModel;
                ICommand exportCommand = viewModel.ExportToExcelCommand;
                if (exportCommand.CanExecute(null))
                {
                    exportCommand.Execute(new ProductExportArgs { FileName = dialog.FileName, MaxLevel = numberDialog.EnteredNumber });
                }
            }
        }


    }

    protected override void OnClosed(EventArgs e)
    {
        base.OnClosed(e);

        Application.Current.Shutdown();
    }
}