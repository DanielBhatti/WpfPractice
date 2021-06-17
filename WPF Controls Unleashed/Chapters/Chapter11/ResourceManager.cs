using System;
using System.Windows;

namespace Chapters.Chapter11
{
	public static class ResourceManager
	{
		private static ResourceDictionary InternalResources { get; set; }

		static ResourceManager()
		{
			InternalResources =
				Application.LoadComponent(new Uri("/Chapters;component/Chapter11/InternalResources.xaml", UriKind.Relative))
				as ResourceDictionary;
		}

		public static T Get<T>(string name) where T : class
		{
			return InternalResources[name] as T;
		}
	}
}