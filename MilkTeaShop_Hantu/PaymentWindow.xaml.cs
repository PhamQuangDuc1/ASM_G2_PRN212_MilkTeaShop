using MilkTeaShop.BLL.Services;
using MilkTeaShop.DAL.Entities;
using MilkTeaShop_Hantu.Models;
using QRCoder;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;

namespace MilkTeaShop_Hantu
{
    public partial class PaymentWindow : Window
    {
        private readonly List<CartItem>? _cart;
        private readonly int _tableId;
        private readonly int _currentUserId;
        private readonly OrderServices _orderService = new();
        private readonly PaymentServices _paymentService = new();
        private readonly TablesServices _tableService = new();
        private readonly IConfiguration _config;

        // Biến để check MoMo
        private string _currentOrderId;
        private string _currentRequestId;
        private System.Windows.Threading.DispatcherTimer _checkTimer;

        public PaymentWindow(List<CartItem> cart, int tableId, IConfiguration config = null, int currentUserId = 1)
        {
            InitializeComponent();

            _cart = cart ?? new List<CartItem>();
            _tableId = tableId;
            _config = config;
            _currentUserId = currentUserId;

            Loaded += PaymentWindow_Loaded;
        }

        private void PaymentWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (InvoiceList != null)
            {
                InvoiceList.ItemsSource = _cart;
                // Thêm sự kiện double-click để edit item
                InvoiceList.MouseDoubleClick += InvoiceList_MouseDoubleClick;
            }

            RecalculateTotals(null, null);
            PaymentMethod_Changed(null, null);
        }

