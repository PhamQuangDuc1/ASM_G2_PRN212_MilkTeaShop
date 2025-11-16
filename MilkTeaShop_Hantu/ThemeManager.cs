using System.Windows;
using System.Windows.Media;

namespace MilkTeaShop_Hantu
{
    public enum ThemeType
    {
        MilkTea,        // Original cream theme
        Matcha,         // Green theme
        Chocolate,      // Dark brown theme
        Strawberry,     // Pink theme
        Taro,           // Purple theme
        Classic         // Blue/Gray theme
    }

    public class ThemeManager
    {
        private static ThemeManager _instance;
        public static ThemeManager Instance => _instance ??= new ThemeManager();

        public event EventHandler<ThemeType> ThemeChanged;

        private ThemeType _currentTheme = ThemeType.MilkTea;

        public ThemeType CurrentTheme
        {
            get => _currentTheme;
            set
            {
                if (_currentTheme != value)
                {
                    _currentTheme = value;
                    ApplyTheme(value);
                    ThemeChanged?.Invoke(this, value);
                }
            }
        }

        public void ApplyTheme(ThemeType theme)
        {
            var app = Application.Current;
            if (app?.Resources == null) return;

            // Clear existing theme colors
            ClearThemeColors(app.Resources);

            // Apply new theme colors
            switch (theme)
            {
                case ThemeType.MilkTea:
                    ApplyMilkTeaTheme(app.Resources);
                    break;
                case ThemeType.Matcha:
                    ApplyMatchaTheme(app.Resources);
                    break;
                case ThemeType.Chocolate:
                    ApplyChocolateTheme(app.Resources);
                    break;
                case ThemeType.Strawberry:
                    ApplyStrawberryTheme(app.Resources);
                    break;
                case ThemeType.Taro:
                    ApplyTaroTheme(app.Resources);
                    break;
                case ThemeType.Classic:
                    ApplyClassicTheme(app.Resources);
                    break;
            }
        }

        private void ClearThemeColors(ResourceDictionary resources)
        {
            var colorKeys = new[]
            {
                "PrimaryBackgroundColor", "SecondaryBackgroundColor", "AccentColor",
                "DarkAccentColor", "TextPrimaryColor", "TextSecondaryColor", "BorderColor"
            };

            var brushKeys = new[]
            {
                "PrimaryBackgroundBrush", "SecondaryBackgroundBrush", "AccentBrush",
                "DarkAccentBrush", "TextPrimaryBrush", "TextSecondaryBrush", "BorderBrush"
            };

            foreach (var key in colorKeys.Concat(brushKeys))
            {
                if (resources.Contains(key))
                    resources.Remove(key);
            }
        }

        private void ApplyMilkTeaTheme(ResourceDictionary resources)
        {
            // Colors
            resources["PrimaryBackgroundColor"] = (Color)ColorConverter.ConvertFromString("#F5F2E8");
            resources["SecondaryBackgroundColor"] = (Color)ColorConverter.ConvertFromString("#FFFFFF");
            resources["AccentColor"] = (Color)ColorConverter.ConvertFromString("#C5A880");
            resources["DarkAccentColor"] = (Color)ColorConverter.ConvertFromString("#A68B5B");
            resources["TextPrimaryColor"] = (Color)ColorConverter.ConvertFromString("#2C2C2C");
            resources["TextSecondaryColor"] = (Color)ColorConverter.ConvertFromString("#666666");
            resources["BorderColor"] = (Color)ColorConverter.ConvertFromString("#E0D5C7");

            // Brushes
            resources["PrimaryBackgroundBrush"] = new SolidColorBrush((Color)resources["PrimaryBackgroundColor"]);
            resources["SecondaryBackgroundBrush"] = new SolidColorBrush((Color)resources["SecondaryBackgroundColor"]);
            resources["AccentBrush"] = new SolidColorBrush((Color)resources["AccentColor"]);
            resources["DarkAccentBrush"] = new SolidColorBrush((Color)resources["DarkAccentColor"]);
            resources["TextPrimaryBrush"] = new SolidColorBrush((Color)resources["TextPrimaryColor"]);
            resources["TextSecondaryBrush"] = new SolidColorBrush((Color)resources["TextSecondaryColor"]);
            resources["BorderBrush"] = new SolidColorBrush((Color)resources["BorderColor"]);
        }

