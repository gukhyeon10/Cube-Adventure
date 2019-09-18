using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcStoreManager : MonoBehaviour {

    [SerializeField]
    GameObject NpcStore;
    [SerializeField]
    GameObject Inventory;

    [SerializeField]
    UIGrid grid_EquipNodeField;
    [SerializeField]
    GameObject gb_EquipPrefab;

    static private NpcStoreManager _instance = null;

    static public NpcStoreManager Instance
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

    //상점 열기
    public void OpenStore()
    { 

        foreach(KeyValuePair<int, Equip> equipNode in InvenManager.Instance.dic_Equip)
        {

            GameObject newEquipNode = NGUITools.AddChild(grid_EquipNodeField.gameObject, gb_EquipPrefab);

            newEquipNode.transform.GetChild(0).GetComponent<UISprite>().spriteName = "Equip_" + equipNode.Value.codeNumber.ToString("D2");
            newEquipNode.transform.GetChild(1).GetComponent<UILabel>().text = equipNode.Value.EquipName;
            newEquipNode.transform.GetChild(2).GetComponent<UILabel>().text = equipNode.Value.price + "원";

        }

        grid_EquipNodeField.Reposition();

        NpcStore.gameObject.SetActive(true);
        Inventory.gameObject.SetActive(true);
    }

    public void CloseStore()
    {

        NGUITools.DestroyChildren(grid_EquipNodeField.transform);

        HeroScript.Instance.isNpcDialog = false;

        NpcStore.gameObject.SetActive(false);
        Inventory.gameObject.SetActive(false);
    }

}
