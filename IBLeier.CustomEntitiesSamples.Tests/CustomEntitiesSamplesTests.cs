using Act.Framework;
using Act.Framework.ComponentModel;
using Act.Framework.Contacts;
using Act.Framework.CustomEntities;
using Act.Framework.Database;
using Act.Framework.MutableEntities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

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
				IFilterCriteria[] filterCriteria = null;

				CustomEntityList<IbolTests> list = manager.GetCustomSubEntities(contact, filterCriteria);
				Assert.IsNotNull(list, "CustomEntityList<IbolTests> is null");
				Trace.WriteLine("GetCustomSubEntitiesTest - GetCustomSubEntities: " + list.Count + " and stop");

				CustomEntitiesSamplesTests.SearchTypesFor(typeof(IFilterCriteria));
			}
		}

		/// <summary>
		/// SearchTypesFor
		/// </summary>
		/// <param name="type"><see cref="IFilterCriteria"/></param>
		/// <remarks>
		/// Alle suchen "finds", Groß-/Kleinschreibung beachten, Suchergebnisse: 1, Aktuelles Dokument
		/// (136):Act.Framework.Activities.IActivityFilterCriteria: 1 finds.
		/// (138):Act.Framework.Activities.IActivityInNotInFilterCriteria: 1 finds.
		/// (139):Act.Framework.FilterClause: 1 finds.
		/// Inherits from IFilterCriteria to define a class that can be used to logically combine multiple IFilterCriteria.
		/// (140):Act.Framework.DateFilterCriteria: 1 finds.
		/// Inherits from IFilterCriteria to define a class that can be used to build date clauses for use in defining filters.
		/// (141):Act.Framework.Activities.ActivityInFilterCriteria: 1 finds.
		/// (142):Act.Framework.InFilterCriteria: 1 finds.
		/// Inherits from IFilterCriteria to define a class that can be used to build IN clauses.
		/// (143):Act.Framework.Activities.ActivityComparisonFilterCriteria: 1 finds.
		/// (144):Act.Framework.ComparisonFilterCriteria: 1 finds.
		/// --- ComparisonFilterCriteria.Operation
		/// (145):Act.Framework.Activities.ActivityNotInFilterCriteria: 1 finds.
		/// (146):Act.Framework.NotInFilterCriteria: 1 finds.
		/// Inherits from InFilterCriteria to define a class that can be used to build NOT IN clauses.
		/// (147):Act.Framework.Activities.ActivityDateFilterCriteria: 1 finds.
		/// (148):Act.Framework.Activities.ActivityFilterClause: 1 finds.
		/// (149):Act.Framework.Activities.ActivityStaticTextFilterCriteria: 1 finds.
		/// (150):Act.Framework.BusinessLink.Filters.BusinessLinkStringFilterCriteria: 1 finds.
		/// -
		/// (151):Act.Framework.StaticTextFilterCriteria: 1 finds.
		/// Takes a string to use as a literal filter criteria. No checks for validity are made. 
		/// Übereinstimmende Zeilen: 15
		/// </remarks>
		static private void SearchTypesFor(Type type)
		{
			Trace.TraceInformation("SearchTypesFor " + type + ".");

			AppDomain appDomain = AppDomain.CurrentDomain;
			Assembly[] assemblies = appDomain.GetAssemblies();
			foreach (Assembly assembly in assemblies.OrderBy(a => a.FullName))
			{
				AssemblyName assemblyName = assembly.GetName();
				try
				{
					Type[] types = assembly.GetTypes();
					Trace.WriteLine(assemblyName.Name + ": " + types.Length + " Types.");

					//TypeInfo[] typeInfo = assembly.DefinedTypes.ToArray();

					foreach (Type t in types)
					{
						Type[] finds = t.FindInterfaces(CustomEntitiesSamplesTests.MyInterfaceFilter, type);
						if (finds.Length != 0)
						{
							Trace.WriteLine(t.FullName + ": " + finds.Length + " finds.");
						}
					}
					foreach (Type t in types)
					{
						if (t == type)
						{
							Trace.WriteLine(t.FullName + " found.");
						}
					}
				}
				catch (ReflectionTypeLoadException ex)
				{
					Trace.TraceError("*** ex1: " + assemblyName.Name);
					Trace.TraceError("*** ex2: " + ex.ToString());
					if (ex.LoaderExceptions != null && ex.LoaderExceptions.Length != 0)
					{
						foreach (var lex in ex.LoaderExceptions)
						{
							Trace.TraceError("***lex: " + lex.Message);
							if (lex is FileNotFoundException fex)
							{
								Trace.TraceError("***fex1: " + fex.FileName);
								Trace.TraceError("***fex2: " + fex.FusionLog);
							}
						}
					}
				}
			}

			Trace.TraceInformation("SearchTypesFor.");
		}

		private static bool MyInterfaceFilter(Type typeObj, Object criteriaObj)
		{
			return (typeObj == (Type)criteriaObj);

			//if (typeObj.ToString() == criteriaObj.ToString())
			//	return true;
			//else
			//	return false;
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