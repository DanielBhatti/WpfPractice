using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace Chapters.Chapter16
{
	public class LineSegmentConverter : IMultiValueConverter
	{
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			// Pick the next point if the previous point is null (we are at the first point)
			Point prev = values[0] == null ? (Point)values[1] : (Point)values[0];

			Point curr = (Point)values[1];

			if (values[2] == DependencyProperty.UnsetValue ||
				values[3] == DependencyProperty.UnsetValue)
				return null;

			double maxWidth = (double)values[2];
			double maxHeight = (double)values[3];


			return new LineGeometry(new Point(prev.X * maxWidth, prev.Y * maxHeight),
										new Point(curr.X * maxWidth, curr.Y * maxHeight));
		}

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			throw new System.NotImplementedException();
		}
	}
}