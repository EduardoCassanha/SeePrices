using System.Collections.Generic;

namespace SeePrices.Models
{
	public class BrapiResponse
	{
		public List<StockResult>? results { get; set; }
	}
	public class StockResult
	{
		public decimal regularMarketPrice { get; set; }
	}
}