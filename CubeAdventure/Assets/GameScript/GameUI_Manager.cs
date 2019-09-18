using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameUI_Manager : MonoBehaviour {

    [SerializeField]
    UISlider slider_HpBar;
    [SerializeField]
    UISlider slider_MpBar;
    [SerializeField]
    UISlider slider_ExpBar;

    [SerializeField]
    UISlider slider_StateDialogHpBar;
    [SerializeField]
    UISlider slider_StateDialogMpBar;
    [SerializeField]
    UISlider slider_StateDialogExpBar;

    [SerializeField]
    GameObject gb_HudPanel;
    [SerializeField]
    GameObject gb_HudPrefab;

    [SerializeField]
    GameObject gb_DialogHudPrefab;

    [SerializeField]
    GameObject gb_DescriptionPanel;
    [SerializeField]
    GameObject gb_EquipDescriptionPrefab;
    GameObject gb_ItemDescription;

    public GameObject gb_TimeAttackPanel;
    [SerializeField]
    UILabel label_TimeAttack;
    [SerializeField]
    UILabel label_TimeAttackKillCount;

    public GameObject gb_TimeAttackResultPanel;
    [SerializeField]
    UILabel label_TimeAttackResult;
    [SerializeField]
    UILabel label_TimeAttackKill;
    [SerializeField]
    UILabel label_TimeAttackDemage;
    [SerializeField]
    UILabel label_TimeAttackHit;
    [SerializeField]
    UILabel label_TimeAttackRank;

    [SerializeField]
    GameObject panel_BossHpBar;

    [SerializeField]
    UIPanel panel_GameClearNotice;

    bool isHpBarUpdate = false;
    public bool isMpBarUpdate = false;
    bool isExpBarUpdate = false;

    static private GameUI_Manager _instance = null;

    static public GameUI_Manager Instance
    {
        get
        {
            return _instance;
        }
    }

    void Awake()
    {
        _instance = this;
        
    }

    // Use this for initialization
    void Start () {
        StateDialogBarUpdate();
        

        StartCoroutine(HpBarUpdate());
        StartCoroutine(MpBarUpdate());
        StartCoroutine(ExpBarUpdate());

    }
	

    public void StateDialogBarUpdate()
    {
        slider_StateDialogExpBar.value = (float)(StatManager.Instance.remainExp) / (float)(StatManager.Instance.maxExp);
        slider_StateDialogHpBar.value = (float)StatManager.Instance.remainHp / (float)StatManager.Instance.maxHp;
        slider_StateDialogMpBar.value = (float)StatManager.Instance.remainMp / (float)StatManager.Instance.maxMp;
    }
    

    public void UpdateHpBar()
    {
        if (isHpBarUpdate)
        {
            StopCoroutine("HpBarUpdate");
            StartCoroutine(HpBarUpdate());
        }
        else
        {
            StartCoroutine(HpBarUpdate());
        }
    }

    IEnumerator HpBarUpdate()
    {
        isHpBarUpdate = true;
        float remainHp = (float)StatManager.Instance.remainHp / (float)StatManager.Instance.maxHp;
        if (slider_HpBar.value >= remainHp + 0.02f || slider_HpBar.value <= remainHp - 0.02f)
        {
            if (slider_HpBar.value >= (float)StatManager.Instance.remainHp / (float)StatManager.Instance.maxHp)
            {
                while (slider_HpBar.value > (float)StatManager.Instance.remainHp / (float)StatManager.Instance.maxHp)
                {
                    slider_HpBar.value -= 0.02f;
                    yield return null;
                }
            }
            else
            {
                while (slider_HpBar.value < (float)StatManager.Instance.remainHp / (float)StatManager.Instance.maxHp)
                {
                    slider_HpBar.value += 0.02f;
                    yield return null;
                }
            }
        }
            
        isHpBarUpdate = false;
    }

    public void UpdateMpBar()
    {
        if(isMpBarUpdate)
        {
            StopCoroutine("MpBarUpdate");
            StartCoroutine(MpBarUpdate());
        }
        else
        {
            StartCoroutine(MpBarUpdate());
        }
    }
    IEnumerator MpBarUpdate()
    {
        isMpBarUpdate = true;

        float remainMp = (float)StatManager.Instance.remainMp / (float)StatManager.Instance.maxMp;
        if(slider_MpBar.value >= remainMp + 0.02f || slider_MpBar.value <= remainMp - 0.02f)
        {
            if (slider_MpBar.value >= remainMp)
            {
                while (slider_MpBar.value > remainMp)
                {
                    slider_MpBar.value -= 0.02f;
                    yield return null;
                }
            }
            else
            {
                while (slider_MpBar.value < remainMp)
                {
                    slider_MpBar.value += 0.02f;
                    yield return null;
                }
            }
        }

        isMpBarUpdate = false;
    }

    public void UpDateExpBar()
    {
        if (isExpBarUpdate)
        {
            StopCoroutine("ExpBarUpdate");
            StartCoroutine(ExpBarUpdate());
        }
        else
        {
            StartCoroutine(ExpBarUpdate());
        }
    }
    IEnumerator ExpBarUpdate()
    {
        isExpBarUpdate = true;

        float remainExp = (float)StatManager.Instance.remainExp / (float)StatManager.Instance.maxExp;
        if (slider_ExpBar.value >= remainExp + 0.005f || slider_ExpBar.value <= remainExp - 0.005f)
        {
            if (slider_ExpBar.value >= (float)StatManager.Instance.remainExp / (float)StatManager.Instance.maxExp)
            {
                while (slider_ExpBar.value > (float)StatManager.Instance.remainExp / (float)StatManager.Instance.maxExp)
                {
                    slider_ExpBar.value -= 0.005f;
                    yield return null;
                }
            }
            else
            {
                while (slider_ExpBar.value < (float)StatManager.Instance.remainExp / (float)StatManager.Instance.maxExp)
                {
                    slider_ExpBar.value += 0.005f;
                    yield return null;
                }
            }

        }
        isExpBarUpdate = false;
    }

    // 몬스터 상단 체력 바 생성
    public GameObject EnemyHudBar()
    {
        GameObject newEnemyHud = Instantiate(gb_HudPrefab, Vector3.zero, Quaternion.identity);
        newEnemyHud.transform.parent = gb_HudPanel.transform;

        newEnemyHud.GetComponent<UISprite>().depth = -2;
        newEnemyHud.transform.GetChild(0).GetComponent<UISprite>().depth = -1;

        return newEnemyHud;
    }

    public GameObject BossHpBar()
    {
        GameObject BossHpBar = Instantiate(gb_HudPrefab, Vector3.zero, Quaternion.identity);
    
        BossHpBar.transform.parent = panel_BossHpBar.transform;

        BossHpBar.GetComponent<UISprite>().depth = 2;
        BossHpBar.transform.GetChild(0).GetComponent<UISprite>().depth = 3;

        return BossHpBar;
    }

    // npc 머리 위 아이콘 생성
    public GameObject NpcDialogHud()
    {
        GameObject newNpcDialogHud = Instantiate(gb_DialogHudPrefab, Vector3.zero, Quaternion.identity);
        newNpcDialogHud.transform.parent = gb_HudPanel.transform;

        return newNpcDialogHud;
    }

    // 아이템 설명 창 초기화
    public void ItemDescriptionInit(int itemKind, int itemCode)
    {
        
        switch(itemKind)
        {
            case (int)ItemKind.EQUIP:
                {
                    Dictionary<int, Equip> equipDefine = InvenManager.Instance.dic_Equip;
                    Equip equip = equipDefine[itemCode];
                    string itemName = equip.EquipName;
                    string itemText = equip.text;
                    string equipPart = "";
                    string itemLevel = "0";
                    switch(equip.EquipPart)
                    {
                        case (int)EquipKind.HEAD:
                            {
                                equipPart = "머리";
                                break;
                            }
                        case (int)EquipKind.NECK:
                            {
                                equipPart = "목";
                                break;
                            }
                        case (int)EquipKind.TOP:
                            {
                                equipPart = "상의";
                                break;
                            }
                        case (int)EquipKind.WEAPON:
                            {
                                equipPart = "무기";
                                break;
                            }
                        case (int)EquipKind.SHIELD:
                            {
                                equipPart = "보조";
                                break;
                            }
                        case (int)EquipKind.BELT:
                            {
                                equipPart = "허리";
                                break;
                            }
                        case (int)EquipKind.SHOES:
                            {
                                equipPart = "신발";
                                break;
                            }
                        case (int)EquipKind.RING:
                            {
                                equipPart = "반지";
                                break;
                            }
                    }

                    string attack = equip.attack.ToString();
                    string defence = equip.defence.ToString();
                    string speed = equip.speed.ToString();
                    
                    // 세부 설정
                    gb_ItemDescription = Instantiate(gb_EquipDescriptionPrefab, gb_DescriptionPanel.transform);
                    gb_ItemDescription.transform.GetChild(1).GetComponent<UISprite>().spriteName = "Equip_" + itemCode.ToString("D2");
                    gb_ItemDescription.transform.GetChild(2).GetComponent<UILabel>().text = itemName;
                    gb_ItemDescription.transform.GetChild(3).GetComponent<UILabel>().text = "Lv." + itemLevel;
                    gb_ItemDescription.transform.GetChild(4).GetComponent<UILabel>().text = equipPart;
                    gb_ItemDescription.transform.GetChild(5).GetComponent<UILabel>().text = "ATK   " + attack;
                    gb_ItemDescription.transform.GetChild(6).GetComponent<UILabel>().text = "DEF   " + defence;
                    gb_ItemDescription.transform.GetChild(7).GetComponent<UILabel>().text = "SPD   " + speed;
                    gb_ItemDescription.transform.GetChild(9).GetComponent<UILabel>().text = itemText;
                    
                    break;
                }
        }
    }

    public void DestoryItemDescription()
    {
        if(gb_ItemDescription != null)
        {
            Destroy(gb_ItemDescription);
            gb_ItemDescription = null;
        }
    }

    public void InitHudPanel()
    {
        if(gb_HudPanel.transform.childCount > 0)
        {
            NGUITools.DestroyChildren(gb_HudPanel.transform);
        }
 

        if(gb_DescriptionPanel.transform.childCount > 0)
        {
            NGUITools.DestroyChildren(gb_DescriptionPanel.transform);
        }

    }

    public void UpdateTimeAttack(float time)
    {
        label_TimeAttack.text = time.ToString("0.00");
    }

    //타임어택 플레이중 킬 카운팅
    public void TimeAttackKillCount()
    {
        label_TimeAttackKillCount.text = HeroScript.Instance.timeAttackKillCount.ToString();
    }

    //타임어택 결과 집계
    public void TimeAttackResult(bool isClear)
    {
        Time.timeScale = 0f;

        label_TimeAttackKill.text = label_TimeAttackKillCount.text;
  
        label_TimeAttackDemage.text = HeroScript.Instance.timeAttackDemage.ToString();

        label_TimeAttackHit.text = HeroScript.Instance.timeAttackHit.ToString();

        //타임어택 클리어
        if(isClear)
        {
            label_TimeAttackResult.text = "SUCCESS";
            if(HeroScript.Instance.timeAttackKillCount <= 10)
            {
                label_TimeAttackRank.text = "C";
            }
            else if(HeroScript.Instance.timeAttackKillCount <= 20)
            {
                label_TimeAttackRank.text = "B"; 
            }
            else
            {
                label_TimeAttackRank.text = "A";
            }
        }
        else
        {
            label_TimeAttackResult.text = "FAIL";
            label_TimeAttackRank.text = "D";
        }

        gb_TimeAttackResultPanel.SetActive(true);
        gb_TimeAttackResultPanel.GetComponent<AudioSource>().Play();
    }

    public void CloseTimeAttackResult()
    {
        gb_TimeAttackResultPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void GameClearNotice()
    {
        panel_GameClearNotice.gameObject.SetActive(true);
        Time.timeScale = 0f;
        panel_GameClearNotice.GetComponent<AudioSource>().Play();
    }

    public void CloseGameClearNotice()
    {
        panel_GameClearNotice.gameObject.SetActive(false);
        Time.timeScale = 1f;
    }
}
