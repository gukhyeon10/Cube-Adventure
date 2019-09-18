using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillEffect : MonoBehaviour {

    StatManager statManager;

    [SerializeField]
    GameObject Hero;

    [SerializeField]
    GameObject[] skillParticlePrefabs;

    List<bool> CoolTimeList = new List<bool>();

    [SerializeField]
    GameObject[] skillHitEffectPrefabs;

    [SerializeField]
    GameObject gb_SkillParent;

    void Start()
    {
        for (int i = 0; i <= skillParticlePrefabs.Length; i++)
        {
            CoolTimeList.Add(false);
        }

        statManager = StatManager.Instance;

    }

    public bool SkillEffectQuarter(int skillNo)
    {
        if(CoolTimeList[skillNo])
        {
            Debug.Log(" 스킬 쿨타임 중입니다.. ");
            return false;
        }
        else
        {

            switch (skillNo)
            {
                case (int)SkillNumber.ATTACK_STRENGTH:
                    {
                        GameObject AttackStrength = Instantiate(skillParticlePrefabs[skillNo], Hero.transform.position + Vector3.up, Quaternion.identity, Hero.transform);
                        StartCoroutine(AttackStrengthCorutine());
                        break;
                    }
                case (int)SkillNumber.AGILITY_STRENGTH:
                    {
                        GameObject AgilityStrength = Instantiate(skillParticlePrefabs[skillNo], Hero.transform.position + Vector3.up, Quaternion.identity, Hero.transform);
                        StartCoroutine(AgilityStrengthCorutine());
                        break;
                    }
                case (int)SkillNumber.POISONING:
                    {
                        Instantiate(skillParticlePrefabs[skillNo], Hero.transform);
                        break;
                    }
                case (int)SkillNumber.FIREBALL:
                    {
                        AttackScript.Instance.PlayActiveSkill();
                        GameObject newFireBall =  Instantiate(skillParticlePrefabs[skillNo], Hero.transform.position + Hero.transform.forward *2f, Quaternion.identity, gb_SkillParent.transform);
                        newFireBall.transform.eulerAngles = Hero.transform.localEulerAngles;
                        break;
                    }
                case (int)SkillNumber.DEFENCE_STRENGTH:
                    {
                        GameObject DefenceStrength = Instantiate(skillParticlePrefabs[skillNo], Hero.transform);
                        StartCoroutine(DefenceStrengthCorutine());
                        break;
                    }
                case (int)SkillNumber.SPLINTER:
                    {
                        AttackScript.Instance.PlayActiveSkill();
                        Instantiate(skillParticlePrefabs[skillNo], Hero.transform.position, Quaternion.identity, gb_SkillParent.transform);
                        break;

                    }
            }

            StartCoroutine(CoolTimeCorutine(skillNo));
            return true;
        }
        
    }

    IEnumerator CoolTimeCorutine(int skillNo)
    {
        CoolTimeList[skillNo] = true;
        switch (skillNo)
        {
            case (int)SkillNumber.ATTACK_STRENGTH:
                {
                    yield return new WaitForSeconds(1f);
                    break;
                }
            case (int)SkillNumber.AGILITY_STRENGTH:
                {
                    yield return new WaitForSeconds(1f);
                    break;
                }
            case (int)SkillNumber.POISONING:
                {
                    yield return new WaitForSeconds(1f);
                    break;
                }
            case (int)SkillNumber.FIREBALL:
                {
                    yield return new WaitForSeconds(1f);
                    break;
                }
            case (int)SkillNumber.DEFENCE_STRENGTH:
                {
                    yield return new WaitForSeconds(1f);
                    break;
                }
            case (int)SkillNumber.SPLINTER:
                {
                    yield return new WaitForSeconds(1f);
                    break;
                }
        }
        CoolTimeList[skillNo] = false;
    }


    /*  요 아래는 스킬 효과 정의 */
    IEnumerator AttackStrengthCorutine()
    {
        float AttackUpgrade = StatManager.Instance.Attack *   // 공격력 *
                              SkillManager.Instance.dic_SkillData[(int)SkillNumber.ATTACK_STRENGTH].percent * 0.01f *  // 스킬 퍼센트 *
                              SkillManager.Instance.dic_LearnSkill[(int)SkillNumber.ATTACK_STRENGTH] * 0.25f; // 스킬레벨 / 4 
        statManager.buffAttack += AttackUpgrade;

        statManager.HeroStatUpdate();

        yield return new WaitForSeconds(10f);
        statManager.buffAttack -= AttackUpgrade;
        Debug.Log("버프 끝");
        statManager.HeroStatUpdate();
    }

    IEnumerator AgilityStrengthCorutine()
    {
        float speedUpgrade = statManager.Speed *   // 스피드 *
                              SkillManager.Instance.dic_SkillData[(int)SkillNumber.AGILITY_STRENGTH].percent * 0.01f *  // 스킬 퍼센트 *
                              SkillManager.Instance.dic_LearnSkill[(int)SkillNumber.AGILITY_STRENGTH] * 0.2f; // 스킬레벨 / 5
        statManager.buffSpeed += speedUpgrade;

        statManager.HeroStatUpdate();
        yield return new WaitForSeconds(10f);
        statManager.buffSpeed -= speedUpgrade;

        statManager.HeroStatUpdate();
    }

    IEnumerator DefenceStrengthCorutine()
    {
        float defenceUpgrade = statManager.Defence *   // 스피드 *
                              SkillManager.Instance.dic_SkillData[(int)SkillNumber.DEFENCE_STRENGTH].percent * 0.01f *  // 스킬 퍼센트 *
                              SkillManager.Instance.dic_LearnSkill[(int)SkillNumber.DEFENCE_STRENGTH] * 0.25f; // 스킬레벨 / 4
        statManager.buffDefence += defenceUpgrade;

        statManager.HeroStatUpdate();

        yield return new WaitForSeconds(10f);
        statManager.buffDefence -= defenceUpgrade;

        statManager.HeroStatUpdate();
    }


    // 보스 스킬 피격 이펙트
    public void PlayHitEffect(string hitEffectName, Vector3 hitPosition)
    {
        switch (hitEffectName)
        {
            case "hitHeroFireEffect":
                {
                    GameObject effect = Instantiate(skillHitEffectPrefabs[0], hitPosition + new Vector3(0f, 2f, 0f), Quaternion.identity, gb_SkillParent.transform);
                    effect.transform.eulerAngles = new Vector3(-58f, -76f, 0.4f);
                    break;
                }
            case "hitGroundFireEffect":
                {
                    Instantiate(skillHitEffectPrefabs[1], hitPosition + new Vector3(0f, 2f, 0f), Quaternion.identity, gb_SkillParent.transform);
                    
                    break;
                }
        }
    }

    public void ClearSkillParent()
    {
        if(gb_SkillParent.transform.childCount > 0)
        {
            foreach(Transform child in gb_SkillParent.transform)
            {
                Destroy(child.gameObject);
            }
        }
    }

    public Transform GetSkillParent
    {
        get
        {
            return this.gb_SkillParent.transform;
        }
    }

}
