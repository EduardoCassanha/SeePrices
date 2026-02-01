using System.Net.Http;
using System.Text.Json;
using System.Globalization;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;

// Data entry
Console.WriteLine("- SeePrices: Stock Monitor -");

Console.CancelKeyPress += (s, e) =>
{
    e.Cancel = true;
    Console.WriteLine("\nMonitoring session terminated.");
    Environment.Exit(0);
};

string ticker;
string pattern = @"^[A-Z]{4}[0-9]{1,2}$";

while (true)
{
    Console.Write("Enter the stock ticker (e.g., PETR4):");
    ticker = Console.ReadLine()?.ToUpper() ?? "";
    
    if (Regex.IsMatch(ticker, pattern))
    {
        break;
    }

    Console.WriteLine("Invalid format. Please use 4 letters and 1 or 2 numbers (Ex: VALE3).");
}

decimal targetPrice = GetTargetPrice();

// Configuration and secrets
var config = new ConfigurationBuilder()
    .AddUserSecrets<Program>()
    .Build();

string? token = config["BrapiToken"];

if (string.IsNullOrEmpty(token))
{
    Console.WriteLine("Error: API Token not found in User Secrets.");
    return;
}

// API request
using HttpClient client = new HttpClient();
client.Timeout = TimeSpan.FromSeconds(10);

while (true)
{
    Console.Clear();
    Console.WriteLine($"--- SeePrices: Monitoring {ticker} ---");
    Console.WriteLine($"Target Price: {targetPrice:C} | Press Ctrl+C to stop");
    Console.WriteLine("---------------------------------------------");

    try
    {

        string url = $"https://brapi.dev/api/quote/{ticker}?token={token}";
        string jsonResponse = await client.GetStringAsync(url);
        var data = JsonSerializer.Deserialize<BrapiResponse>(jsonResponse);

        if (data?.results != null && data.results.Count > 0)
        {
            decimal currentPrice = data.results[0].regularMarketPrice;

            // Actual price with last update
            Console.WriteLine($"[{DateTime.Now:dd/MM/yy HH:mm:ss}] Current Price: {currentPrice:C}");

            ProcessAlert(ticker, targetPrice, currentPrice);
        }
    }
    catch (HttpRequestException ex)
    {
        if (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            Console.WriteLine($"\nError: The ticker '{ticker}' was not found on B3.");
        }
        else
        {
            Console.WriteLine("Connection error: Please check your internet or API token.");
        }
    }
    for (int i = 30; i > 0; i--)
    {
        Console.Write($"\rNext update in {i:D2}s...");
        await Task.Delay(1000);
    }

}

// Definitions
decimal GetTargetPrice()
{
    while (true)
    {
        Console.Write("Enter your target price: ");
        string? input = Console.ReadLine();

        if (decimal.TryParse(input, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out decimal price))
            return price;

        Console.WriteLine("Invalid price format.");
    }
}

void ProcessAlert(string tickerSymbol, decimal target, decimal current)
{
    if (current <= target)
    {
        Console.WriteLine("ALERT: Time to buy!");

        if (OperatingSystem.IsWindows())
        {
            Console.Beep(1000, 800); // Triggers a beep
        }
    }
    else
    {
        Console.WriteLine("Status: Target price not reached. Monitoring...");
    }
}

public class BrapiResponse
{
    public List<StockResult>? results { get; set; }
}
public class StockResult
{
    public decimal regularMarketPrice { get; set; }
}