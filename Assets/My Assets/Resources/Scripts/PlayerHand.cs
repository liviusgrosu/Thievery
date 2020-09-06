using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerHand : NetworkBehaviour
{

    private OVRInput.Controller controller = OVRInput.Controller.LTouch;
    private GameObject grabbedObject, grabbedDoor, grabbedChest, grabbedBall, contactPoint;
    private bool isGrabbing, hasLetGo;
    private PlayerController playerController;

    private LineRenderer lr;
    private RaycastHit ray;

    public float teleportDistance;
    public Material validTeleportMat, invalidTeleportMat;

    private bool canTeleport;
    private Vector3 teleportPoint;
    private void Start()
    {
        playerController = transform.parent.GetComponent<PlayerController>();
        lr = GetComponent<LineRenderer>();
        lr.enabled = false;
    }

    private void Update()
    {
        //Update the hands position and rotation with respect to the Oculus hand controller
        OVRInput.Update();
        transform.localPosition = OVRInput.GetLocalControllerPosition(controller) + new Vector3(0, 0.5f, 0);
        transform.localRotation = OVRInput.GetLocalControllerRotation(controller);


        //If the joint of the grabbed object is destroyed then the player is not longer holding it
        if (grabbedObject != null && grabbedObject.GetComponent<ConfigurableJoint>() == null)
            hasLetGo = true;
        
        //Player is grabbing if they trigger the index button on the controller
        if (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger))
            isGrabbing = true;

        //Player grip trigger
        if(OVRInput.Get(OVRInput.Button.PrimaryHandTrigger))
        {
            //Enable the line renderer
            lr.enabled = true;
            lr.SetPosition(0, transform.position);
            
            if(Physics.Raycast(transform.position, transform.forward, out ray, 20f))
            {
                //If the player is pointing to a valid spot or the distance is short enough then prepare for a teleportation to that very spot
                if (Vector3.Distance(transform.position, ray.point) < teleportDistance && (ray.collider.tag == "Wood" || ray.collider.tag == "Stone" || ray.collider.tag == "Carpet"))
                {
                    canTeleport = true;
                    teleportPoint = ray.point;
                    
                    if(playerController.GetCrouching()) teleportPoint += new Vector3(0, transform.parent.GetComponent<CapsuleCollider>().height / 2f + 0.1f, 0);
                    else teleportPoint += new Vector3(0, transform.parent.GetComponent<CapsuleCollider>().height / 4f  + 0.1f, 0);
                    lr.material = validTeleportMat; //Set the line renderer to green
                }
                //However if it does not any of those 2 conditions then the player cannot prepare for a teleportation to the desired spot
                else
                {
                    canTeleport = false;
                    lr.material = invalidTeleportMat;
                }

                lr.SetPosition(1, ray.point);
            }
        }
        else 
        {
            //If the palyer lets go of the grip button and a teleportation spot has been prepared then teleport the player to that desired spot
            if(canTeleport)
            {
                transform.parent.transform.position = teleportPoint;
                canTeleport = false;
            }
            lr.enabled = false;
        }

        //If the player is grabbing onto an object and lets go of it, 
        //get rid of its joint and give a velocity depending on the 
        //controllers velocity 
        if (!OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger))
        {
            isGrabbing = false;
            hasLetGo = false;
            if (grabbedObject != null)
            {
                Destroy(grabbedObject.GetComponent<ConfigurableJoint>());
                grabbedObject.GetComponent<Rigidbody>().velocity = OVRInput.GetLocalControllerVelocity(controller);
                grabbedObject = null;
            }
            if(grabbedDoor != null)
            {
                grabbedDoor.GetComponent<Door>().DettachToDoor();
                grabbedDoor.gameObject.GetComponent<BoxCollider>().enabled = true;
                grabbedDoor = null;
            }
            if (grabbedChest != null)
            {
                grabbedChest.GetComponent<Door2>().DettachToDoor();
                grabbedChest.gameObject.GetComponent<BoxCollider>().enabled = true;
                grabbedChest = null;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {

        //If the player is wanting to grab onto a loot object then...
        //1) assign it to be the grabbed loot 
        //2) give it a configurable joint component 
        //3) lock its motion and angular motion 
        //4) set its break force depending on whats being grabbed
        if (other.gameObject.tag.Contains("Grabbable") && isGrabbing && !hasLetGo)
        {
            grabbedObject = other.gameObject;

            if (grabbedObject.GetComponent<ConfigurableJoint>() == null) {
                grabbedObject.AddComponent<ConfigurableJoint>();

                grabbedObject.GetComponent<ConfigurableJoint>().connectedBody = GetComponent<Rigidbody>();

                grabbedObject.GetComponent<ConfigurableJoint>().xMotion = ConfigurableJointMotion.Locked;
                grabbedObject.GetComponent<ConfigurableJoint>().yMotion = ConfigurableJointMotion.Locked;
                grabbedObject.GetComponent<ConfigurableJoint>().zMotion = ConfigurableJointMotion.Locked;

                grabbedObject.GetComponent<ConfigurableJoint>().angularXMotion = ConfigurableJointMotion.Locked;
                grabbedObject.GetComponent<ConfigurableJoint>().angularYMotion = ConfigurableJointMotion.Locked;
                grabbedObject.GetComponent<ConfigurableJoint>().angularZMotion = ConfigurableJointMotion.Locked;

                grabbedObject.GetComponent<ConfigurableJoint>().projectionMode = JointProjectionMode.PositionAndRotation;

                grabbedObject.GetComponent<ConfigurableJoint>().breakForce = Mathf.Infinity;

            }
        }

        //If its a door then tell the door that its being attached to
        if (other.gameObject.tag == "Door")
        {
            if (isGrabbing)
            {
                other.gameObject.GetComponent<Door>().AttachToDoor(this.gameObject);
                other.gameObject.GetComponent<BoxCollider>().enabled = false;
                grabbedDoor = other.gameObject;
            }
        }

        //If its a chest then tell the chest that its being attached to
        if (other.gameObject.tag == "Door2")
        {
            if (isGrabbing)
            {
                other.gameObject.GetComponent<Door2>().AttachToDoor(this.gameObject);
                other.gameObject.GetComponent<BoxCollider>().enabled = false;
                grabbedChest = other.gameObject;
            }
        }
    }

    //Check if the player is grabbing onto an object
    public bool IsGrabbingOntoObj()
    {
        return grabbedObject != null ? true : false;
    }
}