using Act.Framework;
using Act.Samples.Tests.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace Act.Samples.Tests
{
	[TestClass()]
	public class UsingFrameworkMetadataTests
	{
		[TestMethod()]
		public void SamplesTest()
		{
			Trace.WriteLine("UsingFrameworkMetadataTests.SamplesTest.Start");
			Trace.Flush();

			using (ActFramework framework = new ActFramework())
			{
				//framework.LogOn("CHuffman", "password", "localhost", "MyDatabase");
				framework.LogOn(Settings.Default.userName, Settings.Default.password, Settings.Default.databaseHost, Settings.Default.databaseName);

				UsingFrameworkMetadata usingFrameworkMetadata = new UsingFrameworkMetadata(framework);
				int count = usingFrameworkMetadata.Samples();
				Assert.AreEqual<int>(0, count, "count <> 0");
				//Assert.Fail();
			}
			Trace.WriteLine("UsingFrameworkMetadataTests.SamplesTest.Stop");
		}
	}
}