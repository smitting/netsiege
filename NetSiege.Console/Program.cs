using System;
using System.IO;
using NetSiege.Api;
using NetSiege.Api.Charts;

namespace NetSiege.ConsoleApp
{
	/// <summary>
	/// Entry point for the console application.
	/// </summary>
	class MainClass
	{
		/// <summary>
		/// Runs a siege test using the settings provided from the
		/// command line argument, outputting any final statistics
		/// and PNG files as specified by those settings.
		/// </summary>
		public static void Main (string[] args)
		{
			try 
			{
				// handle command line
				var settings = TestSettings.ParseCmdArguments(args);
				if (settings.ShowVersion)
				{
					Version();
				}
				if (settings.ShowHelp)
				{
					Usage();
				}

				// run test
				var test = new SeigeTest(settings, false);

				test.OnHit += (SeigeTest sender, NetSiege.Api.Data.Hit hit) => {
					Console.WriteLine (hit);
				};
				test.Start();

				test.Wait ();
				OutputResults(test);
			}
			catch (Exception ex)
			{
				Usage (ex.Message);
			}
		}

		/// <summary>
		/// Outputs the test results according to the current TestSettings
		/// options provided via the command line arguments.
		/// </summary>
		private static void OutputResults(SeigeTest test)
		{
			Console.Write (test.Results.Stats);
			ResultChart.CreatePng ("chart.png", 1024, 768, test);
		}
	
		/// <summary>
		/// Outputs current version information to the console.
		/// </summary>
		private static void Version()
		{
			Console.WriteLine ("NetSeige v0.1A\nBuilt 2014-03-05\nCreated by Scott Mitting\nBased on Siege (www.joedog.org)\n");
		}

		/// <summary>
		/// Outputs usage information to the console, optionally including
		/// an error message.
		/// </summary>
		private static void Usage(string error = null)
		{
			if (error != null)
			{
				Console.Write ("Error: {0}\n\n", error);
			}
			Console.WriteLine ("Usage: NetSeige <urlfile> [options]\n");
			Console.WriteLine (TestSettings.ListCmdArguments (1));
		}
	}	
}
