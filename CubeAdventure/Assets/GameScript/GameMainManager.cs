using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

public class GameMainManager : MonoBehaviour {
    [SerializeField]
    GameObject gb_CubeMap;
    [SerializeField]
    GameObject gb_EnemyParent;

    List<GameObject> list_RecallPortal = new List<GameObject>();

    string nowMapName = "";
    string previousMapName = "";
    Vector3 heroInitPosition = new Vector3(0f, 1f, 0f);

    static private GameMainManager _instance = null;

    static public GameMainManager Instance
    {
        get
        {
            return _instance;
        }
    }

    void Awake()
    {
        _instance = this;
        previousMapName = "StartMap";
        LoadMap("StartMap");
    }

    void OnDestroy()
    {
        _instance = null;
    }

    public void PreviousLoadMap()
    {
        LoadMap(previousMapName);
    }

    //맵 로딩
    public void LoadMap(string mapName)
    {
        if (mapName.Length <= 0)
        {
            Debug.Log("맵 파일이 올바르지않음.");
            return;
        }

        ClearEnemy();

        if (list_RecallPortal.Count > 0)
        {
            list_RecallPortal.Clear();
        }

        foreach (Transform child in gb_CubeMap.transform)
        {
            Destroy(child.gameObject);
        }

        

        TextAsset textAsset = (TextAsset)Resources.Load("Map/" + mapName);

        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(textAsset.text);

        XmlNodeList Objects = xmlDoc.SelectNodes("MapInfo/Object");

        foreach (XmlNode Object in Objects)
        {
            GameObject MapObject = Resources.Load("Object/" + Object.SelectSingleNode("Name").InnerText) as GameObject;
            Vector3 position = new Vector3(float.Parse(Object.SelectSingleNode("X").InnerText),
                                           float.Parse(Object.SelectSingleNode("Y").InnerText),
                                           float.Parse(Object.SelectSingleNode("Z").InnerText));
            Vector3 rotation = new Vector3(0, int.Parse(Object.SelectSingleNode("R").InnerText), 0);

            GameObject newObject = Instantiate(MapObject, position, Quaternion.identity);

            newObject.transform.eulerAngles = rotation;
            newObject.transform.parent = gb_CubeMap.transform;

            switch (Object.SelectSingleNode("Name").InnerText)
            {
                case "ForestGrass01":
                    {
                        Destroy(newObject.GetComponent<BoxCollider>());
                        break;
                    }
                case "ForestGrass02":
                    {
                        Destroy(newObject.GetComponent<BoxCollider>());
                        break;
                    }
                default:
                    {
                        break;
                    }
            }

        }

        XmlNodeList Portals = xmlDoc.SelectNodes("MapInfo/Portal");   // 포탈 로드

        foreach (XmlNode Portal in Portals)
        {
            GameObject PortalObject = Resources.Load("Object/" + Portal.SelectSingleNode("Name").InnerText) as GameObject;
            Vector3 position = new Vector3(float.Parse(Portal.SelectSingleNode("X").InnerText),
                                           float.Parse(Portal.SelectSingleNode("Y").InnerText),
                                           float.Parse(Portal.SelectSingleNode("Z").InnerText));

            GameObject newPortal = Instantiate(PortalObject, position, Quaternion.identity);
            newPortal.transform.eulerAngles = new Vector3(-90f, 0f, 0f);

            newPortal.GetComponent<PortalInfo>().LinkMapName = Portal.SelectSingleNode("LinkMapName").InnerText;
            newPortal.GetComponent<PortalInfo>().portalNumber = int.Parse(Portal.SelectSingleNode("PortalNumber").InnerText);

            newPortal.AddComponent<BoxCollider>();
            newPortal.GetComponent<BoxCollider>().center = Vector3.zero;
            newPortal.GetComponent<BoxCollider>().size = new Vector3(1.2f, 1.2f, 1.2f);
            newPortal.GetComponent<BoxCollider>().isTrigger = true;

            newPortal.transform.tag = "Portal";

            newPortal.transform.parent = gb_CubeMap.transform;

            if (newPortal.name.Substring(0, 4).Equals("Blue"))
            {
                list_RecallPortal.Insert(0, newPortal);
            }


        }

        XmlNodeList Npcs = xmlDoc.SelectNodes("MapInfo/Npc");   // Npc 로드

        foreach (XmlNode Npc in Npcs)
        {
            GameObject NpcObject = Resources.Load("Character/" + Npc.SelectSingleNode("Name").InnerText) as GameObject;
            Vector3 position = new Vector3(float.Parse(Npc.SelectSingleNode("X").InnerText),
                                           float.Parse(Npc.SelectSingleNode("Y").InnerText),
                                           float.Parse(Npc.SelectSingleNode("Z").InnerText));

            Vector3 rotation = new Vector3(0, int.Parse(Npc.SelectSingleNode("R").InnerText), 0);


            GameObject newNpc = Instantiate(NpcObject, position, Quaternion.identity);

            newNpc.transform.eulerAngles = rotation;
            newNpc.transform.parent = gb_CubeMap.transform;

            newNpc.transform.tag = "Npc";
        }

        XmlNodeList Enemys = xmlDoc.SelectNodes("MapInfo/Enemy");  // Enemy 로드

        foreach (XmlNode Enemy in Enemys)
        {
            GameObject EnemyObject = Resources.Load("Character/" + Enemy.SelectSingleNode("Name").InnerText) as GameObject;
            Vector3 position = new Vector3(float.Parse(Enemy.SelectSingleNode("X").InnerText),
                                           float.Parse(Enemy.SelectSingleNode("Y").InnerText),
                                           float.Parse(Enemy.SelectSingleNode("Z").InnerText));


            Vector3 rotation = new Vector3(0, int.Parse(Enemy.SelectSingleNode("R").InnerText), 0);

            GameObject newEnemy = Instantiate(EnemyObject, position, Quaternion.identity);

            newEnemy.transform.eulerAngles = rotation;
            newEnemy.transform.parent = gb_EnemyParent.transform;

            newEnemy.transform.tag = "Enemy";

            newEnemy.gameObject.AddComponent<EnemyScript>();

        }

        nowMapName = mapName;
        if (Enemys.Count == 0)
        {
            previousMapName = nowMapName;
        }
        
    }


