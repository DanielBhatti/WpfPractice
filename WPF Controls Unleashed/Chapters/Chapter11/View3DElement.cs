using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace Chapters.Chapter11
{
	public class View3DElement : FrameworkElement
	{
		private Viewport3D _viewport;
		private Model3DGroup _modelGroup;

		public View3DElement()
		{
			CreateViewport();
		}

		private void CreateViewport()
		{
			_viewport = new Viewport3D();
			_viewport = ResourceManager.Get<Viewport3D>("3DViewport");
			_modelGroup = LocateModelGroup();

			GeometryModel3D model = ResourceManager.Get<GeometryModel3D>("PlaneModel");
			_modelGroup.Children.Add(model);
		}

		private Model3DGroup LocateModelGroup()
		{
			Model3DGroup group =
				((_viewport.Children[0] as ModelVisual3D).Content as Model3DGroup).Children[1] as Model3DGroup;
			return group;
		}

		#region Layout Overrides

		protected override Size MeasureOverride(Size availableSize)
		{
			if (availableSize.Width == double.PositiveInfinity || availableSize.Height == double.PositiveInfinity)
				return Size.Empty;

			_viewport.Measure(availableSize);
			return _viewport.DesiredSize;
		}

		protected override Size ArrangeOverride(Size finalSize)
		{
			_viewport.Arrange(new Rect(finalSize));
			return finalSize;
		}

		protected override Visual GetVisualChild(int index)
		{
			if (index == 0) return _viewport;
			else throw new Exception("Bad Index");
		}

		protected override int VisualChildrenCount
		{
			get { return 1; }
		}

		#endregion
	}
}