using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragIcon : MonoBehaviour {

    GameObject DragIconClone = null;

    Vector3 InitPosition;
   
    
    void Start()
    {
        InitPosition = this.transform.position;    
    }

    // 스킬아이콘 드래그
    public void DragSkill(string skillName)
    {
        Debug.Log(skillName);
        AttackScript.Instance.isSkillDrag = true;

        DragIconClone = NGUITools.AddChild(this.transform.parent.gameObject, this.gameObject);
        DragIconClone.transform.position = InitPosition;

        this.GetComponent<UISprite>().depth++;
        Destroy(DragIconClone.GetComponent<BoxCollider>());

        StartCoroutine(DragSkillCorutine());
    }

    public void DragEnd()
    {

        AttackScript.Instance.DragSkillActive(this.GetComponent<UISprite>().spriteName, this.transform.position);
        AttackScript.Instance.isSkillDrag = false;

    }

    IEnumerator DragSkillCorutine()
    {
        while(true)
        {
            if(AttackScript.Instance.isSkillDrag == false)
            {
                Destroy(DragIconClone);
                DragIconClone = null;
                
                break;
            }
            yield return null;
        }

        this.GetComponent<UISprite>().depth--;
        this.transform.position = InitPosition;
    }
}
