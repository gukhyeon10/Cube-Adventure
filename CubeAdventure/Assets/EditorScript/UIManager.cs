using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Xml;

public class UIManager : MonoBehaviour{

    [SerializeField]
    GameObject CubePallete;
    [SerializeField]
    GameObject NaturePallete;
    [SerializeField]
    GameObject BuildPallete;
    [SerializeField]
    GameObject BridgePallete;
    [SerializeField]
    GameObject EnemyPallete;
    [SerializeField]
    GameObject PortalPallete;
    [SerializeField]
    GameObject NpcPallete;
    

    [SerializeField]
    UILabel label_Angle;

    [SerializeField]
    UILabel label_MapName;

    
    public UIInput input_PortalNumber;

    [SerializeField]
    GameObject gb_MapParentObject;
    [SerializeField]
    GameObject gb_PortalParentObject;
    [SerializeField]
    GameObject gb_NpcParentObject;
    [SerializeField]
    GameObject gb_EnemyParentObject;

    static private UIManager _instance = null;

    static public UIManager Instance
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

    void OnDestroy()
    {
        _instance = null;    
    }

    public void SetPallete(int palleteKind)     // 숫자키 입력으로 하단의 오브젝트 팔레트 활성화
    {
        CubePallete.gameObject.SetActive(false);
        NaturePallete.gameObject.SetActive(false);
        BuildPallete.gameObject.SetActive(false);
        BridgePallete.gameObject.SetActive(false);
        EnemyPallete.gameObject.SetActive(false);
        PortalPallete.gameObject.SetActive(false);
        NpcPallete.gameObject.SetActive(false);


        switch(palleteKind)
        {
            case (int)ObjectKind.CUBE:
                {
                    CubePallete.gameObject.SetActive(true);
                    break;
                }
            case (int)ObjectKind.NATURE:
                {
                    NaturePallete.gameObject.SetActive(true);
                    break;
                }
            case (int)ObjectKind.BUILD:
                {
                    BuildPallete.gameObject.SetActive(true);
                    break;
                }
            case (int)ObjectKind.BRIDGE:
                {
                    BridgePallete.gameObject.SetActive(true);
                    break;
                }
            case (int)ObjectKind.FURNITURE:
                {
                    break;
                }
            case (int)ObjectKind.ENEMY:
                {
                    EnemyPallete.gameObject.SetActive(true);
                    break;
                }
            case (int)ObjectKind.PORTAL:
                {
                    PortalPallete.gameObject.SetActive(true);
                    break;
                }
            case (int)ObjectKind.NPC:
                {
                    NpcPallete.gameObject.SetActive(true);
                    break;
                }
        }
    }

    public void AngleLabelUpdate(int angle)
    {

        label_Angle.text = "Object Angle : " + angle.ToString();
    }


