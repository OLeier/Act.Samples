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
			using (ActFramework framework = new ActFramework())
			{
				framework.LogOn("TestUser", "test-user", "localhost", "ActTestDb");

				GettingaContactList gettingaContactList = new GettingaContactList(framework);
				int count = gettingaContactList.Samples();
				Assert.AreEqual<int>(3, count, "count <> 3");

				//Assert.Fail();
			}
		}
	}
}