using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcSkillLearn : MonoBehaviour {
    [SerializeField]
    UISprite SkillSprite;
	public void SkillLearn()
    {
        SkillManager.Instance.LearnSkill(SkillSprite.spriteName);
    }
}
