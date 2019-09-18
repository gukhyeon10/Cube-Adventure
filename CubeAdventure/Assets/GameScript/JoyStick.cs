using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JoyStick : MonoBehaviour {

    [SerializeField]
    GameObject Gb_JoyStickBackGround;
    [SerializeField]
    GameObject Gb_JoyStick;

    [SerializeField]
    Camera UICamera;

    [SerializeField]
    Animator _anim;

    [SerializeField]
    GameObject _character;

    public bool isShortSkillDrag = false;
    //Vector3 v3JoyStickBg;
    bool isMove;
    Vector3 v3JoyStick;
    float speed;

    Vector2 windowDrag;

	// Use this for initialization
	void Start () {
       // v3JoyStickBg = Gb_JoyStickBackGround.transform.position;
        v3JoyStick = Gb_JoyStick.transform.position;

        Camera.main.transform.position = new Vector3(-14.05095f, 11.83001f, -18.73872f);
        Camera.main.transform.eulerAngles = new Vector3(44.23732f, 64.91228f, 0f);

        speed = 2f;
	}

    void Update()
    {
        if(_anim.GetCurrentAnimatorStateInfo(0).IsName("GetHit_SwordShield") || _anim.GetCurrentAnimatorStateInfo(0).IsName("Die_SwordShield"))
        {
            //맞고있는 중이거나 게임오버가 된다면 이동 에니메이션 캔슬
            return;
        }

        if(!(_anim.GetBool("attackCheck")) && !HeroScript.Instance.isNpcDialog)
        {
            WindowTouchDrag();
            if (_anim.GetBool("runCheck"))
            {
                _character.transform.Translate(Vector3.forward * Time.deltaTime * this.speed * 2.5f);
            }
            else if (_anim.GetBool("walkCheck"))
            {
                _character.transform.Translate(Vector3.forward * Time.deltaTime * this.speed);
            }
            
        }

        CharacterRegen();
    }

    public void ShortSkillDragStart()
    {
        isShortSkillDrag = true;
    }

    public void ShortSkillDragEnd()
    {
        isShortSkillDrag = false;
    }

    void WindowTouchDrag()
    {
        if (!isMove && !InvenManager.Instance.isOpenInventory && !SkillManager.Instance.isOpenSkill && !StatManager.Instance.isOpenState && !isShortSkillDrag)
        {

            //마우스 드래그
            if (!Input.GetMouseButtonDown(0) && Input.GetMouseButton(0))
            {
                float KeyHorizontal = Input.GetAxis("Mouse X");
                _character.transform.eulerAngles = new Vector3(_character.transform.eulerAngles.x, _character.transform.eulerAngles.y + KeyHorizontal * 10f, _character.transform.eulerAngles.z);

            }

        }

    }

    void LateUpdate()
    {
        //Gb_JoyStick.transform.position = v3JoyStick; 

        float dis = Vector3.Distance(Gb_JoyStick.transform.position, v3JoyStick);

        
        if(dis > 0.2f)
        {
            Gb_JoyStick.transform.position = v3JoyStick + (Gb_JoyStick.transform.position - v3JoyStick).normalized * 0.2f;
        }


        if (!isMove)
        {
            Gb_JoyStick.transform.position = v3JoyStick;
            _anim.SetBool("walkCheck", false);
            _anim.SetBool("runCheck", false);
        }
        else
        {
            if(dis > 0f && _anim != null)
            {

                if(!_anim.GetBool("dieCheck"))
                {
                    _character.transform.eulerAngles = new Vector3(
                                _character.transform.eulerAngles.x,
                                Mathf.Atan2(Gb_JoyStick.transform.localPosition.x, Gb_JoyStick.transform.localPosition.y) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y,
                                _character.transform.eulerAngles.z);
                }

                if (0.1f > dis)
                {
                    _anim.SetBool("walkCheck", true);
                    _anim.SetBool("runCheck", false);
                }
                else
                {
                    _anim.SetBool("runCheck", true);
             
                }
            }
            else
            {
                _anim.SetBool("walkCheck", false);
                _anim.SetBool("runCheck", false);
            }
        }

        if(_anim.GetCurrentAnimatorStateInfo(0).IsName("GetHit_SwordShield"))
        {
            _anim.SetBool("hitCheck", false);
        }
        
        
    }
    
    public void IsMoving()
    {
        isMove = true;
    }

    public void NotMoving()
    {
        isMove = false;
    }

    void CameraMove()
    {

    }

    void CharacterRegen()
    {
        if(_character.transform.position.y < -5f)
        {
            _character.transform.position = new Vector3(-2f, 5f, -6f);
        }
    }

   

}
