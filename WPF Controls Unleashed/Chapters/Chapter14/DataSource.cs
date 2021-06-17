using System;
using System.Windows;
using System.Windows.Controls;

namespace Chapters.Chapter14
{
	public class DataSource : ContentControl
	{
		public event EventHandler DataChanged;

		public void DoSomethingtoRaiseEvent()
		{
			// Some action

			// Raise event
			if (DataChanged != null)
			{
				DataChanged(this, EventArgs.Empty);
			}
		}
	}
}