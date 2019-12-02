using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDiscriptionScript : MonoBehaviour {

    //아이템 설명 다이얼로그 활성화
    public void ActiveItemDescription(string ItemSpriteName)
    {
        string ItemKindName = ItemSpriteName.Substring(0, ItemSpriteName.Length - 3);
        int itemKind = 0;
        int itemCode = 0;
        if (ItemSpriteName[ItemSpriteName.Length-2].Equals("0"))
        {
            itemCode = int.Parse(ItemSpriteName.Substring(ItemSpriteName.Length-1));
        }
        else
        {
            itemCode = int.Parse(ItemSpriteName.Substring(ItemSpriteName.Length - 2));
        }
        

        switch(ItemKindName)
        {
            case "Equip":
                {
                    itemKind = (int)ItemKind.EQUIP;
                    break;
                }
            default:
                {
                    Debug.Log("존재하지 않는 아이템 종류");
                    break;
                }
        }
        //Debug.Log("아이템 설명 글을 보는중");
        GameUI_Manager.Instance.ItemDescriptionInit(itemKind, itemCode);
    }

    public void DisableItemDescription()
    {
        GameUI_Manager.Instance.DestoryItemDescription();
    }
}
