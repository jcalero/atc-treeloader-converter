using System;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using atc_treeloader_converter.ViewModels;

namespace atc_treeloader_converter.Controls
{
	/// <summary>
	/// Interaction logic for TechTreeDiagram.xaml
	/// </summary>
	public partial class TechTreeDiagram
	{
		#region Members

		private Vector _totalDelta = new Vector(0d, 0d);

		private int _nodeStickiness = 25;

		//TODO: Move this to ViewModel (Workspace?)
		public int NodeStickiness
		{
			get { return _nodeStickiness; }
			set { _nodeStickiness = value; }
		}

		#endregion Members

		#region Constructors

		public TechTreeDiagram()
		{
			InitializeComponent();
		}

		#endregion Constructors

		#region Methods

		private void TechNode_OnDragDelta(object sender, DragDeltaEventArgs e)
		{
			var nodeThumb = sender as Thumb;
			if (nodeThumb == null) return;
			var techNode = nodeThumb.DataContext as TechNodeViewModel;
			if (techNode == null) return;

			_totalDelta.X += e.HorizontalChange;
			_totalDelta.Y += e.VerticalChange;

			if (_totalDelta.Length < NodeStickiness) return;

			Mouse.OverrideCursor = Cursors.Hand;

			var newPos = new Point(
				Math.Round(techNode.Pos.X + e.HorizontalChange, 2),
				Math.Round(techNode.Pos.Y - e.VerticalChange, 2));
			techNode.Pos = newPos;
		}

		private void Thumb_OnDragCompleted(object sender, DragCompletedEventArgs e)
		{
			_totalDelta = new Vector(0d, 0d);
			Mouse.OverrideCursor = null;
		}

		private void Thumb_OnDragStarted(object sender, DragStartedEventArgs e)
		{
			var techtree = DataContext as TechTreeViewModel;
			if (techtree == null) return;
			var thumb = sender as Thumb;
			if (thumb == null) return;
			var node = thumb.DataContext as TechNodeViewModel;
			if (node == null) return;

			if (Keyboard.IsKeyDown(Key.LeftShift))
			{
				techtree.DeleteNode(node);
				return;
			}

			node.IsSelected = true;

			techtree.WorkspaceViewModel.SelectedNode = node;
		}

		#endregion Methods
	}
}
