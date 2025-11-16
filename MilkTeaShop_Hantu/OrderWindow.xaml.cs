using MilkTeaShop.BLL.Services;
using MilkTeaShop.DAL.Entities;
using MilkTeaShop_Hantu.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using Microsoft.Extensions.Configuration;

namespace MilkTeaShop_Hantu
{
    public partial class OrderWindow : Window
    {
        private readonly CategoryServices _categoryService = new();
        private readonly ProductServices _productService = new();
        private readonly OrderServices _orderService = new();
        private readonly PaymentServices _paymentService = new();

        private ObservableCollection<CartItem> Cart { get; set; } = new();
        private readonly int _tableId;
        private readonly TableWindow? _parentWindow;

        // Pagination properties
        private List<Product> _allProducts = new();
        private List<Product> _filteredProducts = new();
        private int _currentPage = 1;
        private const int _itemsPerPage = 12;
        private bool _showingAll = false;

        // Category selection
        private Category? _selectedCategory;

        public OrderWindow(int tableId, TableWindow parent)
        {
            InitializeComponent();
            _tableId = tableId;
            _parentWindow = parent;
            Init();
        }

        public OrderWindow(int tableId)
        {
            InitializeComponent();
            _tableId = tableId;
            _parentWindow = null;
            Init();
        }

        public OrderWindow()
        {
            InitializeComponent();
            _tableId = 0;
            _parentWindow = null;
            Init();
        }

        private void Init()
        {
            LoadLogo();
            LoadCategories();
            LoadAllProducts();
            CartList.ItemsSource = Cart;

            if (_tableId > 0)
                LoadExistingOrders();
        }

        private void LoadLogo()
        {
            try
            {
                // Load ảnh FullLogo.jpg từ thư mục Images
                string logoPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images", "FullLogo.jpg");
                if (File.Exists(logoPath))
                {
                    CategoryLogoImage.Source = new BitmapImage(new Uri(logoPath, UriKind.Absolute));
                }
                else
                {
                    // Thử tìm trong thư mục gốc project
                    string fallbackPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "FullLogo.jpg");
                    if (File.Exists(fallbackPath))
                    {
                        CategoryLogoImage.Source = new BitmapImage(new Uri(fallbackPath, UriKind.Absolute));
                    }
                    else
                    {
                        CategoryLogoImage.Visibility = Visibility.Collapsed;
                    }
                }
            }
            catch (Exception ex)
            {
                // Nếu không load được logo thì ẩn đi
                CategoryLogoImage.Visibility = Visibility.Collapsed;
                System.Diagnostics.Debug.WriteLine($"Không load được logo FullLogo.jpg: {ex.Message}");
            }
        }

        private void LoadExistingOrders()
        {
            var existingOrders = _orderService.GetOrdersByTable(_tableId);
            foreach (var order in existingOrders)
            {
                foreach (var item in order.OrderDetails)
                {
                    Cart.Add(new CartItem
                    {
                        ProductId = item.ProductId,
                        ProductName = item.Product.ProductName,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice
                    });
                }
            }
            RefreshCart();
        }

        private void LoadCategories()
        {
            var categories = _categoryService.GetAllServices();
            DisplayCategories(categories);
        }

        private void DisplayCategories(List<Category> categories)
        {
            CategoryPanel.Children.Clear();

            // Thêm button "Tất cả" đầu tiên
            var allButton = CreateCategoryButton(null, "🏠 Tất cả", true);
            CategoryPanel.Children.Add(allButton);

            // Icon cho từng category
            var categoryIcons = new Dictionary<string, string>
            {
                { "Đồ uống", "🧋" },
                { "Topping", "🍯" },
                { "Đồ ăn kèm", "🍰" },
                { "Nước trái cây", "🧃" },
                { "Cà phê", "☕" },
                { "Trà sữa", "🥤" },
                { "Sinh tố", "🥤" },
                { "Bánh ngọt", "🧁" }
            };

            foreach (var category in categories)
            {
                var icon = categoryIcons.ContainsKey(category.CategoryName) 
                    ? categoryIcons[category.CategoryName] 
                    : "📂";
                
                var button = CreateCategoryButton(category, $"{icon} {category.CategoryName}", false);
                CategoryPanel.Children.Add(button);
            }
        }

