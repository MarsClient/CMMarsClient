using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// Very simple example of how to use a TextList with a UIInput for chat.
/// </summary>

[AddComponentMenu("NGUI/Examples/Chat Input")]
public class ChatInput : MonoBehaviour
{
	public UITextList textList;
	public UIInput mInput;

	public UIPopupList popupList;

	private ChatType currentChatType;
	private string currentKey;

	public bool isOne;
	
	void Start ()
	{

		if (mInput == null) {  mInput = GetComponent<UIInput>(); }
		mInput.label.maxLineCount = 1;

		currentKey = popupList.value;
		getChatType (currentKey);
		EventDelegate.Add (popupList.onChange, ()=>
		{
			if (UIPopupList.current != null)
			{
				currentKey = UIPopupList.current.value;
				getChatType (currentKey);
			}
		});
		ShowAllContent ();
	}

	public void ShowAllContent ()
	{
		if (textList != null)
		{
			foreach (Message message in Main.Instance.Getmessages ())
			{
				textList.Add (message.content);
			}
		}
	}

	void getChatType (string key)
	{
		string[] str = key.Split ('.');
		currentChatType = (ChatType) int.Parse(str[str.Length - 1]);
//		Debug.Log (currentChatType);
	}

	public void OnClick ()
	{
		OnSubmit ();
	}

	public void OnSubmit ()
	{
		if (textList != null)
		{
			// It's a good idea to strip out all symbols as we don't want user input to alter colors, add new lines, etc
			string text = NGUIText.StripSymbols(mInput.value);

			if (!string.IsNullOrEmpty(text))
			{
				Role r = Main.Instance.role;
				string content = "";
				if (r != null)
				{
					string channel = Localization.Get(currentKey);
					string sender = string.Format (Message.MESSAGE_FORMAT, Message.CLICKINFO_ROLE + "," + r.accountId.ToString () + "," + r.roleName, r.roleName);
					content = channel + sender + text;

					Message message = new Message ();
					message.chatType = currentChatType;
					message.sender = r;
					message.content = content;
					NetSend.SendChat (message);
				}
				else
				{
					content = text;
				}
				textList.scrollValue = 1;
				textList.Add(content);
				mInput.value = "";
				mInput.isSelected = false;
			}
		}
	}

	public void ClickInfo (string[] infos)
	{
//		Debug.Log (infos[0] + "____" + Message.CLICKINFO_ROLE);
		if (infos[0] == Message.CLICKINFO_ROLE)
		{	
			long accountId = long.Parse (infos[1]);
			string roleName = infos[2];
			string show = string.Format (Localization.Get ("game.chat.send.secret"), roleName);
//			popupList.value = show;
			//Debug.Log (show);
		}
	}

	void OnEnable ()
	{
		if (isOne)
			PhotonClient.processResultSync += ProcessResultSync;
	}
	
	void OnDisable ()
	{
		if(isOne)
			PhotonClient.processResultSync -= ProcessResultSync;
	}
	
	void ProcessResultSync (Bundle bundle)
	{
		if (bundle.cmd == Command.SendChat)
		{
			textList.Add (bundle.message.content);
		}
	}
}
