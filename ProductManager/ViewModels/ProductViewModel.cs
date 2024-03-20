using System.Globalization;
using System.Windows;
using System.Windows.Input;
using Domain.Models;
using Persistence.Repositories.Interfaces;
using ProductManager.Commands;
using ProductManager.Views;

namespace ProductManager.ViewModels;

public class ProductViewModel : ViewModel
{
    public event EventHandler? DeleteProductEvent;
    public event EventHandler? EditProductEvent;

    public ICommand DeleteProductCommand { get; set; }
    public ICommand EditProductCommand { get; set; }
    public ICommand AddUpProductCommand { get; set; }
    public ICommand EditCountCommand { get; set; }
    public ICommand SelectLinkCommand { get; set; }

    public Product CurrentProduct
    {
        get => _currentProduct;
        set
        {
            _currentProduct = value;
            OnPropertyChanged(nameof(CurrentProduct));
        }
    }

    public Product? SelectedUpProduct
    {
        get => _selectedUpProduct;
        set
        {
            _selectedUpProduct = value;
            OnPropertyChanged(nameof(SelectedUpProduct));
        }
    }

    public Link? SelectedLink
    {
        get => _selectedLink;
        set
        {
            _selectedLink = value;
            OnPropertyChanged(nameof(SelectedLink));
        }
    }

    public IEnumerable<Product> Products { get; set; }

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
            if (float.TryParse(value, NumberStyles.Float, null, out _) || value == "")
            {
                _enteredPrice = value;
                OnPropertyChanged(nameof(_enteredName));
            }
        }
    }

    public string EnteredCount
    {
        get => _enteredCount;
        set
        {
            if (int.TryParse(value, NumberStyles.Integer, null, out _) || value == "")
            {
                _enteredCount = value;
                OnPropertyChanged(nameof(EnteredCount));
            }
        }
    }

    public string EditCountText
    {
        get => _editCountText;
        set
        {
            if (int.TryParse(value, NumberStyles.Integer, null, out _) || value == "")
            {
                _editCountText = value;
                OnPropertyChanged(nameof(EditCountText));
            }
        }
    }

    private Product _currentProduct;
    private readonly IProductRepository _productRepository;
    private readonly ILinkRepository _linkRepository;

    private Product? _selectedUpProduct;
    private Link? _selectedLink;

    private string _enteredName;
    private string _enteredPrice;
    private string _enteredCount = "";
    private string _editCountText = "";

    public ProductViewModel(
        Product product,
        IProductRepository productRepository,
        ILinkRepository linkRepository)
    {
        _currentProduct = product;
        _productRepository = productRepository;
        _linkRepository = linkRepository;

        _enteredName = product.Name;
        _enteredPrice = product.Price.ToString();

        Products = _productRepository.GetAll();

        DeleteProductCommand = new RelayCommand(DeleteProduct, o => true);
        EditProductCommand = new RelayCommand(EditProduct, o => true);
        AddUpProductCommand = new RelayCommand(AddUpProduct, o => true);
        EditCountCommand = new RelayCommand(EditCount, o => true);
        SelectLinkCommand = new RelayCommand(SelectLink, o => true);
    }

    public void EditProduct(object? parameter)
    {
        //If product isn't changed we shouldn't call update
        bool productChanged = false;

        if (EnteredName?.Length > 0 && EnteredName != CurrentProduct.Name)
        {
            CurrentProduct.Name = EnteredName;
            productChanged = true;
        }

        if (float.TryParse(_enteredPrice, NumberStyles.Float, null, out float price)
            && price != CurrentProduct.Price)
        {
            CurrentProduct.Price = price;
            productChanged = true;
        }

        if (productChanged)
        {
            try
            {
                _productRepository.Update(CurrentProduct);
                EditProductEvent?.Invoke(this, EventArgs.Empty);
                MessageBox.Show("Saved", "Success", MessageBoxButton.OK);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        else
        {
            MessageBox.Show("Nothing to update", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    public void DeleteProduct(object? unused)
    {
        try
        {
            _productRepository.Delete(CurrentProduct.Id);
            DeleteProductEvent?.Invoke(this, EventArgs.Empty);
            MessageBox.Show("Deleted", "Success", MessageBoxButton.OK);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    public void AddUpProduct(object? unused)
    {
        if (SelectedUpProduct == null)
        {
            MessageBox.Show("Select UpProduct", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }
        if (int.TryParse(EnteredCount, NumberStyles.Integer, null, out int count))
        {
            try
            {
                Link link = new()
                {
                    ProductId = _currentProduct.Id,
                    UpProductId = SelectedUpProduct.Id,
                    Count = count
                };
                _linkRepository.Create(link);
                EditProductEvent?.Invoke(this, EventArgs.Empty);
                MessageBox.Show("UpProduct added", "Success", MessageBoxButton.OK);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        else
        {
            MessageBox.Show("Enter count", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    public void SelectLink(object? parameter)
    {
        if (parameter is Link link && SelectedLink == null)
        {
            SelectedLink = link;
            EditCountText = link.Count.ToString();
            EditCountWindow editCountWindow = new(this);
            editCountWindow.Show();
        }
        if (parameter == null)
        {
            SelectedLink = null;
        }
    }

    public void EditCount(object? unused)
    {
        if (int.TryParse(EditCountText, NumberStyles.Integer, null, out int count)
            && SelectedLink != null
            && SelectedLink.Count != count)
        {
            try
            {
                SelectedLink.Count = count;
                _linkRepository.Update(SelectedLink);

                //TODO: fix roundabout way of updating UI
                Product tmp = CurrentProduct;
                CurrentProduct = null;
                CurrentProduct = tmp;
                EditProductEvent?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