        private Border CreateCategoryButton(Category? category, string displayText, bool isAllButton)
        {
            var border = new Border
            {
                Background = new SolidColorBrush(Color.FromRgb(255, 255, 255)),
                BorderBrush = new SolidColorBrush(Color.FromRgb(226, 232, 240)),
                BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(8),
                Margin = new Thickness(2, 3, 2, 3),
                Padding = new Thickness(12, 10, 12, 10),
                Cursor = Cursors.Hand,
                Tag = category
            };

            // Shadow effect
            border.Effect = new DropShadowEffect
            {
                Color = Colors.Black,
                Direction = 320,
                ShadowDepth = 1,
                Opacity = 0.1,
                BlurRadius = 3
            };

            var textBlock = new TextBlock
            {
                Text = displayText,
                FontSize = 13,
                FontWeight = FontWeights.SemiBold,
                Foreground = new SolidColorBrush(Color.FromRgb(74, 85, 104)),
                TextAlignment = TextAlignment.Left,
                VerticalAlignment = VerticalAlignment.Center,
                TextWrapping = TextWrapping.Wrap
            };

            border.Child = textBlock;

            // Event handlers
            border.MouseEnter += (s, e) =>
            {
                if (_selectedCategory != category || (category == null && _selectedCategory != null))
                {
                    border.Background = new SolidColorBrush(Color.FromRgb(237, 242, 247));
                    border.BorderBrush = new SolidColorBrush(Color.FromRgb(59, 130, 246));
                }
            };

            border.MouseLeave += (s, e) =>
            {
                if (_selectedCategory != category || (category == null && _selectedCategory != null))
                {
                    border.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                    border.BorderBrush = new SolidColorBrush(Color.FromRgb(226, 232, 240));
                    textBlock.Foreground = new SolidColorBrush(Color.FromRgb(74, 85, 104));
                }
            };

            border.MouseDown += (s, e) =>
            {
                SelectCategory(category, border);
            };

            // Highlight "Tất cả" by default
            if (isAllButton)
            {
                border.Background = new SolidColorBrush(Color.FromRgb(219, 234, 254));
                border.BorderBrush = new SolidColorBrush(Color.FromRgb(59, 130, 246));
                textBlock.Foreground = new SolidColorBrush(Color.FromRgb(30, 64, 175));
            }

            return border;
        }

        private void SelectCategory(Category? category, Border selectedBorder)
        {
            // Reset tất cả category buttons
            foreach (Border border in CategoryPanel.Children.OfType<Border>())
            {
                border.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                border.BorderBrush = new SolidColorBrush(Color.FromRgb(226, 232, 240));
                
                if (border.Child is TextBlock tb)
                {
                    tb.Foreground = new SolidColorBrush(Color.FromRgb(74, 85, 104));
                }
            }

            // Highlight selected category
            selectedBorder.Background = new SolidColorBrush(Color.FromRgb(219, 234, 254));
            selectedBorder.BorderBrush = new SolidColorBrush(Color.FromRgb(59, 130, 246));
            
            if (selectedBorder.Child is TextBlock selectedText)
            {
                selectedText.Foreground = new SolidColorBrush(Color.FromRgb(30, 64, 175));
            }

            _selectedCategory = category;
            _currentPage = 1;
            _showingAll = false;

            if (category != null)
                LoadProducts(category.Id);
            else
                LoadAllProducts();
        }

        private void LoadAllProducts()
        {
            _allProducts = _productService.GetAllProduct().Where(p => p.IsActive ?? true).ToList();
            _filteredProducts = new List<Product>(_allProducts);
            _currentPage = 1;
            _showingAll = false;
            DisplayCurrentPage();
        }

