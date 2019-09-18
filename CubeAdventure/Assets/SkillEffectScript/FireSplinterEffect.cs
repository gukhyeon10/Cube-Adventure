using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSplinterEffect : MonoBehaviour {

    private void OnParticleCollision(GameObject other)
    {
        if(other.transform.tag.Equals("Hero"))
        {
            int demage = Random.Range(5, 8);
            HeroScript.Instance.AttackedBossSkill(demage, "FireArrow");
        }
    }
}
