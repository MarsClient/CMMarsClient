using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace TabButton
{
	public interface ITabListener
	{
		void TabInitialization(GameObject go, object obj);
		void TabOnClickMeesgae(object t, GameObject go, List<GameObject> btns);
	}

	public class UITabList : MonoBehaviour
	{
		private UIGrid grid;
		public bool isInit = false;

		[HideInInspector]
		public GameObject buttonPrefab;
		public ITabListener tabListener;

		private List<GameObject> btns = new List<GameObject> ();

		public void refresh ()
		{

			for (int i = 0; i < transform.childCount; i++)
			{
				Transform go = transform.FindChild (i.ToString ());
				if (go != null)
				{
					TabEvent bte = go.gameObject.AddComponent <TabEvent>();
					if (tabListener != null) tabListener.TabInitialization (go.gameObject, null);
					else Debug.LogError ("tabListener is null");
					bte.SetRefresh (i, this);
					btns.Add (go.gameObject);
				}
			}
			LayoutBtns ();
		}

		public void refresh (List<object> t)
		{
			Clear ();
			if (grid == null) grid = GetComponent<UIGrid> ();
			char start = 'a';
			char then = 'a';
			for (int i = 0; i < t.Count; i++)
			{
				GameObject go = NGUITools.AddChild (grid.gameObject, buttonPrefab);
				go.SetActive (true);
				TabEvent bte = go.AddComponent <TabEvent>();

				if (start > 'z')
				{
					start = 'a';
					then++;
				}
				go.name = (start++) + (then).ToString ();
				btns.Add (go);
				if (tabListener != null) tabListener.TabInitialization (go, t[i]);
				else Debug.LogError ("tabListener is null");
				bte.SetRefresh (t[i], this);
			}
			Invoke ("LayoutBtns", 0);
		}

		void LayoutBtns () { if (grid != null) grid.Reposition (); if (isInit) { if (btns.Count > 0) { btns[0].GetComponent<TabEvent>().OnClick (); } }}

		public void CallButtonEvent (object t, GameObject go)
		{
			if (tabListener != null) tabListener.TabOnClickMeesgae (t, go, btns);
			else Debug.LogError ("tabListener is null");
		}

		public void Clear ()
		{
			foreach (GameObject o in btns)
			{
				Destroy (o);
			}
			btns.Clear ();
		}
	}
}
