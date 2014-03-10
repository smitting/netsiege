using System;
using System.Collections.Generic;
using System.Linq;
using MonoMac.Foundation;
using MonoMac.AppKit;
using NetSiege.Api;

namespace NetSiege.MacApp
{
	public partial class SiegeTestWindowController : MonoMac.AppKit.NSWindowController
	{

		#region Constructors

		// Called when created from unmanaged code
		public SiegeTestWindowController (IntPtr handle) : base (handle)
		{
			Initialize ();
		}
		// Called when created directly from a XIB file
		[Export ("initWithCoder:")]
		public SiegeTestWindowController (NSCoder coder) : base (coder)
		{
			Initialize ();
		}
		// Call to load from the XIB/NIB file
		public SiegeTestWindowController () : base ("SiegeTestWindow")
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

			nsProgress.DoubleValue = 0.3;


			if (ActiveTest == null)
			{
				var error = new NSAlert {
					MessageText = "Bug: must assign ActiveTest before showing window!!!",
					AlertStyle = NSAlertStyle.Critical
				};
				error.RunModal ();
				return;
			}

			Data = new TestOutputDataSource ();
			tableView.DataSource = Data;


			// start the test, sending results to this window
			ActiveTest.OnHit += (SeigeTest sender, NetSiege.Api.Data.Hit hit) => {
				Data.Lines.Add(hit.ToString());
			};

			ActiveTest.Start ();


			// kill the test when the window closes
			this.Window.WillClose += (object sender, EventArgs e) => {			
				if (ActiveTest != null) {
					ActiveTest.Kill();
					ActiveTest = null;
				}
			};

			timer = NSTimer.CreateRepeatingScheduledTimer (1, delegate
			{
					tableView.ReloadData();
			});


		}

		#endregion

		#region Properties

		private NSTimer timer = null;

		/// <summary>
		/// The test being run.
		/// </summary>
		public SeigeTest ActiveTest = null;

		/// <summary>
		/// Results display data source.
		/// </summary>
		public TestOutputDataSource Data = null;

		#endregion

		//strongly typed window accessor
		public new SiegeTestWindow Window
		{
			get
			{
				return (SiegeTestWindow)base.Window;
			}
		}
	}
}

