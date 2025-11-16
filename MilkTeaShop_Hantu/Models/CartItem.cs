using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkTeaShop_Hantu.Models
{
    public class ToppingItem
    {
        public int ToppingId { get; set; }
        public string ToppingName { get; set; } = string.Empty;
        public decimal ToppingPrice { get; set; }
        public int Quantity { get; set; } = 1;
    }

    public class CartItem
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string SizeName { get; set; } = string.Empty;
        public int SizeId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public string? SugarLevel { get; set; }
        public string? IceLevel { get; set; }
        public string? Notes { get; set; }
        
        // Cũ - để backward compatibility
        public List<string> SelectedToppings { get; set; } = new();
        
        // Mới - để phù hợp với OrderDetailTopping
        public List<ToppingItem> ToppingItems { get; set; } = new();

        // Thêm để binding XAML PaymentWindow
        public string Name => ProductName;
        public decimal Price => UnitPrice;
        public decimal Subtotal => UnitPrice * Quantity;
    }
}

