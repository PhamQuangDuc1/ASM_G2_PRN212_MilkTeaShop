using MilkTeaShop.BLL.Services;

using MilkTeaShop_Hantu.Models;
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
    /// Interaction logic for ProductView.xaml
    /// </summary>
    public partial class ProductView : Window
    {
        private ProductServices _productService = new();
        public ProductView()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //DataGridProduct.ItemsSource = _productService.GetActiveProducts();
            var products = _productService.GetActiveProducts();
            


            var productsDTO = products.Select(p => new ProductDTO
            {
                Id = p.Id,
                ProductName = p.ProductName,
                ImageObject = ConvertPathToImage(p.ImagePath),
                CategoryName = p.Category.CategoryName,
                IsActive = p.IsActive
            }).ToList();
            
            DataGridProduct.ItemsSource = productsDTO;
        }
        private BitmapImage ConvertPathToImage(string path)
        {
            if (string.IsNullOrEmpty(path))
                return null;

            try
            {
                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = new Uri(path, UriKind.RelativeOrAbsolute);
                image.EndInit();
                return image;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading image from path: {ex.Message}");
                return null;
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedProduct = DataGridProduct.SelectedItem as ProductDTO;
            if(selectedProduct == null)
            {
                MessageBox.Show(GetWindow(this), "Please select a product to delete.", "No Selection", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            _productService.DeactivateProduct(selectedProduct.Id);
            if (MessageBox.Show(GetWindow(this), "Are you sure you want to delete the selected product?", "Confirm Deletion", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
            {
                return;
            }
            MessageBox.Show(GetWindow(this), "Product deleted successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            Window_Loaded(sender, e);
            return;
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            var selected = DataGridProduct.SelectedItem as ProductDTO;

            if (selected == null)
            {
                MessageBox.Show("Chưa chọn sản phẩm nào để cập nhật nha ~ 😘");
                return;
            }
            var updateWindow = new DetailProductWindow(selected.Id);
            updateWindow.ShowDialog();
            Window_Loaded(sender, e);
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            var createWindow = new DetailProductWindow();
            createWindow.ShowDialog();
            Window_Loaded(sender, e);
            this.Hide();
        }

        private void ReactiveButton_Click(object sender, RoutedEventArgs e)
        {
            var products = _productService.GetActiveProductsFalse();

            var productsDTO = products.Select(p => new ProductDTO
            {
                Id = p.Id,
                ProductName = p.ProductName,
                ImageObject = ConvertPathToImage(p.ImagePath),
                CategoryName = p.Category.CategoryName,
                IsActive = p.IsActive
            }).ToList();
            DataGridProduct.ItemsSource = productsDTO;
        }

        private void Active_Click(object sender, RoutedEventArgs e)
        {
            var products = _productService.GetActiveProducts();

            var productsDTO = products.Select(p => new ProductDTO
            {
                Id = p.Id,
                ProductName = p.ProductName,
                ImageObject = ConvertPathToImage(p.ImagePath),
                CategoryName = p.Category.CategoryName,
                IsActive = p.IsActive
            }).ToList();
            DataGridProduct.ItemsSource = productsDTO;
        }

        private void DeleteButton_Click_1(object sender, RoutedEventArgs e)
        {
            TableWindow tableWindow = new TableWindow();
            tableWindow.Show();
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
