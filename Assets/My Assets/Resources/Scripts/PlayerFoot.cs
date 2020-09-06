using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFoot : MonoBehaviour {

    public PlayerSoundMixer soundMixer;
    bool played;

    //Reset the sound mixer trigger
    public void PlayAgain() {
        played = false;
    }

    //Check what collider the player is standing on and depending on the tag, play the appropriate sound that it would make
    void OnTriggerStay(Collider other) {
        if (!played) {
            if (other.tag == "Wood") {
                soundMixer.TriggerSound(1);
                played = true;
            }
            if(other.tag == "Stone")
            {
                soundMixer.TriggerSound(2);
                played = true;
            }
            if (other.tag == "Carpet")
            {
                soundMixer.TriggerSound(3);
                played = true;
            }
        }
    }
}
