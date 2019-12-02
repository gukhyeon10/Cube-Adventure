using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.IO;
public class StatManager : MonoBehaviour {

    public bool isOpenState = false;
    public string heroName = "";
    public int Level, str, dex, spd, sp;
    public float Attack, Defence, Speed;
    public float buffAttack, buffDefence, buffSpeed;
    public int maxHp, maxMp, maxExp, remainHp, remainMp, remainExp;

    [SerializeField]
    GameObject MyStatDialog;

    [SerializeField]
    UILabel label_HeroName;
    [SerializeField]
    UILabel label_Stat_Level;
    [SerializeField]
    UILabel label_Stat_Hp;
    [SerializeField]
    UILabel label_Stat_Mp;
    [SerializeField]
    UILabel label_Stat_Exp;

    [SerializeField]
    UILabel label_Stat_STR;
    [SerializeField]
    UILabel label_Stat_DEX;
    [SerializeField]
    UILabel label_Stat_SPD;
    [SerializeField]
    UILabel label_Stat_Sp;

    [SerializeField]
    UILabel label_Stat_Attack;
    [SerializeField]
    UILabel label_Stat_Defence;
    [SerializeField]
    UILabel label_Stat_Speed;

    [SerializeField]
    GameObject gb_StatPointButtonGroup;

    [SerializeField]
    GameObject gb_LevelUpEffect;

    static private StatManager _instance = null;

    static public StatManager Instance
    {
        get
        {
            return _instance;
        }
    }

    void Awake()
    {
        _instance = this;
        HeroDataUpLoad();
    }

    // Use this for initialization
    void Start () {

        Attack = 10f;
        Defence = 10f;
        Speed = 5f;

        buffAttack = 0f;
        buffDefence = 0f;
        buffSpeed = 0f;

        StartCoroutine(RecoveryCorutine());
	}

