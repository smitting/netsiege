using System;
using System.Collections.Generic;
using System.Threading;
using System.Net;
using System.IO;
using NetSiege.Api.Data;

namespace NetSiege.Api
{
	/// <summary>
	/// Signature of events receiving any new hit results.
	/// </summary>
	public delegate void HitEventHandler(SeigeTest sender, Hit hit);

	/// <summary>
	/// Object in charge of conducting a test by launching and maintaining
	/// a set of threads to do the test and maintain the results.
	/// </summary>
	public class SeigeTest
	{
		#region Constructors 

		/// <summary>
		/// Constructor requires the settings to use for this test, and by
		/// default immediately starts the test.
		/// </summary>
		public SeigeTest(TestSettings settings, bool autoStart = true)
		{
			this.Settings = settings;
			if (autoStart)
			{
				Start ();
			}
		}

		#endregion

		#region Properties

		/// <summary>
		/// The settings being used for this test.
		/// </summary>
		public readonly TestSettings Settings;

		/// <summary>
		/// The results of this test.
		/// </summary>
		public readonly TestResult Results = new TestResult();

		/// <summary>
		/// The time the test started.
		/// </summary>
		public DateTime StartTime { get; private set; }

		/// <summary>
		/// The time the test will be finished.
		/// </summary>
		public DateTime StopTime { get; private set;}

		/// <summary>
		/// True iff the test has been started.  Each object can only
		/// run a test lifecycle one tim.
		/// </summary>
		public bool Started { get; private set; }

		/// <summary>
		/// Event triggered when hit data is received.
		/// </summary>
		public event HitEventHandler OnHit;

		#endregion

		#region Methods

		/// <summary>
		/// Creates the threads and starts the test.
		/// </summary>
		public void Start()
		{
			lock (this)
			{
				// check object state
				if (Started)
				{
					throw new Exception ("SeigeTest.Start() called on a test that was already started.");
				}

				// create the threads
				for (var i = 0; i < Settings.ThreadCount; i++)
				{
					TestThreads.Add (new Thread (ThreadMain));
				}

				// set the time range for the test
				StartTime = DateTime.Now;
				StopTime = DateTime.Now.AddSeconds (Settings.TestSeconds);

				// get the threads rolling
				foreach (var t in TestThreads)
				{
					t.Start ();
				}
				Started = true;
			}
		}

		/// <summary>
		/// Politely tells the threads to not run anymore tests.  The Wait()
		/// method can be used to detect when they have all finished.
		/// </summary>
		public void Stop() 
		{
			StopTime = DateTime.Now;
		}

		/// <summary>
		/// Immediately aborts all threads.
		/// </summary>
		public void Kill()
		{
			foreach (var t in TestThreads)
			{
				t.Abort ();
			}
		}

		/// <summary>
		/// Waits for all test threads to finish, with an optional timeout.
		/// </summary>
		public bool Wait(int millisecondsTimeout = 0)
		{
			DateTime end = DateTime.Now.AddMilliseconds (millisecondsTimeout);
			foreach (var t in TestThreads)
			{
				if (millisecondsTimeout > 0)
				{
					t.Join (end - DateTime.Now);
					if (DateTime.Now >= end)
					{
						return false;
					}
				} 
				else
				{
					t.Join ();
				}
			}
			return true;
		}

		#endregion

		#region Private Properties

		/// <summary>
		/// The threads running the test.
		/// </summary>
		private readonly List<Thread> TestThreads = new List<Thread>();

		/// <summary>
		/// Shared random number generator.
		/// </summary>
		private readonly Random rnd = new Random();

		#endregion

		#region Private Methods

		/// <summary>
		/// The logic run by each test thread.
		/// </summary>
		private void ThreadMain()
		{
			while (DateTime.Now < StopTime)
			{
				RandomPause ();
				var url = GetRandomUrl ();
				var hit = TestRequest (url);
				if (hit != null)
				{
					Results.AddHit (hit);
					if (OnHit != null)
					{
						OnHit (this, hit);
					}
				}
			}
		}

		/// <summary>
		/// Pauses the calling thread a random amount of time within the
		/// range set by the TestSettings.
		/// </summary>
		private void RandomPause ()
		{
			var sec = Settings.MinPause + (Settings.MaxPause - Settings.MinPause) * rnd.NextDouble ();
			Thread.Sleep ((int)(sec*1000));
		}

		/// <summary>
		/// Returns one url at random from those available in TestSettings.
		/// </summary>
		private string GetRandomUrl()
		{
			var i = rnd.Next (Settings.Urls.Count);
			return Settings.Urls [i];
		}

		/// <summary>
		/// Does a test request against a URL returning a hit result record.
		/// </summary>
		private static Hit TestRequest(string url)
		{
			try
			{
				var start = DateTime.Now;

				string data;
				var request = (HttpWebRequest)WebRequest.Create (url);
				var response = (HttpWebResponse)request.GetResponse ();
				using (var s = response.GetResponseStream ())
				{
					using (var sr = new StreamReader (s))
					{
						data = sr.ReadToEnd ();
					}
				}

				var end = DateTime.Now;

				var result = new Hit ();
				result.Url = url;
				result.Bytes = data.Length;
				result.ResponseCode = response.StatusCode.ToString ();
				result.Time = (end - start);
				result.StartTime = start;
				response.Close ();

				return result;
			} catch
			{
				// TODO: better error handling.
				return null;
			}
		}

		#endregion
	}
}

