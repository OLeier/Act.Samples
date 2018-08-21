using Act.Framework;
using Act.Framework.ComponentModel;
using Act.Framework.Contacts;
using Act.Shared.Collections;
using System.ComponentModel;

namespace Act.Samples
{
	public class GettingaContactList
	{
		public GettingaContactList(ActFramework framework)
		{
			this.framework = framework;
		}

		private ActFramework framework;

		/// <summary>
		/// Samples
		/// </summary>
		/// <remarks>
		/// The following sample shows how to get a contact list.
		/// </remarks>
		public int Samples()
		{
			// get the company field descriptor
			DBFieldDescriptor companyField =
			framework.Contacts.GetFieldDescriptor("TBL_CONTACT.COMPANYNAME", true);
			// get contacts I have access to, sorted by company
			ContactList contacts = framework.Contacts.GetContacts(
			new SortCriteria[] { new SortCriteria(companyField, ListSortDirection.Ascending) });

			return contacts.Count;
		}

	}
}
