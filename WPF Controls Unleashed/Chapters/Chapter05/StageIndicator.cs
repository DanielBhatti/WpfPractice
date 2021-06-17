using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Chapters.Chapter05
{
	public class StageIndicator : Decorator
	{
		private TabControl _tabControl;

		public static readonly DependencyProperty BackgroundProperty = DependencyProperty.Register(
			"Background", typeof(Brush), typeof(StageIndicator));

		public static readonly DependencyProperty BorderBrushProperty = DependencyProperty.Register(
			"BorderBrush", typeof(Brush), typeof(StageIndicator));

		private static readonly DependencyProperty ItemRectProperty = DependencyProperty.Register(
			"ItemRect", typeof(Rect), typeof(StageIndicator), new FrameworkPropertyMetadata(new Rect(), FrameworkPropertyMetadataOptions.AffectsRender));

		private Rect _prevRect;

		public Brush Background
		{
			get { return (Brush)GetValue(BackgroundProperty); }
			set { SetValue(BackgroundProperty, value); }
		}

		public Brush BorderBrush
		{
			get { return (Brush)GetValue(BorderBrushProperty); }
			set { SetValue(BorderBrushProperty, value); }
		}

		protected override void OnInitialized(System.EventArgs e)
		{
			_tabControl = TemplatedParent as TabControl;
			if (_tabControl != null)
				_tabControl.Loaded += OnTabControlLoaded;
		}

		private void OnTabControlLoaded(object sender, RoutedEventArgs e)
		{
			_tabControl.Loaded -= OnTabControlLoaded;
			_tabControl.SelectionChanged += OnTabChanged;
			_prevRect = GetItemRect();
			SetValue(ItemRectProperty, _prevRect);
		}

		private void OnTabChanged(object sender, SelectionChangedEventArgs e)
		{
			Storyboard sb = new Storyboard();

			Rect newRect = GetItemRect();
			RectAnimation anim = new RectAnimation(_prevRect, newRect, new Duration(TimeSpan.FromMilliseconds(250)));
			Storyboard.SetTargetProperty(anim, new PropertyPath("ItemRect"));
			sb.FillBehavior = FillBehavior.Stop;
			sb.Children.Add(anim);


			sb.Begin(this);

			_prevRect = newRect;
		}

		protected override Size ArrangeOverride(Size arrangeSize)
		{
			_prevRect = GetItemRect();
			SetValue(ItemRectProperty, _prevRect);

			return arrangeSize;
		}

		protected override void OnRender(DrawingContext dc)
		{
			if (_tabControl == null)
			{
				dc.DrawText(new FormattedText("This panel can only be present inside the ControlTemplate for a TabControl", CultureInfo.InvariantCulture, FlowDirection.LeftToRight, new Typeface("Verdana"), 14, Brushes.Red), new Point());
				return;
			}

			Rect itemRect = (Rect)GetValue(ItemRectProperty);
			dc.DrawGeometry(Background, new Pen(BorderBrush, 1), CreateGeometry(itemRect));
		}

		private Geometry CreateGeometry(Rect rect)
		{
			StreamGeometry geom = new StreamGeometry();
			Rect containerRect = new Rect(RenderSize);
			using (StreamGeometryContext context = geom.Open())
			{
				context.BeginFigure(new Point(rect.Left, 0), true, true);
				context.LineTo(containerRect.BottomLeft, true, true);
				context.LineTo(containerRect.BottomRight, true, true);
				context.LineTo(new Point(rect.Right, 0), true, true);
			}

			return geom;
		}

		private Rect GetItemRect()
		{
			TabItem item = _tabControl.ItemContainerGenerator.ContainerFromItem(_tabControl.SelectedItem) as TabItem;
			Rect itemRect = new Rect(item.TranslatePoint(new Point(), _tabControl),
									 new Size(item.ActualWidth, item.ActualHeight));

			return itemRect;
		}
	}
}