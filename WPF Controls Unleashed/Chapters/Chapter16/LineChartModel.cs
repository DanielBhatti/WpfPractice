using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace Chapters.Chapter16
{
	public class LineChartModel
	{
		public List<Point> DataPoints { get; set; }

		public LineChartModel()
		{
			DataPoints = new List<Point>();

			double variation = 0.1;
			double currentValue = 0.5;
			int totalPoints = 100;
			Random random = new Random();
			for (int i = 0; i < totalPoints; i++)
			{
				double y = currentValue + (-variation + 2 * variation * random.NextDouble());
				if (y <= 0.0) y = variation;
				else if (y >= 1.0) y = 1.0 - variation;

				Point p = new Point((double)i / (totalPoints - 1), y);
				DataPoints.Add(p);

				currentValue = y;
				
			}
		}

	}
}