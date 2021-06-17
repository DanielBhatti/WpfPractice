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
	public class ZOrderControl : FrameworkElement
	{
	    private readonly Size ChildSize = new Size(200, 100);

	    public ZOrderControl()
	    {
	        Children = new UIElementCollection(this, this);
	        Offset = 50;
	    }

	    #region Properties

	    public bool IsOrderReversed { get; set; }

	    public int Offset { get; set; }

	    public UIElementCollection Children { get; set; }

	    #endregion

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
	}
}