    //타임어택 전용 맵
    public void LoadTimeAttackRoom()
    {
        //타임어택 NPC에게 말을 건 그자리가 초기위치
        heroInitPosition = HeroScript.Instance.transform.position;


        if (list_RecallPortal.Count > 0)
        {
            list_RecallPortal.Clear();
        }

        SkillManager.Instance.ClearSkillEffect();

        foreach (Transform child in gb_CubeMap.transform)
        {
            Destroy(child.gameObject);
        }

        TextAsset textAsset = (TextAsset)Resources.Load("Map/TimeAttack");

        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(textAsset.text);

        XmlNodeList Objects = xmlDoc.SelectNodes("MapInfo/Object");

        foreach (XmlNode Object in Objects)
        {
            GameObject MapObject = Resources.Load("Object/" + Object.SelectSingleNode("Name").InnerText) as GameObject;
            Vector3 position = new Vector3(float.Parse(Object.SelectSingleNode("X").InnerText),
                                           float.Parse(Object.SelectSingleNode("Y").InnerText),
                                           float.Parse(Object.SelectSingleNode("Z").InnerText));
            Vector3 rotation = new Vector3(0, int.Parse(Object.SelectSingleNode("R").InnerText), 0);

            GameObject newObject = Instantiate(MapObject, position, Quaternion.identity);

            newObject.transform.eulerAngles = rotation;
            newObject.transform.parent = gb_CubeMap.transform;

            switch (Object.SelectSingleNode("Name").InnerText)
            {
                case "ForestGrass01":
                    {
                        Destroy(newObject.GetComponent<BoxCollider>());
                        break;
                    }
                case "ForestGrass02":
                    {
                        Destroy(newObject.GetComponent<BoxCollider>());
                        break;
                    }
                default:
                    {
                        break;
                    }
            }

        }

        
        for(int i = 0; i<15; i++)
        {
            GameObject EnemyObject = Resources.Load("Character/Skeleton") as GameObject;

            Vector3 Position = Vector3.zero;
            Position.y = 1f;

            //-6 ~ 6 범위내로 소환
            Position.x = Random.Range(-6, 6);   
            Position.z = Random.Range(-6, 6);

            bool isOverlap = false;
         
                if(!isOverlap)
                {

                    GameObject newEnemy = Instantiate(EnemyObject, Position, Quaternion.identity);

                    newEnemy.transform.parent = gb_EnemyParent.transform;

                    newEnemy.transform.tag = "Enemy";

                    newEnemy.gameObject.AddComponent<EnemyScript>();

                    newEnemy.GetComponent<EnemyScript>().initPosition = Position;
                
                }
            

        }
        

        GameUI_Manager.Instance.InitHudPanel();
            
        //타임어택 시작!
        StartCoroutine(TimeAttackLife());
    }

