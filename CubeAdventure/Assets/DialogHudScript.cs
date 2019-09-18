using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogHudScript : MonoBehaviour {

    public bool isStoreDialog = false;
    public bool isTimeAttackDialog = false;
    public bool isBossRaidDialog = false;

    public void Dialog()
    {
        if(isStoreDialog)
        {
            Debug.Log("상점을 열었습니다.");
            HeroScript.Instance.isNpcDialog = true;

            //인벤토리창 active = false
            InvenManager.Instance.DisableInventory();

            NpcStoreManager.Instance.OpenStore();
        }
        else if(isTimeAttackDialog)
        {
            Debug.Log("타임어택 방에 들어왔습니다.");

            HeroScript.Instance.isTimeAttackMode = true;
            GameMainManager.Instance.LoadTimeAttackRoom();
            isTimeAttackDialog = false;
        }
        else if(isBossRaidDialog)
        {
            Debug.Log("보스 레이드 방에 들어왔습니다.");
            
            HeroScript.Instance.isBossRaidMode = true;
            GameMainManager.Instance.LoadBossRaidMap();
        }
    }

}
