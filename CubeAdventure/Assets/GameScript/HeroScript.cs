using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroScript : MonoBehaviour {

    static private HeroScript _instance = null;

    static public HeroScript Instance
    {
        get
        {
            return _instance;
        }
    }

    float cameraHeight = 0f;
    float cameraDistanceX = 0f;
    float cameraDistanceZ = 0f;

    public bool isNpcDialog = false;
    public bool isTimeAttackMode = false;
    public int timeAttackKillCount = 0;
    public int timeAttackHit;
    public int timeAttackDemage;

    public bool isBossRaidMode = false;

    bool isDeath = false;

    [SerializeField]
    GameObject gb_LevelUpEffect;
    [SerializeField]
    Animator _anim;

    void Awake()
    {
        _instance = this;
        this.transform.tag = "Hero";
    }

    void OnDestroy()
    {
        _instance = null;    
    }

    // Use this for initialization
    void Start () {
        Camera.main.transform.position = new Vector3(1.47f, 12.57804f, -13.88f);
        Camera.main.transform.eulerAngles = new Vector3(52.21668f, -29.09866f, 0f);

        cameraHeight = Camera.main.transform.position.y - this.transform.position.y;
        cameraDistanceX = Camera.main.transform.position.x - this.transform.position.x + 2f;
        cameraDistanceZ = Camera.main.transform.position.z - this.transform.position.z + 6f;
  
    }
	
	// Update is called once per frame
	void LateUpdate () {
        FollowCamera();
        RemainHpCheck();
	}

    void FollowCamera()
    {
        Camera.main.transform.position = this.transform.position + new Vector3(cameraDistanceX, cameraHeight, cameraDistanceZ);
    }

    void RemainHpCheck()
    {
        if(StatManager.Instance.remainHp <= 0 && !isDeath && !isTimeAttackMode)
        {
            isDeath = true;
            StatManager.Instance.remainHp = 0;
            _anim.SetBool("dieCheck", true);
            StartCoroutine(DeathCorutine());
        }
    }

    IEnumerator DeathCorutine()
    {
        yield return new WaitForSeconds(4f);

        if(!isTimeAttackMode)
        {
            GameMainManager.Instance.PreviousLoadMap();

            this.transform.position = GameMainManager.Instance.HeroComeBack();

            //보스 방에서 죽으면 
            if(isBossRaidMode)
            {
                isBossRaidMode = false;
                GameMainManager.Instance.FailSpecialModeRespon();
                SoundManager.Instance.ChangeBgm(0);
            }
            SkillManager.Instance.ClearSkillEffect();

            StatManager.Instance.remainHp = StatManager.Instance.maxHp / 2;
            StatManager.Instance.remainMp = StatManager.Instance.maxMp / 2;
            StatManager.Instance.remainExp *= 7;
            StatManager.Instance.remainExp /= 10;

            _anim.SetBool("dieCheck", false);
            _anim.SetBool("hitCheck", false);

            isDeath = false;
        }
    }

    //게임오버
    public void DeathAnimation()
    {
        _anim.SetBool("dieCheck", true);
    }

    //부활 
    public void Resurrection()
    {
        _anim.SetBool("dieCheck", false);
        _anim.SetBool("hitCheck", false);
    }

    //충돌 체크
    void OnTriggerEnter(Collider other)
    {
        if(other.tag.Equals("Portal")) // 충돌한게 포탈일때
        {
            if(other.name.Substring(0, 4).Equals("Blue") == false)  // 소환 포탈이 아니라 통로 포탈이라면 (Blue로 시작하는 이름의 포탈은 소환 포탈)
            {

                string nextMapName = other.GetComponent<PortalInfo>().LinkMapName;
                int recallPortalNumber = other.GetComponent<PortalInfo>().portalNumber;

                //InitHudPanel은 여기서만 불러와야함 (gameuimanager 인스턴스가 awake하지않는 시점에서 해당함수를 호출 할 수 있어서)
                GameUI_Manager.Instance.InitHudPanel();

                GameMainManager.Instance.LoadMap(nextMapName);

                this.transform.position =  GameMainManager.Instance.HeroRecall(recallPortalNumber);
                this.transform.position = new Vector3(this.transform.position.x, 1f, this.transform.position.z);

                if(isBossRaidMode)
                {
                    this.transform.position = new Vector3(0f, 1f, 0f);

                    isBossRaidMode = false;
                    // 기본 Bgm
                    SoundManager.Instance.ChangeBgm(0);
                    GameUI_Manager.Instance.GameClearNotice();
                    Debug.Log("게임 끝!!");
                }
            }
        }

        if (other == null)
        {
            return;
        }

        if (other.transform.tag.Equals("EnemyWeapon")) // 충돌한게 적의 무기에 맞은거라면
        {
            if (other.transform.GetComponentInParent<EnemyScript>().isAttackCollider && other.transform.GetComponentInParent<EnemyScript>().isAttackSucces == false)
            {
                other.transform.GetComponentInParent<EnemyScript>().isAttackSucces = true;
                _anim.SetBool("hitCheck", true);
                StatManager.Instance.remainHp -= 10;
                timeAttackHit += 10;
                GameUI_Manager.Instance.UpdateHpBar();
            }
        }
        else if(other.transform.tag.Equals("BossSkeletonWeapon"))
        {
            if (other.transform.GetComponentInParent<BossScript>().isAttackCollider && other.transform.GetComponentInParent<BossScript>().isAttackSuccess == false)
            {
                other.transform.GetComponentInParent<BossScript>().isAttackSuccess = true;
                _anim.SetBool("hitCheck", true);
                StatManager.Instance.remainHp -= 15;
                timeAttackHit += 15;
                GameUI_Manager.Instance.UpdateHpBar();
            }
        }
        
    }
    
    //보스의 스킬 피격
    public void AttackedBossSkill(int demage, string skillName)
    {
        switch(skillName)
        {
            case "FireArrow":
                {
                    Debug.Log("불화살에 맞음");
                    StatManager.Instance.remainHp -= demage;
                    break;
                }
            case "Meteor":
                {
                    Debug.Log("메테오에 맞음");
                    if (!_anim.GetCurrentAnimatorStateInfo(0).IsName("GetHit_SwordShield"))
                    {
                        _anim.SetBool("hitCheck", true);
                    }

                    StatManager.Instance.remainHp -= 20;

                    SkillManager.Instance.PlayHitEffect("hitHeroFireEffect", this.transform.position);

                    break;
                }
        }
        GameUI_Manager.Instance.UpdateHpBar();
        
    }

    
    //레벨업 이펙트
    public void LevelUpEffect()
    {
        Instantiate(gb_LevelUpEffect, this.transform);
        this.GetComponent<AudioSource>().Play();
        StatManager.Instance.StatLevelUp();
    }
    


}
