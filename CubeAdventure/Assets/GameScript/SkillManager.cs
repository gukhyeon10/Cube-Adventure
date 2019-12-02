using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.IO;

public class SkillManager : MonoBehaviour {

    public bool isOpenSkill = false;

    [SerializeField]
    GameObject NpcSkillDialog;
    [SerializeField]
    GameObject MySkillDialog;

    [SerializeField]
    UILabel label_RemainSkillPoint;

    [SerializeField]
    UIGrid NpcSkillGrid;
    [SerializeField]
    UIGrid MySkillGrid;

    [SerializeField]
    GameObject NpcSkillNode;
    [SerializeField]
    GameObject SkillBlank;
    [SerializeField]
    GameObject LearnSkillNode;

    [SerializeField]
    UIGrid LearnSkillField;

    public int skillPoint;
    public Dictionary<int, int> dic_LearnSkill = new Dictionary<int, int>();      // 키 값 : 스킬 번호   벨류 값 : 스킬 레벨  (배운 스킬 목록)
    public Dictionary<int, SkillData> dic_SkillData = new Dictionary<int, SkillData>();    // 키 값 : 스킬 번호    벨류 값 : 스킬 데이터 (스킬 정의)

    SkillEffect SkillEffectManager;
    StatManager HeroStat;

    static private SkillManager _instance = null;

    static public SkillManager Instance
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

        SkillDefineLoad();
        HeroSkillUpLoad();

        for (int i=0; i<25; i++)
        {
            GameObject newNode = NGUITools.AddChild(MySkillGrid.gameObject, SkillBlank);
            
        }
        MySkillGrid.Reposition();

