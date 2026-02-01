# SeePrices
![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white)
![.NET 10](https://img.shields.io/badge/.NET%2010-512BD4?style=for-the-badge&logo=.net&logoColor=white)
![Status](https://img.shields.io/badge/Status-Functional-brightgreen?style=for-the-badge)

**SeePrices** is a C# .NET 10 console application designed to monitor Brazilian stock prices (B3) in real-time and trigger alerts when target prices are reached.

This project was built as a practical learning exercise focused on API consumption, modular architecture, culture-aware formatting, secure secret management, and persistent logging.

## Demo

![SeePrices in action](assets/seeprices.png)

## Features

- **Real-time monitoring**: Fetches stock prices every 30 seconds via [Brapi API](https://brapi.dev/)
- **Price alerts**: Audible notification when target is reached (Windows only)
- **Persistent logging**: All events are logged to timestamped files in the `logs/` directory
- **Brazilian culture support**: Currency (BRL), decimals, and date formatting in pt-BR
- **Secure credentials**: API tokens managed via .NET User Secrets (never exposed in code)
- **Input validation**: Regex-based ticker validation and culture-aware price parsing
- **Funcional Shutdown**: Proper Ctrl+C handling with final log persistence

## Project Structure
```
SeePrices/
├── Program.cs              # Entry point & monitoring loop
├── Models/
│   └── BrapiModels.cs      # API response DTOs
├── Services/
│   ├── Logger.cs           # File-based logging system
│   └── StockMonitor.cs     # Price validation & alert handling
└── logs/                   # Auto-generated log files (gitignored)
```

## Technologies

- **C# / .NET 10**
- **System.Text.Json** for high-performance JSON deserialization
- **Microsoft.Extensions.Configuration** for User Secrets integration
- **HttpClient** with async/await for non-blocking API calls
- **Regex** for ticker format validation
- **CultureInfo** for pt-BR numeric and currency handling

## Getting Started

### Prerequisites
- .NET 10 SDK or higher
- Free API token from [Brapi.dev](https://brapi.dev/)

### Installation
```bash
# Clone the repository
git clone https://github.com/EduardoCassanha/seeprices.git
cd seeprices

# Configure your API token securely
dotnet user-secrets init
dotnet user-secrets set "BrapiToken" "YOUR_TOKEN_HERE"

# Run the application
dotnet run
```

### Usage Example
```
- SeePrices: Stock Monitor -
Enter the stock ticker (e.g., PETR4): VALE3
Enter your target price: 65,50

--- SeePrices: Monitoring VALE3 ---
Target Price: R$ 65,50 | Press Ctrl+C to stop
---------------------------------------------
[14:23:15] VALE3 | Preço: R$ 67,32
Status: Target price not reached. Monitoring...
Next update in 30s...
```

## Roadmap

- [x] Persist monitoring logs to local files
- [x] Extract API communication into dedicated service
- [ ] Support monitoring multiple tickers simultaneously
- [ ] Add CSV export for historical price data
- [ ] Implement WebSocket for real-time updates (reduce API calls)

## Security Note

This project uses .NET User Secrets to keep API tokens out of source control. Never commit your `BrapiToken` or any credentials to version control.

## License

MIT License — feel free to use this project for learning purposes.