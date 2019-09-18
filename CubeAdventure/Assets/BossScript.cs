using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossScript : MonoBehaviour {

    public int remainHp, maxHp;
    public bool isNormalAttacked = false;
    public bool isAttackCollider = false;
    public bool isAttackSuccess = false;
    GameObject Hero;

    Animator _anim;

    GameObject SkillRangePrefab;
    GameObject FireSplinterPrefab;
    GameObject MeteorPrefab;

    GameObject BossHpBar;

    Transform SkillParent;

    // Use this for initialization
    void Start () {
        maxHp = 300;
        remainHp = maxHp;

        SkillParent = SkillManager.Instance.GetSkillEffectIns.GetSkillParent;

        Hero = HeroScript.Instance.transform.gameObject;

        _anim = this.GetComponent<Animator>();

        BossHpBar = GameUI_Manager.Instance.BossHpBar();
        BossHpBar.transform.localScale = new Vector3(5f, 2f, 1f);
        BossHpBar.transform.localPosition = new Vector3(-10f, -25f);

        LoadSkillPrefab();

        StartCoroutine(BossGrowBig());

        HpBarRemainUpdate();
	}

    // 스킬 이펙트 오브젝트 로드
    void LoadSkillPrefab()
    {
        SkillRangePrefab = Resources.Load("BossSkill/BossSkillRange") as GameObject;
        FireSplinterPrefab = Resources.Load("BossSkill/FireSplinter") as GameObject;
        MeteorPrefab = Resources.Load("BossSkill/Meteor") as GameObject;
    }
	
    // 커지는 모션
    IEnumerator BossGrowBig()
    {
        while(this.transform.localScale.x <= 50f)
        {
            this.transform.localScale += Vector3.one;
            yield return new WaitForSeconds(0.01f);
        }

        // 보스 공격 패턴 시작
        StartCoroutine(AttackPattern());
        
    }

	// Update is called once per frame
	void Update () {

        if(!_anim.GetCurrentAnimatorStateInfo(0).IsName("Attack") && remainHp > 0)
        {
            //캐릭터를 향해 부드럽게 회전
            //this.transform.LookAt(Hero.transform);
            Vector3 vec = Hero.transform.position - this.transform.position;
            Vector3 look = Vector3.Slerp(this.transform.forward, vec.normalized, Time.deltaTime * 4f);

            this.transform.rotation = Quaternion.LookRotation(look, Vector3.up);
        }

        if (_anim.GetCurrentAnimatorStateInfo(0).IsName("Attack") && _anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.3f && _anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.5f)
        {
            isAttackCollider = true;
        }
        else
        {
            isAttackCollider = false;
            isAttackSuccess = false;
        }

        // 보스 CLEAR
        if(remainHp <= 0)
        {
            Destroy(BossHpBar);
            BossHpBar = null;

            _anim.SetInteger("State", (int)EnemyState.DEATH);
            StartCoroutine(BossDownGrow());
        }
    }
    

    IEnumerator BossDownGrow()
    {
        yield return new WaitForSeconds(4f);
        while (this.transform.localScale.x >= 3f)
            {
                this.transform.localScale -= Vector3.one;
                yield return new WaitForSeconds(0.01f);
            }
        StopCoroutine("AttackPattern");

        Destroy(this.gameObject);
    }

    IEnumerator AttackPattern()
    {
        while(true)
        {
            NormalAttack();
            yield return new WaitForSeconds(2f);
            MeteorAttack();
            yield return new WaitForSeconds(2f);
            FireSplinter();
            MeteorAttack2();
            yield return new WaitForSeconds(2f);
            MeteorAttack();
            yield return new WaitForSeconds(2f);
            FireSplinter();
        }
    }

    void NormalAttack()
    {
        _anim.SetInteger("State", (int)EnemyState.ATTACK);
        StartCoroutine(StandState());
    }

    IEnumerator StandState()
    {
        yield return new WaitForSeconds(0.5f);
        _anim.SetInteger("State", (int)EnemyState.STAND);
    }

    //불화살 
    void FireSplinter()
    {
        for(int i =0; i<3; i++)
        {
            StartCoroutine(FireSplinterCoroutine());
        }
    }

    IEnumerator FireSplinterCoroutine()
    {
        Vector3 Position = Vector3.zero;
        Position.y = 1.5f;
        //-6 ~ 6 범위내로 소환
        Position.x = Random.Range(-6, 6);
        Position.z = Random.Range(-6, 6);
        GameObject rangeObject = Instantiate(SkillRangePrefab, Position, Quaternion.identity, SkillParent);
        rangeObject.transform.eulerAngles = new Vector3(-90f, 0f, 0f);

        yield return new WaitForSeconds(3f);

        GameObject FireSplinter = Instantiate(FireSplinterPrefab, Position, Quaternion.identity, SkillParent);
        Destroy(rangeObject);

    }

    void MeteorAttack()
    {
            StartCoroutine(MeteorCoroutine(Hero.transform.position));   
    }

    void MeteorAttack2()
    {
        StartCoroutine(MeteorCoroutine2());
    }

    IEnumerator MeteorCoroutine(Vector3 pos)
    {
        Vector3 Position = Vector3.zero;
        Position = pos;
        Position.y = 1.5f;

        GameObject rangeObject = Instantiate(SkillRangePrefab, Position, Quaternion.identity, SkillParent);
        rangeObject.transform.eulerAngles = new Vector3(-90f, 0f, 0f);

        yield return new WaitForSeconds(3f);

        Position.x = Position.x - 23 * Mathf.Cos(45f);
        Position.y = Position.y + 15 * Mathf.Sin(45f);
        GameObject Meteor = Instantiate(MeteorPrefab, Position, Quaternion.identity, SkillParent);
        Meteor.transform.eulerAngles = new Vector3(45f, 90f, 90f);

        Destroy(rangeObject);

    }

    IEnumerator MeteorCoroutine2()
    {
        for(int i=0; i<4; i++)
        {
            Vector3 pos = new Vector3(-8 + i * 4, 0, 2);
            StartCoroutine(MeteorCoroutine(pos));
            yield return new WaitForSeconds(0.5f);
        }
    }

    void PoisonAttack()
    {

    }

    public void NormalAttacked()  // 기본 공격을 당했을때
    {
        if (!isNormalAttacked) // 공격을 받았다면
        {
            StartCoroutine(AttackedCoolTime());
        }
    }

    //유저 캐릭터가 한번 공격했을때 한번만 피격효과내기위해서
    IEnumerator AttackedCoolTime()
    {

        yield return new WaitForSeconds(0.15f);

        this.GetComponent<AudioSource>().PlayOneShot(SoundManager.Instance.EffectSoundList[0]);
        isNormalAttacked = true;
        remainHp -= 10;

        DamagePrintHud(5);

        BoxCollider SwordColider = AttackScript.Instance.GetSwordColider;
        while (true)
        {
            yield return null;
            if (SwordColider.enabled == false)
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
        DamagePrintHud(damage);
    }

    //데미지 HUD 출력
    void DamagePrintHud(int demage)
    {

        //gb_DemageHud.GetComponent<HUDText>().Add(demage.ToString(), Color.red, 0.5f);
        this.GetComponentInChildren<ParticleSystem>().Play();
        HpBarRemainUpdate();

    }

    void HpBarRemainUpdate()
    {
        if(BossHpBar != null)
        {
            BossHpBar.GetComponent<UISlider>().value = (float)remainHp / (float)maxHp;
        }
    }


    void OnDestroy()
    {
        if(BossHpBar != null)
        {
            Destroy(BossHpBar);
        }
    }
}