        this.SkillEffectManager = this.GetComponent<SkillEffect>();
        HeroStat = StatManager.Instance;
	}

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.K))
        {
            SkillDialogActive();
        }
    }

    //스킬 창 활성화

    public void SkillDialogActive()
    {
        NpcSkillDialog.gameObject.SetActive(!(NpcSkillDialog.gameObject.activeSelf));
        MySkillDialog.gameObject.SetActive(!(MySkillDialog.gameObject.activeSelf));
        isOpenSkill = !isOpenSkill;
    }

    // 스킬 습득

    public void LearnSkill(string skillName)
    {
        int skillNo = 0;

        skillName = skillName.Substring(("Skill_").Length);
        if(skillName[0].Equals("0"))
        {
            skillNo = int.Parse(skillName.Substring(1));
        }
        else
        {
            skillNo = int.Parse(skillName);
        }
        
        if(dic_LearnSkill.ContainsKey(skillNo)) // 배운 스킬이라면 레벨업!
        {
            if(dic_LearnSkill[skillNo] < 9)
            {
                dic_LearnSkill[skillNo]++;
                Debug.Log("skill_ " + skillName + " 을 배웠습니다!");
            }
            else
            {
                Debug.Log("스킬의 레벨은 9까지 올릴 수 있습니다.");
            }
        }
        else
        {
            dic_LearnSkill.Add(skillNo, 1);    // 새로운 스킬 1레벨 추가!
        }

        // 모든 스킬 삭제 : 다시 재정립? 하기 위해서
        NGUITools.DestroyChildren(LearnSkillField.transform);

        foreach (KeyValuePair<int, int> learnSkill in dic_LearnSkill)  // 배운 스킬 Grid_LearnSkillField 에 추가하기
        {
            GameObject newSkillNode = NGUITools.AddChild(LearnSkillField.gameObject, LearnSkillNode);

            newSkillNode.GetComponent<UISprite>().spriteName = "Skill_" + learnSkill.Key.ToString("D2");
            newSkillNode.GetComponent<UISprite>().depth++;

            newSkillNode.GetComponentInChildren<UILabel>().depth++;
            newSkillNode.GetComponentInChildren<UILabel>().text = "LV." + learnSkill.Value.ToString();
        }
        LearnSkillField.Reposition();
    }

    //캐릭터 스킬 로드
    void HeroSkillUpLoad()
    {
        string strPath = string.Empty;
#if (UNITY_EDITOR || UNITY_STANDALONE_WIN)
        strPath += (Application.streamingAssetsPath + "/Hero/HeroSkill.xml");
#elif UNITY_ANDROID
        strPath = "jar:file://" + Application.dataPath + "!/assets" + "/Hero/HeroSkill.xml";
#endif

        XmlDocument xmlDoc = new XmlDocument();
        if (File.Exists(strPath))
        {
            //해당 경로에 스킬 세이브 파일 있으면
             xmlDoc.Load(strPath);
        }
        else
        {
            //해당 경로에 파일없어서 초기화 파일
            TextAsset textAsset = (TextAsset)Resources.Load("Hero/HeroSkill");

            xmlDoc.LoadXml(textAsset.text);
        }

        XmlNode HeroSkillPoint = xmlDoc.SelectSingleNode("Hero/SkillPoint");
        this.skillPoint = int.Parse(HeroSkillPoint.SelectSingleNode("Point").InnerText);
        label_RemainSkillPoint.text = "SKILL POINT : " + this.skillPoint.ToString();

        XmlNodeList HeroSkillList = xmlDoc.SelectNodes("Hero/Skill");  // 습득한 스킬

        foreach (XmlNode HeroSkill in HeroSkillList)
        {
            int skillNo = int.Parse(HeroSkill.SelectSingleNode("No").InnerText);
            int skillLevel = int.Parse(HeroSkill.SelectSingleNode("Level").InnerText);

            dic_LearnSkill.Add(skillNo, skillLevel);

        }

        foreach (KeyValuePair<int, int> learnSkill in dic_LearnSkill)  // 배운 스킬 Grid_LearnSkill 에 추가하기
        {
            GameObject newSkillNode = NGUITools.AddChild(LearnSkillField.gameObject, LearnSkillNode);

            newSkillNode.GetComponent<UISprite>().spriteName = "Skill_" + learnSkill.Key.ToString("D2");
            newSkillNode.GetComponent<UISprite>().depth++;

            newSkillNode.GetComponentInChildren<UILabel>().depth++;
            newSkillNode.GetComponentInChildren<UILabel>().text = "LV." + learnSkill.Value.ToString();
        }


        //단축기 스킬 로드 하고 활성화
        XmlNodeList ShortCutSkillList = xmlDoc.SelectNodes("Hero/ShortCut");

        foreach(XmlNode ShortCutSkill in ShortCutSkillList)
        {
            string skillNo = ShortCutSkill.SelectSingleNode("No").InnerText;
            int shortCutIndex = int.Parse(ShortCutSkill.SelectSingleNode("Index").InnerText);

            AttackScript.Instance.SkillButtonGroup[shortCutIndex].GetComponent<UISprite>().spriteName = "Skill_" + skillNo;
            AttackScript.Instance.SkillButtonGroup[shortCutIndex].GetComponent<ClickSkill>().skillSpriteName = "Skill_"+skillNo;
            AttackScript.Instance.SkillButtonGroup[shortCutIndex].gameObject.SetActive(true);
        }


        Debug.Log("캐릭터의 습득한 스킬 로드 성공!");
    }

    //캐릭터 스킬 세이브
    public void HeroSkillSave()
    {
        string strPath = string.Empty;
#if (UNITY_EDITOR || UNITY_STANDALONE_WIN)
        strPath += (Application.streamingAssetsPath + "/Hero/HeroSkill.xml");
#elif UNITY_ANDROID
        strPath = "jar:file://" + Application.dataPath + "!/assets" + "/Hero/HeroSkill.xml";
#endif

        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.AppendChild(xmlDoc.CreateXmlDeclaration("1.0", "utf-8", "yes"));

        //루트 노드 생성
        XmlNode root = xmlDoc.CreateNode(XmlNodeType.Element, "Hero", string.Empty);
        xmlDoc.AppendChild(root);

        //스킬포인트 노드 생성
        XmlNode SkillPointNode = xmlDoc.CreateNode(XmlNodeType.Element, "SkillPoint", string.Empty);
        root.AppendChild(SkillPointNode);

        //캐릭터 능력치 속성 설정
        XmlElement SkillPoint = xmlDoc.CreateElement("Point");
        SkillPoint.InnerText = this.skillPoint.ToString();
        SkillPointNode.AppendChild(SkillPoint);


        foreach (KeyValuePair<int, int> learnSkill in dic_LearnSkill)  // 배운 스킬 노드에 저장하기
        {
            //스킬 노드 생성
            XmlNode SkillNode = xmlDoc.CreateNode(XmlNodeType.Element, "Skill", string.Empty);
            root.AppendChild(SkillNode);

            XmlElement SkillNo = xmlDoc.CreateElement("No");
            SkillNo.InnerText = learnSkill.Key.ToString();
            SkillNode.AppendChild(SkillNo);

            XmlElement SkillLevel = xmlDoc.CreateElement("Level");
            SkillLevel.InnerText = learnSkill.Value.ToString();
            SkillNode.AppendChild(SkillLevel);
        }

        GameObject[] SkillButtonList= AttackScript.Instance.SkillButtonGroup;
        //단축키 설정 스킬 목록
        for(int i=0; i< SkillButtonList.Length; i++)
        {
            //스킬이 활성화 되어있다면
            if(SkillButtonList[i].activeSelf)
            {
                XmlNode ShortCutSkillNode = xmlDoc.CreateNode(XmlNodeType.Element, "ShortCut", string.Empty);
                root.AppendChild(ShortCutSkillNode);

                XmlElement SkillNo = xmlDoc.CreateElement("No");
                SkillNo.InnerText = SkillButtonList[i].GetComponent<UISprite>().spriteName.Substring(6);
                ShortCutSkillNode.AppendChild(SkillNo);

                XmlElement ShortCutIndex = xmlDoc.CreateElement("Index");
                ShortCutIndex.InnerText = i.ToString();
                ShortCutSkillNode.AppendChild(ShortCutIndex);
            }
        }
        
        xmlDoc.Save(strPath);
    }

    //스킬 디파인 로드
    void SkillDefineLoad()
    {

        TextAsset textAsset = (TextAsset)Resources.Load("Skill/SkillDefine");

        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(textAsset.text);

        XmlNodeList SkillList = xmlDoc.SelectNodes("Skill/Info");    // 스킬 정보

        foreach(XmlNode Skill in SkillList)
        {

            int skillNumber = int.Parse(Skill.SelectSingleNode("Number").InnerText);
            SkillData skillData;
            skillData.skillName = Skill.SelectSingleNode("Name").InnerText;
            skillData.consumMp = int.Parse(Skill.SelectSingleNode("Mp").InnerText);
            skillData.coolTime = float.Parse(Skill.SelectSingleNode("CoolTime").InnerText);
            skillData.percent = int.Parse(Skill.SelectSingleNode("Percent").InnerText);

            GameObject newNode = NGUITools.AddChild(NpcSkillGrid.gameObject, NpcSkillNode);   //Npc 스킬 배우기 다이얼로그 초기화

            newNode.transform.GetChild(0).GetComponent<UISprite>().spriteName = "Skill_" + skillNumber.ToString("D2");
            newNode.transform.GetChild(1).GetComponent<UILabel>().text = skillData.skillName;

            dic_SkillData.Add(skillNumber, skillData);
        }


        NpcSkillGrid.Reposition();

        Debug.Log("스킬 정보 로드 성공");
    }

    //스킬 시전
    public void PlaySkill(string skillName)
    {

        int skillNo = 0;

        skillName = skillName.Substring(("Skill_").Length);
        if (skillName[0].Equals("0"))
        {
            skillNo = int.Parse(skillName.Substring(1));
        }
        else
        {
            skillNo = int.Parse(skillName);
        }

        if(HeroStat.remainMp >=dic_SkillData[skillNo].consumMp)
        {
            if(SkillEffectManager.SkillEffectQuarter(skillNo))   // 스킬 쿨타임이 채워졌으면 True 리턴
            {
                HeroStat.remainMp -= dic_SkillData[skillNo].consumMp;
                GameUI_Manager.Instance.UpdateMpBar();
            }
        }
        else
        {
            Debug.Log("마나가 부족합니다.");
        }

    }

    public void PlayHitEffect(string hitEffectName, Vector3 hitPosition)
    {
        SkillEffectManager.PlayHitEffect(hitEffectName, hitPosition);
    }

    public void ClearSkillEffect()
    {
        SkillEffectManager.ClearSkillParent();
    }

    public SkillEffect GetSkillEffectIns
    {
        get
        {
            return this.SkillEffectManager;
        }
    }


}
