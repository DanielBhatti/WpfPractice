using System.Collections.Generic;
using System.ComponentModel;

namespace Chapters.Chapter16
{
	public class AddressBook : INotifyPropertyChanged
	{
		private List<string> _contactNames;
		public event PropertyChangedEventHandler PropertyChanged;

		public List<string> ContactNames
		{
			get { return _contactNames; }
			set
			{
				_contactNames = value;
				NotifyChange("ContactNames");
			}
		}

		public AddressBook()
		{
			_contactNames = null;
		}

		private void NotifyChange(string name)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(name));
			}
		}
	}
}