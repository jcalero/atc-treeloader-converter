using System.Windows;
using atc_treeloader_converter.ViewModels;

namespace atc_treeloader_converter.Controls
{
	/// <summary>
	/// Interaction logic for PartCatalog.xaml
	/// </summary>
	public partial class PartCatalog
	{
		public PartCatalog()
		{
			InitializeComponent();
		}

		private void AddPartClick(object sender, RoutedEventArgs e)
		{
			var techTreeViewModel = AddPartButton.DataContext as
			                        TechTreeViewModel;
			if (techTreeViewModel == null) return;

			var selectedNode =
				techTreeViewModel.WorkspaceViewModel.SelectedNode;
			if (selectedNode == null) return;

			var part = PartsList.SelectedItem as PartViewModel;
			if (part == null) return;

			selectedNode.AddPart(part);
		}
	}
}
