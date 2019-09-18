using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyItemScript : MonoBehaviour
{
    public int index;
    MyItem ItemInfo;
    UIGrid grid_Inventory;

    public void ItemInfoInit(MyItem itemInfo, UIGrid inventory)
    {
        this.ItemInfo = itemInfo;
        this.grid_Inventory = inventory;
        this.index = itemInfo.InvenIndex;
    }

    public MyItem GetItemInfo
    {

        get
        {
            return this.ItemInfo;
        }
    }

    public UIGrid GetGridInventory
    {
        get
        {
            return this.grid_Inventory;
        }
    }

    public void DragEnd()
    {
        if(this.grid_Inventory == null)  // null 이면 parent가 장비창
        {
            return;
        }

        int changeIndex = 0;
        foreach (Transform blankNode in grid_Inventory.transform)
        {
            if( Vector2.Distance(blankNode.localPosition, this.transform.localPosition) < 20)
            {

                
                // 해당 자리에 다른 아이템이 존재한다면
                if(InvenManager.Instance.dic_Inventory.ContainsKey(changeIndex))
                {
                    //데이터 옮기기
                    MyItem tempData = InvenManager.Instance.dic_Inventory[changeIndex];
                    tempData.InvenIndex = this.index;
                    InvenManager.Instance.dic_Inventory[this.index] = tempData;

                    this.ItemInfo.InvenIndex = changeIndex;
                    InvenManager.Instance.dic_Inventory[changeIndex] = this.ItemInfo;

                    //아이템 UI 자리 바꾸기
                    InvenManager.Instance.ItemPositionChange(changeIndex, this.index);
                }
                // 해당 자리에 다른 아이템이 존재하지 않는다면
                else
                {
                    //기존 인덱스 키에 해당하는 데이터 지우고,  새로운 인덱스로 갱신시킨다음에 해당 인덱스 키값에 아이템 추가
                    ItemInfo.InvenIndex = changeIndex;
                    InvenManager.Instance.dic_Inventory.Remove(this.index);
                    InvenManager.Instance.dic_Inventory.Add(changeIndex, ItemInfo);

                    InvenManager.Instance.ItemPositionMove(this.index, changeIndex);
                }
                
                return;
            }
            changeIndex++;
        }

        this.transform.position = grid_Inventory.GetChild(this.index).transform.position;

    }

    // 더블클릭하여 아이템 사용/해제 혹은 소모
    public void UseItem()
    {
        if(this.grid_Inventory == null) // null 이면 장비창 
        {
            InvenManager.Instance.TakeOffEquip(index);
        }
        else
        {
            InvenManager.Instance.UseItem(this.gameObject);
        }
    }
}
 