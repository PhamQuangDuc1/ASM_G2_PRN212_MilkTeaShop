using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;

public class MomoService
{
    private readonly string endpoint;
    private readonly string partnerCode;
    private readonly string accessKey;
    private readonly string secretKey;
    private readonly string returnUrl;
    private readonly string notifyUrl;

    public MomoService(IConfiguration config)
    {
        var momoConfig = config.GetSection("Momo");
        endpoint = momoConfig["Endpoint"];
        partnerCode = momoConfig["PartnerCode"];
        accessKey = momoConfig["AccessKey"];
        secretKey = momoConfig["SecretKey"];
        returnUrl = momoConfig["ReturnUrl"];
        notifyUrl = momoConfig["NotifyUrl"];
    }

    public async Task<string> CreatePaymentUrl(string orderId, decimal amount)
    {
        var requestId = Guid.NewGuid().ToString();
        var rawHash = $"accessKey={accessKey}&amount={amount}&extraData=&ipnUrl={notifyUrl}&orderId={orderId}&orderInfo=Thanh toán đơn hàng&partnerCode={partnerCode}&redirectUrl={returnUrl}&requestId={requestId}&requestType=captureWallet";

        // Sign HMAC SHA256
        string signature;
        using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secretKey)))
        {
            signature = BitConverter.ToString(hmac.ComputeHash(Encoding.UTF8.GetBytes(rawHash))).Replace("-", "").ToLower();
        }

        var payload = new
        {
            partnerCode,
            accessKey,
            requestId,
            amount = amount.ToString("0"),
            orderId,
            orderInfo = "Thanh toán đơn hàng",
            returnUrl,
            notifyUrl,
            extraData = "",
            requestType = "captureWallet",
            signature
        };

        using var client = new HttpClient();
        var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
        var response = await client.PostAsync(endpoint, content);
        var respString = await response.Content.ReadAsStringAsync();

        dynamic data = JsonConvert.DeserializeObject(respString);
        return data.payUrl; // Đây là URL để tạo QR code
    }
}
