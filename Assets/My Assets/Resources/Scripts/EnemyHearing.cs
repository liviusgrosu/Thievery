using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHearing : MonoBehaviour {

    public float minHearingRange;

    private void FixedUpdate()
    {
        transform.parent.GetComponent<EnemyAI>().CmdChangeHearingState(false, false, "noChange");
    }

    private void OnTriggerStay(Collider other) {
        //If the enemy ears intesect with the player sound sphere then execute this code
        if(other.tag == "Player Sound Sphere") {
            string temp = "";
            if (other.name.Contains(" Sound Sphere"))
                temp = other.name.Remove(other.name.IndexOf(" Sound Sphere"), " Sound Sphere".Length);

            //If the distance between the enemies ears and to the player sound sphere is long then the enemy barely hears the player
            if (Vector3.Distance(transform.position, other.transform.position) > minHearingRange)
                transform.parent.GetComponent<EnemyAI>().CmdChangeHearingState(false, true, temp);
            //If the distance between the enemies ears and to the player sound sphere is short then the enemy hears the player
            else if (Vector3.Distance(transform.position, other.transform.position) <= minHearingRange)
                transform.parent.GetComponent<EnemyAI>().CmdChangeHearingState(true, false, temp);
        }
    }
}
