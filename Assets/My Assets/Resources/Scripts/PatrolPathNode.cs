using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PatrolPathNode : MonoBehaviour {

    public Transform nextPoint;

    void Update()
    {
        //Draw the next patrol path node in edit mode
        Debug.DrawRay(transform.position, nextPoint.position - transform.position, Color.green);
    }
}
