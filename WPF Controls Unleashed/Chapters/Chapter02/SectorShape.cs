using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Chapters.Chapter02
{
	public class SectorShape : Shape
	{
		protected override Geometry DefiningGeometry
		{
			get { return GetSectorGeometry(); }
		}

		private Geometry GetSectorGeometry()
		{
			StreamGeometry geometry = new StreamGeometry();
			using (StreamGeometryContext c = geometry.Open())
			{
				c.BeginFigure(new Point(200, 200), 
								true /* isFilled */, true /* isClosed */);

				// First line
				c.LineTo(new Point(175, 50), true /* isFilled */, true /* isClosed */);

				// Bottom arc
				c.ArcTo(new Point(50, 150), new Size(1, 1), 0, true, 
					SweepDirection.Counterclockwise, true /* isFilled */, true /* isClosed */);

				// Second line
				c.LineTo(new Point(200, 200), 
					true /* isFilled */, true /* isClosed */);
			}

			return geometry;
		}
	}
}