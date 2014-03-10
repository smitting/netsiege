using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NetSiege.Api
{
	/// <summary>
	/// All the configurable settings for one test.
	/// </summary>
	public class TestSettings
	{
		#region Properties

		/// <summary>
		/// The minimum time to wait before issueing a new request
		/// on one thread.  Default = 1 second.
		/// </summary>
		public double MinPause = 1.0;

		/// <summary>
		/// The maximum time to wait before issueing a new request
		/// on one thread.  Default = 10 seconds.
		/// </summary>
		public double MaxPause = 10.0;

		/// <summary>
		/// The number of threads to test with.  Default = 5 threads.
		/// </summary>
		public int ThreadCount = 5;

		/// <summary>
		/// The total duration of the test, in seconds.  Default = 5 minutes.
		/// </summary>
		public double TestSeconds = 60*5;

		/// <summary>
		/// The set of URLs to randomly hit during tests.
		/// </summary>
		public readonly List<string> Urls = new List<string>();

		/// <summary>
		/// True iff the console app should output the usage documentation
		/// </summary>
		public bool ShowHelp = false;

		/// <summary>
		/// True iff the version string should be printed.
		/// </summary>
		public bool ShowVersion = false;

		#endregion

		#region Methods

		/// <summary>
		/// Creates a TestSettings instance from the command line arguments, 
		/// throwing exceptions for any invalid options.
		/// </summary>
		public static TestSettings ParseCmdArguments(string[] args)
		{
			// create settings object with default options
			var settings = new TestSettings ();

			string urlfile = null;
			foreach (var arg in args)
			{
				if (arg.StartsWith ("-"))
				{
					// check all long versions without parameters
					if (arg == "-version")
					{
						settings.ShowVersion = true;
					} else if (arg == "-help")
					{
						settings.ShowHelp = true;
					} else if (arg.StartsWith ("-concurrent="))
					{
						settings.ThreadCount = (int)ParseNumber (arg, "-concurrent=".Length);
					} else if (arg.StartsWith ("-time="))
					{
						settings.TestSeconds = ParseTime (arg, "-time=".Length);
					} else if (arg.StartsWith ("-delay="))
					{
						settings.MaxPause = ParseNumber (arg, "-delay=".Length);
					} else if (arg.StartsWith ("-mindelay="))
					{
						settings.MinPause = ParseNumber (arg, "-mindelay=".Length);
					}
					// check all short versions
					else if (arg == "-V")
					{
						settings.ShowVersion = true;
					} else if (arg == "-h")
					{
						settings.ShowHelp = true;
					} else if (arg.StartsWith ("-c"))
					{
						settings.ThreadCount = (int)ParseNumber (arg, "-c".Length);
					} else if (arg.StartsWith ("-t"))
					{
						settings.TestSeconds = ParseTime (arg, "-t".Length);
					} else if (arg.StartsWith ("-d"))
					{
						settings.MaxPause = ParseNumber (arg, "-d".Length);
					} else if (arg.StartsWith ("-D"))
					{
						settings.MinPause = ParseNumber (arg, "-D".Length);
					} 
					// invalid
					else
					{
						throw new ArgumentException ("Unrecognized option " + arg);
					}
				} 
				else
				{
					if (urlfile != null)
					{
						throw new ArgumentException ("Only one UrlFile may be specified");
					}
					urlfile = arg;
				}
			}

			// load UrlFile
			if (urlfile == null)
			{
				throw new ArgumentException ("UrlFile is required");
			}
			settings.Urls.AddRange (File.ReadAllLines (urlfile));

			return settings;
		}

		/// <summary>
		/// Returns a list of command line arguments handled by ParseCmdArguments()
		/// as a string of individual lines, optionally inserting some tabs before
		/// each line.
		/// </summary>
		/// <param name="tabs">(optional) number of tabs to put at the start of each line</param>
		public static string ListCmdArguments(int tabs = 0)
		{
			var sb = new StringBuilder ();
			AddArg (sb, tabs, "-version", "-V", "Prints version information to the screen");
			AddArg (sb, tabs, "-help", "-h", "Prints program help");
			AddArg (sb, tabs, "-concurrent=NUM", "-c NUM", "Sets the number threads to use for a test (default: 5)");
			AddArg (sb, tabs, "-time=NUMm", "-t NUMm", "Sets the time period for the test, where NUM is a number an m is a unit such as -t3600S, -t60M, or -t1H (default 3M");
			AddArg (sb, tabs, "-delay=NUM", "-d NUM", "The random number of seconds to wait between requests (default: 10)");
			AddArg (sb, tabs, "-mindelay=NUM", "-D NUM", "The minimum number of seconds to wait between requests (default: 1)");
			return sb.ToString ();
		}

		#endregion

		#region Private

		/// <summary>
		/// Parses a time with a unit of S, M, or H.
		/// </summary>
		private static double ParseTime(string arg, int i) {
			string unit = ""+arg [arg.Length - 1];
			string s = arg.Substring (i, arg.Length - (i + 1));
			double d;
			if (double.TryParse (s, out d))
			{
				switch (unit.ToUpper ())
				{
				case "S":
					return d;
				case "M":
					return d * 60;
				case "H":
					return d * 60 * 60;
				default:
					throw new ArgumentException ("Invalid unit in argument " + arg);
				}
			} else
			{
				throw new ArgumentException ("Invalid time argument " + arg);
			}
		}

		/// <summary>
		/// Parses a number.
		/// </summary>
		private static double ParseNumber(string arg, int i) {
			double d;
			if (double.TryParse (arg.Substring (i), out d))
			{
				return d;
			} else
			{
				throw new ArgumentException ("Invalid number argument " + arg);
			}
		}

		/// <summary>
		/// Adds one argument to the StringBuilder building usage information.
		/// </summary>
		private static void AddArg(StringBuilder sb, int tabs, string arg, string altArg, string description)
		{
			for (var i = 0; i < tabs; i++)
			{
				sb.Append ("\t");
			}
			sb.Append (altArg.PadRight(8));
			sb.Append (arg.PadRight (20));
			sb.Append (description);
			sb.AppendLine ();
		}

		#endregion
	}
}

