using System.Security.Policy;
using System.Windows.Input;
using Domain.Models;
using Persistence;
using ProductManager.Core;
using ProductManager.Factories;
using ProductManager.Views;

namespace ProductManager.ViewModels;

public class MainViewModel : ViewModel
{
    public ICommand AddProductCommand { get; set; }
    public ICommand SelectProductCommand { get; set; }

    public IEnumerable<Product> Products
    {
        get => _products;
        set
        {
            _products = value;
            OnPropertyChanged(nameof(Products));
        }
    }

    public IEnumerable<Link> Links
    {
        get => _links;
        set
        {
            _links = value;
            OnPropertyChanged(nameof(Links));
        }
    }

    public string EnteredName { get; set; }
    public int EnteredPrice { get; set; }

    private readonly IProductRepository _productRepository;
    private readonly ILinkRepository _linkRepository;
    private readonly IProductViewModelFactory _productVMFactory;

    private IEnumerable<Product> _products;
    private IEnumerable<Link> _links;

    public MainViewModel(ILinkRepository linkRepository, IProductRepository productRepository, IProductViewModelFactory productVMFactory)
    {
        _productRepository = productRepository;
        _linkRepository = linkRepository;
        _productVMFactory = productVMFactory;

        Products = _productRepository.GetAll();
        Links = _linkRepository.GetAll();

        AddProductCommand = new RelayCommand(AddProduct, o => true);
        SelectProductCommand = new RelayCommand(SelectProduct, o => true);
        OnPropertyChanged(nameof(AddProductCommand));
    }

    public void AddProduct(object? unused)
    {
        if (EnteredName?.Length > 0)
        {
            Product newProduct = new() { Name = EnteredName, Price = EnteredPrice };
            _productRepository.Create(newProduct);
            Products = _productRepository.GetAll();
        }
    }

    public void SelectProduct(object? parameter)
    {
        if (parameter is Product product)
        {
            ProductViewModel productViewModel = _productVMFactory.CreateProductViewModel(product);
            productViewModel.DeleteProductEvent += (sender, args) =>
            {
                Products = _productRepository.GetAll();
            };
            productViewModel.EditProductEvent += (sender, args) =>
            {
                Products = _productRepository.GetAll();
            };
            ProductWindow productWindow = new ProductWindow(productViewModel);
            productWindow.Show();
        }
    }

    private void ProductViewModel_EditProductEvent(object? sender, EventArgs e)
    {
        throw new NotImplementedException();
    }
}
