using Act.Framework;
using Act.UI;
using System;
using System.Windows.Forms;

namespace Act.Samples
{
	internal class CustomMenuPlugin : IPlugin
	{

		private const string CONNECTED_MENUBAR = "Connected Menus";
		private const string DISCONNECTED_MENUBAR = "Disconnected Menus";

		private const string CUSTOM_MENU_URN = "act-ui://com.act/application/menu/tools/custom";
		private const string CUSTOM_MENU_TEXT = "Custom";

		private ActApplication application;

		void IPlugin.OnLoad(ActApplication application)
		{
			// hold on to the application
			this.application = application;

			// be sure to keep track of when a user logs in or is logged out
			application.AfterLogon += new EventHandler(Application_AfterLogon);
			application.BeforeLogoff += new EventHandler(Application_BeforeLogoff);

		}

		void IPlugin.OnUnLoad()
		{

			// this is equivalent to being disposed,
			// so make sure to detach from any events
			if (application != null)
			{
				application.AfterLogon -= new EventHandler(Application_AfterLogon);
				application.BeforeLogoff -= new EventHandler(Application_BeforeLogoff);
				ActFramework framework = application.ActFramework;
				if (framework != null)
				{
					framework.Database.BeforeDatabaseLock -= new Act.Framework.Database.DatabaseLockHandler(Database_BeforeDatabaseLock);
				}
			}
		}

		private void Application_AfterLogon(object sender, EventArgs e)
		{
			//TODO: attach to any events or perform actions

			// also, keep track of when we're about to be locked out
			application.ActFramework.Database.BeforeDatabaseLock += new Act.Framework.Database.DatabaseLockHandler(Database_BeforeDatabaseLock);



			// add custom menu
			AddMenuItem(CUSTOM_MENU_URN, CUSTOM_MENU_TEXT, new Act.UI.CommandHandler(DoStuff));

			this.application.ActFramework.Database.BeforeDatabaseLock += new Act.Framework.Database.DatabaseLockHandler(this.BeforeDatabaseLock);


		}

		private void Application_BeforeLogoff(object sender, EventArgs e)
		{
			// make sure to detatch from anything framework/database/user related, 
			//as we are about to be logged out

			// Remove this so it doesn't become part of the customized menus
			RemoveMenuItem(CUSTOM_MENU_URN);
		}

		private void DoStuff(string command)
		{
			MessageBox.Show("Do something");
		}


		public void BeforeDatabaseLock(object obj, Act.Framework.Database.DatabaseLockInformation LockInfo)
		{

		}

		#region Custom Menu

		private void AddMenuItem(string urn, string text, Act.UI.CommandHandler Handler)
		{
			try
			{
				if (MenuItemExists(urn) == true)
				{
					RemoveMenuItem(urn);
				}
				Act.UI.Core.CommandBarControl parentMenu = this.application.Explorer.CommandBarCollection[CONNECTED_MENUBAR].ControlCollection[GetParentControlURN(urn)];
				Act.UI.Core.CommandBarButton newMenu = new Act.UI.Core.CommandBarButton(text, text, null, urn, null, null);
				newMenu.DisplayStyle = Act.UI.Core.CommandBarControl.ItemDisplayStyle.TextOnly;
				this.application.RegisterCommand(urn, new Act.UI.CommandHandler(Handler), Act.UI.RegisterType.Shell);
				parentMenu.AddSubItem(newMenu);
			}
			catch { }
		}

		private void RemoveMenuItem(string urn)
		{
			try
			{
				this.application.RevokeCommand(urn);
				Act.UI.Core.CommandBarControl RemoveMenu = this.application.Explorer.CommandBarCollection[CONNECTED_MENUBAR].ControlCollection[urn];
				this.application.Explorer.CommandBarCollection[CONNECTED_MENUBAR].ControlCollection[GetParentControlURN(urn)].RemoveSubItem(RemoveMenu);
			}
			catch { }
		}

		private bool MenuItemExists(string urn)
		{
			return (application.Explorer.CommandBarCollection[CONNECTED_MENUBAR].ControlCollection[urn] != null);
		}

		private string GetParentControlURN(string urn)
		{
			return urn.Substring(0, urn.LastIndexOf("/"));
		}

		#endregion

		private void Database_BeforeDatabaseLock(object sender, Act.Framework.Database.DatabaseLockInformation databaseLockInformation)
		{
			//TODO: we're about to be forced out, so perform any cleanup
		}

	}
}
