using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour {

    GameObject gb_HudBar, gb_DemageHud;
    int maxHp, remainHp;
    int Attack, defence;
    float speed, attackSpeed;
    int AmountExp;
    public Vector3 initPosition = Vector3.zero;

    GameObject Hero;
    
    Animator _anim;

    bool isFaceHero = false;
    public bool isAttackCollider = false;
    public bool isAttackSucces = false;
    public bool isAttackCoolTime = false;

    public bool isNormalAttacked = false;
    public bool isSkillAttacked = false;

    bool isDie = false;

    void Awake()
    {
        initPosition = this.transform.position;

        this.maxHp = 50;
        this.AmountExp = 50;

        this.speed = 1f;
        this.attackSpeed = 1f;
    }

    // Use this for initialization
    void Start () {
        //Hp바 
        gb_HudBar =  GameUI_Manager.Instance.EnemyHudBar();
        gb_HudBar.transform.localScale = new Vector3(1f, 0.75f, 1f);

        //데미지 HUD
        gb_DemageHud = HudManager.Instance.MakeDemageHud();

        Hero = HeroScript.Instance.gameObject;

        _anim = this.GetComponent<Animator>();
        

    }
    

    void OnEnable()
    {
        this.remainHp = this.maxHp;

        if(gb_HudBar != null)
        {
            HpBarPosUpdate();
            gb_HudBar.gameObject.SetActive(true);
        }

        if(gb_DemageHud != null)
        {
            HpBarRemainUpdate();
            gb_DemageHud.gameObject.SetActive(true);
        }

        this.GetComponent<CapsuleCollider>().enabled = true;

        if(_anim != null)
        {
            _anim.SetInteger("State", (int)EnemyState.STAND);
        }

        this.remainHp = this.maxHp;

        isFaceHero = false;
        isDie = false;
    }
    

    // Update is called once per frame
    void Update () {

        if(remainHp > 0)    // 살아있는동안 캐릭터 감지
        {
            DiscoverHero();
            FaceHeroAction();
        }

        HpBarPosUpdate();    // 몬스터 ui 업데이트
        HpBarRemainUpdate();

        if (isNormalAttacked || isSkillAttacked)     //기본공격이나 스킬공격에 맞으면
        {
            _anim.SetInteger("State", (int)EnemyState.DAMAGE);
            isAttackCoolTime = false;
        }

        if(_anim.GetCurrentAnimatorStateInfo(0).IsName("Attack") && _anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.3f && _anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.5f)
        {
            isAttackCollider = true;
        }
        else
        {
            isAttackCollider = false;
            isAttackSucces = false;
        }
        

        if(remainHp <= 0)                       // 죽으면
        {
            //경험치 Up!!
            if(!isDie)
            {
                //타임어택 모드가 아니고 길에서 만나는 몬스터 처치시 경험치 획득!
                if(HeroScript.Instance.isTimeAttackMode)
                {
                    HeroScript.Instance.timeAttackKillCount++;

                    GameUI_Manager.Instance.TimeAttackKillCount();
                }
                else
                {
                    StatManager.Instance.GetExp(this.AmountExp);
                }
                isDie = true;
            }

            _anim.SetInteger("State", (int)EnemyState.DEATH);

            gb_HudBar.gameObject.SetActive(false);
            gb_DemageHud.gameObject.SetActive(false);

            this.GetComponent<CapsuleCollider>().enabled = false;

            StartCoroutine(EnemyRegen());
            //this.gameObject.SetActive(false);

            //Destroy(this.GetComponent<CapsuleCollider>());
            //Destroy(this.gameObject, 3f);
        }
        
        isSkillAttacked = false;
    }
    
    IEnumerator EnemyRegen()
    {
        this.GetComponent<CapsuleCollider>().enabled = false;
        yield return new WaitForSeconds(3.5f);
        EnemyPoolManager.Instance.EnemyRegen(this.gameObject);
        this.gameObject.SetActive(false);

        this.transform.position = initPosition;
    }

    void HpBarPosUpdate()
    {
        Vector2 screenPos = Camera.main.WorldToScreenPoint(this.transform.position);
        screenPos = new Vector2(screenPos.x, screenPos.y + 45f);  // 머리 위  Hud 체력바 간격
        Vector3 HudBarPosition = UICamera.mainCamera.ScreenToWorldPoint(screenPos);
        HudBarPosition.z = 0f;
        gb_HudBar.transform.position = HudBarPosition;

        gb_DemageHud.transform.position = HudBarPosition;
    }

    void HpBarRemainUpdate()
    {
        gb_HudBar.GetComponent<UISlider>().value = (float)remainHp / (float)maxHp;
    }

    void DiscoverHero()
    {
        Vector2 HeroPosition = new Vector2(Hero.transform.position.x, Hero.transform.position.z);
        Vector2 MyPosition = new Vector2(this.transform.position.x, this.transform.position.z);

        float distance = (float)Mathf.Sqrt(Mathf.Pow(HeroPosition.x - MyPosition.x, 2) + Mathf.Pow(HeroPosition.y - MyPosition.y, 2));

        if(distance<=9f)
        {
            //if(!_anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            //{
                this.transform.LookAt(Hero.transform);  // 공격모션 외에는 유저 캐릭터 향해 보기
            //}

            float moveSpeed = this.speed;   // 이동 속도
            if(distance <= 1.5f)
            {
                isFaceHero = true;
            }
            else if (distance <= 5f)
            {
                _anim.SetInteger("State", (int)EnemyState.RUN);
                moveSpeed *= 2f;
                isFaceHero = false;
            }
            else
            {
                _anim.SetInteger("State", (int)EnemyState.WALK);
                moveSpeed *= 1.5f;
                isFaceHero = false;
            }

            if(!_anim.GetCurrentAnimatorStateInfo(0).IsName("Attack") && !_anim.GetCurrentAnimatorStateInfo(0).IsName("Damage") && !_anim.GetCurrentAnimatorStateInfo(0).IsName("Stand"))
            {
                this.transform.position = Vector3.MoveTowards(this.transform.position, Hero.transform.position, moveSpeed * Time.deltaTime);
            }
        }
        else if(distance > 9f)
        {
            _anim.SetInteger("State", (int)EnemyState.STAND);
        }
        

    }

    //유저와 부딪히면 곧바로 어택
    void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag.Equals("Hero"))
        {
            isFaceHero = true;
            if (isAttackCoolTime)
            {
                _anim.SetInteger("State", (int)EnemyState.STAND);
            }
            else
            {
                FaceHeroAction();
            }
        }

        
    }

    void FaceHeroAction()   // 마주 했을때 액션
    {
        if (isFaceHero)
        {
            if (isAttackCoolTime)
            {
                _anim.SetInteger("State", (int)EnemyState.STAND);
            }
            else
            {
                _anim.SetInteger("State", (int)EnemyState.ATTACK);
                StartCoroutine(AttackCoolTime());

            }

        }
    }

    IEnumerator AttackCoolTime()
    {
        yield return null;

        isAttackCoolTime = true;

        yield return new WaitForSeconds(2f/attackSpeed);

        isAttackCoolTime = false;
    }

    public void NormalAttacked()  // 기본 공격을 당했을때
    {
        if(!isNormalAttacked) // 공격을 받았다면
        {
            this.GetComponent<AudioSource>().PlayOneShot(SoundManager.Instance.EffectSoundList[0]);
            isNormalAttacked = true;
            remainHp -= 10;
            if(HeroScript.Instance.isTimeAttackMode)
            {
                HeroScript.Instance.timeAttackDemage += 10;
            }
            DamagePrintHud(10);
            StartCoroutine(AttackedCoolTime());
        }
    }

    //유저 캐릭터가 한번 공격했을때 한번만 피격효과내기위해서
    IEnumerator AttackedCoolTime()
    {
        BoxCollider SwordColider = AttackScript.Instance.GetSwordColider;
        while(true)
        {
            yield return null;
            if(SwordColider.enabled == false)
            {
                isNormalAttacked = false;
                break;
            }
        }
    }

    //스킬공격을 받았다면
    public void SkillAttacked(int damage)
    {
        this.GetComponent<AudioSource>().PlayOneShot(SoundManager.Instance.EffectSoundList[0]);
        remainHp -= damage;
        if(HeroScript.Instance.isTimeAttackMode)
        {
            HeroScript.Instance.timeAttackDemage += damage;
        }
        isSkillAttacked = true;
        DamagePrintHud(damage);
    }

    void DamagePrintHud(int demage)
    {
        gb_DemageHud.GetComponent<HUDText>().Add(demage.ToString(), Color.red, 0.5f);
        this.GetComponentInChildren<ParticleSystem>().Play();
    }

    void OnDestroy()
    {
        Destroy(gb_HudBar);
        Destroy(gb_DemageHud);
        this.gb_HudBar = null;
        this.gb_DemageHud = null;
    }

}
