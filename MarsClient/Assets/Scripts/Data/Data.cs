using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;





public class User
{
	public Account account = new Account ();
	public Room room = new Room ();
}

public class Room
{
	public int RoomIndex = -1;
	public string RoomName;
	public int Limit;
	public int ActorCount;
	//public List<Account>
}

public class Message
{
	public string from = "";
	public string to = DefineString.TO_WORLD_CHAT;
	public string content;
	public DateTime time;
}

/*public class Item
{
	public int id;
	public string name;
	public string type;
}*/

//public class Role
//{
//	public float speed;
//}

public class RoomMember
{
	public string userId;
	public string userName;
	public float posX;
	public float posY;
	public float posZ;
	public float direct;
	public float act;//0,stand, 1, walk, 2,attack....
}
