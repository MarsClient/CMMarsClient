using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace TabButton
{
	public interface ITabListener
	{
		void TabInitialization(UILabel label, object obj);
		void TabOnClickMeesgae(object t, GameObject go, List<GameObject> btns);
	}

	public class UITabButton : MonoBehaviour
	{
		private UIGrid grid;
		public GameObject buttonPrefab;
		public ITabListener tabListener;

		private List<GameObject> btns = new List<GameObject> ();
		public void refresh (List<object> t)
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
				if (tabListener != null) tabListener.TabInitialization (go.GetComponentInChildren<UILabel>(), t[i]);
				else Debug.LogError ("tabListener is null");
				bte.SetRefresh (t[i], this);
			}
			grid.Reposition ();
		}

		public void CallButtonEvent (object t, GameObject go)
		{
			if (tabListener != null) tabListener.TabOnClickMeesgae (t, go, btns);
			else Debug.LogError ("tabListener is null");
		}
	}
}
