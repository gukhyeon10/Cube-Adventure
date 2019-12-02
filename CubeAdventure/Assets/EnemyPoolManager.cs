using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPoolManager : MonoBehaviour {
    
    // 적 몬스터 풀링 시스템 객체
    static private EnemyPoolManager _instance = null;

    static public EnemyPoolManager Instance
    {
        get
        {
            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
    }

    void Start()
    {
    }

    public void EnemyRegen(GameObject Enemy)
    {
        StartCoroutine(EnemyRegenCorutine(Enemy));
    }
    

    IEnumerator EnemyRegenCorutine(GameObject Enemy)
    {
        yield return new WaitForSeconds(3f);

        //맵로드가 되면서 리젠하는 몬스터의 오브젝트가 Destroy되버렸을 경우 예외처리
        if(Enemy != null)
        {
            Enemy.transform.position = Enemy.GetComponent<EnemyScript>().initPosition;
            Enemy.SetActive(true);
        }
    }
}
