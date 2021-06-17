using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace Chapters.Chapter13
{
	public class GrayscaleEffect : ShaderEffect
	{
		private static PixelShader ShaderInstance = new PixelShader() { UriSource = new Uri(@"pack://application:,,,/Chapters;component/Chapter13/Shaders/Grayscale.ps") };

		public static readonly DependencyProperty InputProperty =
			ShaderEffect.RegisterPixelShaderSamplerProperty("Input", typeof(GrayscaleEffect), 0);

		public Brush Input
		{
			get { return (Brush)GetValue(InputProperty); }
			set { SetValue(InputProperty, value); }
		}

		public GrayscaleEffect()
		{
			PixelShader = ShaderInstance;
			UpdateShaderValue(InputProperty);
		}
	}
}