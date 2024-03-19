using System.Windows;

namespace ProductManager.Views;

public partial class InputDialog : Window
{
    public int EnteredNumber { get; private set; }

    public InputDialog()
    {
        InitializeComponent();
    }

    private void OkButton_Click(object sender, RoutedEventArgs e)
    {
        if (int.TryParse(NumberTextBox.Text, out int number) && number >= 0)
        {
            EnteredNumber = number;
            DialogResult = true;
        }
        else
        {
            MessageBox.Show("Please enter a valid positive number.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
    }
}
