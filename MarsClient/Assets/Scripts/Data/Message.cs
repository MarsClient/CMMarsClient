using UnityEngine;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;


public enum ChatType
{
	Null = 0,
	World,
	Person,
	System,
	Team,
}
public class Message
{
	public const string MESSAGE_FORMAT = "[url={0}][u][ffff00][{1}]: [-][/u][/url]";

	public const string CLICKINFO_ROLE = "ROLE_INFO";
	public const string CLICKINFO_EQUIP = "EQUIP_INFO,";


	[DefaultValue (null)]
	public ChatType chatType;
	[DefaultValue (null)]
	public Role sender;
	[DefaultValue (null)]
	public Role receiver;//only Person isnot null
	[DefaultValue (null)]
	public string content;
}
