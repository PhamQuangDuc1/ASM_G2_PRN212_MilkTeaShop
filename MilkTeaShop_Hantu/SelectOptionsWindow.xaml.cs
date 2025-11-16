using MilkTeaShop.BLL.Services;
using MilkTeaShop.DAL.Entities;
using MilkTeaShop_Hantu.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using SizeEntity = MilkTeaShop.DAL.Entities.Size;

namespace MilkTeaShop_Hantu
{
    public class ToppingSelection
    {
        public int ToppingId { get; set; }
        public string ToppingName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; } = 0;
        public decimal TotalPrice => Price * Quantity;
    }

    public partial class SelectOptionsWindow : Window
    {
        private readonly ProductPriceServices _priceService = new();
        private readonly ProductServices _productService = new();
        private readonly SizeServices _sizeService = new();

        public CartItem? ResultItem { get; private set; }
        private readonly int _productId;
        private readonly CartItem? _editItem;
        private Product? _currentProduct;
        private List<ToppingSelection> _availableToppings = new();

        // Constructor khi thêm mới
        public SelectOptionsWindow(int productId)
        {
            InitializeComponent();
            _productId = productId;
            _editItem = null;
        }

        // Constructor khi chỉnh sửa CartItem
        public SelectOptionsWindow(int productId, CartItem editItem)
        {
            InitializeComponent();
            _productId = productId;
            _editItem = editItem;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                // Lấy thông tin product
                _currentProduct = _productService.GetAllProduct().FirstOrDefault(p => p.Id == _productId);
                if (_currentProduct == null)
                {
                    MessageBox.Show("Không tìm thấy sản phẩm!");
                    Close();
                    return;
                }

                ProductNameText.Text = _currentProduct.ProductName;

                // Load sizes
                var sizes = _sizeService.GetAllProduct();
                SizeComboBox.ItemsSource = sizes;
                SizeComboBox.DisplayMemberPath = "SizeName";
                SizeComboBox.SelectedValuePath = "Id";

                // Kiểm tra CategoryId để ẩn/hiện các control
                bool isToppingOrFood = _currentProduct.CategoryId == 2 || _currentProduct.CategoryId == 3;
                
                if (isToppingOrFood)
                {
                    // Ẩn topping và sugar/ice cho CategoryId 2 (topping) và 3 (đồ ăn kèm)
                    ToppingPanel.Visibility = Visibility.Collapsed;
                    SugarIcePanel.Visibility = Visibility.Collapsed;
                    
                    // Giảm chiều cao window
                    this.Height = 400;
                }
                else
                {
                    // Load toppings cho đồ uống
                    LoadToppings();
                }

                // Load dữ liệu edit nếu có
                LoadEditData(sizes, isToppingOrFood);
                
                // Set default values nếu không edit
                if (_editItem == null)
                {
                    if (sizes.Any()) SizeComboBox.SelectedIndex = 0;
                    
                    if (!isToppingOrFood)
                    {
                        SugarComboBox.SelectedIndex = 4; // 100%
                        IceComboBox.SelectedIndex = 4;   // 100%
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi load dữ liệu: {ex.Message}");
            }
        }

        private void LoadToppings()
        {
            var toppings = _productService.GetAllProduct()
                .Where(p => p.CategoryId == 2) // Chỉ lấy topping
                .ToList();

            _availableToppings = toppings.Select(p => new ToppingSelection
            {
                ToppingId = p.Id,
                ToppingName = p.ProductName,
                Price = _priceService.GetAllProduct().FirstOrDefault(x => x.ProductId == p.Id)?.Price ?? 0,
                Quantity = 0
            }).ToList();

            DisplayToppings();
        }

        private void DisplayToppings()
        {
            ToppingItemsPanel.Children.Clear();

            foreach (var topping in _availableToppings)
            {
                var border = new Border
                {
                    Background = new SolidColorBrush(Color.FromRgb(249, 250, 251)),
                    CornerRadius = new CornerRadius(6),
                    Padding = new Thickness(12),
                    Margin = new Thickness(0, 0, 0, 8),
                    BorderBrush = new SolidColorBrush(Color.FromRgb(229, 231, 235)),
                    BorderThickness = new Thickness(1)
                };

                var grid = new Grid();
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

                // Tên và giá topping
                var infoPanel = new StackPanel();
                var nameText = new TextBlock
                {
                    Text = topping.ToppingName,
                    FontWeight = FontWeights.SemiBold,
                    FontSize = 13,
                    Foreground = new SolidColorBrush(Color.FromRgb(31, 41, 55))
                };
                var priceText = new TextBlock
                {
                    Text = $"{topping.Price:N0} VNĐ",
                    FontSize = 12,
                    Foreground = new SolidColorBrush(Color.FromRgb(107, 114, 128)),
                    Margin = new Thickness(0, 2, 0, 0)
                };
                infoPanel.Children.Add(nameText);
                infoPanel.Children.Add(priceText);

                // Quantity controls
                var quantityPanel = new StackPanel { Orientation = Orientation.Horizontal };
                
                var decreaseBtn = new Button
                {
                    Content = "−",
                    Width = 30,
                    Height = 30,
                    Background = new SolidColorBrush(Color.FromRgb(239, 68, 68)),
                    Foreground = Brushes.White,
                    BorderThickness = new Thickness(0),
                    FontWeight = FontWeights.Bold,
                    Tag = topping
                };
                decreaseBtn.Click += DecreaseTopping_Click;

                var quantityText = new TextBlock
                {
                    Text = topping.Quantity.ToString(),
                    FontWeight = FontWeights.Bold,
                    FontSize = 14,
                    Width = 35,
                    TextAlignment = TextAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Tag = topping
                };

                var increaseBtn = new Button
                {
                    Content = "+",
                    Width = 30,
                    Height = 30,
                    Background = new SolidColorBrush(Color.FromRgb(34, 197, 94)),
                    Foreground = Brushes.White,
                    BorderThickness = new Thickness(0),
                    FontWeight = FontWeights.Bold,
                    Tag = topping
                };
                increaseBtn.Click += IncreaseTopping_Click;

                quantityPanel.Children.Add(decreaseBtn);
                quantityPanel.Children.Add(quantityText);
                quantityPanel.Children.Add(increaseBtn);

                Grid.SetColumn(infoPanel, 0);
                Grid.SetColumn(quantityPanel, 1);
                
                grid.Children.Add(infoPanel);
                grid.Children.Add(quantityPanel);
                border.Child = grid;
                
                ToppingItemsPanel.Children.Add(border);
            }

            UpdateToppingTotal();
        }

        private void IncreaseTopping_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is ToppingSelection topping)
            {
                topping.Quantity++;
                UpdateToppingDisplay();
            }
        }

        private void DecreaseTopping_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is ToppingSelection topping && topping.Quantity > 0)
            {
                topping.Quantity--;
                UpdateToppingDisplay();
            }
        }

        private void UpdateToppingDisplay()
        {
            foreach (Border border in ToppingItemsPanel.Children)
            {
                if (border.Child is Grid grid)
                {
                    var quantityPanel = grid.Children[1] as StackPanel;
                    var quantityText = quantityPanel?.Children[1] as TextBlock;
                    var topping = quantityText?.Tag as ToppingSelection;
                    
                    if (quantityText != null && topping != null)
                    {
                        quantityText.Text = topping.Quantity.ToString();
                        
                        // Highlight nếu quantity > 0
                        if (topping.Quantity > 0)
                        {
                            border.Background = new SolidColorBrush(Color.FromRgb(219, 234, 254));
                            border.BorderBrush = new SolidColorBrush(Color.FromRgb(59, 130, 246));
                        }
                        else
                        {
                            border.Background = new SolidColorBrush(Color.FromRgb(249, 250, 251));
                            border.BorderBrush = new SolidColorBrush(Color.FromRgb(229, 231, 235));
                        }
                    }
                }
            }
            UpdateToppingTotal();
        }

        private void UpdateToppingTotal()
        {
            decimal total = _availableToppings.Sum(t => t.TotalPrice);
            ToppingTotalText.Text = $"Tổng topping: {total:N0} VNĐ";
        }

        private void LoadEditData(List<SizeEntity> sizes, bool isToppingOrFood)
        {
            if (_editItem == null) return;

            SizeComboBox.SelectedItem = sizes.FirstOrDefault(s => s.SizeName == _editItem.SizeName);
            
            if (!isToppingOrFood)
            {
                // Load topping quantities từ ToppingItems
                foreach (var toppingItem in _editItem.ToppingItems)
                {
                    var topping = _availableToppings.FirstOrDefault(t => t.ToppingId == toppingItem.ToppingId);
                    if (topping != null)
                    {
                        topping.Quantity = toppingItem.Quantity;
                    }
                }
                
                UpdateToppingDisplay();
                
                SugarComboBox.SelectedItem = SugarComboBox.Items.Cast<ComboBoxItem>()
                    .FirstOrDefault(i => i.Content.ToString() == _editItem.SugarLevel);
                IceComboBox.SelectedItem = IceComboBox.Items.Cast<ComboBoxItem>()
                    .FirstOrDefault(i => i.Content.ToString() == _editItem.IceLevel);
            }
            
            NotesBox.Text = _editItem.Notes;
        }

        private void SizeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SizeComboBox.SelectedItem is SizeEntity selectedSize)
            {
                var price = _priceService.GetAllProduct()
                    .FirstOrDefault(p => p.ProductId == _productId && p.SizeId == selectedSize.Id)?.Price ?? 0;

                PriceText.Text = $"Giá gốc: {price:N0} VNĐ";
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            if (SizeComboBox.SelectedItem is not SizeEntity selectedSize)
            {
                MessageBox.Show("Hãy chọn size trước khi thêm!");
                return;
            }

            var basePrice = _priceService.GetAllProduct()
                .FirstOrDefault(p => p.ProductId == _productId && p.SizeId == selectedSize.Id)?.Price ?? 0;

            decimal toppingTotal = 0;
            var toppingItems = new List<ToppingItem>();
            var selectedToppingsDisplay = new List<string>();

            // Chỉ xử lý topping nếu không phải CategoryId 2 hoặc 3
            bool isToppingOrFood = _currentProduct?.CategoryId == 2 || _currentProduct?.CategoryId == 3;
            
            if (!isToppingOrFood && ToppingPanel.Visibility == Visibility.Visible)
            {
                var selectedToppings = _availableToppings.Where(t => t.Quantity > 0).ToList();
                toppingTotal = selectedToppings.Sum(t => t.TotalPrice);
                
                // Tạo ToppingItems với quantity
                toppingItems = selectedToppings.Select(t => new ToppingItem
                {
                    ToppingId = t.ToppingId,
                    ToppingName = t.ToppingName,
                    ToppingPrice = t.Price,
                    Quantity = t.Quantity
                }).ToList();

                // Tạo display list để backward compatibility
                selectedToppingsDisplay = selectedToppings.Select(t => 
                    $"{t.ToppingName} x{t.Quantity} ({t.TotalPrice:N0} VNĐ)").ToList();
            }

            ResultItem = new CartItem
            {
                ProductId = _productId,
                ProductName = _currentProduct?.ProductName ?? "",
                SizeName = selectedSize.SizeName,
                SizeId = selectedSize.Id,
                Quantity = _editItem?.Quantity ?? 1,
                UnitPrice = basePrice + toppingTotal,
                SugarLevel = !isToppingOrFood ? ((ComboBoxItem)SugarComboBox.SelectedItem)?.Content?.ToString() : null,
                IceLevel = !isToppingOrFood ? ((ComboBoxItem)IceComboBox.SelectedItem)?.Content?.ToString() : null,
                Notes = NotesBox.Text,
                SelectedToppings = selectedToppingsDisplay,
                ToppingItems = toppingItems
            };

            DialogResult = true;
            Close();
        }
    }
}
