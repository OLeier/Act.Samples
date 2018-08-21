using Act.Framework;
using Act.Framework.ComponentModel;
using Act.Framework.Contacts;
using Act.Shared.ComponentModel;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Act.Samples
{
	[
	   CustomControlAttribute(true), // mark as a custom control
	   LayoutToolboxItemFriendlyName(), // give the control a name
	]
	internal class ContactNameControl : Label, IContactListBoundControl // we implement this interface only for being a contact bound control
	{
		private ContactListComponent dataSource;
		private CurrencyManager listManager;

		public override string Text
		{
			get
			{
				// if we're in the designer, let's show some something static, otherwise,
				// we should be showing the real text which should be the contact name
				return DesignMode ? "[Contact Name]" : base.Text;
			}
			set
			{
				;// we don't want to allow programmatic setting of this, since we want to display the Contact's name
			}
		}

		/// <summary>
		/// ContactListComponent
		/// </summary>
		[
			Category("Data"), // this property will show up in the Data section of the property grid
			TypeConverter(typeof(ContactListComponentReferenceConverter)), // converter for this property
			System.ComponentModel.DefaultValue(null) // no code gen/ serialization if null
		]
		public ContactListComponent ContactListComponent
		{
			get { return dataSource; }
			set
			{
				// detach from last event && listen for the OnConnected, which is when we're bound to contacts
				if (dataSource != null)
					dataSource.Connected -= new ContactListComponent.ConnectedHandler(OnConnected);

				dataSource = value;
				if (dataSource != null) dataSource.Connected += new ContactListComponent.ConnectedHandler(OnConnected);
			}
		}

		private void OnConnected()
		{
			// we're bound to contacts 
			GetListManager();

			if (this.ContactListComponent != null && this.ContactListComponent.FrameworkComponent != null
					&& this.ContactListComponent.FrameworkComponent.Framework != null)
			{
				ActFramework framework = ContactListComponent.FrameworkComponent.Framework;
			}
		}

		private void GetListManager()
		{
			// we're interested in position & item changes
			// we use standard .NET data binding to retrieve to be notified
			if (dataSource != null && BindingContext != null && base.DesignMode == false)
			{
				try
				{
					ContactList cl = dataSource.ContactList;
					if (cl != null)
					{
						if (listManager != null)
						{
							listManager.PositionChanged -= new EventHandler(PositionChanged);
							listManager.ItemChanged -= new ItemChangedEventHandler(ItemChanged);
						}
						listManager = BindingContext[cl] as CurrencyManager;
						listManager.PositionChanged += new EventHandler(PositionChanged);
						listManager.ItemChanged += new ItemChangedEventHandler(ItemChanged);
						ContactChanged(); // force a change since we're now bound
					}
				}
				catch (Exception ex)
				{
					Console.Write(ex.ToString());
				}
			}
		}


		private void PositionChanged(object sender, EventArgs e)
		{
			ContactChanged();
		}

		private void ItemChanged(object sender, ItemChangedEventArgs e)
		{
			ContactChanged();
		}

		private void ContactChanged()
		{
			// this will get called when we have switched to another Contact, or the Contact is refreshed
			this.Text = string.Empty;

			try
			{
				if (listManager.Count > 0 && listManager.Position > -1)
				{
					Contact currentContact = listManager.Current as Contact;
					if (currentContact != null)
					{
						base.Text = currentContact.FullName;
					}
				}
			}
			catch
			{
			}

		}

		// used in conjunction with the LayoutToolboxItemFriendlyName to provide a name for 
		// this control in the Layout Designer
		public static string LayoutToolboxFriendlyName
		{

			get
			{
				return "Contact Label";
			}
		}


	}
}
