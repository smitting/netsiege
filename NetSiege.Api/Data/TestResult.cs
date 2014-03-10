using System;
using System.Collections.Generic;

namespace NetSiege.Api.Data
{
	/// <summary>
	/// An object containing storage for all data associated with one 
	/// test set and subclasses providing the definitions for that
	/// data.
	/// </summary>
	public class TestResult
	{
		#region Properties

		/// <summary>
		/// The individual hit log.
		/// </summary>
		public readonly List<Hit> Hits = new List<Hit> ();

		/// <summary>
		/// The overall statistics about the test.
		/// </summary>
		public readonly Statistics Stats = new Statistics();

		#endregion

		#region Methods

		/// <summary>
		/// Adds one test result to the log and statistics in a 
		/// thread safe manner.
		/// </summary>
		public void AddHit(Hit hit)
		{
			lock (Stats)
			{
				Stats.Update (hit);
			}
			lock (Hits)
			{
				Hits.Add (hit);
			}
		}

		#endregion
	}
}

