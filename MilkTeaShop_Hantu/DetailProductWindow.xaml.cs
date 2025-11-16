using MilkTeaShop.BLL.Services;
using MilkTeaShop.DAL.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MilkTeaShop_Hantu
{
    /// <summary>
    /// Interaction logic for DetailProductWindow.xaml
    /// </summary>
    public partial class DetailProductWindow : Window
    {
        private  ProductServices _service = new ProductServices();
        private CategoryServices _categoryService = new CategoryServices();
        private int _productId;
        private string _currentPath;
        public DetailProductWindow()
        {
            InitializeComponent();
            ProductImage.Source = new BitmapImage(new Uri("Images/default.png", UriKind.Relative)); // or a default image
            TitleLabel.Content="Create New Product";
            fillCategoryComboBox();
        }
        public DetailProductWindow(int id)
        {
            InitializeComponent();
            _productId = id;
            LoadData();
        }
        private void LoadData()
        {
            var product = _service.GetProductById(_productId);
            TitleLabel.Content = "Edit Product";
            if (product != null)
            {
                IdTextBox.Text = _productId.ToString();
                NameTextBox.Text = product.ProductName;
                string imagePath = product.ImagePath ?? "Images/default.png";
                Uri uri;
                if (Uri.IsWellFormedUriString(imagePath, UriKind.Absolute))
                {
                    uri = new Uri(imagePath, UriKind.Absolute);
                }
                else
                {
                    uri = new Uri(imagePath, UriKind.Relative);
                }
                ProductImage.Source = new BitmapImage(uri);
                IsActiveCheckBox.IsChecked = product.IsActive;
                fillCategoryComboBox();
                CategoryComboBox.SelectedValue = product.CategoryId;
            }
            else
            {
                MessageBox.Show("Product not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }
        }
        private void ProductImage_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effects = DragDropEffects.Copy;
            else
                e.Effects = DragDropEffects.None;
        }
        private void ProductImage_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                string imagePath = files[0];

                // load ảnh vào UI
                ProductImage.Source = new BitmapImage(new Uri(imagePath));

                // Save to Images folder and set path
                string imagesFolder = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images");
                Directory.CreateDirectory(imagesFolder);
                string fileName = Guid.NewGuid().ToString() + System.IO.Path.GetExtension(imagePath);
                string destPath = System.IO.Path.Combine(imagesFolder, fileName);
                File.Copy(imagePath, destPath, true);
                _currentPath = System.IO.Path.Combine("Images", fileName);
            }
        }
        private void fillCategoryComboBox()
        {
            var categories = _categoryService.GetAllCategories();
            CategoryComboBox.ItemsSource = categories;
            CategoryComboBox.DisplayMemberPath = "CategoryName";
            CategoryComboBox.SelectedValuePath = "Id";

        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            if(TitleLabel.Content.ToString() == "Create New Product")
            {
                CreateNewProduct();
            }
            else
            {
                UpdateExistingProduct();
            }
            
        }
        private void CreateNewProduct()
        {
            try
            {
                var newProduct = new Product()
                {
                    ProductName = NameTextBox.Text,
                    CategoryId = (int)CategoryComboBox.SelectedValue,
                    ImagePath = _currentPath,
                    IsActive = IsActiveCheckBox.IsChecked ?? true,
                };
                _service.CreateProduct(newProduct);
                MessageBox.Show("Product created successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error creating product: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void UpdateExistingProduct()
        {
            try
            {
                var product = _service.GetProductById(_productId);
                if (product != null)
                {
                    product.ProductName = NameTextBox.Text;
                    product.CategoryId = (int)CategoryComboBox.SelectedValue;
                    if (!string.IsNullOrEmpty(_currentPath))
                    {
                        product.ImagePath = _currentPath;
                    }
                    product.IsActive = IsActiveCheckBox.IsChecked ?? true;
                    _service.UpdateProduct(product);
                    
                    MessageBox.Show("Product updated successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating product: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ProductView productView = new ProductView();
            productView.Show();
            this.Close();
        }

        private void Category_Click(object sender, RoutedEventArgs e)
        {
            CategoryWindow categoryWindow = new CategoryWindow();
            categoryWindow.Show();
            this.Close();
        }
    }
}
