using Act.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Act.Samples.Tests
{
	[TestClass()]
	public class GettingaContactListTests
	{
		[TestMethod()]
		public void SamplesTest()
		{
			ActFramework framework = new ActFramework();
			framework.LogOn("TestUser", "test-user", "localhost", "ActTestDb");

			GettingaContactList gettingaContactList = new GettingaContactList(framework);
			int count = gettingaContactList.Samples();
			Assert.AreEqual<int>(1, count, "count <> 1");

			//Assert.Fail();
		}
	}
}