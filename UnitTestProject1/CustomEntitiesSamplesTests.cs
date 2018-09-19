using Act.Framework;
using Act.Framework.ComponentModel;
using Act.Framework.Contacts;
using Act.Framework.CustomEntities;
using Act.Framework.Database;
using Act.Framework.MutableEntities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;

namespace IBLeier.CustomEntitiesSamples.Tests
{
	[TestClass()]
	public class CustomEntitiesSamplesTests
	{
		#region IbolManagerBase

		[TestMethod()]
		public void DeleteCustomEntityTest()
		{
			using (ActFramework framework = new ActFramework())
			{
				framework.LogOn("TestUser", "test-user", "localhost", "ActTestDb");

				IbolManagerBase ibolManagerBase = new IbolManagerBase(framework);
				ibolManagerBase.DeleteCustomEntity(IbolTests.IbolTableName);
			}
		}

		[TestMethod()]
		public void GetCustomEntityDescriptorTest()
		{
			using (ActFramework framework = new ActFramework())
			{
				framework.LogOn("TestUser", "test-user", "localhost", "ActTestDb");

				IbolManagerBase ibolManagerBase = new IbolManagerBase(framework);
				CustomEntityDescriptor descriptor = ibolManagerBase.GetCustomEntityDescriptor(IbolTests.IbolTableName, (int)ParentEntity.Contacts);
				Assert.IsNotNull(descriptor, "descriptor is null");
			}
		}

		[TestMethod()]
		public void GetFieldTest()
		{
			using (ActFramework framework = new ActFramework())
			{
				framework.LogOn("TestUser", "test-user", "localhost", "ActTestDb");

				IbolManagerBase ibolManagerBase = new IbolManagerBase(framework);
				CustomEntityDescriptor descriptor = ibolManagerBase.GetCustomEntityDescriptor(IbolTests.IbolTableName, (int)ParentEntity.Contacts);
				Assert.IsNotNull(descriptor, "CustomEntityDescriptor is null");

				string alias = "GetFieldTest";
				FieldDataType fieldType = FieldDataType.Character;
				FieldDescriptor des = ibolManagerBase.GetField(descriptor, alias, fieldType);
				Assert.IsNotNull(des, "FieldDescriptor is null");
				Logging.Log("InitializeColumns", des.Name + ", " + des.ColumnName + ", " + des.Alias + ", " + des.TableName);
			}
		}

		#endregion IbolManagerBase

		#region IbolTestsManager

		[TestMethod()]
		public void GetFieldDescriptorTest()
		{
			using (ActFramework framework = new ActFramework())
			{
				Trace.WriteLine("GetFieldDescriptorTest - start");
				framework.LogOn("TestUser", "test-user", "localhost", "ActTestDb");
				Trace.WriteLine("GetFieldDescriptorTest - LogOn");

				IbolTestsManager manager = new IbolTestsManager(framework);
				DBFieldDescriptor descriptor = manager.GetFieldDescriptor("TestId");
				Assert.IsNotNull(descriptor, "CustomEntityDescriptor is null");
				Trace.WriteLine("GetFieldDescriptorTest - GetFieldDescriptor");

				try
				{
					DBFieldDescriptor xyz = manager.GetFieldDescriptor("Xyz");
					Assert.Fail("MutableEntityMetaDataArgumentException expected. " + xyz);
				}
				catch (MutableEntityMetaDataArgumentException ex)
				{
					Trace.WriteLine("GetFieldDescriptorTest: " + ex.ToString());
				}
				Trace.WriteLine("GetFieldDescriptorTest - stop");
			}
		}

		[TestMethod()]
		public void GetCustomSubEntitiesTest()
		{
			using (ActFramework framework = new ActFramework())
			{
				Trace.WriteLine("GetCustomSubEntitiesTest - start");
				framework.LogOn("TestUser", "test-user", "localhost", "ActTestDb");
				Trace.WriteLine("GetCustomSubEntitiesTest - LogOn");

				IbolTestsManager manager = new IbolTestsManager(framework);
				Contact contact = framework.CurrentUser.Contact;

				CustomEntityList<IbolTests> list = manager.GetCustomSubEntities(contact, null);
				Assert.IsNotNull(list, "CustomEntityList<IbolTests> is null");
				Trace.WriteLine("GetCustomSubEntitiesTest - GetCustomSubEntities: " + list.Count + " and stop");
			}
		}

		[TestMethod()]
		public void CreateCustomEntityTest()
		{
			using (ActFramework framework = new ActFramework())
			{
				Trace.WriteLine("CreateCustomEntityTest - start");
				framework.LogOn("TestUser", "test-user", "localhost", "ActTestDb");
				Trace.WriteLine("CreateCustomEntityTest - LogOn");

				IbolTestsManager manager = new IbolTestsManager(framework);
				Contact contact = framework.CurrentUser.Contact;
				ContactList contactAsList = framework.Contacts.GetContactAsContactList(contact);

				IbolTests ibolTest = manager.CreateCustomEntity();
				Assert.IsNotNull(ibolTest, "CreateCustomEntity is null");
				Trace.WriteLine("CreateCustomEntityTest - CreateCustomEntity");

				ibolTest.SetContacts(contactAsList);

				int typeId = 1;
				ibolTest.TypeId = typeId;
				Guid testId = Guid.NewGuid();
				ibolTest.TestId = testId;
				ibolTest.Update();
				Assert.AreEqual(typeId, ibolTest.TypeId, "TypeId");
				Assert.AreEqual(testId, ibolTest.TestId, "TestId");
				Trace.WriteLine("CreateCustomEntityTest - Update and stop");
			}
		}

		#endregion IbolTestsManager
	}
}