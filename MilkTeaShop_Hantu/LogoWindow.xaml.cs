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

namespace Group2.MilkTeaShop.WPF
{
    /// <summary>
    /// Interaction logic for LogoWindow.xaml
    /// </summary>
    public partial class LogoWindow : Window
    {
        public LogoWindow()
        {
            InitializeComponent();
            
            // Fallback load to surface exceptions in debug
            try
            {
                var uri = new Uri("pack://application:,,,/Group2.MilkTeaShop.WPF;component/Images/logo.png", UriKind.Absolute);
                LogoImage.Source = new BitmapImage(uri);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Logo load failed: " + ex.Message);
            }
        }
        
      
    }
}
