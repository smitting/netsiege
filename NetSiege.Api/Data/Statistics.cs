using System;

namespace NetSiege.Api.Data
{
	/// <summary>
	/// Record storing the summary statistics for the entire test.
	/// </summary>
	public class Statistics
	{
		#region Properties

		/// <summary>
		/// The number of test hits conducted.
		/// </summary>
		public int Requests;

		/// <summary>
		/// The total time in seconds for all requests.
		/// </summary>
		public double Total;

		/// <summary>
		/// The minimum request time in seconds.
		/// </summary>
		public double Min;

		/// <summary>
		/// The maximum request time in seconds.
		/// </summary>
		public double Max;

		/// <summary>
		/// The avarage request time in seconds.
		/// </summary>
		public double Average 
		{
			get { return Requests == 0 ? 0 : Total / Requests; }
		}

		#endregion

		#region Methods

		/// <summary>
		/// Updates these statistics to include the results from
		/// one hit.
		/// </summary>
		public void Update(Hit result)
		{
			var s = result.Time.TotalSeconds;
			if (Requests == 0) {
				Min = s;
				Max = s;
			} else {
				Min = Math.Min (Min, s);
				Max = Math.Max (Max, s);
			}
			Requests++;
			Total += s;
		}

		/// <summary>
		/// Returns a string for displaying this stats as text.
		/// </summary>
		public override string ToString ()
		{
			return string.Format ("Stats:\n\tRequests={0}\n\tAverage={1:###0.00} secs\n\tMin:{2:###0.00} secs\n\tMax:{3:###0.00} secs\n\n]", Requests, Average, Min, Max);
		}

		#endregion
	}
}

