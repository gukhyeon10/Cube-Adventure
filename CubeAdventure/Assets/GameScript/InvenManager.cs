using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.IO;

public class InvenManager : MonoBehaviour {

    public bool isOpenInventory = false;
    public int money;
    public int inventoryLimit = 0;
    public Dictionary<int, Equip> dic_Equip = new Dictionary<int, Equip>();    // 장비 아이템 정보 정의
    public Dictionary<int, Recovery> dic_Recovery = new Dictionary<int, Recovery>();  // 회복 아이템 정보 정의
    public Dictionary<int, Other> dic_Other = new Dictionary<int, Other>();   // 기타 아이템 정보 정의

    //키값이 인벤토리 인덱스
    public Dictionary<int, MyItem> dic_Inventory = new Dictionary<int, MyItem>();   // 나의 인벤토리에 있는 아이템
    public Dictionary<int, Equip> dic_Wearing = new Dictionary<int, Equip>();  // 입고 있는 장비 아이템

    static private InvenManager _instance = null;

    static public InvenManager Instance
    {
        get
        {
            return _instance;
        }
    }

    [SerializeField]
    GameObject gb_Inventory;
    [SerializeField]
    UIGrid grid_InventoryBlank;
    [SerializeField]
    UIGrid grid_StoreInventoryBlank;
    [SerializeField]
    GameObject gb_EquipBlank;
    [SerializeField]
    GameObject gb_ItemField;
    [SerializeField]
    GameObject gb_StoreItemField;
    [SerializeField]
    GameObject gb_EquipField;

    [SerializeField]
    GameObject prefab_EquipItem;

    void Awake()
    {
        _instance = this;
    }

    // Use this for initialization
    void Start () {
        ItemDefineLoad();
        HeroInvenUpLoad();

        inventoryLimit = grid_InventoryBlank.transform.childCount;
	}

    void Update()
    {
        OpenInventory();
    }

    //캐릭터 인벤토리 업로드
    void HeroInvenUpLoad()
    {
        string strPath = string.Empty;
#if (UNITY_EDITOR || UNITY_STANDALONE_WIN)
        strPath += (Application.streamingAssetsPath + "/Hero/HeroInven.xml");
#elif UNITY_ANDROID
        strPath = "jar:file://" + Application.dataPath + "!/assets" + "/Hero/HeroInven.xml";
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
            TextAsset textAsset = (TextAsset)Resources.Load("Hero/HeroInven");

            xmlDoc.LoadXml(textAsset.text);
        }

        XmlNode HeroInven = xmlDoc.SelectSingleNode("Hero/Info");

        this.money = int.Parse(HeroInven.SelectSingleNode("Money").InnerText);

        XmlNodeList ItemList = xmlDoc.SelectNodes("Hero/Item");   // 인벤토리 아이템 리스트

        foreach (XmlNode Item in ItemList)
        {
            MyItem item;
            item.itemCode = int.Parse(Item.SelectSingleNode("Code").InnerText);
            item.itemKind = int.Parse(Item.SelectSingleNode("Kind").InnerText);
            item.itemCount = int.Parse(Item.SelectSingleNode("Count").InnerText);
            item.InvenIndex = int.Parse(Item.SelectSingleNode("Index").InnerText);

            dic_Inventory.Add(item.InvenIndex, item);

            ProduceItem(item);
        }

        XmlNodeList WearingList = xmlDoc.SelectNodes("Hero/Wear");   // 착용중인 아이템 리스트

