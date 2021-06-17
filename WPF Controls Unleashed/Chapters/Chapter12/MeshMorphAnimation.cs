using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Media.Media3D;

namespace Chapters.Chapter12
{
	public enum SurfaceType
	{
		Plane,
		Sphere,
		Cylinder,
		Cone,
	}

	public class MeshMorphAnimation : Point3DCollectionAnimationBase
	{
		#region Dependency Properties

		public static readonly DependencyProperty StartSurfaceProperty = DependencyProperty.Register(
			"StartSurface", typeof (SurfaceType?), typeof (MeshMorphAnimation));

		public static readonly DependencyProperty EndSurfaceProperty = DependencyProperty.Register(
			"EndSurface", typeof (SurfaceType?), typeof (MeshMorphAnimation));

		public static readonly DependencyProperty HorizontalPointsProperty = DependencyProperty.Register(
			"HorizontalPoints", typeof (int), typeof (MeshMorphAnimation));

		public static readonly DependencyProperty VerticalPointsProperty = DependencyProperty.Register(
			"VerticalPoints", typeof (int), typeof (MeshMorphAnimation));

		public SurfaceType? StartSurface
		{
			get { return (SurfaceType?) GetValue(StartSurfaceProperty); }
			set { SetValue(StartSurfaceProperty, value); }
		}

		public SurfaceType? EndSurface
		{
			get { return (SurfaceType?) GetValue(EndSurfaceProperty); }
			set { SetValue(EndSurfaceProperty, value); }
		}

		public int HorizontalPoints
		{
			get { return (int) GetValue(HorizontalPointsProperty); }
			set { SetValue(HorizontalPointsProperty, value); }
		}

		public int VerticalPoints
		{
			get { return (int) GetValue(VerticalPointsProperty); }
			set { SetValue(VerticalPointsProperty, value); }
		}

		#endregion

		private Point3DCollection _startPoints;
		private Point3DCollection _endPoints;
		private bool _collectionsCreated = false;

		protected override Freezable CreateInstanceCore()
		{
			return new MeshMorphAnimation();
		}

		protected override Point3DCollection GetCurrentValueCore(Point3DCollection src,
		                                                         Point3DCollection dest,
		                                                         AnimationClock clock)
		{
			if (!_collectionsCreated)
			{
				_startPoints = StartSurface.HasValue
				               	? MeshCreator.CreatePositions(StartSurface.Value, HorizontalPoints, VerticalPoints)
				               	: src;
				_endPoints = EndSurface.HasValue
				             	? MeshCreator.CreatePositions(EndSurface.Value, HorizontalPoints, VerticalPoints)
				             	: dest;
				_collectionsCreated = true;
				return _startPoints;
			}
			if (clock.CurrentProgress >= 1)
				return _endPoints;

			return Interpolate(clock.CurrentProgress.Value);
		}

		private Point3DCollection Interpolate(double progress)
		{
			Point3DCollection points = new Point3DCollection();
			for (int i = 0; i < _startPoints.Count; i++)
			{
				double x = _startPoints[i].X + (_endPoints[i].X - _startPoints[i].X)*progress;
				double y = _startPoints[i].Y + (_endPoints[i].Y - _startPoints[i].Y)*progress;
				double z = _startPoints[i].Z + (_endPoints[i].Z - _startPoints[i].Z)*progress;

				Point3D p = new Point3D(x, y, z);
				points.Add(p);
			}

			return points;
		}
	}
}