        // Thêm phương thức để edit item từ PaymentWindow
        private void InvoiceList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (InvoiceList.SelectedItem is CartItem selectedItem)
            {
                var optionsWindow = new SelectOptionsWindow(selectedItem.ProductId, selectedItem) { Owner = this };
                if (optionsWindow.ShowDialog() == true && optionsWindow.ResultItem != null)
                {
                    var updatedItem = optionsWindow.ResultItem;
                    var index = _cart.IndexOf(selectedItem);
                    
                    if (index >= 0)
                    {
                        // Thay thế item cũ bằng item mới đã được chỉnh sửa
                        _cart[index] = updatedItem;
                        
                        // Refresh ListView và recalculate totals
                        InvoiceList.Items.Refresh();
                        RecalculateTotals(null, null);
                    }
                }
            }
        }

        #region CALCULATION
        private void RecalculateTotals(object sender, RoutedEventArgs e)
        {
            decimal subtotal = (_cart ?? Enumerable.Empty<CartItem>()).Sum(c => c.UnitPrice * c.Quantity);
            decimal discount = 0;
            if (TxtDiscount != null)
                decimal.TryParse(TxtDiscount.Text, out discount);

            decimal vatAmount = (ChkVAT?.IsChecked ?? false) ? subtotal * 0.1m : 0;
            decimal total = subtotal + vatAmount - discount;

            if (TxtSubtotal != null) TxtSubtotal.Text = $"{subtotal:N0}đ";
            if (TxtVATAmount != null) TxtVATAmount.Text = $"{vatAmount:N0}đ";
            if (TxtTotal != null) TxtTotal.Text = $"{total:N0}đ";

            UpdateChange();

            if (RbMomo?.IsChecked == true)
                _ = GenerateMomoQRCodeAsync();
        }
        private void TxtReceived_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateChange();
        }


        private void UpdateChange()
        {
            decimal received = 0;
            decimal total = 0;

            if (TxtReceived != null)
                decimal.TryParse(TxtReceived.Text.Replace("đ", "").Trim(), out received);

            if (TxtTotal != null)
                decimal.TryParse(new string(TxtTotal.Text.Where(c => char.IsDigit(c) || c == '.').ToArray()), out total);

            if (TxtChange != null)
                TxtChange.Text = $"{Math.Max(received - total, 0):N0}đ";
        }
        #endregion

        #region PAYMENT METHOD
        private void PaymentMethod_Changed(object sender, RoutedEventArgs e)
        {
            if (CashPanel != null)
                CashPanel.Visibility = RbCash?.IsChecked == true ? Visibility.Visible : Visibility.Collapsed;

            if (MomoPanel != null)
                MomoPanel.Visibility = RbMomo?.IsChecked == true ? Visibility.Visible : Visibility.Collapsed;

            if (RbMomo?.IsChecked == true)
                _ = GenerateMomoQRCodeAsync();
        }

        private async System.Threading.Tasks.Task GenerateMomoQRCodeAsync()
        {
            if (_config == null || _cart == null || !_cart.Any() || MomoQRCode == null)
                return;

            try
            {
                decimal subtotal = (_cart ?? Enumerable.Empty<CartItem>()).Sum(c => c.UnitPrice * c.Quantity);
                decimal discount = 0;
                if (TxtDiscount != null)
                    decimal.TryParse(TxtDiscount.Text, out discount);

                decimal vatAmount = (ChkVAT?.IsChecked ?? false) ? subtotal * 0.1m : 0;
                decimal amount = subtotal + vatAmount - discount;
                if (amount <= 0) return;

                string endpoint = _config["Momo:Endpoint"];
                string partnerCode = _config["Momo:PartnerCode"];
                string accessKey = _config["Momo:AccessKey"];
                string secretKey = _config["Momo:SecretKey"];
                string returnUrl = _config["Momo:ReturnUrl"];
                string notifyUrl = _config["Momo:NotifyUrl"];
                string requestType = _config["Momo:RequestType"];

                string orderId = DateTime.Now.Ticks.ToString();
                string requestId = Guid.NewGuid().ToString();

                // Lưu biến toàn cục để check tự động
                _currentOrderId = orderId;
                _currentRequestId = requestId;

                string orderInfo = "Thanh toán đơn hàng MilkTeaShop";
                string extraData = "";

                string rawHash = $"accessKey={accessKey}&amount={amount:0}&extraData={extraData}&ipnUrl={notifyUrl}&orderId={orderId}&orderInfo={orderInfo}&partnerCode={partnerCode}&redirectUrl={returnUrl}&requestId={requestId}&requestType={requestType}";
                string signature;
                using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secretKey));
                signature = BitConverter.ToString(hmac.ComputeHash(Encoding.UTF8.GetBytes(rawHash))).Replace("-", "").ToLower();

                var payload = new
                {
                    partnerCode,
                    partnerName = "MilkTeaShop",
                    storeId = "MilkTeaShop01",
                    requestId,
                    amount = amount.ToString("0"),
                    orderId,
                    orderInfo,
                    redirectUrl = returnUrl,
                    ipnUrl = notifyUrl,
                    lang = "vi",
                    requestType,
                    autoCapture = true,
                    extraData,
                    signature
                };

                using var client = new HttpClient();
                var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
                var resp = await client.PostAsync(endpoint, content);
                dynamic data = JsonConvert.DeserializeObject(await resp.Content.ReadAsStringAsync());

                string payUrl = data?.payUrl;
                if (string.IsNullOrEmpty(payUrl)) return;

                // Tạo QR hiển thị ngay
                using var qrGen = new QRCodeGenerator();
                using var qrData = qrGen.CreateQrCode(payUrl, QRCodeGenerator.ECCLevel.Q);
                using var qrCode = new PngByteQRCode(qrData);
                byte[] qrBytes = qrCode.GetGraphic(20);

                using var ms = new MemoryStream(qrBytes);
                var img = new BitmapImage();
                img.BeginInit();
                img.StreamSource = ms;
                img.CacheOption = BitmapCacheOption.OnLoad;
                img.EndInit();
                MomoQRCode.Source = img;

                // Start auto-check payment
                StartCheckingPaymentStatus(orderId, requestId);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tạo QR: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion

        #region AUTO CHECK MOMO PAYMENT
        private void StartCheckingPaymentStatus(string orderId, string requestId)
        {
            if (_checkTimer != null)
            {
                _checkTimer.Stop();
                _checkTimer = null;
            }

            _checkTimer = new System.Windows.Threading.DispatcherTimer();
            _checkTimer.Interval = TimeSpan.FromSeconds(5);
            _checkTimer.Tick += async (s, e) => await CheckMomoTransaction(orderId, requestId);
            _checkTimer.Start();
        }

        private async System.Threading.Tasks.Task CheckMomoTransaction(string orderId, string requestId)
        {
            try
            {
                string queryUrl = _config["Momo:QueryUrl"] ?? "https://test-payment.momo.vn/v2/gateway/api/query";
                string partnerCode = _config["Momo:PartnerCode"];
                string accessKey = _config["Momo:AccessKey"];
                string secretKey = _config["Momo:SecretKey"];

                string rawHash = $"accessKey={accessKey}&orderId={orderId}&partnerCode={partnerCode}&requestId={requestId}";
                string signature;
                using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secretKey)))
                {
                    signature = BitConverter.ToString(hmac.ComputeHash(Encoding.UTF8.GetBytes(rawHash))).Replace("-", "").ToLower();
                }

                var payload = new
                {
                    partnerCode,
                    requestId,
                    orderId,
                    lang = "vi",
                    signature
                };

                using var client = new HttpClient();
                var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
                var resp = await client.PostAsync(queryUrl, content);
                var json = await resp.Content.ReadAsStringAsync();

                dynamic data = JsonConvert.DeserializeObject(json);
                string resultCode = data?.resultCode?.ToString();

                if (resultCode == "0")
                {
                    _checkTimer.Stop();

                    // ✅ Hiển thị tích xanh trên Border
                    MomoQRCodeBorder.BorderBrush = Brushes.Green;
                    MomoQRCodeBorder.BorderThickness = new Thickness(3);

                    MessageBox.Show("Thanh toán MoMo thành công!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);

                    // Gọi ProcessPayment
                    ProcessPayment(2, $"Thanh toán {_cart?.Count ?? 0} món bằng MoMo");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi kiểm tra MoMo: {ex.Message}");
            }
        }
        #endregion

        #region CONFIRM PAYMENT
        private void ConfirmPayment_Click(object sender, RoutedEventArgs e)
        {
            if (RbCash?.IsChecked == true)
                ProcessPayment(1, $"Thanh toán {_cart?.Count ?? 0} món bằng tiền mặt");
            else
                MessageBox.Show("Vui lòng quét mã QR MoMo để thanh toán.", "Thông báo");
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            // Hủy thanh toán -> trở về OrderWindow với giỏ hàng cũ
            DialogResult = false;
            Close();
        }

        private void ProcessPayment(int methodId, string note)
        {
            try
            {
                decimal subtotal = (_cart ?? Enumerable.Empty<CartItem>()).Sum(c => c.UnitPrice * c.Quantity);
                decimal discount = 0;
                decimal vatAmount = (ChkVAT?.IsChecked == true) ? subtotal * 0.1m : 0;
                if (TxtDiscount != null) decimal.TryParse(TxtDiscount.Text, out discount);
                decimal total = subtotal + vatAmount - discount;

                if (methodId == 1 && TxtReceived != null)
                {
                    decimal.TryParse(TxtReceived.Text, out decimal received);
                    if (received < total)
                    {
                        MessageBox.Show("Tiền khách đưa không đủ!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                }

                var order = new Order
                {
                    TableId = _tableId,
                    OrderDate = DateTime.Now,
                    Status = "Completed",
                    TotalPrice = total,
                    FinalPrice = total,
                    CreatedBy = 3,
                    OrderSource = "InStore"
                };
                _orderService.CreateOrder(order);

                if (_cart != null)
                {
                    foreach (var item in _cart)
                    {
                        _orderService.AddOrderDetail(new OrderDetail
                        {
                            
                            OrderId = order.Id,
                            ProductId = item.ProductId,
                            Quantity = item.Quantity,
                            UnitPrice = item.UnitPrice,
                            SugarLevel = item.SugarLevel,
                            IceLevel = item.IceLevel,
                            Notes = item.Notes
                        });
                    }
                }

                _paymentService.AddPayment(order.Id, methodId, total, note);

                if (_tableId > 0)
                    _tableService.UpdateTableStatus(_tableId, "InProcess");

                // Thanh toán thành công -> quay về TableWindow
                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi xử lý thanh toán: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion
    }
}
