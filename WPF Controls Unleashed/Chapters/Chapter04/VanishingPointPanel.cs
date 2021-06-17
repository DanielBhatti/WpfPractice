using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Chapters.Chapter04
{
	public class VanishingPointPanel : Panel
	{
		public static readonly DependencyProperty ZFactorProperty = DependencyProperty.Register(
			"ZFactor", typeof(double), typeof(VanishingPointPanel),
			new FrameworkPropertyMetadata(0.8D, FrameworkPropertyMetadataOptions.AffectsArrange));

		public static readonly DependencyProperty ItemHeightProperty = DependencyProperty.Register(
			"ItemHeight", typeof(double), typeof(VanishingPointPanel),
			new FrameworkPropertyMetadata(30.0D, FrameworkPropertyMetadataOptions.AffectsMeasure | 
													FrameworkPropertyMetadataOptions.AffectsArrange));

		public double ItemHeight
		{
			get { return (double)GetValue(ItemHeightProperty); }
			set { SetValue(ItemHeightProperty, value); }
		}

		public double ZFactor
		{
			get { return (double) GetValue(ZFactorProperty); }
			set { SetValue(ZFactorProperty, value); }
		}

		protected override Size MeasureOverride(Size constraint)
		{
			if (constraint.Width == double.PositiveInfinity || constraint.Height == double.PositiveInfinity)
				return Size.Empty;

			foreach (UIElement child in InternalChildren)
			{
				Size childSize = new Size(constraint.Width, ItemHeight);
				child.Measure(childSize);
			}

			return new Size(constraint.Width, ItemHeight * InternalChildren.Count);
		}

		protected override Size ArrangeOverride(Size finalSize)
		{
			int currentIndex = 0;

			for (int index = InternalChildren.Count - 1; index >= 0; index--)
			{
				Rect rect = CalculateRect(finalSize, currentIndex);
				InternalChildren[index].Arrange(rect);

				currentIndex++;
			}
			return finalSize;
		}

		private Rect CalculateRect(Size panelSize, int index)
		{
			double zFactor = Math.Pow(ZFactor, index);
			Size itemSize = new Size(panelSize.Width*zFactor, ItemHeight*zFactor);

			double left = (panelSize.Width - itemSize.Width)*0.5;
			double top = panelSize.Height;
			for (int i = 0; i <= index; i++)
			{
				top -= Math.Pow(ZFactor, i)*ItemHeight;
			}

			Rect rect = new Rect(itemSize);
			rect.Location = new Point(left, top);
			return rect;
		}
	}
}