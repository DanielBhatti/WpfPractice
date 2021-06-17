using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Media3D;

namespace Chapters.Chapter11
{
	public class GardenViewPanel3D : Panel
	{
		private Viewport3D _viewport;
		private ContainerUIElement3D _modelContainer;
		private Random _random = new Random();

		public static readonly DependencyProperty ElementWidthProperty = DependencyProperty.Register(
			"ElementWidth", typeof(double), typeof(GardenViewPanel3D));

		public static readonly DependencyProperty ElementHeightProperty = DependencyProperty.Register(
			"ElementHeight", typeof(double), typeof(GardenViewPanel3D));

		private static readonly DependencyProperty LinkedModelProperty = DependencyProperty.Register(
			"LinkedModel", typeof(ModelUIElement3D), typeof(GardenViewPanel3D));

		public double ElementWidth
		{
			get { return (double)GetValue(ElementWidthProperty); }
			set { SetValue(ElementWidthProperty, value); }
		}

		public double ElementHeight
		{
			get { return (double)GetValue(ElementHeightProperty); }
			set { SetValue(ElementHeightProperty, value); }
		}


		public GardenViewPanel3D()
		{
			CreateViewport();
		}

		protected override void OnInitialized(EventArgs e)
		{
			AddVisualChild(_viewport);
		}

		private void CreateViewport()
		{
			_viewport = ResourceManager.Get<Viewport3D>("3DViewport_Interactive");
			_modelContainer = LocateModelContainer();
		}

		private ContainerUIElement3D LocateModelContainer()
		{
			ContainerUIElement3D container = _viewport.Children[0] as ContainerUIElement3D;
			return container;
		}

		#region Layout Overrides

		protected override Size MeasureOverride(Size availableSize)
		{
			if (availableSize.Width == double.PositiveInfinity || availableSize.Height == double.PositiveInfinity)
				return Size.Empty;

			_viewport.Measure(availableSize);

			foreach (UIElement child in Children)
			{
				child.Measure(new Size(ElementWidth, ElementHeight));
			}
			return availableSize;
		}

		protected override Size ArrangeOverride(Size finalSize)
		{
			_viewport.Arrange(new Rect(finalSize));

			foreach (UIElement child in Children)
			{
				child.Arrange(new Rect(new Size(ElementWidth, ElementHeight)));
			}
			return finalSize;
		}

		protected override Visual GetVisualChild(int index)
		{
			if (index == 0) return _viewport;
			else throw new Exception("Bad Index");
		}

		protected override int VisualChildrenCount
		{
			get
			{
				int count = base.VisualChildrenCount == 0 ? 0 : 1;
				return count;
			}
		}

		protected override void OnRender(DrawingContext dc)
		{
			dc.DrawRectangle(Brushes.Transparent, null, new Rect(RenderSize));
		}

		protected override void OnVisualChildrenChanged(DependencyObject visualAdded,
																										DependencyObject visualRemoved)
		{
			base.OnVisualChildrenChanged(visualAdded, visualRemoved);

			if (visualAdded != null && visualAdded != _viewport)
			{
				ModelUIElement3D model = CreateModel(visualAdded);
				visualAdded.SetValue(LinkedModelProperty, model);
				_modelContainer.Children.Add(model);
			}
			if (visualRemoved != null && visualRemoved != _viewport)
			{
				ModelUIElement3D model = visualRemoved.GetValue(LinkedModelProperty) as ModelUIElement3D;
				model.Model = null;
				visualRemoved.ClearValue(LinkedModelProperty);
				_modelContainer.Children.Remove(model);
			}
		}

		private ModelUIElement3D CreateModel(DependencyObject visualAdded)
		{
			int index = Math.Max(0, InternalChildren.Count - 1);
			ModelUIElement3D model = new ModelUIElement3D();
			model.Transform = new TranslateTransform3D();

			// Prepare the GeometryModel
			GeometryModel3D geomModel = ResourceManager.Get<GeometryModel3D>("PlaneModel").Clone();
			(geomModel.Material as DiffuseMaterial).Brush = (new VisualBrush(visualAdded as Visual));
			double zPos = -1 * index;
			double xPos = -index / 2 + _random.NextDouble() * index;

			TranslateTransform3D trans = geomModel.Transform as TranslateTransform3D;
			trans.OffsetX = xPos;
			trans.OffsetZ = zPos;

			model.Model = geomModel;
			return model;
		}

		#endregion
	}
}