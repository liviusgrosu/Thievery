using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySight : MonoBehaviour {

    public float minSeeingRange;
    private RaycastHit ray;

    private void FixedUpdate()
    {
        transform.parent.GetComponent<EnemyAI>().CmdChangeSeeingState(false, false, "noChange");
        
    }

    private void OnTriggerStay(Collider other) {
        //If the enemy eyes intesect with the player then execute this code
        if (other.tag == "Player") {
            if (Physics.Raycast(transform.position, other.transform.position - transform.parent.transform.position, out ray, 10f))
            {
                if (ray.collider.tag == "Player" || ray.collider.tag == "PlayerBody")
                {
                    //If the distance between the enemies eyes and to the player is long then the enemy barely sees the player
                    if (Vector3.Distance(transform.position, other.transform.position) > minSeeingRange)
                        transform.parent.GetComponent<EnemyAI>().CmdChangeSeeingState(false, true, other.name);
                    //If the distance between the enemies eyes and to the player is long then the enemy sees the player
                    else if (Vector3.Distance(transform.position, other.transform.position) <= minSeeingRange)
                        transform.parent.GetComponent<EnemyAI>().CmdChangeSeeingState(true, false, other.name);
                }
            }
        }
    }
}
