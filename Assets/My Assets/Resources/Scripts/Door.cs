using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {

    public Transform parent;
    private Rigidbody rb;
    public GameStateController gameSC;

    public Vector3 lastPos;
    public Quaternion lastRot;

    public bool useClamps;
    public float minRot, maxRot;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.isKinematic = true;

        //Set the limits of the door 
        JointLimits limits = new JointLimits();
        limits.min = minRot;
        limits.max = maxRot;

        GetComponent<HingeJoint>().limits = limits;
        if(useClamps) GetComponent<HingeJoint>().useLimits = true;
    }

    private void FixedUpdate()
    {
        //Calcualte door physics
        InteractDoor();
    }

    //Calculate door rotation according to the angle of the hand and the door itself 
    //Only do so if there exists a hand that is interacting with the door
    void InteractDoor()
    {
        if (parent != null)
        {
            Vector3 targetDelta = parent.position - transform.position;
            targetDelta.y = 0;

            float angleDiff = Vector3.Angle(transform.forward, targetDelta);

            Vector3 cross = Vector3.Cross(transform.forward, targetDelta);

            GetComponent<Rigidbody>().angularVelocity = cross * angleDiff * 50f;
        }

    }

    //Attach the hand to door
    //Turn on/off the correct physics parameters
    //Play a door sound
    public void AttachToDoor(GameObject obj)
    {
        parent = obj.transform;
        rb.useGravity = true;
        rb.isKinematic = false;
        if (!GetComponent<AudioSource>().isPlaying) GetComponent<AudioSource>().Play();
    }

    //Detach the hand to door
    //Turn on/off the correct physics parameters
    public void DettachToDoor()
    {
        parent = null;
        rb.useGravity = false;
        rb.isKinematic = true;
    }
}
