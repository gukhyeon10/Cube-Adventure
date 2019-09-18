using System.Collections;
using System.Collections.Generic;
using UnityEngine;


	public enum ItemKind
    {
        EQUIP = 0,
        RECOVERY = 1,
        OTHER = 2,
    }
    
    public enum EquipKind
    {
        HEAD = 0,
        NECK =1,
        TOP = 2,
        WEAPON = 3,
        SHIELD = 4,
        BELT = 5,
        SHOES = 6,
        RING = 7,
        FIRST_RING = 8,
        SECOND_RING = 9,
        THIRD_RING = 10,
    }

    
    
    public struct Equip
    {
    public int codeNumber;
        public string EquipName;
        public int EquipPart; 
        public float attack;
        public float defence;
        public float speed;
        public int price;
    public string text;
    }

    public struct Recovery
    {
    public int codeNumber;
        public string RecoveryName;
        public int hpRecovery;
        public int mpRecovery;
        public int price;
    public string text;
    }

    public struct Other
    {
    public int codeNumber;
    public string OtherName;
        public int price;
    public string text;
    }

    public struct MyItem
{
    public int itemKind;
    public int itemCode;
    public int itemCount;
    public int InvenIndex;
}



