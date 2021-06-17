using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace ShaderLibrary
{
	public class DisplacementEffect : ShaderEffect
	{
		private static PixelShader ShaderInstance;

		static DisplacementEffect()
		{
			ShaderInstance = new PixelShader() { UriSource = new Uri(@"pack://application:,,,/Chapters;component/Chapter13/Shaders/Displacement.ps") };
		}

		public DisplacementEffect()
		{
			this.PixelShader = ShaderInstance;

			UpdateShaderValue(InputProperty);
			UpdateShaderValue(Input2Property);
			UpdateShaderValue(ScaleXProperty);
			UpdateShaderValue(ScaleYProperty);
		}

		public Brush Input
		{
			get { return (Brush)GetValue(InputProperty); }
			set { SetValue(InputProperty, value); }
		}

		public Brush Input2
		{
			get { return (Brush)GetValue(Input2Property); }
			set { SetValue(Input2Property, value); }
		}

		public double ScaleX
		{
			get { return (double)GetValue(ScaleXProperty); }
			set { SetValue(ScaleXProperty, value); }
		}

		public double ScaleY
		{
			get { return (double)GetValue(ScaleYProperty); }
			set { SetValue(ScaleYProperty, value); }
		}

		public static readonly DependencyProperty InputProperty =
			ShaderEffect.RegisterPixelShaderSamplerProperty("Input", typeof(DisplacementEffect), 0);

		public static readonly DependencyProperty Input2Property =
			ShaderEffect.RegisterPixelShaderSamplerProperty("Input2", typeof(DisplacementEffect), 1);

		public static readonly DependencyProperty ScaleXProperty =
			DependencyProperty.Register("ScaleX", typeof(double), typeof(DisplacementEffect), new UIPropertyMetadata(1.0D, PixelShaderConstantCallback(0)));

		public static readonly DependencyProperty ScaleYProperty =
			DependencyProperty.Register("ScaleY", typeof(double), typeof(DisplacementEffect), new UIPropertyMetadata(1.0D, PixelShaderConstantCallback(1)));

	}
}
