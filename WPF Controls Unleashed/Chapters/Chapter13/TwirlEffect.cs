using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace Chapters.Chapter13
{
	public class TwirlEffect : ShaderEffect
	{
		private static PixelShader ShaderInstance = new PixelShader() 
												{UriSource = new Uri(@"pack://application:,,,/Chapters;component/Chapter13/Shaders/Twirl.ps")};

		public static readonly DependencyProperty RadiusProperty = DependencyProperty.Register("Radius", typeof(double),
												typeof(TwirlEffect), new UIPropertyMetadata(PixelShaderConstantCallback(0)));
		public static readonly DependencyProperty AngleProperty = DependencyProperty.Register("Angle", typeof(double),
												typeof(TwirlEffect), new UIPropertyMetadata(PixelShaderConstantCallback(1)));
		public static readonly DependencyProperty InputProperty =
		ShaderEffect.RegisterPixelShaderSamplerProperty("Input", typeof(TwirlEffect), 0);



		public double Radius
		{
			get { return (double)GetValue(RadiusProperty); }
			set { SetValue(RadiusProperty, value); }
		}
		public double Angle
		{
			get { return (double)GetValue(AngleProperty); }
			set { SetValue(AngleProperty, value); }
		}
		public Brush Input
		{
			get { return (Brush)GetValue(InputProperty); }
			set { SetValue(InputProperty, value); }
		}


		public TwirlEffect()
		{
			PixelShader = ShaderInstance;
			UpdateShaderValue(InputProperty);
			UpdateShaderValue(RadiusProperty);
			UpdateShaderValue(AngleProperty);
		}
	}
}