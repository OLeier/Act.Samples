using Act.Framework;
using Act.Framework.Contacts;
using Act.Framework.MutableEntities;
using System;
using System.ComponentModel;
using System.Diagnostics;

namespace Act.Samples
{
	public class UsingFrameworkMetadata
	{
		public UsingFrameworkMetadata(ActFramework framework)
		{
			this.framework = framework;
		}

		private readonly ActFramework framework;

		/// <summary>
		/// Samples
		/// </summary>
		/// <remarks>
		/// The following is an example of using Framework metadata. In this example: print the field display name of
		/// any contact string fields that can be edited, do not allow empty values, and do not have any default value
		/// (thus, string fields are 'blank' when we create a new contact):
		/// </remarks>
		public int Samples()
		{
			Trace.WriteLine("Samples.Start");

			// filter to just return string fields on a Contact
			ContactFieldDescriptor[] fields =
			this.framework.Contacts.GetContactFieldDescriptors(new Type[] { typeof(string) });
			Trace.WriteLine("Samples.GetContactFieldDescriptors: " + fields.Length);

			// we're going to look for editable fields that don't allow empty values
			// and don't have default values, so we'll need these attribute types
			Type allowsEmptyType = typeof(AllowEmptyFieldAttribute);
			Type defaultValueType = typeof(DefaultFieldValueAttribute);
			// initialize our attributes
			AllowEmptyFieldAttribute allowsEmptyFieldAttr;
			DefaultFieldValueAttribute defaultFieldAttr;
			AttributeCollection attributes;
			ContactFieldDescriptor contactField;
			int count = 0;
			for (int i = 0; i < fields.Length; i++)
			{
				contactField = fields[i];
				Trace.WriteLine("Samples.ContactFieldDescriptor: " + contactField.ColumnName + ", " + contactField.DisplayName + ", " + contactField.Name);

				if (contactField.ColumnName == "CUST_Klassifizierung_121244260")
				{
					Trace.WriteLine("Samples.ContactFieldDescriptor.TableName: " + contactField.TableName);
				}

				// make sure we can modify this field
				if (!contactField.IsReadOnly)
				{
					attributes = contactField.Attributes;
					// check if we don't all empty values
					allowsEmptyFieldAttr = attributes[allowsEmptyType] as AllowEmptyFieldAttribute;
					if (allowsEmptyFieldAttr != null && !allowsEmptyFieldAttr.AllowEmpty)
					{
						// now check to see we don't have a default value
						defaultFieldAttr = attributes[defaultValueType] as DefaultFieldValueAttribute;
						if (defaultFieldAttr == null || defaultFieldAttr.DefaultValue == null)
						{
							// we found one
							Console.WriteLine(contactField.DisplayName);
							//Act!Architecture Reference 12
							count++;
						}
					}
				}
			}
			Trace.WriteLine("Samples.Stop: " + count);
			return count;
		}

	}
}
