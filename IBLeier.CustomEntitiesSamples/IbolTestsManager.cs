using Act.Framework;
using Act.Framework.ComponentModel;
using Act.Framework.Contacts;
using Act.Framework.CustomEntities;
using Act.Framework.Database;
using Act.Framework.MutableEntities;
using System;
using System.Collections.Generic;

namespace IBLeier.CustomEntitiesSamples
{
	[CLSCompliant(false)]
	public class IbolTestsManager : IbolManagerBase
	{
		public IbolTestsManager(ActFramework framework) :
			base(framework)
		{
			this.Descriptor = this.GetCustomEntityDescriptor(IbolTests.IbolTableName, (int)ParentEntity.Contacts);
			this.manager = this.ActFramework.CustomEntities.GetSubEntityManager<IbolTests>(this.Descriptor);
			this.InitializeColumns(this.dataTypes);
		}
		private readonly CustomSubEntityManager<IbolTests> manager;

		private readonly Dictionary<string, FieldDataType> dataTypes = new Dictionary<string, FieldDataType>()
		{
			{ "TypeId", FieldDataType.Number},		// System.Int32,  CUST_IBOL_Tests.CUST_IBOL_TypeId_011818499
			{ "TestId", FieldDataType.Character}	// System.String, CUST_IBOL_Tests.CUST_IBOL_TestId_011833559
		};

		public DBFieldDescriptor GetFieldDescriptor(string name)
		{
			//DBFieldDescriptor[] dBFieldDescriptors = this.manager.GetFieldDescriptors();
			//foreach (DBFieldDescriptor des in dBFieldDescriptors)
			//{
			//	Logging.Log("GetFieldDescriptor", des.Name + ", " + des.ColumnName + ", " + des.DisplayName + ", " + des.TableName);
			//}

			return this.manager.GetCustomEntityFieldDescriptor(name, FieldNameType.Alias);
		}

		public CustomEntityList<IbolTests> GetCustomSubEntities(Contact contact, IFilterCriteria[] filterCriteria)
		{
			CustomEntityList<IbolTests> list = this.manager.GetCustomSubEntities(contact, null, filterCriteria);
			return list;
		}

		public IbolTests CreateCustomEntity()
		{
			IbolTests result = this.manager.CreateCustomEntity();
			return result;
		}
	}
}
