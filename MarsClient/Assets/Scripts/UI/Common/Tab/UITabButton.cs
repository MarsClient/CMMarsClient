using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace TabButton
{
	public class UITabButton : MonoBehaviour
	{
		
		private UIGrid grid;
		public GameObject buttonPrefab;


		private List<GameObject> btns = new List<GameObject> ();
		public void refresh (List<object> t, ButtonTabInit buttonTabMeesgae)
		{
			if (grid == null) grid = GetComponent<UIGrid> ();
			char start = 'a';
			char then = 'a';
			for (int i = 0; i < t.Count; i++)
			{
				GameObject go = NGUITools.AddChild (grid.gameObject, buttonPrefab);
				go.SetActive (true);
				ButtonTabEvent bte = go.AddComponent <ButtonTabEvent>();

				if (start > 'z')
				{
					start = 'a';
					then++;
				}
				go.name = (start++) + (then).ToString ();
				btns.Add (go);
				buttonTabMeesgae (go.GetComponentInChildren<UILabel>(), t[i]);
				bte.SetRefresh (t[i], this);
			}
			grid.Reposition ();
		}

		public delegate void ButtonTabInit(UILabel label, object obj);

		public delegate void ButtonTabMeesgae(object t, GameObject go, List<GameObject> btns);
		public ButtonTabMeesgae buttonTabMeesgae;

		public void CallButtonEvent (object t, GameObject go)
		{
			if (buttonTabMeesgae != null) buttonTabMeesgae (t, go, btns);
		}
	}
}
