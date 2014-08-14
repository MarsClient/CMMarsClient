using UnityEngine;
using System.Collections;

public class LableLoadingShow : MonoBehaviour {

	UILabel m_Label;
//
	void Start ()
	{
		m_Label = GetComponent<UILabel> ();
		InvokeRepeating ("UpdateText", 0, 0.25f);
	}

	int i = 0;
	void UpdateText ()
	{

		if (i == 0)
		{
			m_Label.text = "Loading";
		}
		if (i == 1)
		{
			m_Label.text = "Loading.";
		}
		if (i == 2)
		{
			m_Label.text = "Loading..";
		}
		if (i == 3)
		{
			m_Label.text = "Loading...";
			i = 0;
		}
		++i;
	}
}
