using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using atc_treeloader_converter.ViewModels;
using KerbalParser;

namespace atc_treeloader_converter.Models
{
	public class PartCollection : IList<Part>
	{
		#region Members

		public DirectoryInfo Directory { get; set; }

		private readonly IList<Part> _parts = new List<Part>();

		#endregion Members

		#region Constructors

		public PartCollection()
		{
		}

		public PartCollection(string path)
		{
			Directory = new DirectoryInfo(path);
		}

		public PartCollection(DirectoryInfo directory)
		{
			Directory = directory;
		}

		#endregion Constructors

		#region Methods

		public event ProgressFileSearch Progress;

		protected virtual void OnProgress()
		{
			var handler = Progress;
			if (handler != null) handler(this, EventArgs.Empty);
		}

		public List<KerbalNode> LoadParts()
		{
			var partlist = new List<KerbalNode>();
			var filter = new List<string> { "PART" };
			var parser = new Parser(filter, false, true);
			foreach (var file in Directory.GetFiles("*.cfg", SearchOption.AllDirectories))
			{
				OnProgress();
				var parts = parser.ParseConfig(file.FullName);
				if (parts == null || parts.Count < 1) continue;
				foreach (var p in parts.Where(
				                              p => !partlist.Contains(p)
				                                   && p.Name == "PART"))
				{
					if (!p.Values.ContainsKey("name")) continue;

					var part = new Part(p.Values["name"].First());
					part.PopulateFromSource(p);
					part.FileName = file.FullName;
					_parts.Add(part);
				}
			}
			return partlist;
		}

		#endregion Methods

		#region Interface Members

		public IEnumerator<Part> GetEnumerator()
		{
			return _parts.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable)_parts).GetEnumerator();
		}

		public void Add(Part item)
		{
			_parts.Add(item);
		}

		public void Clear()
		{
			_parts.Clear();
		}

		public bool Contains(Part item)
		{
			return _parts.Contains(item);
		}

		public void CopyTo(Part[] array, int arrayIndex)
		{
			_parts.CopyTo(array, arrayIndex);
		}

		public bool Remove(Part item)
		{
			return _parts.Remove(item);
		}

		public int Count
		{
			get { return _parts.Count; }
		}

		public bool IsReadOnly
		{
			get { return _parts.IsReadOnly; }
		}

		public int IndexOf(Part item)
		{
			return _parts.IndexOf(item);
		}

		public void Insert(int index, Part item)
		{
			_parts.Insert(index, item);
		}

		public void RemoveAt(int index)
		{
			_parts.RemoveAt(index);
		}

		public Part this[int index]
		{
			get { return _parts[index]; }
			set { _parts[index] = value; }
		}

		#endregion Interface Members
	}
}
