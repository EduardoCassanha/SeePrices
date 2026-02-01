using System;
using System.Globalization;

namespace SeePrices.Services
{
    public static class StockMonitor
    {
        public static decimal GetTargetPrice()
        {
            while(true)
            {
                Console.Write("Enter your target price: ");
                string? input = Console.ReadLine();

                // Using CurrentCulture to match the application's pt-BR configuration
                if (decimal.TryParse(input, NumberStyles.Any, CultureInfo.CurrentCulture, out decimal price))
                    return price;

                Console.WriteLine("Invalid price format.");
            }
        }

        public static void ProcessAlert(string tickerSymbol, decimal target, decimal current)
        {
            // Trigger alert when current price reaches or falls below the target
            if (current <= target)
            {
                Console.WriteLine("ALERT: Time to buy!");

                Logger.Log($"{tickerSymbol} reached the target price. Target: {target:C} | Current: {current:C}");

                // Play an audible alert on Windows environments
                if (OperatingSystem.IsWindows())
                {
                    Console.Beep(1000, 500);
                }
            }
            else
            {
                Console.WriteLine("Status: Target price not reached. Monitoring...");
            }
        }
    }
}