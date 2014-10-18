using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using atc_treeloader_converter.ViewModels;
using KerbalParser;
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace atc_treeloader_converter.Views
{
	/// <summary>
	/// Interaction logic for ConverterWindow.xaml
	/// </summary>
	public partial class ConverterWindow
	{
		private KerbalConfig _config;
		private readonly TechTreeViewModel _treeData;
		private string _kspFolder;
		private PartCollectionViewModel _partCollection;

		public bool EnableConvertButton { get; set; }

		public ConverterWindow()
		{
			_treeData = new TechTreeViewModel();
			InitializeComponent();
		}

		public KerbalConfig ParseTree(string path)
		{
			var parser = new Parser();
			return parser.ParseConfig(path);
		}

		private void LoadTree(object sender, RoutedEventArgs e)
		{
			var dlg = new OpenFileDialog
			          {
				          DefaultExt = ".cfg",
				          Filter = "Tech Tree Config Files|*.cfg"
			          };

			var result = dlg.ShowDialog();

			if (result == false) return;

			var nameNodeHashtable = new Dictionary<string, TechNodeViewModel>();

			if (_treeData == null) return;

			_config = ParseTree(dlg.FileName);

			foreach (var tree in
				_config.Where(
				              tree => tree.Name != "REMOVENODE" &&
				                      tree.Values.ContainsKey("name")))
			{
				var v = tree.Values;
				var name = v["name"].First();
				TechNodeViewModel techNodeViewModel;

				if (nameNodeHashtable.ContainsKey(name))
				{
					techNodeViewModel = nameNodeHashtable[name];
				}
				else
				{
					techNodeViewModel = new TechNodeViewModel();
					nameNodeHashtable.Add(name, techNodeViewModel);
				}

				techNodeViewModel.TechNode.PopulateFromSource(tree);

				if (v.ContainsKey("parents"))
				{
					var parentsString = v["parents"].First();
					var parents = parentsString.Split(',');

					foreach (var parent
						in parents.
							Where(
							      parent =>
							      !nameNodeHashtable.ContainsKey(parent)))
					{
						nameNodeHashtable.Add(parent, new TechNodeViewModel());
					}

					foreach (var parent
						in parents
							.Where(
							       parent => !String.IsNullOrEmpty(parent) &&
							                 nameNodeHashtable.
								                 ContainsKey(parent)))
					{
						techNodeViewModel.Parents.Add(nameNodeHashtable[parent]);
					}
				}

				_treeData.TechTree.Add(techNodeViewModel);

				if (_partCollection != null)
				{
					techNodeViewModel.PopulateParts(_partCollection);
				}
			}

			var saver = new ATCSaver();
			var fileInfo = new FileInfo(dlg.FileName);

			_treeData.Save(saver, fileInfo.DirectoryName + "/atc_" + fileInfo.Name);

			ResultLabel.Content = _treeData.TechTree.Count + " nodes saved to atc__" + fileInfo.Name;
		}

		private void LoadKSPFolder(object sender, RoutedEventArgs e)
		{
			var dlg = new CommonOpenFileDialog { Title = "Select your KSP installation folder", IsFolderPicker = true };

			var result = dlg.ShowDialog();

			if (result != CommonFileDialogResult.Ok) return;

			_kspFolder = dlg.FileName;

			FindParts(_kspFolder);
			ConvertButton.IsEnabled = true;
		}

		public void FindParts(string path)
		{
			_partCollection = new PartCollectionViewModel();

			_partCollection.LoadParts(path);
		}

		private void PartsCheckboxChecked(object sender, RoutedEventArgs e)
		{
			if (String.IsNullOrEmpty(_kspFolder))
			{
				ConvertButton.IsEnabled = false;
			}
		}

		private void PartsCheckboxUnchecked(object sender, RoutedEventArgs e)
		{
			ConvertButton.IsEnabled = true;
		}
	}
}
