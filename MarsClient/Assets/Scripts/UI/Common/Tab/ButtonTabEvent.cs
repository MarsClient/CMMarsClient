using UnityEngine;
using System.Collections;
namespace TabButton
{
	public class ButtonTabEvent : MonoBehaviour {

		private object o;
		private UITabButton tab;
		public void SetRefresh (object t, UITabButton tab)
		{
			this.o = (object)t;

			this.tab = tab;
		}
		void OnClick ()
		{
			if (o != null)
			{
				tab.CallButtonEvent (o, gameObject);
			}
		}
	}
}