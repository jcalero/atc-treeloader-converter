using System.ComponentModel;
using System.Runtime.CompilerServices;
using ksp_techtree_edit.Annotations;

namespace atc_treeloader_converter.ViewModels
{
	public class NotificationViewModel : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			var handler = PropertyChanged;
			if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