    public void OpenLoadFile()     // 불러올 맵 파일 
    {
        string filePath = "";
#if UNITY_EDITOR
        filePath = EditorUtility.OpenFilePanel("Open Map File Dialog"
                                            , Application.dataPath
                                            , "xml");
#endif
        if (filePath.Length != 0)  // 파일 선택
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(filePath);

            foreach(Transform child in gb_MapParentObject.transform)  // 모든 오브젝트 삭제
            {
                Destroy(child.gameObject);
            }

            foreach(Transform child in gb_PortalParentObject.transform)
            {
                Destroy(child.gameObject);
            }

            foreach(Transform child in gb_NpcParentObject.transform)
            {
                Destroy(child.gameObject);
            }

            foreach(Transform child in gb_EnemyParentObject.transform)
            {
                Destroy(child.gameObject);
            }


            XmlNodeList nodeList = xmlDoc.SelectNodes("MapInfo/Object");

            foreach(XmlNode node in nodeList)
            {
                GameObject MapObject = Resources.Load("Object/" + node.SelectSingleNode("Name").InnerText) as GameObject;
                Vector3 position = new Vector3(float.Parse(node.SelectSingleNode("X").InnerText),
                                               float.Parse(node.SelectSingleNode("Y").InnerText),
                                               float.Parse(node.SelectSingleNode("Z").InnerText));
                Vector3 rotation = new Vector3(0, int.Parse(node.SelectSingleNode("R").InnerText), 0);

                GameObject newObject = Instantiate(MapObject, position, Quaternion.identity);

                newObject.transform.eulerAngles = rotation;
                newObject.transform.parent = gb_MapParentObject.transform;

            }

            XmlNodeList portalList = xmlDoc.SelectNodes("MapInfo/Portal");

            foreach(XmlNode node in portalList)
            {
                GameObject PortalObject = Resources.Load("Object/" + node.SelectSingleNode("Name").InnerText) as GameObject;
                Vector3 position = new Vector3(float.Parse(node.SelectSingleNode("X").InnerText),
                                               float.Parse(node.SelectSingleNode("Y").InnerText),
                                               float.Parse(node.SelectSingleNode("Z").InnerText));

                GameObject newPortal = Instantiate(PortalObject, position, Quaternion.identity);
                newPortal.transform.localEulerAngles = new Vector3(-90f, 0, 0);
                newPortal.GetComponent<PortalInfo>().LinkMapName = node.SelectSingleNode("LinkMapName").InnerText;
                newPortal.GetComponent<PortalInfo>().portalNumber = int.Parse(node.SelectSingleNode("PortalNumber").InnerText);
                newPortal.AddComponent<BoxCollider>();
                newPortal.GetComponent<BoxCollider>().center = Vector3.zero;
                newPortal.GetComponent<BoxCollider>().size = new Vector3(1.2f, 1.2f, 1.2f);

                newPortal.transform.parent = gb_PortalParentObject.transform;
            }
            
            XmlNodeList npcList = xmlDoc.SelectNodes("MapInfo/Npc");

            foreach(XmlNode node in npcList)
            {
                GameObject NpcObject = Resources.Load("Character/" + node.SelectSingleNode("Name").InnerText) as GameObject;
                Vector3 position = new Vector3(float.Parse(node.SelectSingleNode("X").InnerText),
                                               float.Parse(node.SelectSingleNode("Y").InnerText),
                                               float.Parse(node.SelectSingleNode("Z").InnerText));
                Vector3 rotation = new Vector3(0, int.Parse(node.SelectSingleNode("R").InnerText), 0);

                GameObject newNpc = Instantiate(NpcObject, position, Quaternion.identity);

                newNpc.transform.eulerAngles = rotation;

                newNpc.transform.parent = gb_NpcParentObject.transform;

                Destroy(newNpc.GetComponent<NpcDialog>());
            }


            XmlNodeList enemyList = xmlDoc.SelectNodes("MapInfo/Enemy");

            foreach(XmlNode node in enemyList)
            {
                GameObject EnemyObject = Resources.Load("Character/" + node.SelectSingleNode("Name").InnerText) as GameObject;
                Vector3 position = new Vector3(float.Parse(node.SelectSingleNode("X").InnerText),
                                               float.Parse(node.SelectSingleNode("Y").InnerText),
                                               float.Parse(node.SelectSingleNode("Z").InnerText));
                Vector3 rotation = new Vector3(0, int.Parse(node.SelectSingleNode("R").InnerText), 0);

                GameObject newEnemy = Instantiate(EnemyObject, position, Quaternion.identity);

                newEnemy.transform.eulerAngles = rotation;

                newEnemy.transform.parent = gb_EnemyParentObject.transform;
            }

            label_MapName.text = filePath.Substring(filePath.LastIndexOf("/") + 1);             // 우측 상단에 현재 맵 xml 파일의 이름 출력 
            label_MapName.text = label_MapName.text.Substring(0, label_MapName.text.Length - 4);

            Debug.Log("Map Xml File  Load Succes!");
            
        }

    }

    public void OpenSaveFile()    //저장할 맵 파일
    {
        string filePath = "";
#if UNITY_EDITOR
        filePath = EditorUtility.SaveFilePanel("Save Map File Dialog"
                                   , Application.dataPath
                                   , label_MapName.text
                                   , "xml");
#endif
        if (filePath.Length != 0)
        {

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.AppendChild(xmlDoc.CreateXmlDeclaration("1.0", "utf-8", "yes"));

            XmlNode root = xmlDoc.CreateNode(XmlNodeType.Element, "MapInfo", string.Empty);
            xmlDoc.AppendChild(root);


            for(int i=0; i<gb_MapParentObject.transform.childCount; i++)
            {
                XmlNode childNode = xmlDoc.CreateNode(XmlNodeType.Element, "Object", string.Empty);
                root.AppendChild(childNode);

                Transform mapChildObject = gb_MapParentObject.transform.GetChild(i);

                XmlElement name = xmlDoc.CreateElement("Name");
                name.InnerText = mapChildObject.name.Substring(0, mapChildObject.name.Length - 7);
                childNode.AppendChild(name);

                XmlElement x = xmlDoc.CreateElement("X");
                x.InnerText = mapChildObject.position.x.ToString();
                childNode.AppendChild(x);

                XmlElement y = xmlDoc.CreateElement("Y");
                y.InnerText = mapChildObject.position.y.ToString();
                childNode.AppendChild(y);

                XmlElement z = xmlDoc.CreateElement("Z");
                z.InnerText = mapChildObject.position.z.ToString();
                childNode.AppendChild(z);

                XmlElement rotation = xmlDoc.CreateElement("R");
                rotation.InnerText = mapChildObject.eulerAngles.y.ToString();
                childNode.AppendChild(rotation);
                
            }

            for(int i= 0; i<gb_PortalParentObject.transform.childCount; i++)
            {
                XmlNode childNode = xmlDoc.CreateNode(XmlNodeType.Element, "Portal", string.Empty);
                root.AppendChild(childNode);

                Transform portalObject = gb_PortalParentObject.transform.GetChild(i);

                XmlElement name = xmlDoc.CreateElement("Name");
                name.InnerText = portalObject.name.Substring(0, portalObject.name.Length - 7);
                childNode.AppendChild(name);

                XmlElement x = xmlDoc.CreateElement("X");
                x.InnerText = portalObject.position.x.ToString();
                childNode.AppendChild(x);

                XmlElement y = xmlDoc.CreateElement("Y");
                y.InnerText = portalObject.position.y.ToString();
                childNode.AppendChild(y);

                XmlElement z = xmlDoc.CreateElement("Z");
                z.InnerText = portalObject.position.z.ToString();
                childNode.AppendChild(z);

                XmlElement linkMapName = xmlDoc.CreateElement("LinkMapName");
                linkMapName.InnerText = portalObject.GetComponent<PortalInfo>().LinkMapName;
                childNode.AppendChild(linkMapName);

                XmlElement portalNumber = xmlDoc.CreateElement("PortalNumber");
                portalNumber.InnerText = portalObject.GetComponent<PortalInfo>().portalNumber.ToString();
                childNode.AppendChild(portalNumber);

            }

            for(int i = 0; i<gb_NpcParentObject.transform.childCount; i++)
            {
                XmlNode childNode = xmlDoc.CreateNode(XmlNodeType.Element, "Npc", string.Empty);
                root.AppendChild(childNode);

                Transform NpcObject = gb_NpcParentObject.transform.GetChild(i);

                XmlElement name = xmlDoc.CreateElement("Name");
                name.InnerText = NpcObject.name.Substring(0, NpcObject.name.Length - 7);
                childNode.AppendChild(name);

                XmlElement x = xmlDoc.CreateElement("X");
                x.InnerText = NpcObject.position.x.ToString();
                childNode.AppendChild(x);

                XmlElement y = xmlDoc.CreateElement("Y");
                y.InnerText = NpcObject.position.y.ToString();
                childNode.AppendChild(y);

                XmlElement z = xmlDoc.CreateElement("Z");
                z.InnerText = NpcObject.position.z.ToString();
                childNode.AppendChild(z);

                XmlElement rotation = xmlDoc.CreateElement("R");
                rotation.InnerText = NpcObject.eulerAngles.y.ToString();
                childNode.AppendChild(rotation);

            }

            for(int i = 0; i<gb_EnemyParentObject.transform.childCount; i++)
            {
                XmlNode childNode = xmlDoc.CreateNode(XmlNodeType.Element, "Enemy", string.Empty);
                root.AppendChild(childNode);

                Transform EnemyObject = gb_EnemyParentObject.transform.GetChild(i);

                XmlElement name = xmlDoc.CreateElement("Name");
                name.InnerText = EnemyObject.name.Substring(0, EnemyObject.name.Length - 7);
                childNode.AppendChild(name);

                XmlElement x = xmlDoc.CreateElement("X");
                x.InnerText = EnemyObject.position.x.ToString();
                childNode.AppendChild(x);

                XmlElement y = xmlDoc.CreateElement("Y");
                y.InnerText = EnemyObject.position.y.ToString();
                childNode.AppendChild(y);

                XmlElement z = xmlDoc.CreateElement("Z");
                z.InnerText = EnemyObject.position.z.ToString();
                childNode.AppendChild(z);

                XmlElement rotation = xmlDoc.CreateElement("R");
                rotation.InnerText = EnemyObject.eulerAngles.y.ToString();
                childNode.AppendChild(rotation);
            }

            xmlDoc.Save(filePath);
            Debug.Log("Map Xml File  Save Succes!");

        }
    }
}
