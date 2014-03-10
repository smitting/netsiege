using System;

namespace NetSiege.Api.Data
{
	/// <summary>
	/// Record storing the results of an individual web hit.
	/// </summary>
	public class Hit
	{
		/// <summary>
		/// The time this request was started.
		/// </summary>
		public DateTime StartTime;

		/// <summary>
		/// The url tested.
		/// </summary>
		public string Url;

		/// <summary>
		/// The number of bytes downloaded by this hit.
		/// </summary>
		public int Bytes;

		/// <summary>
		/// How long this request took.
		/// </summary>
		public TimeSpan Time;

		/// <summary>
		/// The HTTP response code for this request.
		/// </summary>
		public string ResponseCode;

		/// <summary>
		/// Returns a string representation of this object to be displayed in the
		/// output of a console application.
		/// </summary>
		public override string ToString ()
		{
			return string.Format ("[Hit Time={0:##0.00}s Url={1}]", Time.TotalSeconds, Url);
		}
	}
}

