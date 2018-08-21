using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Threading;

namespace IBLeier.BindingExamples
{
	internal static class Logging
	{
		const string LoggingFile = @"D:\Logs\BindingExamples_{0:yyyy-MM-dd}.txt";

		static object lockObject = new object();
		static string oldPathLog;

		public static void Log(string methode, string nachricht)
		{
			lock (Logging.lockObject)
			{
				DateTime now = DateTime.Now;
				string info = string.Format(CultureInfo.InvariantCulture, "{0:s},{1:fff};{2};{3};{4}\r\n",
					now, now, Thread.CurrentThread.ManagedThreadId, methode, nachricht);

				Console.Write(info);
				Trace.Write(info);

				string fn = string.Format(CultureInfo.InvariantCulture, Logging.LoggingFile, now);
				if (Logging.oldPathLog != fn)
				{
					Logging.oldPathLog = fn;
					File.AppendAllText(fn, "DateTime;ManagedThreadId;Methode;Nachricht\r\n");
				}

				//Trace.WriteLine(fn);
				File.AppendAllText(fn, info);
			}
		}
	}
}
