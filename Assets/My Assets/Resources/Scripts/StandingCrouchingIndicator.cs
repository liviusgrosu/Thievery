using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StandingCrouchingIndicator : MonoBehaviour {

    [Header("Symbols")]
    public Sprite standingSymbol;
    public Sprite crouchingSymbol;
    [Space(10)]
    public PlayerController player;

    private Image currentImg;

    private void Start() {
        currentImg = GetComponent<Image>();
    }

    //Display through UI the appropriate player standing/crouching level
    void Update() {
        if (player.GetCrouching() && currentImg != crouchingSymbol) currentImg.sprite = crouchingSymbol;
        if (!player.GetCrouching() && currentImg != standingSymbol) currentImg.sprite = standingSymbol;
    }
}

