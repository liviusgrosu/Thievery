using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoudnessIndicator : MonoBehaviour {

    [Header("Symbols")]
    public Sprite[] soundLevelsImgs;
    [Space(10)]
    public PlayerSoundMixer mixer;

    private Image currentSoundImgLvl;

    private void Start() {
        currentSoundImgLvl = GetComponent<Image>();
    }

    //Display the appropriate brightness level of the player through a UI image
    void Update() {
        currentSoundImgLvl.sprite = soundLevelsImgs[mixer.GetSoundLevel() - 1];
    }
}
