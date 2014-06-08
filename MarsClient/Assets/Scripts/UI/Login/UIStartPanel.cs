using UnityEngine;
using System.Collections;


[System.Serializable]
public class LoginMode
{
	public GameObject mainObj;
	public UIInput user;
	public UIInput password;

	private Error loginSuccess ()
	{
		Error e = null;
		if (user.text == "" || user.text == "")
		{
			e = new Error ();
			e.message = Localization.instance.Get ("game.login.null.error");
		}
		return e;
	}

	public void Login ()
	{
		if (loginSuccess () == null)
		{

		}
	}
}

[System.Serializable]
public class RegisterMode
{
	public GameObject mainObj;
	public UIInput user;
	public UIInput password;
	public UIInput againPassword;
}

public class UIStartPanel : MonoBehaviour {

	public LoginMode loginMode;
	public RegisterMode registerMode;

	public void Start ()
	{
		loginMode.mainObj.SetActive (true);
		registerMode.mainObj.SetActive (false);
	}
	public void LoginGame ()
	{
//		string user = loginMode.user.text;
//		string pw = loginMode.password.text;
//		Toast.ShowNormalText (user + '\n' + pw);
	}

}
