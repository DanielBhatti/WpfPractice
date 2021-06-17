using System.Windows;
using System.Windows.Controls;

namespace Chapters.Chapter10
{
	public class SkinThemeControl : ContentControl
	{
		static SkinThemeControl()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(SkinThemeControl), new FrameworkPropertyMetadata(typeof(SkinThemeControl)));
		}
	}
}