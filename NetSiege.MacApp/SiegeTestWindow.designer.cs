// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoMac.Foundation;
using System.CodeDom.Compiler;

namespace NetSiege.MacApp
{
	[Register ("SiegeTestWindowController")]
	partial class SiegeTestWindowController
	{
		[Outlet]
		MonoMac.AppKit.NSImageCell imgResult { get; set; }

		[Outlet]
		MonoMac.AppKit.NSProgressIndicator nsProgress { get; set; }

		[Outlet]
		MonoMac.AppKit.NSTableView tableView { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (nsProgress != null) {
				nsProgress.Dispose ();
				nsProgress = null;
			}

			if (tableView != null) {
				tableView.Dispose ();
				tableView = null;
			}

			if (imgResult != null) {
				imgResult.Dispose ();
				imgResult = null;
			}
		}
	}

	[Register ("SiegeTestWindow")]
	partial class SiegeTestWindow
	{
		
		void ReleaseDesignerOutlets ()
		{
		}
	}
}
