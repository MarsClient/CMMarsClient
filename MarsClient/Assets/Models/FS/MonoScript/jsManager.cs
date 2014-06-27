using System;
using UnityEngine;

[Serializable]
public class jsManager : MonoBehaviour
{
    public GameObject burette;

    public void OnGUI()
    {
        int num = Screen.width / 8;
        int height = Screen.height;
        Debug.Log(Screen.height);
        if (GUI.Button(new Rect((float) (num - 50), (float) (height * 0), (float) 100, (float) 40), "Nomal_Idle"))
        {
            this.burette.animation.CrossFade("Nomal_Idle", 0.3f);
        }
        if (GUI.Button(new Rect((float) (num + 50), (float) (height * 0), (float) 100, (float) 40), "Nomal_Walk"))
        {
            this.burette.animation.CrossFade("Nomal_Walk", 0.3f);
        }
        if (GUI.Button(new Rect((float) (num + 150), (float) (height * 0), (float) 100, (float) 40), "Nomal_Run"))
        {
            this.burette.animation.CrossFade("Nomal_Run", 0.3f);
        }
        if (GUI.Button(new Rect((float) (num + 250), (float) (height * 0), (float) 100, (float) 40), "change_1"))
        {
            this.burette.animation.CrossFade("change_1", 0.3f);
        }
        if (GUI.Button(new Rect((float) (num + 350), (float) (height * 0), (float) 100, (float) 40), "change_2"))
        {
            this.burette.animation.CrossFade("change_2", 0.3f);
        }
        if (GUI.Button(new Rect((float) (num + 450), (float) (height * 0), (float) 100, (float) 40), "Battle_Idle"))
        {
            this.burette.animation.CrossFade("Battle_Idle", 0.3f);
        }
        if (GUI.Button(new Rect((float) (num + 550), (float) (height * 0), (float) 100, (float) 40), "Battle_Run"))
        {
            this.burette.animation.CrossFade("Battle_Run", 0.3f);
        }
        if (GUI.Button(new Rect((float) (num + 650), (float) (height * 0), (float) 100, (float) 40), "Battle_Run_B"))
        {
            this.burette.animation.CrossFade("Battle_Run_B", 0.3f);
        }
        if (GUI.Button(new Rect((float) (num - 50), (float) (height - 560), (float) 100, (float) 40), "Battle_Run_L"))
        {
            this.burette.animation.CrossFade("Battle_Run_L", 0.1f);
        }
        if (GUI.Button(new Rect((float) (num + 50), (float) (height - 560), (float) 100, (float) 40), "Battle_Run_R"))
        {
            this.burette.animation.CrossFade("Battle_Run_R", 0.1f);
        }
        if (GUI.Button(new Rect((float) (num + 150), (float) (height - 560), (float) 100, (float) 40), "Run_Attack"))
        {
            this.burette.animation.CrossFade("Run_Attack", 0.3f);
        }
        if (GUI.Button(new Rect((float) (num + 250), (float) (height - 560), (float) 100, (float) 40), "skill_01"))
        {
            this.burette.animation.CrossFade("skill_01", 0.3f);
        }
        if (GUI.Button(new Rect((float) (num + 350), (float) (height - 560), (float) 100, (float) 40), "skill_02"))
        {
            this.burette.animation.CrossFade("skill_02", 0.3f);
        }
        if (GUI.Button(new Rect((float) (num + 450), (float) (height - 560), (float) 100, (float) 40), "skill_03"))
        {
            this.burette.animation.CrossFade("skill_03", 0.3f);
        }
        if (GUI.Button(new Rect((float) (num + 550), (float) (height - 560), (float) 100, (float) 40), "jump"))
        {
            this.burette.animation.CrossFade("jump", 0.3f);
        }
        if (GUI.Button(new Rect((float) (num + 650), (float) (height - 560), (float) 100, (float) 40), "Nomal_Talk"))
        {
            this.burette.animation.CrossFade("Nomal_Talk", 0.3f);
        }
        if (GUI.Button(new Rect((float) (num - 50), (float) (height - 520), (float) 100, (float) 40), "MagicAttack_B"))
        {
            this.burette.animation.CrossFade("MagicAttack_B", 0.3f);
        }
        if (GUI.Button(new Rect((float) (num + 50), (float) (height - 520), (float) 100, (float) 40), "MagicAttack_M"))
        {
            this.burette.animation.CrossFade("MagicAttack_M", 0.3f);
        }
        if (GUI.Button(new Rect((float) (num + 150), (float) (height - 520), (float) 100, (float) 40), "MagicAttack_E"))
        {
            this.burette.animation.CrossFade("MagicAttack_E", 0.3f);
        }
        if (GUI.Button(new Rect((float) (num + 250), (float) (height - 520), (float) 100, (float) 40), "Dmg01"))
        {
            this.burette.animation.CrossFade("Dmg01", 0.3f);
        }
        if (GUI.Button(new Rect((float) (num + 350), (float) (height - 520), (float) 100, (float) 40), "Dmg02"))
        {
            this.burette.animation.CrossFade("Dmg02", 0.3f);
        }
        if (GUI.Button(new Rect((float) (num + 450), (float) (height - 520), (float) 100, (float) 40), "Dmg03"))
        {
            this.burette.animation.CrossFade("Dmg03", 0.3f);
        }
        if (GUI.Button(new Rect((float) (num + 550), (float) (height - 520), (float) 100, (float) 40), "dead"))
        {
            this.burette.animation.CrossFade("dead", 0.3f);
        }
        if (GUI.Button(new Rect((float) (num + 650), (float) (height - 520), (float) 100, (float) 40), "Lelvel_Up"))
        {
            this.burette.animation.CrossFade("Lelvel_Up", 0.3f);
        }
    }

    public void Start()
    {
        this.burette = GameObject.FindWithTag("Player");
    }

    public void Update()
    {
    }
}

