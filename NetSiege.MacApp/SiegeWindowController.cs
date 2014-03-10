using System;
using System.Collections.Generic;
using System.Linq;
using MonoMac.Foundation;
using MonoMac.AppKit;
using NetSiege.Api;
using System.IO;

namespace NetSiege.MacApp
{
	public partial class SiegeWindowController : MonoMac.AppKit.NSWindowController
	{

		#region Constructors

		// Called when created from unmanaged code
		public SiegeWindowController (IntPtr handle) : base (handle)
		{
			Initialize ();
		}
		// Called when created directly from a XIB file
		[Export ("initWithCoder:")]
		public SiegeWindowController (NSCoder coder) : base (coder)
		{
			Initialize ();
		}
		// Call to load from the XIB/NIB file
		public SiegeWindowController () : base ("SiegeWindow")
		{
			Initialize ();
		}
		// Shared initialization code
		void Initialize ()
		{
		}

		public override void AwakeFromNib ()
		{
			base.AwakeFromNib ();

			btnBrowseUrl.Activated += (object sender, EventArgs e) => {

				var openPanel = new NSOpenPanel();
				openPanel.ReleasedWhenClosed = true;
				openPanel.Prompt = "Select URL file";

				var result = openPanel.RunModal();
				if (result == 1) {

					txtUrl.StringValue = openPanel.Url.AbsoluteString;

				}

			};

			btnStart.Activated += (object sender, EventArgs e) => {

				// TODO: check input arguments
				/*
				var alert = new NSAlert {
					MessageText = "Testing... start siege here",
					AlertStyle = NSAlertStyle.Informational
				};
				alert.AddButton("OK");
				alert.AddButton("Cancel");
				alert.RunModal();
*/


				// build test
				var settings = new TestSettings();
				settings.MaxPause = txtDelay.DoubleValue;
				settings.ThreadCount = txtThreads.IntValue;
				settings.TestSeconds = txtDuration.DoubleValue;

				var path = txtUrl.StringValue;
				path = path.Replace("file://localhost", "");
				settings.Urls.AddRange(File.ReadAllLines(path));

				var test = new SeigeTest(settings, false);


				// launch window
				var testWindow = new SiegeTestWindowController();
				testWindow.ActiveTest = test;
				testWindow.Window.MakeKeyAndOrderFront(this);

				// close this window
				this.Close();

			};
		}

		#endregion

		//strongly typed window accessor
		public new SiegeWindow Window
		{
			get
			{
				return (SiegeWindow)base.Window;
			}
		}
	}
}

