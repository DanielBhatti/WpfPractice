using System;
using System.Windows;
using System.Windows.Media;

namespace Chapters.Chapter13
{
	public class SqueezeTransform : GeneralTransform, ICloneable
	{
		public static readonly DependencyProperty LeftProperty = DependencyProperty.Register("Left", typeof(double),
												typeof(SqueezeTransform), new UIPropertyMetadata(0.0));
		public static readonly DependencyProperty RightProperty = DependencyProperty.Register("Right", typeof(double),
												typeof(SqueezeTransform), new UIPropertyMetadata(0.0));



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

		private bool IsInverse { get; set; }
		protected override Freezable CreateInstanceCore()
		{
			return new SqueezeTransform();
		}

		public override bool TryTransform(Point inPoint, out Point result)
		{
			result = new Point();
			if (IsInverse)
			{
				if (inPoint.X < Left || inPoint.X > Right)
					return false; // Transform does not exist


				double ratio = (inPoint.X - Left)/(Right - Left);
				result.X = inPoint.X* ratio;
				result.Y = inPoint.Y;
			}
			else
			{
				double ratio = inPoint.X;
				result.X = Left + (Right-Left) * ratio;
				result.Y = inPoint.Y;
			}

			return true;
 		}

		public override Rect TransformBounds(Rect rect)
		{
			throw new System.NotImplementedException();
		}

		public override GeneralTransform Inverse
		{
			get
			{
				SqueezeTransform transform = this.Clone() as SqueezeTransform;
				transform.IsInverse = !IsInverse;

				return transform;
			}
		}

		protected override void CloneCore(Freezable sourceFreezable)
		{
			SqueezeTransform transform = sourceFreezable as SqueezeTransform;
			base.CloneCore(transform);
			transform.CopyProperties(this);
		}

		public object Clone()
		{
			SqueezeTransform transform = new SqueezeTransform();
			transform.CopyProperties(this);

			return transform;
		}

		private void CopyProperties(SqueezeTransform transform)
		{
			IsInverse = transform.IsInverse;
			Left = transform.Left;
			Right = transform.Right;
		}
	}
}