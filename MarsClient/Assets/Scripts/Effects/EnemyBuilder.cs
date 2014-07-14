using UnityEngine;
using System.Collections;

public class EnemyBuilder : MonoBehaviour {

	void OnTriggerEnter(Collider other) 
	{
		collider.enabled = false;
		if (other.tag == TagLayerDefine.PLAYER_TAG)
		{
			foreach (ParticleSystem ps in GetComponentsInChildren<ParticleSystem>())
			{
				ps.loop = false;
			}
			FightManager.instance.InitLocalData (this.name);
		}
	}
}
