using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Chapters.Chapter07
{
	public class HScrollingPanel : Panel
	{
		public static readonly DependencyProperty ChildSizeProperty = DependencyProperty.Register(
			"ChildSize", typeof (Size), typeof (HScrollingPanel), new PropertyMetadata(new Size(200, 100)));

		public static readonly DependencyProperty ViewportSizeProperty = DependencyProperty.Register(
			"ViewportSize", typeof (double), typeof (HScrollingPanel));

		public static readonly DependencyProperty ExtentSizeProperty = DependencyProperty.Register(
			"ExtentSize", typeof (double), typeof (HScrollingPanel));

		public static readonly DependencyProperty HorizontalOffsetProperty = DependencyProperty.Register(
			"HorizontalOffset", typeof (double), typeof (HScrollingPanel),
			new PropertyMetadata(0.0, OnHorizontalOffsetChanged));

		private static void OnHorizontalOffsetChanged(DependencyObject d,
		                                              DependencyPropertyChangedEventArgs e)
		{
			HScrollingPanel panel = d as HScrollingPanel;
			double x = (double) e.NewValue;
			panel._trans.X = -1*x;
		}

		private TranslateTransform _trans;

		public Size ChildSize
		{
			get { return (Size) GetValue(ChildSizeProperty); }
			set { SetValue(ChildSizeProperty, value); }
		}

		public double HorizontalOffset
		{
			get { return (double) GetValue(HorizontalOffsetProperty); }
			set { SetValue(HorizontalOffsetProperty, value); }
		}

		public double ViewportSize
		{
			get { return (double) GetValue(ViewportSizeProperty); }
			set { SetValue(ViewportSizeProperty, value); }
		}

		public double ExtentSize
		{
			get { return (double) GetValue(ExtentSizeProperty); }
			set { SetValue(ExtentSizeProperty, value); }
		}

		public HScrollingPanel()
		{
			_trans = new TranslateTransform();
			RenderTransform = _trans;
		}

		protected override Size MeasureOverride(Size constraint)
		{
			if (constraint.Width == double.PositiveInfinity || constraint.Height == double.PositiveInfinity)
				return Size.Empty;

			UpdateScrollInfo(constraint);

			foreach (UIElement child in InternalChildren)
			{
				child.Measure(ChildSize);
			}

			return constraint;
		}

		protected override Size ArrangeOverride(Size finalSize)
		{
			UpdateScrollInfo(finalSize);
			for (int i = 0; i < InternalChildren.Count; i++)
			{
				InternalChildren[i].Arrange(new Rect(new Point(i*ChildSize.Width, 0), ChildSize));
			}
			return finalSize;
		}

		private void UpdateScrollInfo(Size size)
		{
			// Adjust ViewportSize
			if (size.Width != ViewportSize)
			{
				ViewportSize = size.Width;
			}

			// Adjust ExtentSize
			double extent = InternalChildren.Count*ChildSize.Width - ViewportSize;
			if (extent != ExtentSize)
			{
				ExtentSize = extent;
			}
		}
	}
}