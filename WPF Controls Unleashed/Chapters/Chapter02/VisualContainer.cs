using System.Windows;
using System.Windows.Media;

namespace Chapters.Chapter02
{
	public class VisualContainer : UIElement
	{
		private SectorVisual _visual = new SectorVisual();

		protected override Visual GetVisualChild(int index)
		{
			return _visual;
		}

		protected override int VisualChildrenCount
		{
			get { return 1; }
		}
	}
}