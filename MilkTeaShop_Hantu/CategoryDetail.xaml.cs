using MilkTeaShop.BLL.Services;
using MilkTeaShop.DAL.Entities;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for CategoryDetail.xaml
    /// </summary>
    public partial class CategoryDetail : Window
    {
        private int _categoryId;
        private CategoryServices _categoryService = new CategoryServices();
        public CategoryDetail()
        {
            InitializeComponent();
            TitleLabel.Content = "Create New Category";
        }
      
        public CategoryDetail(int ID)
        {

            InitializeComponent();
            _categoryId = ID;
            LoadData();
            TitleLabel.Content = "Update Category";
        }

        private void LoadData()
        {
           var data = _categoryService.GetCategoryById(_categoryId);
            if (data != null)
            {
                IdTextBox.Text = data.Id.ToString();
                NameTextBox.Text = data.CategoryName;
                
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if(TitleLabel.Content.ToString() == "Create New Category")
            {
                var newCategory = new Category
                {
                    CategoryName = NameTextBox.Text,
                    
                };
                _categoryService.CreateCategory(newCategory);
                MessageBox.Show("Category created successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            else
            {
                var existingCategory = _categoryService.GetCategoryById(_categoryId);
                if (existingCategory != null)
                {
                    existingCategory.CategoryName = NameTextBox.Text;
                    
                    _categoryService.UpdateCategory(existingCategory);
                    MessageBox.Show("Category updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                this.Close();
            }
        }

        private void BackHomeButton_Click(object sender, RoutedEventArgs e)
        {
            CategoryWindow categoryWindow = new CategoryWindow();
            categoryWindow.Show();
            this.Close();
        }
    }
}