        private void LoadProducts(int categoryId)
        {
            _allProducts = _productService.GetAllProduct()
                .Where(p => p.CategoryId == categoryId && (p.IsActive ?? true)).ToList();
            _filteredProducts = new List<Product>(_allProducts);
            _currentPage = 1;
            _showingAll = false;
            DisplayCurrentPage();
        }

        private void DisplayCurrentPage()
        {
            if (_showingAll)
            {
                DisplayProducts(_filteredProducts);
                UpdatePaginationControls(showAll: true);
                return;
            }

            var totalPages = (int)Math.Ceiling((double)_filteredProducts.Count / _itemsPerPage);
            if (totalPages == 0) totalPages = 1;

            var startIndex = (_currentPage - 1) * _itemsPerPage;
            var productsToShow = _filteredProducts.Skip(startIndex).Take(_itemsPerPage).ToList();

            DisplayProducts(productsToShow);
            UpdatePaginationControls(showAll: false);
        }

        private void UpdatePaginationControls(bool showAll = false)
        {
            if (showAll)
            {
                PageInfoText.Text = $"Hiển thị tất cả ({_filteredProducts.Count} sản phẩm)";
                PrevPageButton.IsEnabled = false;
                NextPageButton.IsEnabled = false;
                ShowAllButton.Content = "📄 Phân trang";
                return;
            }

            var totalPages = (int)Math.Ceiling((double)_filteredProducts.Count / _itemsPerPage);
            if (totalPages == 0) totalPages = 1;

            PageInfoText.Text = $"Trang {_currentPage}/{totalPages} ({_filteredProducts.Count} sản phẩm)";
            PrevPageButton.IsEnabled = _currentPage > 1;
            NextPageButton.IsEnabled = _currentPage < totalPages;
            ShowAllButton.Content = "📋 Tất cả";
        }

        private void DisplayProducts(List<Product> products)
        {
            ProductPanel.Children.Clear();

            foreach (var p in products)
            {
                var btn = new Button
                {
                    Width = 130,
                    Height = 150,
                    Margin = new Thickness(5),
                    Tag = p,
                    Background = Brushes.White,
                    BorderBrush = Brushes.LightGray,
                    BorderThickness = new Thickness(1),
                    Cursor = Cursors.Hand
                };

                var stack = new StackPanel
                {
                    Orientation = Orientation.Vertical,
                    HorizontalAlignment = HorizontalAlignment.Center
                };

                var img = new Image { Width = 100, Height = 100, Margin = new Thickness(0, 5, 0, 5), Stretch = Stretch.UniformToFill };
                img.Source = LoadImage(p.ImagePath ?? "");

                stack.Children.Add(img);
                stack.Children.Add(new TextBlock
                {
                    Text = p.ProductName,
                    FontWeight = FontWeights.Bold,
                    FontSize = 14,
                    TextAlignment = TextAlignment.Center,
                    TextWrapping = TextWrapping.Wrap
                });

                btn.Content = stack;
                btn.Click += (s, e) => OpenSelectOptions(p.Id);
                btn.MouseEnter += (s, e) => btn.Background = new SolidColorBrush(Color.FromRgb(240, 248, 255));
                btn.MouseLeave += (s, e) => btn.Background = Brushes.White;

                ProductPanel.Children.Add(btn);
            }
        }

        private BitmapImage LoadImage(string path)
        {
            if (string.IsNullOrEmpty(path))
                return new BitmapImage(new Uri("pack://application:,,,/Resources/no-image.png"));

            try
            {
                string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images", path);
                return File.Exists(fullPath) ? new BitmapImage(new Uri(fullPath, UriKind.Absolute))
                                             : new BitmapImage(new Uri(path, UriKind.RelativeOrAbsolute));
            }
            catch
            {
                return new BitmapImage(new Uri("pack://application:,,,/Resources/no-image.png"));
            }
        }

