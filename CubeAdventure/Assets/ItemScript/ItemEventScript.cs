using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemEventScript : MonoBehaviour {

    //아이템을 샀을때
	public void BuyItem(string ItemSpriteName)
    {
        string ItemKindName = ItemSpriteName.Substring(0, ItemSpriteName.Length - 3);
        int itemKind = 0;
        int itemCode = 0;
        if (ItemSpriteName[ItemSpriteName.Length - 2].Equals("0"))
        {
            itemCode = int.Parse(ItemSpriteName.Substring(ItemSpriteName.Length - 1));
        }
        else
        {
            itemCode = int.Parse(ItemSpriteName.Substring(ItemSpriteName.Length - 2));
        }
        
        switch (ItemKindName)
        {
            case "Equip":
                {
                    itemKind = (int)ItemKind.EQUIP;
                    //장비 아이템은 한개가 MAX
                    InvenManager.Instance.InventoryAddItem(itemKind, itemCode, 1);
                    break;
                }
            default:
                {
                    Debug.Log("존재하지 않는 아이템 종류");
                    break;
                }
        }



    }
}
