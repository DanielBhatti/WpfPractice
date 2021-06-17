using System;
using System.Windows;

namespace Chapters.Chapter14
{
	public class DataChangedEventManager : WeakEventManager
	{
		private static DataChangedEventManager _mgr;
		private DataChangedEventManager() {}

		public static void AddListener(DataSource source, IWeakEventListener listener)
		{
			CurrentManager.ProtectedAddListener(source, listener);
		}

		public static void RemoveListener(DataSource source, IWeakEventListener listener)
		{
			CurrentManager.ProtectedRemoveListener(source, listener);
		}
	
		protected override void StartListening(object source)
		{
			DataSource evtSource = source as DataSource;
			evtSource.DataChanged += PrivateOnDataChanged;
		}

		protected override void StopListening(object source)
		{
			DataSource evtSource = source as DataSource;
			evtSource.DataChanged -= PrivateOnDataChanged;
		}

		private void PrivateOnDataChanged(object sender, EventArgs e)
		{
			DeliverEvent(sender, e);
		}

		private static DataChangedEventManager CurrentManager
		{
			get
			{
				Type mgrType = typeof (DataChangedEventManager);
				_mgr = GetCurrentManager(mgrType) as DataChangedEventManager;
				if (_mgr == null)
				{
					_mgr = new DataChangedEventManager();
					SetCurrentManager(mgrType, _mgr);
				}

				return _mgr;
			}
		}
	}
}