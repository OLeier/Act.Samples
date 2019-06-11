using Microsoft.VisualStudio.TestTools.UnitTesting;
using Msdn.BindingExamples;
using System.Windows.Forms;

namespace IBLeier.BindingExamples.Tests
{
	[TestClass()]
	public class SearcherTests
	{
		[TestMethod()]
		public void SearchControlTest()
		{
			using (Form1 form = new Form1())
			{
				Control[] controls;
				string name;

				controls = Searcher.SearchControl(form, "customers.custName");
				Assert.IsNotNull(controls, "control is null");
				Assert.AreEqual(1, controls.Length, "control <> 1");
				name = controls[0].Name;
				Assert.AreEqual("text1", name, "name <> text1");

				controls = Searcher.SearchControl(form, "customers.custToOrders.OrderAmount");
				Assert.IsNotNull(controls, "control is null");
				Assert.AreEqual(1, controls.Length, "control <> 1");
				name = controls[0].Name;
				Assert.AreEqual("text3", name, "name <> text3");
			}
		}
	}
}