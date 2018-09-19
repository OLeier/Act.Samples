using Act.Framework;
using Act.Framework.Contacts;
using Act.Framework.MetaData;
using System.Collections;
using System.ComponentModel;

namespace Act.CodeSamples
{
	public class ContactSamples
	{
		/// <summary>
		/// Retrieves name of the Contact table of given field if there are lot of custom fields and additional Contact tables
		/// </summary>
		/// <param name="dbFieldName">real name of the field</param>
		/// <returns>table name as string if field exist in database; otherwise return empty string</returns>
		/// <remarks>
		/// Getting a table name
		/// This sample retrieves the name of the Contact table for the given real field name.It is useful in cases where the Act! database contains a large number of custom fields and additional Contact tables.
		/// The sample shows the following tasks:
		/// - Using the SDK to get Contact field descriptors.
		/// - Comparing the ColumnName of each contact field descriptor in the list with the parameter.
		/// - When we find the contact field descriptor with the same column name as the one in the parameter, we return its Table name.If not found – we return empty string.
		/// </remarks>
		private string GetTableName(string dbFieldName)
		{
			ActFramework ACTFM = new ActFramework();
			ACTFM.LogOn("C:\\Documents and Settings\\Administrator\\My Documents\\ACT\\Act for Win 8\\Databases\\Act8Demo.pad", "Chris Huffman", "");
			ContactFieldDescriptor[] cfd = ACTFM.Contacts.GetContactFieldDescriptors();
			for (int i = 0; i < cfd.Length; i++)
			{
				if (dbFieldName.Equals(cfd[i].ColumnName))
					return cfd[i].TableName;
			}
			return "";
		}

		/// <summary>
		/// Retrieves names of all contact custom fields from the Act! Database
		/// </summary>
		/// <returns>Array list with names of the custom fields</returns>
		/// <remarks>
		/// Getting custom contact fields
		/// This sample retrieves names of the custom contact fields and puts them into an Array list.
		/// The sample shows the following tasks:
		/// - Using the SDK to get Contact field descriptors.
		/// - Getting the table of each contact field descriptor using the TableByName property of SchemaMetadata.
		/// - If column in the table is custom (IsCustom = true), adding the field name to the Array list.
		/// </remarks>
		public /*override*/ ArrayList GetContactCustomFields()
		{
			ActFramework ACTFM = new ActFramework();
			ACTFM.LogOn("C:\\Documents and Settings\\Administrator\\My Documents\\ACT\\Act for Win 8\\Databases\\Act8Demo.pad", "Chris Huffman", "");
			SchemaMetaData smd = ACTFM.SchemaMetaData;
			ReadOnlyHashtable tblByName = smd.TableByName;
			ArrayList customFieldArray = new ArrayList();
			ContactFieldDescriptor[] cfdList = ACTFM.Contacts.GetContactFieldDescriptors();
			ContactFieldDescriptor cfd;
			Table tContact;
			Column col;
			for (int i = 0; i < cfdList.Length; i++)
			{
				cfd = cfdList[i];
				tContact = (Act.Framework.MetaData.Table)tblByName[cfd.TableName];
				col = (Column)tContact.ColumnByDisplayName[cfd.DisplayName];
				if (col != null)
				{
					if (col.IsCustom)
					{
						customFieldArray.Add(col.Name);
					}
				}
			}
			return customFieldArray;
		}

		/// <summary>
		/// Inserts Secondary contact to MyRecord
		/// </summary>
		/// <remarks>
		/// Inserting a secondary contact
		/// This sample inserts a secondary contact into MyRecord in an Act! database.
		/// The sample shows the following tasks:
		/// - Getting MyRecord
		/// - Using the SDK to retrieve the list of all secondary contacts for MyRecord.
		/// - Using an implementation of IBindingList interface to add a new secondary contact to MyRecord and set properties.
		/// - After adding all properties, updating the secondary contact in the Act! database
		/// </remarks>
		private void InsertSecondaryContact()
		{
			ActFramework ACTFM = new ActFramework();
			ACTFM.LogOn("C:\\Documents and Settings\\Administrator\\My Documents\\ACT\\Act for Win 8\\Databases\\Act8Demo.pad", "Chris Huffman", "");
			Contact actContact = ACTFM.Contacts.GetMyRecord();
			ContactList secondaryContactsList = ACTFM.Contacts.GetSecondaryContacts(null, actContact);
			Contact secondaryContact = (Contact)((IBindingList)secondaryContactsList).AddNew();
			string firstName = "John";
			string lastName = "Smith";
			string title = "Accountant";
			//.
			//.(add values for other properties)
			//.
			secondaryContact.FullName = firstName + " " + lastName;
			secondaryContact.Fields["Contact.Title", false] = title;
			//.
			//.(assign additional properties to secondary contact)
			//.
			secondaryContact.Update();
		}

		/// <summary>
		/// Delete secondary contact from MyRecord
		/// </summary>
		/// <param name="secondaryContactID">ID of the secondary contact which has to be deleted </param>
		/// <remarks>
		/// Deleting a secondary contact
		/// This sample deletes a secondary contact from MyRecord in an Act! database.
		/// The sample shows the following tasks:
		/// - Getting MyRecord.
		/// - Using the SDK to retrieve the list of all secondary contacts for MyRecord.
		/// - Comparing the ID of each secondary contact in the list to the ID of the contact we want to delete.
		/// - When we find the secondary contact with the same ID as the one in the parameter, removing this secondary contact from the list using the implementation of IBindingList interface.
		/// </remarks>
		private void DeleteSecondaryContact(string secondaryContactID)
		{
			ActFramework ACTFM = new ActFramework();
			ACTFM.LogOn("C:\\Documents and Settings\\Administrator\\My Documents\\ACT\\Act for Win 8\\Databases\\Act8Demo.pad", "Chris Huffman", "");
			Contact actContact = ACTFM.Contacts.GetMyRecord();
			ContactList secondaryContactsList = ACTFM.Contacts.GetSecondaryContacts(null, actContact);
			Contact secondaryContact;
			for (int i = 0; i < secondaryContactsList.Count; i++)
			{
				secondaryContact = secondaryContactsList[i];
				if (secondaryContact.ID.ToString().Equals(secondaryContactID))
				{
					((IBindingList)secondaryContactsList).Remove(secondaryContact);
				}
			}
		}
	}
}