        private void OpenSelectOptions(int productId, CartItem? selectedItem = null)
        {
            var optionsWindow = selectedItem != null 
                ? new SelectOptionsWindow(productId, selectedItem) 
                : new SelectOptionsWindow(productId);
            
            optionsWindow.Owner = this;
            
            if (optionsWindow.ShowDialog() == true && optionsWindow.ResultItem != null)
            {
                var item = optionsWindow.ResultItem;
                
                if (selectedItem != null)
                {
                    var index = Cart.IndexOf(selectedItem);
                    if (index >= 0)
                    {
                        Cart[index] = item;
                    }
                }
                else
                {
                    var existing = Cart.FirstOrDefault(c =>
                        c.ProductId == item.ProductId &&
                        c.SizeName == item.SizeName &&
                        string.Join(",", c.SelectedToppings) == string.Join(",", item.SelectedToppings));

                    if (existing != null)
                        existing.Quantity += item.Quantity;
                    else
                        Cart.Add(item);
                }

                RefreshCart();
            }
        }

        #region PAGINATION HANDLERS
        private void PrevPageButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentPage > 1)
            {
                _currentPage--;
                DisplayCurrentPage();
            }
        }

        private void NextPageButton_Click(object sender, RoutedEventArgs e)
        {
            var totalPages = (int)Math.Ceiling((double)_filteredProducts.Count / _itemsPerPage);
            if (_currentPage < totalPages)
            {
                _currentPage++;
                DisplayCurrentPage();
            }
        }

        private void ShowAllButton_Click(object sender, RoutedEventArgs e)
        {
            _showingAll = !_showingAll;
            DisplayCurrentPage();
        }
        #endregion

        #region CART HANDLERS
        private void CartList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (CartList.SelectedItem is CartItem selectedItem)
                OpenSelectOptions(selectedItem.ProductId, selectedItem);
        }

        private void RemoveFromCart_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.CommandParameter is CartItem item)
            {
                Cart.Remove(item);
                RefreshCart();
            }
        }

        private void IncreaseQuantity_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.CommandParameter is CartItem item)
            {
                item.Quantity++;
                RefreshCart();
            }
        }

        private void DecreaseQuantity_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.CommandParameter is CartItem item)
            {
                if (item.Quantity > 1) item.Quantity--;
                else Cart.Remove(item);
                RefreshCart();
            }
        }

        private void RefreshCart()
        {
            decimal total = Cart.Sum(c => c.UnitPrice * c.Quantity);
            TotalText.Text = $"Tổng cộng: {total:N0}đ";
            CartList.Items.Refresh();
        }
        #endregion

        private void Checkout_Click(object sender, RoutedEventArgs e)
        {
            if (!Cart.Any())
            {
                MessageBox.Show("Chưa có sản phẩm trong giỏ.", "Thông báo");
                return;
            }

            var paymentWindow = new PaymentWindow(Cart.ToList(), _tableId, App.Configuration) { Owner = this };
            bool? result = paymentWindow.ShowDialog();

            if (result == true)
            {
                Cart.Clear();
                RefreshCart();
                
                if (_parentWindow != null)
                {
                    _parentWindow.LoadTables();
                    _parentWindow.Show();
                }
                this.Close();
            }
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string keyword = SearchBox.Text.Trim();
            
            if (string.IsNullOrEmpty(keyword))
            {
                _filteredProducts = new List<Product>(_allProducts);
            }
            else
            {
                _filteredProducts = _productService.SearchProducts(keyword)
                    .Where(p => p.IsActive ?? true && _allProducts.Any(ap => ap.Id == p.Id))
                    .ToList();
            }
            
            _currentPage = 1;
            _showingAll = false;
            DisplayCurrentPage();
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
            => SearchBox_TextChanged(sender, new TextChangedEventArgs(e.RoutedEvent, UndoAction.None));
    }
}
