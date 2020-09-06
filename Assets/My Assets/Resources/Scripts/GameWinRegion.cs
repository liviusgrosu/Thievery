using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameWinRegion : MonoBehaviour {
    public GameObject lastHint;

    //Confirm that the player is in the winning region and tell that to the game state controller
    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player")
        {
            if (lastHint.activeSelf) lastHint.SetActive(false);
            GameObject.Find("Game State Controller").GetComponent<GameStateController>().CmdAddPlayerToWinRegions(other.gameObject.name, true);
        }
    }

    //Confirm that the player is NOT in the winning region and tell that to the game state controller
    private void OnTriggerExit(Collider other) {
        if (other.tag == "Player")
            GameObject.Find("Game State Controller").GetComponent<GameStateController>().CmdAddPlayerToWinRegions(other.gameObject.name, false);
    }
}