    IEnumerator TimeAttackLife()
    {
        HeroScript.Instance.timeAttackKillCount = 0;
        HeroScript.Instance.timeAttackHit = 0;
        HeroScript.Instance.timeAttackDemage = 0;

        GameUI_Manager.Instance.gb_TimeAttackPanel.gameObject.SetActive(true);
        GameUI_Manager.Instance.TimeAttackKillCount();

        bool isClear = true;
        float time = 30f;
        while(time > 0f)
        {
            GameUI_Manager.Instance.UpdateTimeAttack(time);
            time -= Time.deltaTime;

            if(StatManager.Instance.remainHp <= 0)
            {
                time = 0f;
                isClear = false;
                
                HeroScript.Instance.DeathAnimation();
                yield return new WaitForSeconds(2f);
            }

            yield return null;
        }
        time = 0f;
        GameUI_Manager.Instance.UpdateTimeAttack(time);
        
        Debug.Log("Time Attack TimeOver!");
        GameUI_Manager.Instance.gb_TimeAttackPanel.gameObject.SetActive(false);
        HeroScript.Instance.timeAttackKillCount = 0;

        GameUI_Manager.Instance.InitHudPanel();

        LoadMap(nowMapName);

        // 타임 어택 결과 집계
        GameUI_Manager.Instance.TimeAttackResult(isClear);

        //초기 위치
        HeroScript.Instance.transform.position = heroInitPosition;

        yield return new WaitForSeconds(1f);

        StatManager.Instance.remainHp = StatManager.Instance.maxHp / 2;
        StatManager.Instance.remainMp = StatManager.Instance.maxMp / 2;

        HeroScript.Instance.isTimeAttackMode = false;
      
        HeroScript.Instance.Resurrection();
    }

