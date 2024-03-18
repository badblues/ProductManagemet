using System.Windows;
using System.Windows.Input;
using Domain.Models;
using Persistence;
using ProductManager.Core;

namespace ProductManager.ViewModels;

public class ProductViewModel : ViewModel
{
    public event EventHandler DeleteProductEvent;

    public event EventHandler EditProductEvent;

    public ICommand DeleteCommand { get; set; }
    public ICommand EditCommand { get; set; }
    public ICommand AddUpProductCommand { get; set; }
    public ICommand EditLinkCommand { get; set; }

    public Product CurrentProduct
    {
        get => _currentProduct;
        set
        {
            _currentProduct = value;
            OnPropertyChanged(nameof(CurrentProduct));
        }
    }

    public IEnumerable<Product> Products { get; set; }

    public Product SelectedUpProduct
    {
        get => _selectedUpProduct;
        set
        {
            _selectedUpProduct = value;
            OnPropertyChanged(nameof(SelectedUpProduct));
        }
    }

    public string EnteredName
    {
        get => _enteredName;
        set
        {
            _enteredName = value;
            OnPropertyChanged(nameof(EnteredName));
        }
    }

    public string EnteredPrice
    {
        get => _enteredPrice;
        set
        {
            _enteredPrice = value;
            OnPropertyChanged(nameof(EnteredPrice));
        }
    }

    public string EnteredCount
    {
        get => _enteredCount;
        set
        {
            _enteredCount = value;
            OnPropertyChanged(nameof(EnteredCount));
        }
    }

    private Product _currentProduct;
    private readonly IProductRepository _productRepository;
    private readonly ILinkRepository _linkRepository;

    private Product _selectedUpProduct;

    private string _enteredName;
    private string _enteredPrice;
    private string _enteredCount;

    public ProductViewModel(Product product, IProductRepository productRepository, ILinkRepository linkRepository)
    {
        _currentProduct = product;
        _productRepository = productRepository;
        _linkRepository = linkRepository;

        _enteredName = product.Name;
        _enteredPrice = product.Price.ToString();

        Products = _productRepository.GetAll();

        DeleteCommand = new RelayCommand(DeleteProduct, o => true);
        EditCommand = new RelayCommand(EditProduct, o => true);
        AddUpProductCommand = new RelayCommand(AddUpProduct, o => true);
        EditLinkCommand = new RelayCommand(EditLink, o => true);
    }

    public void EditProduct(object? unused)
    {
        Product updatedProduct = CurrentProduct;
        updatedProduct.Name = EnteredName;
        updatedProduct.Price = float.Parse(EnteredPrice);
        _productRepository.Update(updatedProduct);
        CurrentProduct = updatedProduct;
        EditProductEvent?.Invoke(this, EventArgs.Empty);
        MessageBox.Show("Saved", "Success", MessageBoxButton.OK);
    }

    public void DeleteProduct(object? unused)
    {
        _productRepository.Delete(CurrentProduct.Id);
        DeleteProductEvent?.Invoke(this, EventArgs.Empty);
        MessageBox.Show("Deleted", "Success", MessageBoxButton.OK);
    }

    public void AddUpProduct(object? unused)
    {
        if (_currentProduct != null)
        { 
            Link link = new Link
            {
                ProductId = _currentProduct.Id,
                UpProductId = SelectedUpProduct.Id,
                Count = int.Parse(EnteredCount)
            };
            _linkRepository.Create(link);
            EditProductEvent?.Invoke(this, EventArgs.Empty);
            MessageBox.Show("UpProduct added", "Success", MessageBoxButton.OK);
        }
    }

    public void EditLink(object? parameter)
    {
        if (parameter is Link link)
        {
            _linkRepository.Update(link);
        } 
    }

}
