netsiege
========

Based on SEIGE, but written in c#, measures the response time for downloading a set of URLs from multiple threads for both load testing and performance measurements.

I created NetSiege because I needed to be able to have a timestamp for the start of each request to measure the effects of App Pool Recycling on a new feature, and I also wanted to be able to create PNG charts with graphs of my test results automatically without having to copy the data into Excel.

Most of this software was written within an hour and then cleaned up later.  This was coded using Xamarin Studio on OS X, but should be binary compatible with Windows, Linux, and OS X.




projects
========

| Project | Description |
| ------- | ----------- |
| NetSiege.Api | this is the .NET library containing the core functionality |
| NetSiege.Console | this is a console application mimicing many command line arguments from SEIGE. |
| NetSiege.MacApp | this is an incomplete MonoMac GUI application for OSX. |




