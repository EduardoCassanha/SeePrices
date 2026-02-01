using System.Net.Http;
using System.Text.Json;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

// Data entry
Console.WriteLine("- SeePrices: Stock Monitor -");
Console.Write("Enter the stock ticker (e.g., PETR4):");
string ticker = Console.ReadLine()?.ToUpper() ?? "PETR4";

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

try
{

    string url = $"https://brapi.dev/api/quote/{ticker}?token={token}";
    string jsonResponse = await client.GetStringAsync(url);
    var data = JsonSerializer.Deserialize<BrapiResponse>(jsonResponse);

    if (data?.results != null && data.results.Count > 0)
    {
        decimal currentPrice = data.results[0].regularMarketPrice;

        Console.WriteLine($"\nSuccess! Monitoring {ticker}");
        Console.WriteLine($"Target: {targetPrice:C} | Current: {currentPrice:C}");
        Console.WriteLine("------------------------------");

        ProcessAlert(ticker, targetPrice, currentPrice);
    }
    else
    {
        Console.WriteLine("Error: Stock data not found.");
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

// DEFINITIONS
decimal GetTargetPrice()
{
    while (true)
    {
        Console.Write("Enter your target price: ");
        if (decimal.TryParse(Console.ReadLine(), out decimal price))
            return price;

        Console.WriteLine("Invalid entry. Please use numeric values (e.g., 15.50).");
    }
}

void ProcessAlert(string tickerSymbol, decimal target, decimal current)
{
    Console.WriteLine($"\n-- Checking {tickerSymbol} --");
    Console.WriteLine($"Current Price: {current:C}");
    Console.WriteLine($"Target price: {target:C}");

    if (current <= target)
    {
        Console.WriteLine("ALERT: Time to buy!");
    }
    else
    {
        Console.WriteLine("Status: Waiting...");
    }
}

public class BrapiResponse
{
    public List<StockResult> results { get; set; }
}
public class StockResult
{
    public decimal regularMarketPrice { get; set; }
}