    //HP와 MP가 회복되는 자연스러운 연출
    IEnumerator RecoveryCorutine()
    {
        while(true)
        {
            if(remainHp < maxHp)
            {
                if(remainHp >0)
                {
                    remainHp += 3;
                }
                if (remainHp > maxHp)
                {
                    remainHp = maxHp;
                }
            }

            if(remainMp<maxMp)
            {
                if(remainHp > 0)
                {
                    remainMp += 5;
                }
                if (remainMp> maxMp)
                {
                    remainMp = maxMp;
                }
            }

            yield return new WaitForSeconds(1f);
            GameUI_Manager.Instance.UpdateHpBar();
            GameUI_Manager.Instance.UpdateMpBar();
            GameUI_Manager.Instance.StateDialogBarUpdate();
            HeroStatUpdate();
        }
    }
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.S))
        {
            MyStatDialog.gameObject.SetActive(!(MyStatDialog.gameObject.activeSelf));
            isOpenState = !isOpenState;
            HeroStatUpdate();
            GameUI_Manager.Instance.StateDialogBarUpdate();
        }

	}

    // 캐릭터 데이터 로드
    public void HeroDataUpLoad()
    {
        string strPath = string.Empty;
        bool isInitFile = false;
#if(UNITY_EDITOR || UNITY_STANDALONE_WIN)
        strPath += (Application.streamingAssetsPath + "/Hero/HeroData.xml");
#elif UNITY_ANDROID
        label_HeroName.text = "apple";
        
        strPath += (Application.streamingAssetsPath + "/Hero/HeroData.xml");
         label_HeroName.text = "app";
#endif
        XmlDocument xmlDoc = new XmlDocument();
        //캐릭터 데이터 파일이 없을때 예외처리 
        if (File.Exists(strPath))
        {
            Debug.Log("해당 경로 파일 있음");

            xmlDoc.Load(strPath);
        }
        else
        {
            // 파일 초기값  해당 파일이 없을때
            isInitFile = true;
            TextAsset textAsset = (TextAsset)Resources.Load("Hero/HeroData");

            xmlDoc.LoadXml(textAsset.text);
        }   

            XmlNode HeroData = xmlDoc.SelectSingleNode("Hero/Info");

            this.heroName = HeroData.SelectSingleNode("Name").InnerText;
            label_HeroName.text = this.heroName;

            this.Level = int.Parse(HeroData.SelectSingleNode("Level").InnerText);
        
        
            this.maxHp = int.Parse(HeroData.SelectSingleNode("MaxHp").InnerText);
            this.remainHp = int.Parse(HeroData.SelectSingleNode("RemainHp").InnerText);
            this.maxMp = int.Parse(HeroData.SelectSingleNode("MaxMp").InnerText);
            this.remainMp = int.Parse(HeroData.SelectSingleNode("RemainMp").InnerText);
            this.maxExp = int.Parse(HeroData.SelectSingleNode("MaxExp").InnerText);
            this.remainExp = int.Parse(HeroData.SelectSingleNode("RemainExp").InnerText);

            this.str = int.Parse(HeroData.SelectSingleNode("STR").InnerText);
            this.dex = int.Parse(HeroData.SelectSingleNode("DEX").InnerText);
            this.spd = int.Parse(HeroData.SelectSingleNode("SPD").InnerText);
            this.sp = int.Parse(HeroData.SelectSingleNode("SP").InnerText);

        if (isInitFile)
        {
            this.Level++;
            this.sp += 5;
        }


        HeroStatUpdate();
    }
    //캐릭터 데이터 세이브
    public void HeroDataSave()
    {
        string strPath = string.Empty;
#if (UNITY_EDITOR || UNITY_STANDALONE_WIN)
        strPath += (Application.streamingAssetsPath + "/Hero/HeroData.xml");
#elif UNITY_ANDROID
        strPath = "jar:file://" + Application.dataPath + "!/assets" + "/Hero/HeroData.xml";
#endif
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.AppendChild(xmlDoc.CreateXmlDeclaration("1.0", "utf-8", "yes"));

        //루트 노드 생성
        XmlNode root = xmlDoc.CreateNode(XmlNodeType.Element, "Hero", string.Empty);
        xmlDoc.AppendChild(root);

        //자식 노드 생성
        XmlNode InfoNode = xmlDoc.CreateNode(XmlNodeType.Element, "Info", string.Empty);
        root.AppendChild(InfoNode);

        //캐릭터 능력치 속성 설정
        XmlElement Name = xmlDoc.CreateElement("Name");
        Name.InnerText = "주인공";
        InfoNode.AppendChild(Name);
        
        XmlElement Level = xmlDoc.CreateElement("Level");
        Level.InnerText = this.Level.ToString();
        InfoNode.AppendChild(Level);
        
        XmlElement MaxHp = xmlDoc.CreateElement("MaxHp");
        MaxHp.InnerText = this.maxHp.ToString();
        InfoNode.AppendChild(MaxHp);
        
        XmlElement MaxMp = xmlDoc.CreateElement("MaxMp");
        MaxMp.InnerText = this.maxMp.ToString();
        InfoNode.AppendChild(MaxMp);
        
        XmlElement MaxExp = xmlDoc.CreateElement("MaxExp");
        MaxExp.InnerText = this.maxExp.ToString();
        InfoNode.AppendChild(MaxExp);
        
        XmlElement RemainHp = xmlDoc.CreateElement("RemainHp");
        RemainHp.InnerText = this.remainHp.ToString();
        InfoNode.AppendChild(RemainHp);

        XmlElement RemainMp = xmlDoc.CreateElement("RemainMp");
        RemainMp.InnerText = this.remainMp.ToString();
        InfoNode.AppendChild(RemainMp);

        XmlElement RemainExp = xmlDoc.CreateElement("RemainExp");
        RemainExp.InnerText = this.remainExp.ToString();
        InfoNode.AppendChild(RemainExp);

        XmlElement STR = xmlDoc.CreateElement("STR");
        STR.InnerText = this.str.ToString();
        InfoNode.AppendChild(STR);

        XmlElement DEX = xmlDoc.CreateElement("DEX");
        DEX.InnerText = this.dex.ToString();
        InfoNode.AppendChild(DEX);

        XmlElement SPD = xmlDoc.CreateElement("SPD");
        SPD.InnerText = this.spd.ToString();
        InfoNode.AppendChild(SPD);

        XmlElement SP = xmlDoc.CreateElement("SP");
        SP.InnerText = this.sp.ToString();
        InfoNode.AppendChild(SP);

        xmlDoc.Save(strPath);


    }

    //스텟 포인트 소모
    public void HeroStatUpGrade(string statName)
    {
        switch(statName)
        {
            case "Button_STR":
                {
                    this.str++;
                    Attack = 10 + this.str * this.Level;
                    break;
                }
            case "Button_DEX":
                {
                    this.dex++;
                    Defence = 10 + this.dex * this.Level;
                    break;
                }
            case "Button_SPD":
                {
                    this.spd++;
                    Speed = 10 + this.spd * this.Level;
                    break;
                }
        }

        this.sp--;
        HeroStatUpdate();
    }

    //스텟 업데이트
    public void HeroStatUpdate()
    {

        label_Stat_Level.text = this.Level.ToString();

        label_Stat_Hp.text = this.remainHp.ToString() + " / " + this.maxHp.ToString();


        label_Stat_Mp.text = this.remainMp.ToString() + " / " + this.maxMp.ToString();


        label_Stat_Exp.text = this.remainExp.ToString() + " / " + this.maxExp.ToString();

        label_Stat_STR.text = this.str.ToString();
        label_Stat_DEX.text = this.dex.ToString();
        label_Stat_SPD.text = this.spd.ToString();

        label_Stat_Attack.text = (this.Attack + this.buffAttack).ToString();
        label_Stat_Defence.text = (this.Defence + this.buffDefence).ToString();
        label_Stat_Speed.text = (this.Speed + this.buffSpeed).ToString();

        label_Stat_Sp.text = this.sp.ToString();
        if (this.sp > 0)
        {
            gb_StatPointButtonGroup.gameObject.SetActive(true);
        }
        else
        {
            gb_StatPointButtonGroup.gameObject.SetActive(false);
        }
    }


    //스탯 창 여닫기
    public void ActiveMyStatDialog()
    {
        MyStatDialog.gameObject.SetActive(!(MyStatDialog.gameObject.activeSelf));
        isOpenState = !isOpenState;
    }

    // 경험치 획득 및 레벨업
    public void GetExp(int amountExp)
    {
        this.remainExp += amountExp;
        //레벨업!
        if(this.remainExp>= this.maxExp)
        {
            this.Level++;
            this.remainExp -= this.maxExp;
            this.sp += 3;

            HeroScript.Instance.LevelUpEffect();
        }
        
        GameUI_Manager.Instance.UpDateExpBar();


    }

    //레벨업 시 스텟 상향
    public void StatLevelUp()
    {
        maxHp += 10;
        maxMp += 15;
        maxExp += 20;
    }

}
