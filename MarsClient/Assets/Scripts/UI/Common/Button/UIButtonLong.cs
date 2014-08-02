using UnityEngine;
using System.Collections;

public abstract class UIButtonLong : MonoBehaviour {

	public float updateFrameRate = 0.03333f;


	private const string MethodName = "UpdateInterval";
	void OnPress (bool isPress)
	{
		if (isPress == true)
		{
			BeginPressEvent ();
			InvokeRepeating (MethodName, 0, updateFrameRate);
		}
		else
		{
			CancelInvoke (MethodName);
			EndPressEvent ();
		}
	}

	private void UpdateInterval ()
	{
		UpdatePressEvent ();
	}

	protected virtual void BeginPressEvent ()
	{}
	protected virtual void UpdatePressEvent ()
	{}
	protected virtual void EndPressEvent ()
	{}
}
