using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    [SerializeField]
    AudioSource Audio;

    public AudioClip[] BgmList;
    public AudioClip[] EffectSoundList;

    bool isBgm;
    bool isEffectSound;

    static private SoundManager _instance = null;

    static public SoundManager Instance
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
    public void MuteBgm()
    {

    }

    public void MuteEffectSound()
    {
        isEffectSound = !isEffectSound;
    }

    public void ChangeBgm(int bgmNo)
    {
        Audio.clip = BgmList[bgmNo];
        Audio.Play();
    }
    
}
