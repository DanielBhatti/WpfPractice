using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;

namespace Chapters.Chapter17
{
	public class LinearGradientBrushExtension : MarkupExtension
	{
		private static Regex GradientStopsRegex = new Regex(
					@"(?<GradientStop>\( (?<Offset>\d (\.\d)?) \| (?<Color>\#([0-9a-fA-F]{6} | [0-9a-fA-F]{8}))\))", 
					RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.Singleline | RegexOptions.IgnorePatternWhitespace | RegexOptions.IgnoreCase);
		
		[ConstructorArgument("colors")]
		public string Colors { get; set; }
		public double Angle { get; set; }

		public LinearGradientBrushExtension()
		{
			
		}

		public LinearGradientBrushExtension(string colors)
		{
			Colors = colors;
		}

		public override object ProvideValue(IServiceProvider serviceProvider)
		{
			MatchCollection m = GradientStopsRegex.Matches(Colors);

			GradientStopCollection stops = new GradientStopCollection();
			foreach (Match match in m)
			{
				double offset = double.Parse(match.Groups["Offset"].Value);
				string hexColor = match.Groups["Color"].Value;
				Color color = (Color) ColorConverter.ConvertFromString(hexColor);

				stops.Add(new GradientStop(color, offset));
			}
			return new LinearGradientBrush(stops, Angle);
		}




	}
}