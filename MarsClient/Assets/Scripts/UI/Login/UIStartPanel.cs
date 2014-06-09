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

	void ProcessResults (Bundle bundle)
	{
		if (bundle.cmd == Command.Register)
		{
			if (bundle.error == null)
			{
				new DialogContent ()
					.SetMessage ("game.register.success")
						.SetNoBtn ("game.dialog.yes")
						.ShowWaiting ();
			}
			else
			{
				if (bundle.error == null)
				{
					new DialogContent ()
						.SetMessage ("game.register.success")
							.SetNoBtn ("game.dialog.yes")
							.ShowWaiting ();
				}
			}
		}
	}
}
