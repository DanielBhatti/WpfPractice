using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace Chapters.Chapter18
{
	public delegate void RangeChangedEventHandler(object sender, RangeChangedEventArgs args);

	[TemplatePart(Name = PARTID_StartThumb, Type = typeof(Thumb))]
	[TemplatePart(Name = PARTID_EndThumb, Type = typeof(Thumb))]
	[TemplatePart(Name = PARTID_Track, Type = typeof(Canvas))]
	public class RangeSelector : Control
	{
		private Thumb _startThumb;
		private Thumb _endThumb;
		private Canvas _track;

		private const string PARTID_StartThumb = "PART_Start";
		private const string PARTID_EndThumb = "PART_End";
		private const string PARTID_Track = "PART_Track";


		#region Dependency Properties
		public static readonly DependencyProperty MinimumProperty = DependencyProperty.Register(
			"Minimum", typeof(double), typeof(RangeSelector), new PropertyMetadata(0.0, OnMinimumChanged, CoerceMinimum));

		public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register(
			"Maximum", typeof(double), typeof(RangeSelector), new PropertyMetadata(0.0, OnMaximumChanged, CoerceMaximum));

		public static readonly DependencyProperty RangeStartProperty = DependencyProperty.Register(
			"RangeStart", typeof(double), typeof(RangeSelector), new PropertyMetadata(0.0, OnRangeStartChanged, CoerceRangeStart));

		public static readonly DependencyProperty RangeEndProperty = DependencyProperty.Register(
			"RangeEnd", typeof(double), typeof(RangeSelector), new PropertyMetadata(0.0, OnRangeEndChanged, CoerceRangeEnd));

		public static readonly DependencyProperty ComputedStartOffsetProperty = DependencyProperty.Register(
			"ComputedStartOffset", typeof(double), typeof(RangeSelector), new PropertyMetadata(0.0));

		public static readonly DependencyProperty ComputedEndOffsetProperty = DependencyProperty.Register(
			"ComputedEndOffset", typeof(double), typeof(RangeSelector), new PropertyMetadata(0.0));

		public static readonly DependencyProperty ComputedRangeWidthProperty = DependencyProperty.Register(
			"ComputedRangeWidth", typeof(double), typeof(RangeSelector));

		public double Minimum
		{
			get { return (double)GetValue(MinimumProperty); }
			set { SetValue(MinimumProperty, value); }
		}

		public double Maximum
		{
			get { return (double)GetValue(MaximumProperty); }
			set { SetValue(MaximumProperty, value); }
		}

		public double RangeStart
		{
			get { return (double)GetValue(RangeStartProperty); }
			set { SetValue(RangeStartProperty, value); }
		}

		public double RangeEnd
		{
			get { return (double)GetValue(RangeEndProperty); }
			set { SetValue(RangeEndProperty, value); }
		}

		public double ComputedStartOffset
		{
			get { return (double)GetValue(ComputedStartOffsetProperty); }
			private set { SetValue(ComputedStartOffsetProperty, value); }
		}

		public double ComputedEndOffset
		{
			get { return (double)GetValue(ComputedEndOffsetProperty); }
			private set { SetValue(ComputedEndOffsetProperty, value); }
		}

		public double ComputedRangeWidth
		{
			get { return (double)GetValue(ComputedRangeWidthProperty); }
			set { SetValue(ComputedRangeWidthProperty, value); }
		}

		#endregion

		#region Events
		public static readonly RoutedEvent RangeChangedEvent = EventManager.RegisterRoutedEvent("RangeChanged", RoutingStrategy.Bubble, typeof(RangeChangedEventHandler),
											 typeof(RangeSelector));

		public event RangeChangedEventHandler RangeChanged
		{
			add { AddHandler(RangeChangedEvent, value); }
			remove { RemoveHandler(RangeChangedEvent, value); }
		}

		#endregion

		static RangeSelector()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(RangeSelector), new FrameworkPropertyMetadata(typeof(RangeSelector)));
			FocusableProperty.OverrideMetadata(typeof(RangeSelector), new FrameworkPropertyMetadata(true));
		}

		public RangeSelector()
		{
			Loaded += delegate
						{
							if (_startThumb != null)
							{
								ComputedStartOffset = CalcX(RangeStart, _startThumb);
							}
							if (_endThumb != null)
							{
								ComputedEndOffset = CalcX(RangeEnd, _endThumb);
							}
							ComputedRangeWidth = ComputedEndOffset - ComputedStartOffset;
						};
		}

		private static void OnMinimumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			d.CoerceValue(RangeStartProperty);
			d.CoerceValue(RangeEndProperty);
			d.CoerceValue(MaximumProperty);

			RangeSelector selector = (RangeSelector)d;
			if (!selector.IsLoaded) return;
			selector.ComputedStartOffset = selector.CalcX(selector.RangeStart, selector._startThumb);
			selector.ComputedEndOffset = selector.CalcX(selector.RangeEnd, selector._endThumb);
			selector.ComputedRangeWidth = selector.ComputedEndOffset - selector.ComputedStartOffset;
		}

		private static void OnMaximumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			d.CoerceValue(RangeStartProperty);
			d.CoerceValue(RangeEndProperty);

			RangeSelector selector = (RangeSelector)d;
			if (!selector.IsLoaded) return;
			selector.ComputedStartOffset = selector.CalcX(selector.RangeStart, selector._startThumb);
			selector.ComputedEndOffset = selector.CalcX(selector.RangeEnd, selector._endThumb);
			selector.ComputedRangeWidth = selector.ComputedEndOffset - selector.ComputedStartOffset;
		}

		private static object CoerceMinimum(DependencyObject d, object basevalue)
		{
			double newValue = (double)basevalue;
			RangeSelector selector = (RangeSelector)d;
			if (newValue > selector.RangeStart) return selector.RangeStart;

			return basevalue;
		}

		private static object CoerceMaximum(DependencyObject d, object basevalue)
		{
			double newValue = (double)basevalue;
			RangeSelector selector = (RangeSelector)d;
			if (newValue < selector.RangeEnd) return selector.RangeEnd;

			return basevalue;
		}

		private static void OnRangeStartChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			RangeSelector selector = (RangeSelector)d;
			d.CoerceValue(MinimumProperty);
			d.CoerceValue(RangeEndProperty);

			if (!selector.IsLoaded) return;
			selector.ComputedStartOffset = selector.CalcX((double)e.NewValue, selector._startThumb);
			selector.ComputedRangeWidth = selector.ComputedEndOffset - selector.ComputedStartOffset;

			selector.RaiseRangeChangedEvent((double)e.NewValue, selector.RangeEnd);
		}

		private static void OnRangeEndChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			RangeSelector selector = (RangeSelector)d;
			d.CoerceValue(RangeStartProperty);
			d.CoerceValue(MaximumProperty);

			if (!selector.IsLoaded) return;
			selector.ComputedEndOffset = selector.CalcX((double)e.NewValue, selector._endThumb);
			selector.ComputedRangeWidth = selector.ComputedEndOffset - selector.ComputedStartOffset;

			selector.RaiseRangeChangedEvent(selector.RangeStart, (double)e.NewValue);
		}

		private void RaiseRangeChangedEvent(double start, double end)
		{
			if (AutomationPeer.ListenerExists(AutomationEvents.PropertyChanged))
			{
				RangeSelectorAutomationPeer peer = (RangeSelectorAutomationPeer) UIElementAutomationPeer.FromElement(this);
				if (peer != null)
				{
					peer.RaiseRangeChangedEvent(start, end);
				}
			}

			var args = new RangeChangedEventArgs(start, end);
			args.Source = this;

			RaiseEvent(args);
		}

		private static object CoerceRangeStart(DependencyObject d, object basevalue)
		{
			RangeSelector selector = (RangeSelector)d;
			double newStart = (double)basevalue;
			if (newStart < selector.Minimum) return selector.Minimum;
			if (newStart > selector.RangeEnd) return selector.RangeEnd;

			return basevalue;
		}

		private static object CoerceRangeEnd(DependencyObject d, object basevalue)
		{
			RangeSelector selector = (RangeSelector)d;
			double newValue = (double)basevalue;
			if (newValue < selector.RangeStart) return selector.RangeStart;
			if (newValue > selector.Maximum) return selector.Maximum;

			return basevalue;
		}

		private double CalcX(double value, Thumb thumb)
		{
			double denom = Maximum - Minimum;
			if (denom <= 0) denom = 1;

			double newValue = ((value - Minimum) / denom) * _track.ActualWidth;
			return Math.Min(newValue, _track.ActualWidth - thumb.ActualWidth);
		}

		public override void OnApplyTemplate()
		{
			// Track
			_track = GetTemplateChild(PARTID_Track) as Canvas;
			if (_track == null) return;

			// Start Thumb
			_startThumb = GetTemplateChild(PARTID_StartThumb) as Thumb;
			if (_startThumb != null)
			{
				SetupThumb(_startThumb, () => ComputedStartOffset, (x) => RangeStart = x);
			}

			// End Thumb
			_endThumb = GetTemplateChild(PARTID_EndThumb) as Thumb;
			if (_endThumb != null)
			{
				SetupThumb(_endThumb, () => ComputedEndOffset, (x) => RangeEnd = x);
			}
		}

		private void SetupThumb(Thumb thumb, Func<double> getOffset, Action<double> updaterAction)
		{
			thumb.DragDelta += delegate(object sender, DragDeltaEventArgs args)
										{
											double change = getOffset() + args.HorizontalChange;
											if (change < 0)
												change = 0;
											else if (change > _track.ActualWidth - thumb.ActualWidth)
												change = _track.ActualWidth - thumb.ActualWidth;

											double ratio = change / (_track.ActualWidth - thumb.ActualWidth);
											double newValue = Minimum + ratio * (Maximum - Minimum);

											updaterAction(newValue);
										};
		}

		#region Automation
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new RangeSelectorAutomationPeer(this);
		}
		#endregion
	}
}