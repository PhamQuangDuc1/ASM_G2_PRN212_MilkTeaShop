using MilkTeaShop.BLL.Services;
using MilkTeaShop.DAL.Entities;
using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MilkTeaShop_Hantu
{
    public partial class TableWindow : Window
    {
        private readonly TablesServices _tableService = new();

        public TableWindow()
        {
            InitializeComponent();
            InitializeThemeSelector();
            LoadLogo();
            LoadTables();
        }

        private void LoadLogo()
        {
            try
            {
                // Load ảnh FullLogo.jpg từ thư mục Images
                string logoPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images", "FullLogo.jpg");
                if (File.Exists(logoPath))
                {
                    LogoImage.Source = new BitmapImage(new Uri(logoPath, UriKind.Absolute));
                }
                else
                {
                    // Thử tìm trong thư mục gốc project
                    string fallbackPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "FullLogo.jpg");
                    if (File.Exists(fallbackPath))
                    {
                        LogoImage.Source = new BitmapImage(new Uri(fallbackPath, UriKind.Absolute));
                    }
                    else
                    {
                        LogoImage.Visibility = Visibility.Collapsed;
                    }
                }
            }
            catch (Exception ex)
            {
                // Nếu không load được logo thì ẩn đi
                LogoImage.Visibility = Visibility.Collapsed;
                System.Diagnostics.Debug.WriteLine($"Không load được logo FullLogo.jpg: {ex.Message}");
            }
        }

        private void InitializeThemeSelector()
        {
            // Populate theme selector
            var themes = ThemeManager.GetAllThemes()
                .Select(ThemeManager.GetThemeDisplayName)
                .ToList();
            
            ThemeSelector.ItemsSource = themes;
            ThemeSelector.SelectedIndex = 0; // Default to first theme (Milk Tea)
        }

        private void ThemeSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ThemeSelector.SelectedIndex >= 0)
            {
                var selectedTheme = ThemeManager.GetAllThemes()[ThemeSelector.SelectedIndex];
                ThemeManager.Instance.CurrentTheme = selectedTheme;
            }
        }

        public void LoadTables()
        {
            TablePanel.Children.Clear();
            var tables = _tableService.GetAllTables();

            foreach (var table in tables)
            {
                var btn = new Button
                {
                    Width = 100,
                    Height = 100,
                    Margin = new Thickness(10),
                    Tag = table,
                    Background = GetTableColor(table.Status),
                    Cursor = Cursors.Hand,
                    Content = new StackPanel
                    {
                        Orientation = Orientation.Vertical,
                        Children =
                        {
                            new TextBlock
                            {
                                Text = table.TableName,
                                FontWeight = FontWeights.Bold,
                                FontSize = 14,
                                TextAlignment = TextAlignment.Center
                            },
                            new TextBlock
                            {
                                Text = table.Status,
                                FontSize = 12,
                                TextAlignment = TextAlignment.Center,
                                Foreground = Brushes.Black
                            }
                        }
                    }
                };
                btn.Click += Table_Click;
                TablePanel.Children.Add(btn);
            }
        }

        private Brush GetTableColor(string status)
        {
            return status switch
            {
                "Sẵn sàng" => Brushes.LightGreen,   // Ready
                "InProcess" => Brushes.Orange,      // Just paid, waiting clean
                "Đang phục vụ" => Brushes.LightCoral, // Currently ordering
                _ => Brushes.LightGray
            };
        }

        private void Table_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not Button btn || btn.Tag is not Table table) return;

            switch (table.Status)
            {
                case "Sẵn sàng":
                    // Bàn rảnh -> mở OrderWindow
                    var orderWindow = new OrderWindow(table.TableId, this);
                    Hide();
                    orderWindow.ShowDialog();
                    LoadTables();
                    Show();
                    break;

                case "InProcess":
                    // Bàn vừa thanh toán xong, hỏi có muốn reset lại ko
                    var confirm = MessageBox.Show(
                        $"Bàn {table.TableName} vừa thanh toán xong.\nChuyển sang 'Sẵn sàng' để nhận order mới?",
                        "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question);

                    if (confirm == MessageBoxResult.Yes)
                    {
                        _tableService.UpdateTableStatus(table.TableId, "Sẵn sàng");
                        LoadTables();
                    }
                    break;

                case "Đang phục vụ":
                    MessageBox.Show($"Bàn {table.TableName} đang được phục vụ, không thể tạo order mới.",
                        "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    break;

                default:
                    MessageBox.Show($"Trạng thái bàn không hợp lệ: {table.Status}", "Lỗi",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
            }
        }
    }
}
