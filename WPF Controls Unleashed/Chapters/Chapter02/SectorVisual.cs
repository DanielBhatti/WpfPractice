using System.Windows;
using System.Windows.Media;

namespace Chapters.Chapter02
{
	public class SectorVisual : DrawingVisual
	{
		public SectorVisual()
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

			// Draw the geometry
            using (DrawingContext context = RenderOpen())
            {
                Pen pen = new Pen(Brushes.Black, 1);
                context.DrawGeometry(Brushes.CornflowerBlue, pen, geometry);
            }
		}
	}
}