using System.Security.Policy;
using System.Windows.Input;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using Domain.Models;
using Persistence;
using ProductManager.Core;
using ProductManager.Factories;
using ProductManager.Views;

namespace ProductManager.ViewModels;

public class MainViewModel : ViewModel
{
    public ICommand OpenAddProductWindowCommand {  get; set; }
    public ICommand SelectProductCommand { get; init; }
    public ICommand RemoveLinkCommand { get; init; }
    public ICommand EditCountCommand { get; init; }
    public ICommand ExportToExcelCommand { get; init; }

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

    public string EnteredCount { get; set; } = "";

    private readonly IProductRepository _productRepository;
    private readonly ILinkRepository _linkRepository;
    private readonly IViewModelFactory _productVMFactory;
    private readonly AddProductViewModel _addProductViewModel;
    private readonly AddProductWindow _addProductWindow;

    private IEnumerable<Product> _products;
    private IEnumerable<Link> _links;

    public MainViewModel(
        ILinkRepository linkRepository,
        IProductRepository productRepository,
        IViewModelFactory productVMFactory,
        AddProductViewModel addProductViewModel,
        AddProductWindow addProductWindow)
    {
        _productRepository = productRepository;
        _linkRepository = linkRepository;
        _productVMFactory = productVMFactory;
        _addProductViewModel = addProductViewModel;
        _addProductViewModel.AddProductEvent += (sender, args) =>
        {
            Products = _productRepository.GetAll();
        };
        _addProductWindow = addProductWindow;

        Products = _productRepository.GetAll();
        Links = _linkRepository.GetAll();

        SelectProductCommand = new RelayCommand(SelectProduct, o => true);
        RemoveLinkCommand = new RelayCommand(RemoveLink, o => true);
        OpenAddProductWindowCommand = new RelayCommand(OpenAddProductWindow, o => true);
        ExportToExcelCommand = new RelayCommand(ExportToExcel, o => true);
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

    public void RemoveLink(object? parameter)
    {
        if (parameter is Link link)
        {
            _linkRepository.Delete(link.UpProductId, link.ProductId);
            Products = _productRepository.GetAll();
        }
    }

    public void OpenAddProductWindow(object? unused)
    {
        _addProductWindow.Show();
    }

    public void ExportToExcel(object? parameter)
    {
        if (parameter is string filePath)
        {
            using (XLWorkbook workbook = new XLWorkbook())
            {
                IXLWorksheet worksheet = workbook.Worksheets.Add("Products");

                worksheet.Cell(1, 1).Value = "Product";
                worksheet.Cell(1, 2).Value = "Count";
                worksheet.Cell(1, 3).Value = "Cost";
                worksheet.Cell(1, 4).Value = "Price";
                worksheet.Cell(1, 5).Value = "Total count";

                int row = 2;

                foreach (Product product in Products)
                {
                    worksheet.Cell(row, 1).Value = product.Name;
                    worksheet.Cell(row, 2).Value = "NaN";
                    worksheet.Cell(row, 3).Value = "NaN";
                    worksheet.Cell(row, 4).Value = product.Price;
                    worksheet.Cell(row, 5).Value = "NaN";
                    row++;
                }

                workbook.SaveAs(filePath);
            }


         }
    }
}
