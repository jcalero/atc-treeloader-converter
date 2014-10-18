using System.Windows;
using atc_treeloader_converter.ViewModels;

namespace atc_treeloader_converter.Controls
{
	/// <summary>
	/// Interaction logic for NodePartsList.xaml
	/// </summary>
	public partial class NodePartsList
	{
		public NodePartsList()
		{
			InitializeComponent();
		}

		private void RemovePartClick(object sender, RoutedEventArgs e)
		{
			var part = PartsListBox.SelectedItem as PartViewModel;
			if (part == null) return;

			var techTree = DataContext as TechTreeViewModel;
			if (techTree == null) return;

			var node = techTree.WorkspaceViewModel.SelectedNode;

			node.RemovePart(part);
		}
	}
}
