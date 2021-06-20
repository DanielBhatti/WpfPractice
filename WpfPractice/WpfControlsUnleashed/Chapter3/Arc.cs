using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace WpfPractice.WpfControlsUnleashed
{
	public class Arc : Shape
	{
		protected override Geometry DefiningGeometry
		{
			get { return GetArcGeometry(); }
		}

		public double StartAngle
		{
			get { return (double)GetValue(StartAngleProperty); }
			set { SetValue(StartAngleProperty, value); }
		}

		public double EndAngle
		{
			get { return (double)GetValue(EndAngleProperty); }
			set { SetValue(EndAngleProperty, value); }
		}

		public static readonly DependencyProperty StartAngleProperty = DependencyProperty.Register(
			nameof(StartAngle), typeof(double), typeof(Arc),
			new FrameworkPropertyMetadata(0D, FrameworkPropertyMetadataOptions.AffectsRender));

		public static readonly DependencyProperty EndAngleProperty = DependencyProperty.Register(
			nameof(EndAngle), typeof(double), typeof(Arc),
			new FrameworkPropertyMetadata(0D, FrameworkPropertyMetadataOptions.AffectsRender));


		private Geometry GetArcGeometry()
		{
			Point startPoint = PointAtAngle(Math.Min(StartAngle, EndAngle));
			Point endPoint = PointAtAngle(Math.Max(StartAngle, EndAngle));

			Size arcSize = new Size(Math.Max(0, (RenderSize.Width - StrokeThickness) / 2),
									Math.Max(0, (RenderSize.Height - StrokeThickness) / 2));
			bool isLargeArc = Math.Abs(EndAngle - StartAngle) > 180;

			StreamGeometry geom = new StreamGeometry();
			using (StreamGeometryContext context = geom.Open())
			{
				context.BeginFigure(startPoint, false, false);
				context.ArcTo(endPoint, arcSize, 0, isLargeArc, SweepDirection.Counterclockwise, true, false);
			}

			geom.Transform = new TranslateTransform(StrokeThickness / 2, StrokeThickness / 2);
			return geom;
		}

		private Point PointAtAngle(double angle)
		{
			double radAngle = angle * (Math.PI / 180);
			double xRadius = (RenderSize.Width - StrokeThickness) / 2;
			double yRadius = (RenderSize.Height - StrokeThickness) / 2;

			double x = xRadius + xRadius * Math.Cos(radAngle);
			double y = yRadius - yRadius * Math.Sin(radAngle);

			return new Point(x, y);
		}
	}
}
