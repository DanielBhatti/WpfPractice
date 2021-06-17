using System.Windows;

namespace Chapters.Chapter18
{
	public class RangeChangedEventArgs : RoutedEventArgs
	{
		public double RangeStart { get; private set; }
		public double RangeEnd { get; private set; }

		public RangeChangedEventArgs(double start, double end)
		{
			RangeStart = start;
			RangeEnd = end;
			RoutedEvent = RangeSelector.RangeChangedEvent;
		}
	}
}