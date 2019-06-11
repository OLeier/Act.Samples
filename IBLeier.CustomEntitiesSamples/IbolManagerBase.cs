using Act.Framework;
using Act.Framework.CustomEntities;
using Act.Framework.Database;
using System;
using System.Collections.Generic;

namespace IBLeier.CustomEntitiesSamples
{
	public class IbolManagerBase
	{
		/// <summary>
		/// IbolManagerBase - constructor
		/// </summary>
		/// <param name="framework"></param>
		[CLSCompliant(false)]
		public IbolManagerBase(ActFramework framework)
		{
			this.ActFramework = framework ?? throw new ArgumentNullException("framework");
		}
		[CLSCompliant(false)]
		protected ActFramework ActFramework { get; private set; }
		[CLSCompliant(false)]
		protected CustomEntityDescriptor Descriptor { get; set; }

		[CLSCompliant(false)]
		protected void InitializeColumns(Dictionary<string, FieldDataType> dataTypes)
		{
			if (dataTypes == null)
			{
				throw new ArgumentNullException("dataTypes");
			}

			foreach (var item in dataTypes)
			{
				FieldDescriptor des = this.GetField(this.Descriptor, item.Key, item.Value);
				Logging.Log("InitializeColumns", des.Name + ", " + des.ColumnName + ", " + des.Alias + ", " + des.TableName);
			}
		}

		[CLSCompliant(false)]
		public CustomEntityDescriptor GetCustomEntityDescriptor(string entityName, int parentEntities)
		{
			CustomEntityDescriptor descriptor = this.ActFramework.CustomEntities.GetCustomEntityDescriptor(entityName);
			if (descriptor != null)
			{
				Logging.Log("GetCustomEntityDescriptor", descriptor.Name + ", " + descriptor.DisplayName + ", " + descriptor.ParentEntity);
				return descriptor;
			}

			this.ActFramework.Database.LockDatabase(DatabaseLockReason.SchemaChanges);
			try
			{
				if (parentEntities <= 0)
				{
					descriptor = this.ActFramework.CustomEntities.CreateCustomEntity(entityName, entityName, false, "Automatische Wiedervorlage und Aktionen (Matrix)");
				}
				else
				{
					descriptor = this.ActFramework.CustomEntities.CreateCustomSubEntity(entityName, entityName, (ParentEntity)parentEntities, false, "Automatische Wiedervorlage und Aktionen (Matrix)");
				}
			}
			finally
			{
				this.ActFramework.Database.UnlockDatabase();
			}
			Logging.Log("GetCustomEntityDescriptor", descriptor.Name + ", " + descriptor.DisplayName + descriptor.ParentEntity);
			return descriptor;
		}

		[CLSCompliant(false)]
		public void DeleteCustomEntity(string entityName)
		{
			CustomEntityDescriptor descriptor = this.ActFramework.CustomEntities.GetCustomEntityDescriptor(entityName);
			if (descriptor == null)
			{
				Logging.Log("DeleteCustomEntity", entityName);
				return;
			}

			this.ActFramework.Database.LockDatabase(DatabaseLockReason.SchemaChanges);
			try
			{
				this.ActFramework.CustomEntities.DeleteCustomEntity(descriptor);
			}
			finally
			{
				this.ActFramework.Database.UnlockDatabase();
			}
			Logging.Log("DeleteCustomEntity", entityName);
			return;
		}

		[CLSCompliant(false)]
		public FieldDescriptor GetField(CustomEntityDescriptor descriptor, string alias, FieldDataType fieldType)
		{
			string name = "IBOL_" + alias;

			FieldDescriptorCollection fieldDescriptors = this.ActFramework.Fields.GetFields(descriptor);
			//foreach (FieldDescriptor des in fieldDescriptors)
			//{
			//	Logging.Log("GetFieldDescriptor", des.Name + ", " + des.ColumnName + ", " + des.Alias + ", " + des.EntityName + ", " + des.TableName);
			//}

			FieldDescriptor field = fieldDescriptors.Find(name);
			if (field != null)
			{
				return field;
			}

			field = new FieldDescriptor(name, alias, descriptor, fieldType); // name will be changed, the alias not

			this.ActFramework.Database.LockDatabase(DatabaseLockReason.SchemaChanges);
			try
			{
				this.ActFramework.Fields.Save(field);
			}
			finally
			{
				this.ActFramework.Database.UnlockDatabase();
			}
			return field;
		}
	}
}
