using System;
using System.Windows.Media.Animation;
using System.Windows.Media.Media3D;

namespace Chapters.Chapter12
{
	public abstract class Point3DCollectionAnimationBase : AnimationTimeline
	{
		public override sealed Type TargetPropertyType
		{
			get { return typeof (Point3DCollection); }
		}

		public override sealed object GetCurrentValue(object defaultOriginValue,
		                                              object defaultDestinationValue,
		                                              AnimationClock animationClock)
		{
			if (defaultOriginValue == null)
			{
				throw new ArgumentNullException("defaultOriginValue");
			}
			if (defaultDestinationValue == null)
			{
				throw new ArgumentNullException("defaultDestinationValue");
			}
			return GetCurrentValue((Point3DCollection) defaultOriginValue,
			                       (Point3DCollection) defaultDestinationValue,
			                       animationClock);
		}

		public Point3DCollection GetCurrentValue(Point3DCollection defaultOriginValue,
		                                         Point3DCollection defaultDestinationValue,
		                                         AnimationClock animationClock)
		{
			if (animationClock == null)
			{
				throw new ArgumentException("animationClock");
			}

			return GetCurrentValueCore(defaultOriginValue, defaultDestinationValue, animationClock);
		}

		protected abstract Point3DCollection GetCurrentValueCore(Point3DCollection defaultOriginValue,
		                                                         Point3DCollection defaultDestinationValue,
		                                                         AnimationClock animationClock);
	}
}