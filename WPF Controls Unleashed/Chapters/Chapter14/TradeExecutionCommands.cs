using System.Windows.Input;

namespace Chapters.Chapter14
{
	public static class TradeExecutionCommands
	{
		public static readonly RoutedCommand SendNegotiation;
		public static readonly RoutedCommand StartStreaming;
		public static readonly RoutedCommand Bid;
		public static readonly RoutedCommand Ask;
		public static readonly RoutedCommand Buy;

		static TradeExecutionCommands()
		{
			SendNegotiation = new RoutedCommand("SendNegotiation", typeof(TradeExecutionCommands));
			StartStreaming = new RoutedCommand("StartStreaming", typeof(TradeExecutionCommands));
			Bid= new RoutedCommand("Bid", typeof(TradeExecutionCommands));
			Ask = new RoutedCommand("Ask", typeof(TradeExecutionCommands));
			Buy = new RoutedCommand("Buy", typeof(TradeExecutionCommands));
		}
	}
}