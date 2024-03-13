using System.Windows.Input;
using Domain.Models;
using Persistence;
using ProductManager.Core;

namespace ProductManager.ViewModels;

public class MainViewModel : ViewModel
{

    private IProductRepository _productRepository;
    private ILinkRepository _linkRepository;

    private IEnumerable<Product> _products;
    private IEnumerable<Link> _links;

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

    public string EnteredName { get; set; } = "asdjflkajs;dlkf";
    public int EnteredPrice { get; set; }

    public ICommand AddProductCommand { get; init; } 

    public MainViewModel(ILinkRepository linkRepository, IProductRepository productRepository)
    {
        _productRepository = productRepository;
        _linkRepository = linkRepository;

        Products = _productRepository.GetAll();
        Links = _linkRepository.GetAll();

        AddProductCommand = new RelayCommand(AddProduct, o => true);
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

}