        private void ApplyMatchaTheme(ResourceDictionary resources)
        {
            // Colors
            resources["PrimaryBackgroundColor"] = (Color)ColorConverter.ConvertFromString("#F0F5E8");
            resources["SecondaryBackgroundColor"] = (Color)ColorConverter.ConvertFromString("#FFFFFF");
            resources["AccentColor"] = (Color)ColorConverter.ConvertFromString("#7CB342");
            resources["DarkAccentColor"] = (Color)ColorConverter.ConvertFromString("#558B2F");
            resources["TextPrimaryColor"] = (Color)ColorConverter.ConvertFromString("#1B5E20");
            resources["TextSecondaryColor"] = (Color)ColorConverter.ConvertFromString("#4A4A4A");
            resources["BorderColor"] = (Color)ColorConverter.ConvertFromString("#C8E6C9");

            // Brushes
            resources["PrimaryBackgroundBrush"] = new SolidColorBrush((Color)resources["PrimaryBackgroundColor"]);
            resources["SecondaryBackgroundBrush"] = new SolidColorBrush((Color)resources["SecondaryBackgroundColor"]);
            resources["AccentBrush"] = new SolidColorBrush((Color)resources["AccentColor"]);
            resources["DarkAccentBrush"] = new SolidColorBrush((Color)resources["DarkAccentColor"]);
            resources["TextPrimaryBrush"] = new SolidColorBrush((Color)resources["TextPrimaryColor"]);
            resources["TextSecondaryBrush"] = new SolidColorBrush((Color)resources["TextSecondaryColor"]);
            resources["BorderBrush"] = new SolidColorBrush((Color)resources["BorderColor"]);
        }

        private void ApplyChocolateTheme(ResourceDictionary resources)
        {
            // Colors
            resources["PrimaryBackgroundColor"] = (Color)ColorConverter.ConvertFromString("#F3F0ED");
            resources["SecondaryBackgroundColor"] = (Color)ColorConverter.ConvertFromString("#FFFFFF");
            resources["AccentColor"] = (Color)ColorConverter.ConvertFromString("#8D6E63");
            resources["DarkAccentColor"] = (Color)ColorConverter.ConvertFromString("#5D4037");
            resources["TextPrimaryColor"] = (Color)ColorConverter.ConvertFromString("#3E2723");
            resources["TextSecondaryColor"] = (Color)ColorConverter.ConvertFromString("#6D4C41");
            resources["BorderColor"] = (Color)ColorConverter.ConvertFromString("#D7CCC8");

            // Brushes
            resources["PrimaryBackgroundBrush"] = new SolidColorBrush((Color)resources["PrimaryBackgroundColor"]);
            resources["SecondaryBackgroundBrush"] = new SolidColorBrush((Color)resources["SecondaryBackgroundColor"]);
            resources["AccentBrush"] = new SolidColorBrush((Color)resources["AccentColor"]);
            resources["DarkAccentBrush"] = new SolidColorBrush((Color)resources["DarkAccentColor"]);
            resources["TextPrimaryBrush"] = new SolidColorBrush((Color)resources["TextPrimaryColor"]);
            resources["TextSecondaryBrush"] = new SolidColorBrush((Color)resources["TextSecondaryColor"]);
            resources["BorderBrush"] = new SolidColorBrush((Color)resources["BorderColor"]);
        }

        private void ApplyStrawberryTheme(ResourceDictionary resources)
        {
            // Colors
            resources["PrimaryBackgroundColor"] = (Color)ColorConverter.ConvertFromString("#FDF2F8");
            resources["SecondaryBackgroundColor"] = (Color)ColorConverter.ConvertFromString("#FFFFFF");
            resources["AccentColor"] = (Color)ColorConverter.ConvertFromString("#EC407A");
            resources["DarkAccentColor"] = (Color)ColorConverter.ConvertFromString("#C2185B");
            resources["TextPrimaryColor"] = (Color)ColorConverter.ConvertFromString("#880E4F");
            resources["TextSecondaryColor"] = (Color)ColorConverter.ConvertFromString("#AD1457");
            resources["BorderColor"] = (Color)ColorConverter.ConvertFromString("#F8BBD9");

            // Brushes
            resources["PrimaryBackgroundBrush"] = new SolidColorBrush((Color)resources["PrimaryBackgroundColor"]);
            resources["SecondaryBackgroundBrush"] = new SolidColorBrush((Color)resources["SecondaryBackgroundColor"]);
            resources["AccentBrush"] = new SolidColorBrush((Color)resources["AccentColor"]);
            resources["DarkAccentBrush"] = new SolidColorBrush((Color)resources["DarkAccentColor"]);
            resources["TextPrimaryBrush"] = new SolidColorBrush((Color)resources["TextPrimaryColor"]);
            resources["TextSecondaryBrush"] = new SolidColorBrush((Color)resources["TextSecondaryColor"]);
            resources["BorderBrush"] = new SolidColorBrush((Color)resources["BorderColor"]);
        }

