# SeePrices

![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white)
![.NET 10](https://img.shields.io/badge/.NET%2010-512BD4?style=for-the-badge&logo=.net&logoColor=white)
![Status](https://img.shields.io/badge/Status-Functional-brightgreen?style=for-the-badge)

A real-time stock monitor built in C# that integrates with financial APIs to track B3 (Brazilian Stock Exchange) assets. This project focuses on asynchronous programming, secure credential management, and resilient error handling.

## Features
* Real-Time Data: Fetches live prices directly from the [Brapi API](https://brapi.dev/).
* Secure Secrets: Implements *.NET User Secrets* to ensure API tokens are never exposed in the source code.
* Resilient Logic: Includes custom `try-catch` filters to handle network issues and invalid tickers (HTTP 404).
* Smart Validation: Robust input handling for target prices and stock symbols.

## Security & Best Practices
Following professional standards, this project separates logic from credentials. *API Tokens are stored locally using the Secret Manager*, keeping the repository safe for public display and contribution.

## Technologies
* C# / .NET 10
* System.Text.Json For high-performance data processing.
* Microsoft.Extensions.Configuration: To manage secure user secrets.

## Roadmap
- [ ] Implement a *Watchdog Loop* for automatic updates every 30 seconds.
- [ ] Add *Audio Alerts* using `Console.Beep` when target prices are reached.
- [ ] Export monitoring logs to local files.