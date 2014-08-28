using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerUnit : HitUnit {

	#region playersUnit
	public static List<PlayerUnit> playersUnit = new List<PlayerUnit> ();
	public static PlayerUnit TryGetPlayerUnit (long roleId)
	{
		return TryGetPlayerUnit (null, roleId);
	}
	public static PlayerUnit TryGetPlayerUnit (Transform target = null, long roleId = 0)
	{
		for (int i = 0; i < playersUnit.Count; i++)
		{
			PlayerUnit playerUnit = playersUnit[i];
			if (target != null)
			{
				if (playerUnit.transform == target)
				{
					return playerUnit;
				}
			}
			else if (roleId != 0)
			{
				if (playerUnit.role.roleId == roleId)
				{
					return playerUnit;
				}
			}
		}
		return null;
	}
	#endregion

	private Role role;
	private AiPlayer m_player;
	
	void Awake ()
	{
		base.Awake ();
		m_player = GetComponent<AiPlayer>();
		if (FightManager.instance != null)
		{
			Add ();
		}
	}

	void Add ()
	{
		playersUnit.Add (this);
	}

	void OnDestory ()
	{
		Remove ();
	}

	void Remove ()
	{
		playersUnit.Remove (this);
	}

	void LateUpdate ()
	{
		updateUIShow ();
	}

	public override void DataRefresh (object t)
	{
		role = t as Role;
		if (label != null && role != null)
		{
			label.bitmapFont = GameUIAtlas.Instance.normalFont;
			if (role.roleId == Main.Instance.role.roleId)
			{
				label.color = Color.yellow;
			}
			label.text = role.roleName;
		}
		if (m_player != null)
		{
			m_player.SetOwn (Main.Instance.role.roleId == role.roleId);
		}
	}

	public void RefreshMulPlayerState (Role r)
	{
		if (r != null)
		{
			role = r;
			transform.position = new Vector3 (r.x, 0, r.z);
			transform.forward = new Vector3 (r.xRo, 0, r.zRo);
			Clip c = (Clip) r.action;
//			Debug.LogError (c);
			m_ac.Play (c);
		}
	}

	public override void UnitDeath ()
	{

	}

	void OnEnable ()
	{
		NetClient.processResultSync += ProcessResultSync;
	}

	void OnDisable ()
	{
		NetClient.processResultSync -= ProcessResultSync;
	}

	void ProcessResultSync (Bundle bundle)
	{
		if (bundle.cmd == Command.UpdatePlayer || bundle.cmd == Command.TeamUpdate)
		{
			if (role != null && role.roleId == bundle.role.roleId)
			{
				RefreshMulPlayerState (bundle.role);
			}
		}
		if (bundle.cmd == Command.DestroyPlayer)
		{
			if (role != null && role.roleId == bundle.role.roleId)
			{
				Destroy (gameObject);
			}
		}
	}

}
