
using System.Windows.Input;
using Domain.Models;
using Persistence;
using ProductManager.Core;

namespace ProductManager.ViewModels;

public class AddProductViewModel : ViewModel
{
    public event EventHandler AddProductEvent;

    public string EnteredName { get; set; } = "";
    public string EnteredPrice { get; set; } = "";

    public ICommand AddProductCommand { get; init; }

    private readonly IProductRepository _productRepository;

    public AddProductViewModel(IProductRepository productRepository)
    {
        _productRepository = productRepository;

        AddProductCommand = new RelayCommand(AddProduct, o => true);
    }

    public void AddProduct(object? unused)
    {
        if (EnteredName.Length > 0 && EnteredPrice.Length > 0)
        {
            Product newProduct = new()
            {
                Name = EnteredName,
                Price = float.Parse(EnteredPrice)
            };
            _productRepository.Create(newProduct);
            AddProductEvent?.Invoke(this, EventArgs.Empty);
        }
    }
}