        foreach (XmlNode Wear in WearingList)
        {
            Equip wearItem;
            int codeNumber = int.Parse(Wear.SelectSingleNode("Code").InnerText);

            Equip wearInfo = dic_Equip[codeNumber];

            wearItem = wearInfo;

            dic_Wearing.Add(wearItem.EquipPart, wearItem);

            ProduceWear(wearItem);
        }
    }

    //아이템 목록 저장
    public void HeroInvenSave()
    {
        string strPath = string.Empty;
#if (UNITY_EDITOR || UNITY_STANDALONE_WIN)
        strPath += (Application.streamingAssetsPath + "/Hero/HeroInven.xml");
#elif UNITY_ANDROID
        strPath = "jar:file://" + Application.dataPath + "!/assets" + "/Hero/HeroInven.xml";
#endif

        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.AppendChild(xmlDoc.CreateXmlDeclaration("1.0", "utf-8", "yes"));

        //루트 노드 생성
        XmlNode root = xmlDoc.CreateNode(XmlNodeType.Element, "Hero", string.Empty);
        xmlDoc.AppendChild(root);

        //자식 노드 생성
        XmlNode InfoNode = xmlDoc.CreateNode(XmlNodeType.Element, "Info", string.Empty);
        root.AppendChild(InfoNode);

        //캐릭터 인벤토리 기본정보 속성 설정
        XmlElement Name = xmlDoc.CreateElement("Name");
        Name.InnerText = StatManager.Instance.heroName;
        InfoNode.AppendChild(Name);

        XmlElement Money = xmlDoc.CreateElement("Money");
        Money.InnerText = this.money.ToString();
        InfoNode.AppendChild(Money);

        //인벤토리 아이템 노드 추가
        foreach(KeyValuePair<int, MyItem> ItemNode in dic_Inventory)
        {
            //아이템 노드 생성
            XmlNode Item = xmlDoc.CreateNode(XmlNodeType.Element, "Item", string.Empty);
            root.AppendChild(Item);

            //아이템 정보 설정
            XmlElement Code = xmlDoc.CreateElement("Code");
            Code.InnerText = ItemNode.Value.itemCode.ToString();
            Item.AppendChild(Code);

            XmlElement Kind = xmlDoc.CreateElement("Kind");
            Kind.InnerText = ItemNode.Value.itemKind.ToString();
            Item.AppendChild(Kind);

            XmlElement Count = xmlDoc.CreateElement("Count");
            Count.InnerText = ItemNode.Value.itemCount.ToString();
            Item.AppendChild(Count);

            XmlElement Index = xmlDoc.CreateElement("Index");
            Index.InnerText = ItemNode.Value.InvenIndex.ToString();
            Item.AppendChild(Index);

        }

        //착용중인 장비 아이템 노드 추가
        foreach (KeyValuePair<int, Equip> WearNode in dic_Wearing)
        {
            //아이템 노드 생성
            XmlNode Wear = xmlDoc.CreateNode(XmlNodeType.Element, "Wear", string.Empty);
            root.AppendChild(Wear);

            //아이템 정보 설정
            XmlElement Code = xmlDoc.CreateElement("Code");
            Code.InnerText = WearNode.Value.codeNumber.ToString();
            Wear.AppendChild(Code);
        }
        xmlDoc.Save(strPath);
    }

    // 게임 아이템 정의 딕셔너리 로드
    public void ItemDefineLoad()
    {

        TextAsset textAsset = (TextAsset)Resources.Load("Item/ItemDefine");


        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(textAsset.text);
        
        
        XmlNodeList EquipList = xmlDoc.SelectNodes("Item/Equip");   // 장비 아이템 정보 리스트

        foreach(XmlNode EquipNode in EquipList)
        {
            int codeNumber = int.Parse(EquipNode.SelectSingleNode("Code").InnerText);
            string equipName = EquipNode.SelectSingleNode("Name").InnerText;
            int equipPart = int.Parse(EquipNode.SelectSingleNode("Part").InnerText);
            int equipAttack = int.Parse(EquipNode.SelectSingleNode("Attack").InnerText);
            int equipDefence = int.Parse(EquipNode.SelectSingleNode("Defence").InnerText);
            int equipSpeed = int.Parse(EquipNode.SelectSingleNode("Speed").InnerText);
            int equipPrice = int.Parse(EquipNode.SelectSingleNode("Price").InnerText);
            string equipText = EquipNode.SelectSingleNode("Text").InnerText;

            Equip equipItem;
            equipItem.codeNumber = codeNumber;
            equipItem.EquipName = equipName;
            equipItem.EquipPart = equipPart;
            equipItem.attack = equipAttack;
            equipItem.defence = equipDefence;
            equipItem.speed = equipSpeed;
            equipItem.price = equipPrice;
            equipItem.text = equipText;

            dic_Equip.Add(codeNumber, equipItem);
        }


        XmlNodeList RecoveryList = xmlDoc.SelectNodes("Item/Recovery");   // 회복 아이템 정보 리스트

        foreach(XmlNode RecoveryNode in RecoveryList)
        {
            int codeNumber = int.Parse(RecoveryNode.SelectSingleNode("Code").InnerText);
            string recoveryName = RecoveryNode.SelectSingleNode("Name").InnerText;
            int recoveryHp = int.Parse(RecoveryNode.SelectSingleNode("Hp").InnerText);
            int recoveryMp = int.Parse(RecoveryNode.SelectSingleNode("Mp").InnerText);
            int recoveryPrice = int.Parse(RecoveryNode.SelectSingleNode("Price").InnerText);
            string recoveryText = RecoveryNode.SelectSingleNode("Text").InnerText;

            Recovery recovery;
            recovery.codeNumber = codeNumber;
            recovery.RecoveryName = recoveryName;
            recovery.hpRecovery = recoveryHp;
            recovery.mpRecovery = recoveryMp;
            recovery.price = recoveryPrice;
            recovery.text = recoveryText;

            dic_Recovery.Add(codeNumber, recovery);
        }
         
        XmlNodeList OtherList = xmlDoc.SelectNodes("Item/Other");   // 기타 아이템 정보 리스트

        foreach(XmlNode OtherNode in OtherList)
        {
            int codeNumber = int.Parse(OtherNode.SelectSingleNode("Code").InnerText);
            string otehrName = OtherNode.SelectSingleNode("Name").InnerText;
            int otherPrice = int.Parse(OtherNode.SelectSingleNode("Price").InnerText);
            string otherText = OtherNode.SelectSingleNode("Text").InnerText;

            Other other;
            other.codeNumber = int.Parse(OtherNode.SelectSingleNode("Code").InnerText);
            other.OtherName = OtherNode.SelectSingleNode("Name").InnerText;
            other.price = int.Parse(OtherNode.SelectSingleNode("Price").InnerText);
            other.text = OtherNode.SelectSingleNode("Text").InnerText;

            dic_Other.Add(codeNumber, other);
        }

    }

    void OpenInventory()
    {
        if(Input.GetKeyDown(KeyCode.I) && !HeroScript.Instance.isNpcDialog)   // npc와 대화를 하지않고 i키를 눌렀을때
        {

            gb_Inventory.gameObject.SetActive(!(gb_Inventory.gameObject.activeSelf));
            isOpenInventory = !isOpenInventory;
        }
    }

    // 인벤토리 활성화/비활성화
    public void ActiveInventory()
    {
        if(!HeroScript.Instance.isNpcDialog)
        {
            gb_Inventory.gameObject.SetActive(!(gb_Inventory.gameObject.activeSelf));
            isOpenInventory = !isOpenInventory;
        }
    }

    // 인벤토리 비활성화
    public void DisableInventory()
    {
        gb_Inventory.gameObject.SetActive(false);
        isOpenInventory = false;
    }

    // 아이템을 구입하거나 얻었을때 아이템 추가
    public void InventoryAddItem(int itemKind, int itemCode, int itemCount)
    {
        MyItem newItem;
        newItem.itemCode = itemCode;
        newItem.itemKind = itemKind;
        newItem.itemCount = 1;

        for (int i = 0; i < inventoryLimit; i++)
        {
            if (!dic_Inventory.ContainsKey(i))
            {
                newItem.InvenIndex = i;
                dic_Inventory.Add(i, newItem);
                //아이템 UI생성
                ProduceItem(newItem);

                break;
            }
        }

        //인벤토리 꽉찼을 때 예외 처리
    }

    //인벤토리 창에 아이템 UI 생성
    void ProduceItem(MyItem item)
    {
        switch (item.itemKind)
        {
            case (int)ItemKind.EQUIP:
                {
                    //상점 열었을때 인벤토리의 아이템 오브젝트 
                    GameObject newObject = Instantiate(prefab_EquipItem, grid_StoreInventoryBlank.GetChild(item.InvenIndex).transform.position, Quaternion.identity, gb_StoreItemField.transform);
                    newObject.GetComponent<UISprite>().spriteName = "Equip_" + item.itemCode.ToString("D2");
                    newObject.GetComponent<UISprite>().depth = grid_StoreInventoryBlank.GetChild(item.InvenIndex).GetComponent<UISprite>().depth + 1;
                    newObject.GetComponent<MyItemScript>().ItemInfoInit(item, grid_StoreInventoryBlank);
                    //그냥 인벤토리 열었을 때의 아이템 오브젝트
                    newObject = Instantiate(prefab_EquipItem, grid_InventoryBlank.GetChild(item.InvenIndex).transform.position, Quaternion.identity, gb_ItemField.transform);
                    newObject.GetComponent<UISprite>().spriteName = "Equip_" + item.itemCode.ToString("D2");
                    newObject.GetComponent<UISprite>().depth = grid_InventoryBlank.GetChild(item.InvenIndex).GetComponent<UISprite>().depth + 1;
                    newObject.GetComponent<MyItemScript>().ItemInfoInit(item, grid_InventoryBlank);

                    break;
                }
            case (int)ItemKind.RECOVERY:
                {
                    break;
                }
            case (int)ItemKind.OTHER:
                {
                    break;
                }
        }
    }

    //장착하고 있는 장비 아이템 UI생성
    void ProduceWear(Equip wear)
    {
        MyItem wearItemInfo;
        wearItemInfo.itemCode = wear.codeNumber;
        wearItemInfo.itemCount = 1;
        wearItemInfo.itemKind = (int)ItemKind.EQUIP;

        //반지 
        if (wear.EquipPart == (int)EquipKind.RING)
        {
            wearItemInfo.InvenIndex = wear.EquipPart;
        }
        //반지아닌 그외 장비들
        else
        {
            wearItemInfo.InvenIndex = wear.EquipPart;

            GameObject wearEquip = Instantiate(prefab_EquipItem, gb_EquipBlank.transform.GetChild(wear.EquipPart).transform.position, Quaternion.identity, gb_EquipField.transform);
            wearEquip.GetComponent<UISprite>().spriteName = "Equip_" + wear.codeNumber.ToString("D2");
            wearEquip.GetComponent<UISprite>().depth = gb_EquipBlank.transform.GetChild(wear.EquipPart).GetComponent<UISprite>().depth + 1;
            wearEquip.GetComponent<MyItemScript>().ItemInfoInit(wearItemInfo, null);
            
        }
       
    }

    //자리 바뀔 아이템
    public void ItemPositionChange(int fromIndex, int toIndex)
    {
        foreach(Transform Item in gb_StoreItemField.transform)
        {
            if (Item.GetComponent<MyItemScript>().index == fromIndex)
            {
                Item.position = grid_StoreInventoryBlank.GetChild(toIndex).transform.position;
                Item.GetComponent<MyItemScript>().index = toIndex;
                MyItem TempItem = Item.GetComponent<MyItemScript>().GetItemInfo;
                TempItem.InvenIndex = toIndex;
                Item.GetComponent<MyItemScript>().ItemInfoInit(TempItem, grid_StoreInventoryBlank);
            }
            else if(Item.GetComponent<MyItemScript>().index == toIndex)
            {
                Item.position = grid_StoreInventoryBlank.GetChild(fromIndex).transform.position;
                Item.GetComponent<MyItemScript>().index = fromIndex;
                MyItem TempItem = Item.GetComponent<MyItemScript>().GetItemInfo;
                TempItem.InvenIndex = fromIndex;
                Item.GetComponent<MyItemScript>().ItemInfoInit(TempItem, grid_StoreInventoryBlank);
            }
        }

        foreach (Transform Item in gb_ItemField.transform)
        {
            if (Item.GetComponent<MyItemScript>().index == fromIndex)
            {
                Item.position = grid_InventoryBlank.GetChild(toIndex).transform.position;
                Item.GetComponent<MyItemScript>().index = toIndex;
                MyItem TempItem = Item.GetComponent<MyItemScript>().GetItemInfo;
                TempItem.InvenIndex = toIndex;
                Item.GetComponent<MyItemScript>().ItemInfoInit(TempItem, grid_InventoryBlank);
            }
            else if (Item.GetComponent<MyItemScript>().index == toIndex)
            {
                Item.position = grid_InventoryBlank.GetChild(fromIndex).transform.position;
                Item.GetComponent<MyItemScript>().index = fromIndex;
                MyItem TempItem = Item.GetComponent<MyItemScript>().GetItemInfo;
                TempItem.InvenIndex = fromIndex;
                Item.GetComponent<MyItemScript>().ItemInfoInit(TempItem, grid_InventoryBlank);
            }
        }
    }

    // 아이템 인벤토리에서 위치 이동
    public void ItemPositionMove(int fromIndex, int toIndex)
    {
        foreach (Transform Item in gb_StoreItemField.transform)
        {
            if (Item.GetComponent<MyItemScript>().index == fromIndex)
            {
                Item.position = grid_StoreInventoryBlank.GetChild(toIndex).transform.position;
                Item.GetComponent<MyItemScript>().index = toIndex;
                break;
            }
           
        }

        foreach (Transform Item in gb_ItemField.transform)
        {
            if (Item.GetComponent<MyItemScript>().index == fromIndex)
            {
                Item.position = grid_InventoryBlank.GetChild(toIndex).transform.position;
                Item.GetComponent<MyItemScript>().index = toIndex;
                break;
            }
           
        }
    }


    //아이템 사용(착용/해제 및 소모)
    public void UseItem(GameObject gb_UseItem)
    {
        if(gb_UseItem.GetComponent<MyItemScript>().GetGridInventory == grid_StoreInventoryBlank)   // 상점 창에서 더블클릭을 하였다면 판매
        {
            SellingItem(gb_UseItem);
            return;
        }

        MyItem useItem = gb_UseItem.GetComponent<MyItemScript>().GetItemInfo;
        switch(useItem.itemKind)
        {
            case (int)ItemKind.EQUIP:
                {
                    TakeOnEquip(gb_UseItem);
                    break;
                }
            case (int)ItemKind.RECOVERY:
                {
                    break;
                }
        }
    }

    //장비 착용
    void TakeOnEquip(GameObject TakeOnEquip)
    {
        MyItem Item = TakeOnEquip.GetComponent<MyItemScript>().GetItemInfo;
        Equip equip = dic_Equip[Item.itemCode];
        //반지는 따로 로직
        if(equip.EquipPart == (int)EquipKind.RING)
        {

        }
        else
        {
            dic_Inventory.Remove(Item.InvenIndex);  // 일단 인벤토리 딕셔너리에서 빼고

            foreach(Transform StoreInvenItem in gb_StoreItemField.transform)
            {
                if(StoreInvenItem.GetComponent<MyItemScript>().index == Item.InvenIndex)
                {
                    Destroy(StoreInvenItem.gameObject);  // 상점 창 인벤토리 오브젝트 제거
                    break;
                }
            }



            //장비 교체
            if (dic_Wearing.ContainsKey(equip.EquipPart))
            {
                TakeOffEquip(equip.EquipPart);
            }
            //장착
            
            dic_Wearing.Add(equip.EquipPart, equip);
            TakeOnEquip.transform.position = gb_EquipBlank.transform.GetChild(equip.EquipPart).transform.position;
            Item.InvenIndex = equip.EquipPart;
            TakeOnEquip.GetComponent<MyItemScript>().ItemInfoInit(Item, null);
            TakeOnEquip.GetComponent<MyItemScript>().index = equip.EquipPart;
            TakeOnEquip.GetComponent<UISprite>().depth = gb_EquipBlank.transform.GetChild(equip.EquipPart).GetComponent<UISprite>().depth + 1;
            TakeOnEquip.transform.parent = gb_EquipField.transform;
                
            

        }

    }


    //장비창에서 장비 해제
    public void TakeOffEquip(int equipPart)   // 매개변수   차고있는 장비 파츠 
    {
        foreach (Transform wearingEquip in gb_EquipField.transform)
        {
            if(wearingEquip.GetComponent<MyItemScript>().index == equipPart)
            {
                GameObject OffEquip = wearingEquip.gameObject;
                MyItem OffItem = OffEquip.GetComponent<MyItemScript>().GetItemInfo;
                for(int index= 0; index < grid_InventoryBlank.transform.childCount; index++)
                {

                    //인벤토리에 공간이있으면
                    if(!dic_Inventory.ContainsKey(index))
                    {
                        OffEquip.transform.position = grid_InventoryBlank.GetChild(index).transform.position;
                        OffEquip.GetComponent<MyItemScript>().index = index;
                        OffEquip.transform.parent = gb_ItemField.transform;
                        OffEquip.GetComponent<UISprite>().depth = grid_InventoryBlank.GetChild(index).GetComponent<UISprite>().depth + 1;
                        OffItem.InvenIndex = index;
                        OffEquip.GetComponent<MyItemScript>().ItemInfoInit(OffItem, grid_InventoryBlank);
                        dic_Inventory.Add(index, OffItem);

                        GameObject newObject = Instantiate(prefab_EquipItem, grid_StoreInventoryBlank.GetChild(index).transform.position, Quaternion.identity, gb_StoreItemField.transform);
                        newObject.GetComponent<UISprite>().spriteName = OffEquip.GetComponent<UISprite>().spriteName;
                        newObject.GetComponent<UISprite>().depth = grid_StoreInventoryBlank.GetChild(index).GetComponent<UISprite>().depth + 1;
                        newObject.GetComponent<MyItemScript>().ItemInfoInit(OffEquip.GetComponent<MyItemScript>().GetItemInfo, grid_StoreInventoryBlank);

                        dic_Wearing.Remove(equipPart);

                        return;
                    }
                }
                //인벤토리 공간이 없다면
                break;
            }
        }
    }

    void SellingItem(GameObject Sell_Item)
    {
        MyItem Item = Sell_Item.GetComponent<MyItemScript>().GetItemInfo;
        Destroy(Sell_Item);
        dic_Inventory.Remove(Item.InvenIndex);  // 일단 인벤토리 딕셔너리에서 빼고

        foreach(Transform InvenItem in gb_ItemField.transform)
        {
            if(Item.InvenIndex == InvenItem.GetComponent<MyItemScript>().index)
            {
                Destroy(InvenItem.gameObject);
                break;
            }
        }
    }

}
