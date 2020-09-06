using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwagBag : MonoBehaviour {

    public Text scoreText;
    private GameStateController gameState;
    public PlayerHand hand;

    float newCamRot, curCamRot;

    int currScore = 0;

    void Start() {
        gameState = GameObject.Find("Game State Controller").GetComponent<GameStateController>();
        scoreText.text = "Score: " + 0;
    }

    //Update the total player score
    void FixedUpdate() {
        if (scoreText != null) scoreText.text = "Score: " + gameState.GetLootScore() + " / " + gameState.GetScoreToWin();
    }

    //Check if loot is colliding with the swag bag (player body), increase score, play a sound, and destroy the loot
    private void OnTriggerStay(Collider other) {
        if (other.tag == "GrabbableLoot" && hand.IsGrabbingOntoObj()) {
            currScore += other.gameObject.GetComponent<Loot>().score;
            gameState.CmdIncreaseScorePool(other.gameObject.GetComponent<Loot>().score);
            if(!GetComponent<AudioSource>().isPlaying) GetComponent<AudioSource>().Play();
            Destroy(other.gameObject);
        }
    }
}