        private void ApplyTaroTheme(ResourceDictionary resources)
        {
            // Colors
            resources["PrimaryBackgroundColor"] = (Color)ColorConverter.ConvertFromString("#F3E5F5");
            resources["SecondaryBackgroundColor"] = (Color)ColorConverter.ConvertFromString("#FFFFFF");
            resources["AccentColor"] = (Color)ColorConverter.ConvertFromString("#9C27B0");
            resources["DarkAccentColor"] = (Color)ColorConverter.ConvertFromString("#7B1FA2");
            resources["TextPrimaryColor"] = (Color)ColorConverter.ConvertFromString("#4A148C");
            resources["TextSecondaryColor"] = (Color)ColorConverter.ConvertFromString("#6A1B9A");
            resources["BorderColor"] = (Color)ColorConverter.ConvertFromString("#E1BEE7");

            // Brushes
            resources["PrimaryBackgroundBrush"] = new SolidColorBrush((Color)resources["PrimaryBackgroundColor"]);
            resources["SecondaryBackgroundBrush"] = new SolidColorBrush((Color)resources["SecondaryBackgroundColor"]);
            resources["AccentBrush"] = new SolidColorBrush((Color)resources["AccentColor"]);
            resources["DarkAccentBrush"] = new SolidColorBrush((Color)resources["DarkAccentColor"]);
            resources["TextPrimaryBrush"] = new SolidColorBrush((Color)resources["TextPrimaryColor"]);
            resources["TextSecondaryBrush"] = new SolidColorBrush((Color)resources["TextSecondaryColor"]);
            resources["BorderBrush"] = new SolidColorBrush((Color)resources["BorderColor"]);
        }

        private void ApplyClassicTheme(ResourceDictionary resources)
        {
            // Colors
            resources["PrimaryBackgroundColor"] = (Color)ColorConverter.ConvertFromString("#F5F7FA");
            resources["SecondaryBackgroundColor"] = (Color)ColorConverter.ConvertFromString("#FFFFFF");
            resources["AccentColor"] = (Color)ColorConverter.ConvertFromString("#2196F3");
            resources["DarkAccentColor"] = (Color)ColorConverter.ConvertFromString("#1976D2");
            resources["TextPrimaryColor"] = (Color)ColorConverter.ConvertFromString("#212121");
            resources["TextSecondaryColor"] = (Color)ColorConverter.ConvertFromString("#757575");
            resources["BorderColor"] = (Color)ColorConverter.ConvertFromString("#E3F2FD");

            // Brushes
            resources["PrimaryBackgroundBrush"] = new SolidColorBrush((Color)resources["PrimaryBackgroundColor"]);
            resources["SecondaryBackgroundBrush"] = new SolidColorBrush((Color)resources["SecondaryBackgroundColor"]);
            resources["AccentBrush"] = new SolidColorBrush((Color)resources["AccentColor"]);
            resources["DarkAccentBrush"] = new SolidColorBrush((Color)resources["DarkAccentColor"]);
            resources["TextPrimaryBrush"] = new SolidColorBrush((Color)resources["TextPrimaryColor"]);
            resources["TextSecondaryBrush"] = new SolidColorBrush((Color)resources["TextSecondaryColor"]);
            resources["BorderBrush"] = new SolidColorBrush((Color)resources["BorderColor"]);
        }

        public static string GetThemeDisplayName(ThemeType theme)
        {
            return theme switch
            {
                ThemeType.MilkTea => "?? Milk Tea",
                ThemeType.Matcha => "?? Matcha",
                ThemeType.Chocolate => "?? Chocolate",
                ThemeType.Strawberry => "?? Strawberry",
                ThemeType.Taro => "?? Taro",
                ThemeType.Classic => "?? Classic",
                _ => theme.ToString()
            };
        }

        public static ThemeType[] GetAllThemes()
        {
            return Enum.GetValues<ThemeType>();
        }
    }
}