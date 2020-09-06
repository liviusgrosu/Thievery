using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door2 : MonoBehaviour {

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

        if (useClamps)
        {
            //Set the limits of the door 
            JointLimits limits = new JointLimits();
            limits.min = minRot;
            limits.max = maxRot;

            GetComponent<HingeJoint>().limits = limits;
            GetComponent<HingeJoint>().useLimits = true;
        }
    }

    private void FixedUpdate()
    {
        //Calcualte chest physics
        InteractDoor();
    }

    //Calculate chest rotation according to the angle of the hand and the chest itself 
    //Only do so if there exists a hand that is interacting with the chest
    void InteractDoor()
    {
        if (parent != null)
        {
            Vector3 targetDelta = parent.position - transform.position;
            //targetDelta.z = 0;

            float angleDiff = Vector3.Angle(transform.up, targetDelta);

            Vector3 cross = Vector3.Cross(transform.up, targetDelta);

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
