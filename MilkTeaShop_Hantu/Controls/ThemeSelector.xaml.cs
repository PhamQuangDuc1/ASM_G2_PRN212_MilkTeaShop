using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MilkTeaShop_Hantu.Controls
{
    public partial class ThemeSelector : UserControl
    {
        public class ThemeItem
        {
            public string DisplayName { get; set; } = "";
            public ThemeType Theme { get; set; }
            public SolidColorBrush PreviewColor { get; set; } = new SolidColorBrush(Colors.Gray);
        }

        public ObservableCollection<ThemeItem> ThemeItems { get; set; }

        public ThemeSelector()
        {
            InitializeComponent();
            InitializeThemes();
        }

        private void InitializeThemes()
        {
            ThemeItems = new ObservableCollection<ThemeItem>
            {
                new ThemeItem 
                { 
                    DisplayName = "🧋 Milk Tea", 
                    Theme = ThemeType.MilkTea, 
                    PreviewColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#C5A880")) 
                },
                new ThemeItem 
                { 
                    DisplayName = "🍵 Matcha", 
                    Theme = ThemeType.Matcha, 
                    PreviewColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#7CB342")) 
                },
                new ThemeItem 
                { 
                    DisplayName = "🍫 Chocolate", 
                    Theme = ThemeType.Chocolate, 
                    PreviewColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#8D6E63")) 
                },
                new ThemeItem 
                { 
                    DisplayName = "🍓 Strawberry", 
                    Theme = ThemeType.Strawberry, 
                    PreviewColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#EC407A")) 
                },
                new ThemeItem 
                { 
                    DisplayName = "🟣 Taro", 
                    Theme = ThemeType.Taro, 
                    PreviewColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9C27B0")) 
                },
                new ThemeItem 
                { 
                    DisplayName = "💙 Classic", 
                    Theme = ThemeType.Classic, 
                    PreviewColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2196F3")) 
                }
            };

            ThemeButtons.ItemsSource = ThemeItems;
        }

        private void ThemeButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is ThemeType selectedTheme)
            {
                // Apply the selected theme
                ThemeManager.Instance.CurrentTheme = selectedTheme;
            }
        }
    }
}