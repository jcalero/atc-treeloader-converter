using atc_treeloader_converter.ViewModels;

namespace atc_treeloader_converter.Controls
{
	/// <summary>
	/// Interaction logic for SideBar.xaml
	/// </summary>
	public partial class SideBar
	{
		public SideBar()
		{
			InitializeComponent();
			var techTreeViewModel = DataContext as TechTreeViewModel;

			if (techTreeViewModel == null) return;

			NodePartsListBox.DataContext = techTreeViewModel;

			PartsListBox.AddPartButton.DataContext = techTreeViewModel;
		}
	}
}
