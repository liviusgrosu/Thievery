using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFootController : MonoBehaviour
{

    public PlayerController player;

    public GameObject leftFoot;
    public GameObject rightFoot;

    public float slowCrouchSync;
    public float crouchSync;
    public float slowSync;
    public float walkingSync;
    public float runningSync;

    private bool leftFootActive, rightFootActive, isCycling;

    private void Start()
    {
        //Start with the left foot active
        leftFootActive = true;
        leftFoot.SetActive(leftFootActive);
        rightFoot.SetActive(leftFootActive);
    }

    private void Update()
    {
        //Cycle between each foot if the player is moving 
        if (player.IsPlayerMoving() && !isCycling) StartCoroutine(CycleFeet());
    }

    //Enumerator that cycles between each active foot 
    IEnumerator CycleFeet()
    {
        isCycling = true;

        //Left foot is now active and it checks for the floor collider and plays a sound
        if (leftFootActive)
        {
            leftFootActive = false;
            rightFootActive = true;
            rightFoot.GetComponent<PlayerFoot>().PlayAgain();
        }

        //Right foot is now active and it checks for the floor collider and plays a sound
        else if (rightFootActive)
        {
            leftFootActive = true;
            rightFootActive = false;
            leftFoot.GetComponent<PlayerFoot>().PlayAgain();
        }

        //Wait 1 second in between each foot activation
        rightFoot.SetActive(rightFootActive);
        leftFoot.SetActive(leftFootActive);
        yield return new WaitForSeconds(1.0f);
        isCycling = false;
    }

    //Depending on the movement of the player, change the cycle speed between each foot
    //This only works if the movement is analog and not teleportation
    float GetCycleSpeed()
    {
        switch (player.GetSpeedLevel())
        {
            case 1: return slowCrouchSync;
            case 2: return crouchSync;
            case 3: return slowSync;
            case 4: return walkingSync;
            case 5: return runningSync;
            default: return 1.0f;
        }
    }
}
