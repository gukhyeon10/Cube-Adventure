using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallEffect : MonoBehaviour {

    private void OnParticleCollision(GameObject other)
    {
        if (other.transform.tag.Equals("Enemy"))
        {
            Debug.Log(other.transform.name);
            other.GetComponent<EnemyScript>().SkillAttacked(20);
        }
        else if (other.transform.tag.Equals("Boss"))
        {
            int demage = 17;
            demage = Random.Range(15, 25);
            other.GetComponent<BossScript>().SkillAttacked(demage);
        }
    }
}
