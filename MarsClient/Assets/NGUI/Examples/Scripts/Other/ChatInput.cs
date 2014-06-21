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
		Debug.Log (currentChatType);
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
					content = "[" + Localization.Get(currentKey) + "]" + string.Format (Message.MESSAGE_FORMAT, r.accountId.ToString () + "," + r.roleId.ToString (), r.roleName) + text;

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

	void OnEnable ()
	{
		PhotonClient.processResults += ProcessResults;
	}
	
	void OnDisable ()
	{
		PhotonClient.processResults -= ProcessResults;
	}
	
	void ProcessResults (Bundle bundle)
	{
		if (bundle.cmd == Command.SendChat)
		{
			textList.Add (bundle.message.content);
		}
	}
}
