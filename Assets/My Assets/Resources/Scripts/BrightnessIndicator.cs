using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BrightnessIndicator : MonoBehaviour {

    [Header("Symbols")]
    public Sprite[] brightnessLevelsImgs;
    [Space(10)]
    public PlayerController player;

    private Image currentBrightnessImgLvl;

    private void Start() {
        currentBrightnessImgLvl = GetComponent<Image>();
    }

    void Update() {
        //Set the appropriate brightness sprite with respect to the players brightness level
        currentBrightnessImgLvl.sprite = brightnessLevelsImgs[player.GetBrightnessLevel() - 1];
    }
}

