using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcDialog : MonoBehaviour {

    bool isStoreNpc = false;
    bool isTimeAttackNpc = false;
    bool isBossRaidNpc = false;
    GameObject Hero;
    GameObject DialogHud;
	// Use this for initialization
	void Start () {
        if(this.transform.name.Equals("Barbarian Wyder-Blue(Clone)") || this.transform.name.Equals("Barbarian Wyder-Gold(Clone)"))
        {
            isStoreNpc = true;
        }

        if(this.transform.name.Equals("Barbarian Wyder-Green(Clone)"))
        {
            isTimeAttackNpc = true;
        }

        if(this.transform.name.Equals("Barbarian Wyder-Red(Clone)"))
        {
            isBossRaidNpc = true;
        }



        Hero = HeroScript.Instance.gameObject;
        DialogHud = null;
	}

    void Update()
    {
        DiscoverHero();
    }

    //캐릭터 바라보기
    void DiscoverHero()
    {
        Vector2 HeroPosition = new Vector2(Hero.transform.position.x, Hero.transform.position.z);
        Vector2 MyPosition = new Vector2(this.transform.position.x, this.transform.position.z);

        float distance = (float)Mathf.Sqrt(Mathf.Pow(HeroPosition.x - MyPosition.x, 2) + Mathf.Pow(HeroPosition.y - MyPosition.y, 2));

        if (distance <= 4f)
        {
            this.transform.LookAt(Hero.transform);
            if(DialogHud == null)
            {
                DialogHud = GameUI_Manager.Instance.NpcDialogHud();
                DialogHud.transform.localScale = new Vector3(0.5f, 0.5f, 1f);
                DialogHudPosition();
                if (isStoreNpc)
                {
                    DialogHud.GetComponent<DialogHudScript>().isStoreDialog = true;
                }

                if (isTimeAttackNpc)
                {
                    DialogHud.GetComponent<DialogHudScript>().isTimeAttackDialog = true;
                }

                if (isBossRaidNpc)
                {
                    DialogHud.GetComponent<DialogHudScript>().isBossRaidDialog = true;
                }
            }
            else
            {
                DialogHudPosition();
            }
        }
        else
        {
            if(DialogHud != null)
            {
                Destroy(DialogHud);
                DialogHud = null;
            }
        }

    }

    void DialogHudPosition()
    {
        Vector2 screenPos = Camera.main.WorldToScreenPoint(this.transform.position);
        screenPos = new Vector2(screenPos.x, screenPos.y + 45f);  // 머리 위  Dialog Hud 간격
        Vector3 HudPosition = UICamera.mainCamera.ScreenToWorldPoint(screenPos);
        HudPosition.z = 0f;
        DialogHud.transform.position = HudPosition;
    }

}
