using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightIndicator : MonoBehaviour {

    private Light lightSource;

    private void Start() {
        lightSource = GetComponent<Light>();
    }

    public void OnTriggerStay(Collider other) {

        //If the player or the players body is within distance to the light source then calculate the players brightness level depending on how far away it is 
        if (other.tag == "Player") {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, (other.transform.position - transform.position), out hit)) {
                if (hit.collider.tag == "Player")
                {
                    other.gameObject.GetComponent<PlayerController>().SetBrightnessLevel(Vector3.Distance(transform.position, other.transform.position) / lightSource.range);
                }
                if (hit.collider.tag == "PlayerBody")
                {
                    other.gameObject.GetComponent<PlayerController>().SetBrightnessLevel(Vector3.Distance(transform.position, other.transform.position) / lightSource.range);
                }
                else
                {
                    other.gameObject.GetComponent<PlayerController>().SetBrightnessLevel(1.0f);
                }
            }
        }
    }

    //If the player exits the light source then assume they are in the dark and are barely visible
    public void OnTriggerExit(Collider other) {
        if(other.tag == "Player") {
            other.gameObject.GetComponent<PlayerController>().SetBrightnessLevel(1.0f);
        }
    }
}