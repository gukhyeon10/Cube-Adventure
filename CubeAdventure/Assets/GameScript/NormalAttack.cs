using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalAttack : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag.Equals("Enemy"))
        {
            Debug.Log("Enemy Attack!");
            other.GetComponent<EnemyScript>().NormalAttacked();
        }
        else if(other.tag.Equals("Boss"))
        {
            Debug.Log("Boss Attack!");
            other.GetComponent<BossScript>().NormalAttacked();
        }
    }
}
