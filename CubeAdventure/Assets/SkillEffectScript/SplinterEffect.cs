using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplinterEffect : MonoBehaviour {

    void OnParticleCollision(GameObject other)
    {
        if (other.transform.tag.Equals("Enemy"))
        {
            int demage = 10;
            demage = Random.Range(10, 15);
            other.GetComponent<EnemyScript>().SkillAttacked(demage);
        }
        else if(other.transform.tag.Equals("Boss"))
        {
            int demage = 8;
            demage = Random.Range(8, 13);
            other.GetComponent<BossScript>().SkillAttacked(demage);
        }
    }
}
