using MilkTeaShop.BLL.Services;
using MilkTeaShop.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MilkTeaShop_Hantu
{
    /// <summary>
    /// Interaction logic for CategoryWindow.xaml
    /// </summary>
    public partial class CategoryWindow : Window
    {

        private CategoryServices categoryService = new CategoryServices();
        private ProductServices productService = new ProductServices();
        public CategoryWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var categories = categoryService.GetAllCategories();
            CategoryDataGrid.ItemsSource = categories;
        }

        

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var selectedCategory = CategoryDataGrid.SelectedItem;

            if (selectedCategory == null)
            {

                MessageBox.Show("Chưa chọn category nào để cập nhật nha ~ 😘");
                return;
            }
            int id = ((Category)selectedCategory).Id;

            var categoryDetail = new CategoryDetail(id);
            categoryDetail.ShowDialog();
            Window_Loaded(sender, e);
        }

        private void Createbutton_Click(object sender, RoutedEventArgs e)
        {
            var categoryDetail = new CategoryDetail();
            categoryDetail.ShowDialog();
            Window_Loaded(sender, e);
        }

        private void Deletebutton_Click(object sender, RoutedEventArgs e)
        {
            var selectedCategory = CategoryDataGrid.SelectedItem;
            if (selectedCategory == null)
            {
                MessageBox.Show("Chưa chọn category nào để xóa nha ~ 😘");
                return;
            }
            int id = ((Category)selectedCategory).Id;
            var relatedProducts = productService.GetProductsByCategory(id);

            MessageBoxResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa Category này không?", "Xác nhận xóa", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result != MessageBoxResult.Yes)
            {
                return; // Người dùng chọn No hoặc đóng hộp thoại
            }
            if (relatedProducts.Any())
            {
                // Show danh sách Product liên quan
                string list = string.Join(Environment.NewLine,
                                          relatedProducts.Select(p => $"Id: {p.Id}, Name: {p.ProductName}"));

                MessageBox.Show(
                    $"Không thể xóa Category này vì có các Product liên quan:\n\n{list}",
                    "Xóa thất bại",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
                return;
            }

            // Nếu không có product liên quan → xóa bình thường
            categoryService.DeleteCategory(id);
            MessageBox.Show("Category deleted successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            Window_Loaded(sender, e);
        }

        private void Backbutton_Click(object sender, RoutedEventArgs e)
        {
            TableWindow tableWindow = new TableWindow();
            tableWindow.Show();
            this.Close();
        }
    }
}
