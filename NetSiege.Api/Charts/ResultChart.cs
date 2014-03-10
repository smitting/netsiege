using System;
using System.Drawing;

namespace NetSiege.Api.Charts 
{
	/// <summary>
	/// Methods for building PNG files from test result data.
	/// </summary>
	public class ResultChart
	{
		#region methods

		/// <summary>
		/// Writes a PNG file of the given size where each result is plotted
		/// against a timeline on the y-axis and an x-axis representing the
		/// duration of each result, lower being less time, higher being more time.
		/// </summary>
		public static void CreatePng(string path, int width, int height, SeigeTest test)
		{
			// check test status
			if (test.Started == false)
			{
				throw new ArgumentException ("Cannot plot a test that has not started.");
			}

			// create the image
			var bmp = new Bitmap (width, height);

			// plot each hit
			using (var g = Graphics.FromImage (bmp))
			{
				g.FillRectangle (Brushes.White, 0, 0, width, height);
				var pen = new Pen (Color.Black);
				foreach (var log in test.Results.Hits)
				{
					int x = (int)(PercentOfTimeframe (log.StartTime, test.StartTime, test.StopTime) * (double)width);
					int y = (int)((double)height - PercentOfMax (log.Time.TotalSeconds, test.Results.Stats.Max) * (double)height);
					g.DrawLine (pen, x - 1, y, x + 1, y);
					g.DrawLine (pen, x, y - 1, x, y + 1);
				}
			}

			// write to disk
			bmp.Save(path);
		}

		#endregion

		#region Private

		/// <summary>
		/// Converts a number to a percentage of its maximum.
		/// </summary>
		private static double PercentOfMax(double value, double max)
		{
			return max == 0 ? 0 : value / max;
		}

		/// <summary>
		/// Converts a time to a parcentage within a time frame.
		/// </summary>
		private static double PercentOfTimeframe(DateTime time, DateTime start, DateTime end)
		{
			var span = (end - start).TotalSeconds;
			var elapsed = (time - start).TotalSeconds;
			return span == 0 ? 0 : elapsed / span;
		}

		#endregion
	}
}

