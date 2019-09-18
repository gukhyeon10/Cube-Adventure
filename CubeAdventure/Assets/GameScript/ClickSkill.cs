using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickSkill : MonoBehaviour {

    public string skillSpriteName ="";

    void Start()
    {
        skillSpriteName = this.GetComponent<UISprite>().spriteName;
    }

    void OnEnable()
    {
        skillSpriteName = this.GetComponent<UISprite>().spriteName;
    }

    public void SkillClick()
    {
        SkillManager.Instance.PlaySkill(skillSpriteName);
    }

	public void SkillDoubleClick()
    {
        SkillManager.Instance.PlaySkill(skillSpriteName);
    }
}
