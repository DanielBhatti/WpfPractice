using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;

namespace Chapters.Chapter18
{
	public class RangeSelectorAutomationPeer : FrameworkElementAutomationPeer, IValueProvider
	{
		private string _currentValue;

		public RangeSelectorAutomationPeer(RangeSelector owner)
			: base(owner)
		{
		}

		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.Custom;
		}

		protected override string GetClassNameCore()
		{
			return OwningRangeSelector.GetType().Name;
		}

		public override object GetPattern(PatternInterface patternInterface)
		{
			switch (patternInterface)
			{
				case PatternInterface.Value:
					return this;
			}

			return base.GetPattern(patternInterface);
		}

		public void SetValue(string value)
		{
			string[] range = value.Split(new[] { ',' });
			if (range.Length == 2)
			{
				double start, end;

				// Set the range only if both values parse correctly
				if (double.TryParse(range[0], out start) &&
					double.TryParse(range[1], out end))
				{
					OwningRangeSelector.RangeStart = start;
					OwningRangeSelector.RangeEnd = end;
				}
			}
		}

		public string Value
		{
			get
			{
				return FormatValue(OwningRangeSelector.RangeStart, OwningRangeSelector.RangeEnd);
			}
		}

		private static string FormatValue(double start, double end)
		{
			return string.Format("{0:F2},{1:F2}", start, end);
		}

		public bool IsReadOnly
		{
			get { return false; }
		}

		private RangeSelector OwningRangeSelector
		{
			get { return (RangeSelector)Owner; }
		}
		
		public void RaiseRangeChangedEvent(double start, double end)
		{
			string newValue = FormatValue(start, end);
			RaisePropertyChangedEvent(ValuePatternIdentifiers.ValueProperty, _currentValue, newValue);

			_currentValue = newValue;
		}

	}

 

 

}