
// 1. DATA ENTRY

Console.WriteLine("- SeePricees: Stock Monitor -");

Console.Write("Enter the stock ticker: ");
string ticker = Console.ReadLine()?.ToUpper() ?? "NONE"; // ToUpper ensures uppercase

// 2. EXECUTION
decimal targetPrice = GetTargetPrice();

Console.WriteLine($"\nSuccess! Monitoring {ticker}");
Console.WriteLine($"Target price set at: {targetPrice:C}");
Console.WriteLine("------------------------------");

// Comparison logic
CheckPrice(ticker, targetPrice);

// 3. DEFINITIONS

decimal GetTargetPrice()
{
    bool validEntry = false;
    decimal convertedPrice = 0;

    do
    {
        Console.Write("Enter a valid price: ");
        string? entry = Console.ReadLine();

        if (decimal.TryParse(entry, out convertedPrice))
        {
            validEntry = true;
        }
        else
        {
            Console.WriteLine("Invalid input. Please enter a numeric value (e.g., 34.50).");
        }
    } while (!validEntry);

    return convertedPrice;
}

void CheckPrice(string tickerSymbol, decimal target)
{
    decimal currentPrice = 35.00m; // Real price simulation

    Console.WriteLine($"\n-- Checking {tickerSymbol} --");
    Console.WriteLine($"Current Price: {currentPrice:C}");
    Console.WriteLine($"Target price: {target:C}");

    if (currentPrice <= target)
    {
        Console.WriteLine("ALERT: Time to buy!");
    }
    else
    {
        Console.WriteLine("Status: Waiting...");
    }
}