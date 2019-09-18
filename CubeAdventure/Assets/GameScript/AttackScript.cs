using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackScript : MonoBehaviour {
    [SerializeField]
    GameObject _character;

    [SerializeField]
    Animator _anim;

    public GameObject[] SkillButtonGroup;

    [SerializeField]
    BoxCollider SwordColider;

    public bool isSkillDrag = false;

    static private AttackScript _instance = null;

    static public AttackScript Instance
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

    void Start()
    {
    }

    void LateUpdate()
    {
        if(_anim.GetCurrentAnimatorStateInfo(0).IsName("NormalAttack01_SwordShield") && _anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            _anim.SetBool("attackCheck", false);
            SwordColider.enabled = false;
        }
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            _anim.SetBool("attackCheck", true);
        }
    }
    public void NormalAttack() // 기본 공격 버튼
    {
        _anim.SetBool("attackCheck", true);
        SwordColider.enabled = true;
    }

    

    public void PlayActiveSkill()
    {
        _anim.SetBool("attackCheck", true);
    }
    

    public void DragSkillActive(string Skill_UIName, Vector2 DragPosition)
    {

        for(int i=0; i< SkillButtonGroup.Length; i++)
        {
            float dis = Vector2.Distance(SkillButtonGroup[i].transform.position, DragPosition);

            if(dis < 0.1f)
            {
                SkillButtonGroup[i].GetComponent<UISprite>().spriteName = Skill_UIName;
                SkillButtonGroup[i].GetComponent<ClickSkill>().skillSpriteName = Skill_UIName;

                if(SkillButtonGroup[i].gameObject.activeSelf == false)
                {
                    SkillButtonGroup[i].gameObject.SetActive(true);
                }

                break;
            }
            else
            {

            }
        }

    }

    public void DisableSkill(GameObject skill)
    {
        float dis = Vector2.Distance(skill.transform.localPosition, Vector2.zero);
        if(dis>30f)
        {
            skill.gameObject.SetActive(false);
            skill.transform.localPosition = Vector3.zero;
        }
        else
        {
            skill.transform.localPosition = Vector3.zero;
        }
    }

    public BoxCollider GetSwordColider
    {
        get
        {
            return SwordColider;
        }
    }
    
    
    
}