    //보스 레이드 맵 로드
    public void LoadBossRaidMap()
    {
        //타임어택 NPC에게 말을 건 그자리가 초기위치
        heroInitPosition = HeroScript.Instance.transform.position;

        if (list_RecallPortal.Count > 0)
        {
            list_RecallPortal.Clear();
        }

        //스킬 이펙트 모두 지우기
        SkillManager.Instance.ClearSkillEffect();

        foreach (Transform child in gb_CubeMap.transform)
        {
            Destroy(child.gameObject);
        }

        TextAsset textAsset = (TextAsset)Resources.Load("Map/BossRaid");

        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(textAsset.text);

        XmlNodeList Objects = xmlDoc.SelectNodes("MapInfo/Object");

        foreach (XmlNode Object in Objects)
        {
            GameObject MapObject = Resources.Load("Object/" + Object.SelectSingleNode("Name").InnerText) as GameObject;
            Vector3 position = new Vector3(float.Parse(Object.SelectSingleNode("X").InnerText),
                                           float.Parse(Object.SelectSingleNode("Y").InnerText),
                                           float.Parse(Object.SelectSingleNode("Z").InnerText));
            Vector3 rotation = new Vector3(0, int.Parse(Object.SelectSingleNode("R").InnerText), 0);

            GameObject newObject = Instantiate(MapObject, position, Quaternion.identity);

            newObject.transform.eulerAngles = rotation;
            newObject.transform.parent = gb_CubeMap.transform;

            switch (Object.SelectSingleNode("Name").InnerText)
            {
                case "ForestGrass01":
                    {
                        Destroy(newObject.GetComponent<BoxCollider>());
                        break;
                    }
                case "ForestGrass02":
                    {
                        Destroy(newObject.GetComponent<BoxCollider>());
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }

        GameObject EnemyObject = Resources.Load("Character/BossSkeleton") as GameObject;

        Vector3 Position = Vector3.zero;
        Position.y = 1f;
        Position.x = -0.9f;
        Position.z = 3;

      
                GameObject newEnemy = Instantiate(EnemyObject, Position, Quaternion.identity);

                newEnemy.transform.parent = gb_EnemyParent.transform;

                newEnemy.transform.tag = "Boss";

        newEnemy.AddComponent<BossScript>();

        newEnemy.transform.localScale = Vector3.one * 3f;


        XmlNodeList Portals = xmlDoc.SelectNodes("MapInfo/Portal");   // 포탈 로드

        foreach (XmlNode Portal in Portals)
        {
            GameObject PortalObject = Resources.Load("Object/" + Portal.SelectSingleNode("Name").InnerText) as GameObject;
            Vector3 position = new Vector3(float.Parse(Portal.SelectSingleNode("X").InnerText),
                                           float.Parse(Portal.SelectSingleNode("Y").InnerText),
                                           float.Parse(Portal.SelectSingleNode("Z").InnerText));

            GameObject newPortal = Instantiate(PortalObject, position, Quaternion.identity);
            newPortal.transform.eulerAngles = new Vector3(-90f, 0f, 0f);

            newPortal.GetComponent<PortalInfo>().LinkMapName = Portal.SelectSingleNode("LinkMapName").InnerText;
            newPortal.GetComponent<PortalInfo>().portalNumber = int.Parse(Portal.SelectSingleNode("PortalNumber").InnerText);

            newPortal.AddComponent<BoxCollider>();
            newPortal.GetComponent<BoxCollider>().center = Vector3.zero;
            newPortal.GetComponent<BoxCollider>().size = new Vector3(1.2f, 1.2f, 1.2f);
            newPortal.GetComponent<BoxCollider>().isTrigger = true;

            newPortal.transform.tag = "Portal";

            newPortal.transform.parent = gb_CubeMap.transform;

            if (newPortal.name.Substring(0, 4).Equals("Blue"))
            {
                list_RecallPortal.Insert(0, newPortal);
            }


        }

        GameUI_Manager.Instance.InitHudPanel();


        //보스 BGM
        SoundManager.Instance.ChangeBgm(1);

        HeroScript.Instance.transform.position = new Vector3(0f, 1f, -8f);
    }

    // 캐릭터 데이터 세이브
    public void GameDataSave()
    {
        StatManager.Instance.HeroDataSave();
        SkillManager.Instance.HeroSkillSave();
        InvenManager.Instance.HeroInvenSave();
    }

    public void ClearEnemy()
    {
        if(gb_EnemyParent.transform.childCount > 0)
        {
            NGUITools.DestroyChildren(gb_EnemyParent.transform);
        }
    }


    // 캐릭터 초기 포인트 지점
    public Vector3 HeroRecall(int recallPortalNumber)
    {
        if(list_RecallPortal.Count > 0)
        {
            foreach(GameObject portal in list_RecallPortal)
            {
                if(portal.GetComponent<PortalInfo>().portalNumber == recallPortalNumber)
                {
                    heroInitPosition = portal.transform.position;

                    Debug.Log(nowMapName + " 로딩 성공!");
                    return portal.transform.position;
                }
            }

            Debug.Log("링크된 포인트 지점인 포탈이 없습니다. " + "해당 맵 이름 : " + nowMapName + "    공백인 포탈 번호 : " + recallPortalNumber.ToString());
            LoadMap(previousMapName);
            return heroInitPosition;
        }
        else
        {
            Debug.Log("초기 포인트 지점인 포탈이 없습니다.   해당 맵 이름 : " + nowMapName);
            LoadMap(previousMapName);
            return heroInitPosition;
        }
    }

    public Vector3 HeroComeBack()
    {
        if (list_RecallPortal.Count > 0)
        {
            foreach (GameObject portal in list_RecallPortal)
            {
                if(portal.transform.name.Equals("Blue_Portal(Clone)"))
                {
                    return portal.transform.position;
                }
            }

        }
        else
        {
            return new Vector3(2f, 1f, -6f);
        }

        return new Vector3(2f, 1f, -6f);
    }

    public void FailSpecialModeRespon()
    {
        HeroScript.Instance.transform.position = heroInitPosition;
    }
}
