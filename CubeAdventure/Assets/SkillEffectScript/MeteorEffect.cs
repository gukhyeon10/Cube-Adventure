using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorEffect : MonoBehaviour {

    private void OnParticleCollision(GameObject other)
    {
        if(other.transform.tag.Equals("Hero"))
        {
            HeroScript.Instance.AttackedBossSkill(25, "Meteor");

        }
        else
        {
            SkillManager.Instance.PlayHitEffect("hitGroundFireEffect", other.transform.position);
        }
    }
}
