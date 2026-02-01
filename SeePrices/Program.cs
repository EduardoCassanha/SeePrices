using System.Net.Http;
using System.Text.Json;
using System.Globalization;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;
using SeePrices.Models;
using SeePrices.Services;

// Force application to use Brazilian culture (currency, decimals, date format)
var culture = new CultureInfo("pt-BR");
CultureInfo.DefaultThreadCurrentCulture = culture;
CultureInfo.DefaultThreadCurrentUICulture = culture;

// Data entry
Console.WriteLine("- SeePrices: Stock Monitor -");

Logger.Log("Application started", "START");

// Handle Ctrl+C to stop monitoring and persist logs
Console.CancelKeyPress += (sender, e) =>
{
    e.Cancel = true;

    Console.WriteLine("\n\n[SYSTEM] Ending session...");

    SeePrices.Services.Logger.Log("Application stopped by user (Ctrl+C)", "STOP");

    // The console stays open by 5s until close
    System.Threading.Thread.Sleep(5000);

    Environment.Exit(0);
};

string ticker;
// B3 ticker format: 4 letters + 1 or 2 numbers (e.g., PETR4, VALE3)
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

decimal targetPrice = StockMonitor.GetTargetPrice();

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

Console.Clear();
Console.WriteLine($"--- SeePrices: Monitoring {ticker} ---");
Console.WriteLine($"Target Price: {targetPrice:C} | Press Ctrl+C to stop");
Console.WriteLine("---------------------------------------------");

// API request
using HttpClient client = new HttpClient();
client.Timeout = TimeSpan.FromSeconds(10);

while (true)
{

    try
    {

        string url = $"https://brapi.dev/api/quote/{ticker}?token={token}";
        string jsonResponse = await client.GetStringAsync(url);
        var data = JsonSerializer.Deserialize<BrapiResponse>(jsonResponse);

        if (data?.results != null && data.results.Count > 0)
        {
            decimal currentPrice = data.results[0].regularMarketPrice;

            // Actual price with last update
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] {ticker} | Preço: {currentPrice:C}");

            StockMonitor.ProcessAlert(ticker, targetPrice, currentPrice);
        }
    }
    catch (HttpRequestException ex)
    {
        Logger.Log($"Connection error while fetching {ticker}: {ex.Message}", "ERROR");
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
    // Clear countdown line before printing the next update
    Console.Write("\r" + new string(' ', 40) + "\r");
}