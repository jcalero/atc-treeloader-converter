namespace atc_treeloader_converter.ViewModels
{
	public class WorkspaceViewModel : NotificationViewModel
	{
		#region Members

		private TechNodeViewModel _selectedNode;

		public TechNodeViewModel SelectedNode
		{
			get { return _selectedNode; }
			set
			{
				if (value == _selectedNode) return;
				_selectedNode = value;
				OnPropertyChanged();
			}
		}

		#endregion Members
	}
}
