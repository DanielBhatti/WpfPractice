/*----------
* Created on: 02/29/2008
* Created by: Pavan Podila
* http://blog.pixelingene.com
----------*/

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;

namespace Chapters.Chapter04
{
	[ContentProperty("Children")]
	public class ZOrderControl2 : FrameworkElement
	{
		private int _offset = 50;
		private readonly Size ChildSize = new Size(200, 100);
		private UIElementCollection _children;

		public ZOrderControl2()
		{
			Children = new UIElementCollection(this, this);
		}

		#region Dependency Properties

		public static readonly DependencyProperty IsOrderReversedProperty = DependencyProperty.
			Register(
			"IsOrderReversed", typeof (bool), typeof (ZOrderControl2),
			new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsArrange,
			                              OnIsOrderReversedChanged));

		private static void OnIsOrderReversedChanged(DependencyObject d,
		                                             DependencyPropertyChangedEventArgs e)
		{
			ZOrderControl2 control = d as ZOrderControl2;
			Reparent(control);
		}

		private static void Reparent(ZOrderControl2 control)
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

		#endregion

		#region Properties

		public bool IsOrderReversed
		{
			get { return (bool) GetValue(IsOrderReversedProperty); }
			set { SetValue(IsOrderReversedProperty, value); }
		}

		public UIElementCollection Children
		{
			get { return _children; }
			set { _children = value; }
		}

		public int Offset
		{
			get { return _offset; }
			set { _offset = value; }
		}

		#endregion

		#region Layout Overrides

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
				Children[index].Arrange(new Rect(new Point(index*Offset, 0), ChildSize));
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

		protected override int VisualChildrenCount
		{
			get { return Children.Count; }
		}

		#endregion
	}
}