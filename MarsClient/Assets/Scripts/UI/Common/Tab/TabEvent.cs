using UnityEngine;
using System.Collections;
namespace TabButton
{
	public class TabEvent : MonoBehaviour {

		private object o;
		private UITabList tab;
		public void SetRefresh (object t, UITabList tab)
		{
			this.o = (object)t;

			this.tab = tab;
		}
		public void OnClick ()
		{
			if (o != null)
			{
				tab.CallButtonEvent (o, gameObject);
			}
		}
	}
}