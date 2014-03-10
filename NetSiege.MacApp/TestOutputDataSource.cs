using System;
using System.Collections.Generic;
using MonoMac.Foundation;
using MonoMac.AppKit;

namespace NetSiege.MacApp
{
	/// <summary>
	/// Provides data to the raw output table for a seige window.
	/// </summary>
	[Register("TestOutputDataSource")]
	public class TestOutputDataSource : NSTableViewDataSource
	{
		#region Constructor

		/// <summary>
		///
		/// </summary>
		public TestOutputDataSource() : base()
		{
		}

		#endregion

		#region Properties

		/// <summary>
		/// The actual data being rendered.
		/// </summary>
		public readonly List<string> Lines = new List<string>();

		#endregion

		#region NSTableViewDataSource Implementation

		public override int GetRowCount (NSTableView tableView)
		{
			return Lines.Count;
		}

		public override NSObject GetObjectValue (NSTableView tableView, NSTableColumn tableColumn, int rowIndex)
		{
			if (rowIndex >= Lines.Count)
				return null;

			var line = Lines[rowIndex];

			switch (tableColumn.Identifier) {
			case "line":
				return new NSString(line);
			}
			return null;
		}

		#endregion
	}
}

