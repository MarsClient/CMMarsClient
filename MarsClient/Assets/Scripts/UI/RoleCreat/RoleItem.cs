using UnityEngine;
using System.Collections;

public class RoleItem : MonoBehaviour {

	public UISprite sprite;
	public UILabel label;
	public UILabel nameLbl;

	public Role role;

	public void SetData (Role r) 
	{ 
		nameLbl.text = r.roleName;
		label.text = string.Format (Localization.Get ("game.role.level.pro"), 
			               r.level, Localization.Get ("game.role." + r.profession.ToString())); 
		this.role = r;
	}
	public void SetColor (bool isMine) { sprite.color = isMine ? Color.yellow : new Color (1f, 1f, 1f, 0f); }
//	public void disableClick () { sprite.color = Color.white; }
//	public void ableClick () { sprite.color = Color.yellow; }
}
