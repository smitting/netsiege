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
	[Register ("SiegeWindowController")]
	partial class SiegeWindowController
	{
		[Outlet]
		MonoMac.AppKit.NSButton btnBrowseUrl { get; set; }

		[Outlet]
		MonoMac.AppKit.NSButton btnStart { get; set; }

		[Outlet]
		MonoMac.AppKit.NSTextField txtDelay { get; set; }

		[Outlet]
		MonoMac.AppKit.NSTextField txtDuration { get; set; }

		[Outlet]
		MonoMac.AppKit.NSTextField txtThreads { get; set; }

		[Outlet]
		MonoMac.AppKit.NSTextField txtUrl { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (btnStart != null) {
				btnStart.Dispose ();
				btnStart = null;
			}

			if (btnBrowseUrl != null) {
				btnBrowseUrl.Dispose ();
				btnBrowseUrl = null;
			}

			if (txtUrl != null) {
				txtUrl.Dispose ();
				txtUrl = null;
			}

			if (txtDuration != null) {
				txtDuration.Dispose ();
				txtDuration = null;
			}

			if (txtDelay != null) {
				txtDelay.Dispose ();
				txtDelay = null;
			}

			if (txtThreads != null) {
				txtThreads.Dispose ();
				txtThreads = null;
			}
		}
	}

	[Register ("SiegeWindow")]
	partial class SiegeWindow
	{
		
		void ReleaseDesignerOutlets ()
		{
		}
	}
}
