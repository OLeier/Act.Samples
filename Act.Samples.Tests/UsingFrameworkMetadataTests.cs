using Act.Framework;
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
				framework.LogOn("TestUser", "test-user", "localhost", "ActTestDb");

				UsingFrameworkMetadata usingFrameworkMetadata = new UsingFrameworkMetadata(framework);
				int count = usingFrameworkMetadata.Samples();
				Assert.AreEqual<int>(0, count, "count <> 0");
				//Assert.Fail();
			}
			Trace.WriteLine("UsingFrameworkMetadataTests.SamplesTest.Stop");
		}
	}
}