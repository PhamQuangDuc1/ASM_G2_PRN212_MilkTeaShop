using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace MilkTeaShop_Hantu.Models
{
    public class ProductDTO
    {
        public int Id { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public BitmapImage? ImageObject { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public bool? IsActive { get; set; }
    }
}