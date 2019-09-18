using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionManager : MonoBehaviour {

    bool isBgmPlay = true;
    bool isEffectSoundPlay = true;
    bool isEffectPrint = true;

    [SerializeField]
    GameObject gb_OptionDialog;

    public void OptionDialogActive()
    {
        gb_OptionDialog.SetActive(!(gb_OptionDialog.gameObject.activeSelf));
    }

	public void GameSave()
    {
        GameMainManager.Instance.GameDataSave();
    }
}
