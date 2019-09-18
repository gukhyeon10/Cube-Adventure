using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    // Enum 필드
	public enum ObjectKind
    {
        CUBE = 0, 
        NATURE = 1,
        BUILD = 2,
        BRIDGE = 3,
        FURNITURE = 4,
        ENEMY = 5,
        PORTAL = 6,
        NPC = 7,
    }

    public enum SkillNumber
{
    ATTACK_STRENGTH = 1,
    AGILITY_STRENGTH = 2,
    POISONING = 3,
    FIREBALL = 4,
    DEFENCE_STRENGTH = 5,
    SPLINTER = 6,
}
    public struct SkillData
{
    public string skillName;
    public int consumMp;
    public float coolTime;
    public int percent;
}
    
