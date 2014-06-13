using UnityEngine;
using System.Collections;


[System.Serializable]
public class LoginMode
{
	public const string LOGIN_STATE_CHECK = "~Login.key.check";
	public const string LOGIN_USER_KEY = "~Login.key.user";
	public const string LOGIN_PASSWORD_KEY = "~Login.key.password";


	public GameObject mainObj;
	public UIInput user;
	public UIInput password;
	public UICheckbox checkBox;

	public void init ()
	{
		user.text = PlayerPrefs.GetString (LOGIN_USER_KEY, "");
		password.text = PlayerPrefs.GetString (LOGIN_PASSWORD_KEY, "");
		if (checkBox != null)
		{
			checkBox.isChecked = (1 == PlayerPrefs.GetInt (LOGIN_STATE_CHECK, 0));
			checkBox.onStateChange = (bool isState)=>
			{
				//Debug.Log (isState);
				PlayerPrefs.SetInt (LOGIN_STATE_CHECK, isState ? 1 : 0);
				if (user.text != "" && password.text != "")
				{
					PlayerPrefs.SetString (LOGIN_USER_KEY, user.text);
					PlayerPrefs.SetString (LOGIN_PASSWORD_KEY, password.text);
				}
			};
			if (checkBox.isChecked)
			{
				user.validator = (string currentText, char nextChar)=>
				{
					string input = currentText + nextChar.ToString ();
					PlayerPrefs.SetString (LOGIN_USER_KEY, input);
					return nextChar;
				};
				password.validator = (string currentText, char nextChar)=>
				{
					string input = currentText + nextChar.ToString ();
					PlayerPrefs.SetString (LOGIN_PASSWORD_KEY, input);
					return nextChar;
				};
			}
		}
	}

	private Error loginSuccess ()
	{
		Error e = null;
		if (user.text == "" || password.text == "")
		{
			e = new Error ();
			e.message = "game.login.null.error";
		}
		return e;
	}

	public void Login ()
	{
		Error e = loginSuccess ();
		if (e == null)
		{
			Account a = new Account ();
			a.id = user.text;
			a.pw = password.text;
			NetSend.SendLogin (a);
			new DialogContent ().SetMessage ("game.dialog.waiting").SetNoBtn ("game.dialog.no").ShowWaiting ();
		}
		else
		{
			new DialogContent ()
				.SetMessage (e.message)
					.SetNoBtn ("game.dialog.no")
					.ShowWaiting ();
		}
	}
	public void Clear () { user.text = ""; password.text = ""; }
}

[System.Serializable]
public class RegisterMode
{
	public GameObject mainObj;
	public UIInput user;
	public UIInput password;
	public UIInput againPassword;

	private Error registerSuccess ()
	{
		Error e = null;
		if (user.text == "" || password.text == "" || againPassword.text == "")
		{
			e = new Error ();
			e.message = "game.login.null.error";
		}
		else if (password.text != againPassword.text)
		{
			e = new Error ();
			e.message = "game.register.not.same";
		}
		else if (user.text.Length < 8 || password.text.Length < 6)
		{
			e = new Error ();
			e.message = "game.register.length";
		}
		else
		{
			Account a = new Account ();
			a.id = user.text;
			a.pw = password.text;
			NetSend.SendRegister (a);
		}
		return e;
	}

	public void Clear () { user.text = ""; password.text = ""; againPassword.text = ""; }

	public void Register ()
	{
		Error e = registerSuccess ();
		if (e == null)
		{
			//TODO
		}
		else
		{
			new DialogContent ()
				.SetMessage (e.message)
					.SetNoBtn ("game.dialog.no")
					.ShowWaiting ();
		}
	}
}

public class UIStartPanel : MonoBehaviour {

	public LoginMode loginMode;
	public RegisterMode registerMode;

	public void Start ()
	{
		loginMode.init ();
		loginMode.mainObj.SetActive (true);
		registerMode.mainObj.SetActive (false);

		PhotonClient.ProcessResults += ProcessResults;
	}
	public void LoginGame ()
	{
		loginMode.Login ();
	}

	public void Register ()
	{
		loginMode.mainObj.SetActive (false);
		registerMode.mainObj.SetActive (true);
	}

	public void RegisterAccount ()
	{
		registerMode.Register ();
	}

	public void Back ()
	{
		Start ();
	}

	public void RegisterDoneLogin ()
	{
		loginMode.user.text = registerMode.user.text;
		loginMode.password.text = registerMode.password.text;
	}

	void ProcessResults (Bundle bundle)
	{
		if (bundle.cmd == Command.Register)
		{
			if (bundle.error == null){ new DialogContent ().SetMessage ("game.register.success").SetYesBtn ("game.dialog.login").SetNoBtn ("game.dialog.no").SetDelegateBtn (LoginGameDelegate).Show (); }
			else{ new DialogContent ().SetMessage (bundle.error.message).SetNoBtn ("game.dialog.yes").ShowWaiting (); registerMode.Clear (); }
		}
		else if (bundle.cmd == Command.Login)
		{
			if (bundle.error == null){ new DialogContent ().SetMessage ("game.dialog.login.success").SetNoBtn ("game.dialog.no").ShowWaiting (); }
			else { new DialogContent ().SetMessage (bundle.error.message).SetNoBtn ("game.dialog.no").ShowWaiting (); }
		}
	}

	void LoginGameDelegate (bool isYes)
	{
		if (isYes) 
		{ 
			RegisterDoneLogin ();
			Back ();
			registerMode.Clear ();
		}
	}
}
