using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;

namespace WpfPractice.WpfControlsUnleashed
{
	[ContentProperty("Children")]
	public class ZOrderControl : FrameworkElement
	{
		private readonly Size ChildSize = new Size(200, 100);

		public bool IsOrderReversed
		{
			get { return (bool)GetValue(IsOrderReversedProperty); }
			set { SetValue(IsOrderReversedProperty, value); }
		}

		public UIElementCollection Children { get; set; } 

		protected override int VisualChildrenCount
		{
			get { return Children.Count; }
		}

		public int Offset { get; set; } = 50;

		public static readonly DependencyProperty IsOrderReversedProperty = DependencyProperty.
			Register(
			"IsOrderReversed", typeof(bool), typeof(ZOrderControl),
			new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsArrange,
										  OnIsOrderReversedChanged));

		public ZOrderControl()
        {
			Children = new UIElementCollection(this, this);
		}

		private static void OnIsOrderReversedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ZOrderControl control = d as ZOrderControl;
			Reparent(control);
		}

		private static void Reparent(ZOrderControl control)
		{
			for (int i = 0; i < control.Children.Count; i++)
			{
				control.RemoveVisualChild(control.Children[i]);
			}

			for (int i = 0; i < control.Children.Count; i++)
			{
				control.AddVisualChild(control.Children[i]);
			}
		}

		protected override Size MeasureOverride(Size constraint)
		{
			if (constraint.Width == double.PositiveInfinity || constraint.Height == double.PositiveInfinity)
				return Size.Empty;

			for (int i = 0; i < Children.Count; i++)
			{
				Children[i].Measure(ChildSize);
			}

			return constraint;
		}

		protected override Size ArrangeOverride(Size finalSize)
		{
			for (int index = 0; index < Children.Count; index++)
			{
				Children[index].Arrange(new Rect(new Point(index * Offset, 0), ChildSize));
			}

			return finalSize;
		}

		protected override Visual GetVisualChild(int index)
		{
			if (index < 0 || index >= Children.Count)
			{
				throw new Exception("Bad Index");
			}

			// Works well for load-time
			// Reverse the Z order
			if (IsOrderReversed)
			{
				return Children[Children.Count - 1 - index];
			}

			// Normal Z order
			return Children[index];
		}
	}
}
