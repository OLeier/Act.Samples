using Act.Framework;
using Act.Framework.Associations;
using Act.Framework.Contacts;
using Act.Framework.Groups;

namespace Act.CodeSamples
{
	public class Associations
	{
		/// <summary>
		/// Retrieves an association manager given two existing database entities.
		/// </summary>
		/// <remarks>
		/// The entities passed in in this case do not necessarily need to be the entities you wish
		/// to relate. Instead they are used to determine entity types for future associations
		/// supported by the returned AssociationManager.
		/// </remarks>
		/// <remarks>
		/// Retrieving an associations manager
		/// The associations manager is retrieved using the the GetAssociationManager method of the AssociationsManager.This method supports 5 overloads so the SDK consumer may retrieve the needed manager in several ways. Examples of two different methods are provided below.
		/// </remarks>
		private AssociationManager GetAssociationManager(Contact contact, Group group)
		{
			ActFramework ACTFM = new ActFramework();
			ACTFM.LogOn("C:\\Documents and Settings\\Administrator\\My Documents\\ACT\\Act for Win 8\\Databases\\Act8Demo.pad", "Chris Huffman", "");
			AssociationManager manager = ACTFM.Associations.GetAssociationManager(contact, group);
			return manager;
		}

		/// <summary>
		/// Retrieves an association manager given the name of two entity types. IE "Contact" or "Group"
		/// </summary>
		private AssociationManager GetAssociationManager(string entityOne, string entityTwo)
		{
			ActFramework ACTFM = new ActFramework();
			ACTFM.LogOn("C:\\Documents and Settings\\Administrator\\My Documents\\ACT\\Act for Win 8\\Databases\\Act8Demo.pad", "Chris Huffman", "");
			AssociationManager manager = ACTFM.Associations.GetAssociationManager(entityOne, entityTwo);
			return manager;
		}

		/// <summary>
		/// Creates a new association between two contacts
		/// </summary>
		/// <remarks>
		/// Creating an association between two entities
		/// This sample creates a new association between two contacts.
		/// The sample shows the following tasks:
		/// - Retrieve the appropriate AssocaitionManager for a Contact to Contact association
		/// - Two methods of setting detailed information for the Association
		/// - Persisting the association to the database
		/// </remarks>
		private Association CreateAssociation(Contact contactOne, Contact contactTwo)
		{
			//Log into the framework and retrieve the correct AssociationManager
			ActFramework ACTFM = new ActFramework();
			ACTFM.LogOn("C:\\Documents and Settings\\Administrator\\My Documents\\ACT\\Act for Win 8\\Databases\\Act8Demo.pad", "Chris Huffman", "");

			Framework.MutableEntities.MutableEntity company = null;
			Framework.MetaData.Entity opportunity = null;
			AssociationManager associationManager = ACTFM.Associations.GetAssociationManager(company, opportunity);

			//Create the assocaition
			Association association = associationManager.CreateAssociation(contactOne, contactTwo);

			//First means of setting association information
			association.Fields[Framework.Associations.StandardField.Details] = "Association Details";

			//Second meand of setting association information
			AssociationFieldDescriptor field = associationManager.GetFieldDescriptor(Framework.Associations.StandardField.Entity1Role);
			field.SetValue(association, "First entities role in this assocaition");

			//Persist the association to the database.
			association.Update();

			return association;
		}

		/// <summary>
		/// Retrieves an array of Contact-Contact associations for the given contact
		/// and updates the association information.
		/// </summary>
		/// <remarks>
		/// Retrieving a list of associations
		/// This sample demonstrates how to retrieve and update a list of associated entities given a specific entity.
		/// </remarks>
		private void UpdateAssociatons(Contact contact, string newRole)
		{
			//Log into the framework and retrive the correct AssociationManager
			ActFramework ACTFM = new ActFramework();
			ACTFM.LogOn("C:\\Documents and Settings\\Administrator\\My Documents\\ACT\\Act for Win 8\\Databases\\Act8Demo.pad", "Chris Huffman", "");
			AssociationManager associationManager = ACTFM.Associations.GetAssociationManager("Group", "Opportunity");
			//Retrieve the associations for the passed in contact.
			Association[] associations = associationManager.GetAssociations(contact);
			foreach (Association association in associations)
			{
				//Update the associaton.
				association.Fields[Framework.Associations.StandardField.Entity1Role] = newRole;
				association.Update();
			}
		}

		/// <summary>
		/// Retrieves an entity list of contacts associated with a given contact
		/// and updates the associaton information.
		/// </summary>
		/// <remarks>
		/// Retrieving a list of associated entities(EntityList) and update association details
		/// This sample demonstrates the use of an AssociationFieldDescriptor to interact with other entities in the Act! framework.Currently supported entities are Opportunities, Companies, Groups and Contacts.
		/// </remarks>
		private void UpdateAssociatons2(Contact contact, string newRole)
		{
			//Log into the framework and retrieve the correct AssociationManager
			ActFramework ACTFM = new ActFramework();
			ACTFM.LogOn("C:\\Documents and Settings\\Administrator\\My Documents\\ACT\\Act for Win 8\\Databases\\Act8Demo.pad", "Chris Huffman", "");
			AssociationManager associationManager = ACTFM.Associations.GetAssociationManager("Group", "Opportunity");
			//Retrieve the entity list.
			Act.Framework.Entities.EntityList entityList = associationManager.GetAssociatedEntities(null, contact);
			//Retrieve the field descriptor
			AssociationFieldDescriptor roleField = associationManager.GetFieldDescriptor(Framework.Associations.StandardField.Entity1Role);
			//Enumerate each entity and update the role details.
			foreach (Act.Framework.Entities.Entity entity in entityList)
			{
				roleField.SetValue(entity, newRole);
				((IUpdateableEntity)entity).Update();
			}
		}
	}
}
