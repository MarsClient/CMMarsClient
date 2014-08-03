using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerUnit : HitUnit {

	public static List<PlayerUnit> playersUnit = new List<PlayerUnit> ();

	private Role role;
	
	void Awake ()
	{
		base.Awake ();
		//m_ac = GetComponent <AiAnimation>();
		Add ();
	}

	void Add ()
	{
		playersUnit.Add (this);
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
		PhotonClient.processResultSync += ProcessResultSync;
	}

	void OnDisable ()
	{
		PhotonClient.processResultSync -= ProcessResultSync;
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
