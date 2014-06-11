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
