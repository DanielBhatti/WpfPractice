using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace Chapters.Chapter13
{
	public class SqueezeEffect : ShaderEffect
	{
		private static PixelShader ShaderInstance = new PixelShader() 
												{UriSource = new Uri(@"pack://application:,,,/Chapters;component/Chapter13/Shaders/Squeeze.ps")};

		public static readonly DependencyProperty LeftProperty = DependencyProperty.Register("Left", typeof(double),
												typeof(SqueezeEffect), new UIPropertyMetadata(PixelShaderConstantCallback(0)));
		public static readonly DependencyProperty RightProperty = DependencyProperty.Register("Right", typeof(double),
												typeof(SqueezeEffect), new UIPropertyMetadata(PixelShaderConstantCallback(1)));
		public static readonly DependencyProperty InputProperty =
																RegisterPixelShaderSamplerProperty("Input", typeof(SqueezeEffect), 0);



		public double Left
		{
			get { return (double)GetValue(LeftProperty); }
			set { SetValue(LeftProperty, value); }
		}
		public double Right
		{
			get { return (double)GetValue(RightProperty); }
			set { SetValue(RightProperty, value); }
		}
		public Brush Input
		{
			get { return (Brush)GetValue(InputProperty); }
			set { SetValue(InputProperty, value); }
		}

		private SqueezeTransform _transform = new SqueezeTransform();
		protected override GeneralTransform EffectMapping
		{
			get
			{
				_transform.Left = Left;
				_transform.Right = Right;
				return _transform;
			}
		}

		public SqueezeEffect()
		{
			PixelShader = ShaderInstance;
			UpdateShaderValue(InputProperty);
			UpdateShaderValue(LeftProperty);
			UpdateShaderValue(RightProperty);
		}
	}
}