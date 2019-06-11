using Act.Framework;
using Act.Samples.Tests.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace Act.Samples.Tests
{
	[TestClass()]
	public class GettingaContactListTests
	{
		[TestMethod()]
		public void SamplesTest()
		{
			Trace.WriteLine("GettingaContactListTests.SamplesTest-Start");
			using (ActFramework framework = new ActFramework())
			{
				framework.LogOn(Settings.Default.userName, Settings.Default.password, Settings.Default.databaseHost, Settings.Default.databaseName);

				GettingaContactList gettingaContactList = new GettingaContactList(framework);
				int count = gettingaContactList.Samples();
				Assert.AreEqual<int>(4, count, "count <> 4");

				//Assert.Fail();
			}
			Trace.WriteLine("GettingaContactListTests.SamplesTest-Stop");
		}
	}
}