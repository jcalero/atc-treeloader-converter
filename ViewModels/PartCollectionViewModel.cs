using System;
using System.Collections.ObjectModel;
using atc_treeloader_converter.Models;

namespace atc_treeloader_converter.ViewModels
{
	public delegate void ProgressFileSearch(object sender, EventArgs e);

	public class PartCollectionViewModel : NotificationViewModel
	{
		private WorkspaceViewModel _workspaceViewModel;

		#region Members

		public ObservableCollection<PartViewModel>
			PartCollection { get; private set; }

		public WorkspaceViewModel WorkspaceViewModel
		{
			get { return _workspaceViewModel; }
			set
			{
				if (_workspaceViewModel == value) return;
				_workspaceViewModel = value;
				OnPropertyChanged();
			}
		}

		#endregion Members

		#region Construtctors

		public PartCollectionViewModel()
		{
			PartCollection = new ObservableCollection<PartViewModel>();
		}

		#endregion Construtctors

		#region Methods

		public void LoadParts(string path)
		{
			var partCollection = new PartCollection(path);
			partCollection.LoadParts();

			foreach (var part in partCollection)
			{
				var partViewModel = new PartViewModel(part);
				PartCollection.Add(partViewModel);
			}
		}

		#endregion Methods
	}
}
