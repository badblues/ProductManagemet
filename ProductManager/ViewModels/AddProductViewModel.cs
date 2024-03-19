using System.Globalization;
using System.Windows;
using System.Windows.Input;
using Domain.Models;
using Persistence.Repositories.Interfaces;
using ProductManager.Commands;

namespace ProductManager.ViewModels;

public class AddProductViewModel : ViewModel
{
    public event EventHandler? AddProductEvent;

    public string EnteredName { get; set; } = "";

    public string EnteredPrice
    {
        get => _enteredPrice;
        set
        {
            if (float.TryParse(value, NumberStyles.Float, null, out _) || value == "")
                _enteredPrice = value;
        }
    }

    public ICommand AddProductCommand { get; init; }

    private readonly IProductRepository _productRepository;

    private string _enteredPrice = "";

    public AddProductViewModel(IProductRepository productRepository)
    {
        _productRepository = productRepository;
        AddProductCommand = new RelayCommand(AddProduct, o => true);
    }

    public void AddProduct(object? unused)
    {
        if (EnteredName?.Length > 0
            && float.TryParse(EnteredPrice, NumberStyles.Float, null, out float price))
        {
            try
            {
                Product newProduct = new()
                {
                    Name = EnteredName,
                    Price = price
                };
                _productRepository.Create(newProduct);
                AddProductEvent?.Invoke(this, EventArgs.Empty);
                MessageBox.Show("Created", "Success", MessageBoxButton.OK);
            }
            catch (Exception)
            {
                MessageBox.Show("Error creating product", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        else
        {
            MessageBox.Show("Invalid data", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
