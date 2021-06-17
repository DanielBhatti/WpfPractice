using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Chapters.Chapter08
{
	public partial class StaggeredPanel : VirtualizingPanel, IScrollInfo
	{
		public static readonly DependencyProperty ItemWidthProperty = DependencyProperty.
			Register(
			"ItemWidth", typeof(double), typeof(StaggeredPanel), new FrameworkPropertyMetadata(200.0, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange));

		public static readonly DependencyProperty ItemHeightProperty = DependencyProperty.
			Register(
			"ItemHeight", typeof(double), typeof(StaggeredPanel), new FrameworkPropertyMetadata(100.0, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange));

		public static readonly DependencyProperty StaggerValueProperty = DependencyProperty.Register(
			"StaggerValue", typeof(double), typeof(StaggeredPanel), new PropertyMetadata(0.2));

		public double ItemWidth
		{
			get { return (double)GetValue(ItemWidthProperty); }
			set { SetValue(ItemWidthProperty, value); }
		}

		public double ItemHeight
		{
			get { return (double)GetValue(ItemHeightProperty); }
			set { SetValue(ItemHeightProperty, value); }
		}

		public double StaggerValue
		{
			get { return (double)GetValue(StaggerValueProperty); }
			set { SetValue(StaggerValueProperty, value); }
		}

		private ItemsControl ItemsOwner { get; set; }

		public StaggeredPanel()
		{
			this.RenderTransform = _transform;
			CanVerticallyScroll = false;
			CanHorizontallyScroll = false;
		}

		protected override void OnInitialized(EventArgs e)
		{
			ItemsOwner = ItemsControl.GetItemsOwner(this) as ItemsControl;
		}

		protected override Size MeasureOverride(Size availableSize)
		{
			UpdateScrollInfo(availableSize);

			// Virtualize items
			VirtualizeItems();

			// Measure
			foreach (UIElement child in InternalChildren)
			{
				child.Measure(new Size(ItemWidth, ItemHeight));
			}

			// Cleanup
			CleanupItems();

			return availableSize;
		}

		protected override Size ArrangeOverride(Size finalSize)
		{
			int childCount = InternalChildren.Count;
			for (int i = 0; i < childCount; i++ )
			{
				int index = StartIndex + i;
				double left = index*ItemWidth*StaggerValue;
				Rect arrangeRect = new Rect(left, 0, ItemWidth, ItemHeight);
				InternalChildren[i].Arrange(arrangeRect);
			}

			return finalSize;
		}

		private void UpdateScrollInfo(Size availableSize)
		{
			// See how many items there are
			int itemCount = ItemsOwner.Items.Count;
			bool viewportChanged = false;
			bool extentChanged = false;

			double extent = CalculateExtent(availableSize, itemCount);
			// Update extent
			if (extent != ExtentWidth)
			{
				ExtentWidth = extent;
				extentChanged = true;
			}

			// Update viewport
			if (availableSize.Width != ViewportWidth)
			{
				ViewportWidth = availableSize.Width;
				viewportChanged = true;
			}

			if ((extentChanged || viewportChanged) && ScrollOwner != null)
			{
				HorizontalOffset = CalculateHorizontalOffset(HorizontalOffset);
				ScrollOwner.InvalidateScrollInfo();

				_transform.X = -1*HorizontalOffset;
			}
		}

		private double CalculateExtent(Size size, int count)
		{
			if (count == 0) return 0;

			double visibleArea = ItemWidth*StaggerValue;
			return (count-1)*visibleArea + ItemWidth;
		}

		private void VirtualizeItems()
		{
			UpdateIndexRange();

			IItemContainerGenerator generator = ItemsOwner.ItemContainerGenerator;

			GeneratorPosition startPos = generator.GeneratorPositionFromIndex(StartIndex);
			int childIndex = startPos.Offset == 0 ? startPos.Index : startPos.Index + 1;
			using (generator.StartAt(startPos, GeneratorDirection.Forward, true))
			{
				for (int i = StartIndex; i <= EndIndex; i++, childIndex++)
				{
					bool isNewlyRealized;
					UIElement child = generator.GenerateNext(out isNewlyRealized) as UIElement;
					if (isNewlyRealized)
					{
						if (childIndex >= InternalChildren.Count)
						{
							AddInternalChild(child);
						}
						else
						{
							InsertInternalChild(childIndex, child);
						}
						generator.PrepareItemContainer(child);
					}
				}
			}
		}

		private void CleanupItems()
		{
			IItemContainerGenerator generator = ItemsOwner.ItemContainerGenerator;
			for (int i = InternalChildren.Count - 1; i >= 0; i--)
			{
				GeneratorPosition position = new GeneratorPosition(i, 0);
				int itemIndex = generator.IndexFromGeneratorPosition(position);
				if (itemIndex < StartIndex || itemIndex > EndIndex)
				{
					generator.Remove(position, 1);
					RemoveInternalChildRange(i, 1);
				}
			}
		}

		private void UpdateIndexRange()
		{
			double left = HorizontalOffset;
			double right = Math.Min(ExtentWidth, HorizontalOffset + ViewportWidth);
			StartIndex = CalculateIndexFromOffset(left);
			EndIndex = CalculateIndexFromOffset(right);

			Debug.WriteLine("Index Range : [ " + StartIndex + ", " + EndIndex + " ]");
		}

		private int CalculateIndexFromOffset(double offset)
		{
			if (offset >= ExtentWidth - ItemWidth && offset <= ExtentWidth)
				return ItemsOwner.Items.Count - 1;

			double visibleArea = ItemWidth*StaggerValue;
			int index = (int)Math.Floor(offset/visibleArea);

			return index;
		}
	}
}