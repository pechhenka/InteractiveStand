using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

    [Tooltip("UI Elements")]
    public Image Background;
    public Image[] BTNBackgrounds;
    public Image[] BTNBacks;
    public Text TimeUI;
    public Text DateUI;

	void Start () {
        RefreshUI(false);
    }

    void FixedUpdate()
    {
        TimeUI.text = DateTime.Now.ToString("HH:mm");
        DateUI.text = DateTime.Now.ToString("dd.MM.yyyy");
    }

    void RefreshUI(bool BlindMode)
    {
        Background.sprite = Data.Instance.Background;

        foreach(var BTNBG in BTNBackgrounds)
            BTNBG.sprite = Data.Instance.BTNBackground;
        foreach (var BTNBG in BTNBacks)
            BTNBG.sprite = Data.Instance.BTNBack;

        if (BlindMode)
        {
            Background.color = Color.black;
            foreach (var BTNBG in BTNBackgrounds)
                BTNBG.color = Color.black;
            foreach (var BTNBG in BTNBacks)
                BTNBG.color = Color.black;
        }
        else
        {
            Background.color = Color.white;
            foreach (var BTNBG in BTNBackgrounds)
                BTNBG.color = Color.white;
            foreach (var BTNBG in BTNBacks)
                BTNBG.color = Color.white;
        }
    }

    public void OnClickBlindMode()
    {
        bool Result = Data.Instance.Season != "Blind";
        Data.Instance.Refresh(Result);
        RefreshUI(Result);
    }
}
