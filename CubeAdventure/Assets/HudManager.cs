using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HudManager : MonoBehaviour {

    [SerializeField]
    GameObject DemageHud_Prefab;

    static private HudManager _instance = null;

    static public HudManager Instance
    {
        get
        {
            return _instance;
        }
    }

    void Awake()
    {
        _instance = this;

    }

    public GameObject MakeDemageHud()
    {
        return NGUITools.AddChild(this.gameObject, DemageHud_Prefab);
    }
}
