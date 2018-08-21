using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Forms;

namespace IBLeier.BindingExamples
{
	public static class Searcher
	{
		static public Control[] SearchControl(Control control, string name)
		{
			Logging.Log("SearchControl-Control", control?.Name + ", " + control?.Text);
			if (control == null)
			{
				return null;
			}

			List<Control> list = new List<Control>();
			foreach (object obj in control.DataBindings)
			{
				Binding b = (Binding)obj;
				Logging.Log("SearchControl-db", b.BindingMemberInfo.BindingPath + ", " + b.BindingMemberInfo.BindingMember + ", " + b.BindingMemberInfo.BindingField);
				if (b.BindingMemberInfo.BindingMember == name)
				{
					list.Add(control);
				}
			}

			foreach (DictionaryEntry entry in control.BindingContext)
			{
				WeakReference weakReference = (WeakReference)entry.Value;
				Logging.Log("SearchControl-bc", entry.Key.ToString() + ": " + weakReference.Target.GetType().FullName);
				if (weakReference.Target is PropertyManager pm)
				{
					foreach (Binding b in pm.Bindings)
					{
						Logging.Log("SearchControl-pm", b.BindingMemberInfo.BindingPath + ", " + b.BindingMemberInfo.BindingMember + ", " + b.BindingMemberInfo.BindingField);
						if (b.BindingMemberInfo.BindingMember == name)
						{
							//list.Add(control);
						}
					}
				}
				else if (weakReference.Target is CurrencyManager cm)
				{
					foreach (Binding b in cm.Bindings)
					{
						Logging.Log("SearchControl-cm", b.BindingMemberInfo.BindingPath + ", " + b.BindingMemberInfo.BindingMember + ", " + b.BindingMemberInfo.BindingField);
						if (b.BindingMemberInfo.BindingMember == name)
						{
							//list.Add(control);
						}
					}
				}
			}

			foreach (Control c in control.Controls)
			{
				Control[] controls = Searcher.SearchControl(c, name);
				list.AddRange(controls);
			}

			Logging.Log("SearchControl-list", list.Count.ToString(CultureInfo.InvariantCulture));
			return list.ToArray();
		}
	}
}
