using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace Chapters.Chapter08
{
	public partial class StaggeredPanel : VirtualizingPanel, IScrollInfo
	{
		TranslateTransform _transform = new TranslateTransform();
		private int StartIndex { get; set; }
		private int EndIndex { get; set; }


		#region Vertical movement

		public void LineUp()
		{
		}

		public void LineDown()
		{
		}

		public void PageUp()
		{
		}

		public void PageDown()
		{
		}

		public void MouseWheelUp()
		{
			LineLeft();
		}

		public void MouseWheelDown()
		{
			LineRight();
		}

		public void SetVerticalOffset(double offset)
		{
		}

		#endregion

		public void LineLeft()
		{
			SetHorizontalOffset(HorizontalOffset - 16.0);
		}

		public void LineRight()
		{
			SetHorizontalOffset(HorizontalOffset + 16.0);
		}

		public void PageLeft()
		{
			SetHorizontalOffset(HorizontalOffset - ViewportWidth);
		}

		public void PageRight()
		{
			SetHorizontalOffset(HorizontalOffset + ViewportWidth);
		}

		public void MouseWheelLeft()
		{
			SetHorizontalOffset(HorizontalOffset - SystemParameters.WheelScrollLines);
		}

		public void MouseWheelRight()
		{
			SetHorizontalOffset(HorizontalOffset + SystemParameters.WheelScrollLines);
		}

		public void SetHorizontalOffset(double offset)
		{
			HorizontalOffset = CalculateHorizontalOffset(offset);

			if (ScrollOwner != null)
				ScrollOwner.InvalidateScrollInfo();

			_transform.X = -1 * HorizontalOffset;

			// Force us to realize the correct children
			InvalidateMeasure();
		}

		private double CalculateHorizontalOffset(double offset)
		{
			if (offset < 0 || ViewportWidth >= ExtentWidth)
			{
				offset = 0;
			}
			else
			{
				if (offset + ViewportWidth >= ExtentWidth)
				{
					offset = ExtentWidth - ViewportWidth;
				}
			}
			return offset;
		}

		public Rect MakeVisible(Visual visual, Rect rectangle)
		{
			return rectangle;
		}

		public bool CanVerticallyScroll { get; set; }
		public bool CanHorizontallyScroll { get; set; }
		public double ExtentWidth { get; set; }
		public double ExtentHeight { get; set; }
		public double ViewportWidth { get; set; }
		public double ViewportHeight { get; set; }
		public double HorizontalOffset { get; set; }
		public double VerticalOffset { get; set; }
		public ScrollViewer ScrollOwner { get; set; }

	}
}