using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;

public class EnemyAI : NetworkBehaviour {
    [SyncVar] public bool canHearPlayer;
    [SyncVar] public bool canBarelyHearPlayer;

    [SyncVar] public bool canSeePlayer;
    [SyncVar] public bool canBarelySeePlayer;

    [SyncVar] public GameObject player;

    public Material[] stateMaterials;
    [SyncVar] public int state = 0;

    public MeshRenderer meshRenderer;

    private bool initState1 = false, initState2 = false, initState3 = false, initState4 = false;

    [Header("AI Timers")]
    public float cautTimer;
    public float investTimer;
    public float coolDownTimer;

    public float currCautTimer = 0.0f;
    public float currInvestTimer = 0.0f;
    public float currCoolDownTimer = 0.0f;

    [Header("AI Speeds")]
    public float patrolSpeed;
    public float investSpeed;
    public float aggroSpeed;

    private float currSpeed;

    [Header("AI Stationary Route")]
    public float[] stationaryRouteAngles;
    public float timeBetweenEachAngle;
    private int currentAngleIndex = 0;
    private Vector3 stationarySpot, stationaryDirection;
    private bool isCurrStationaryPatrolling;

    [SerializeField] string patrolPathName;
    private GameObject patrolPath; 

    private NavMeshAgent agent;
    private NavMeshPath navMeshPath;
    private Vector3 lastPlayerPos = new Vector3(-9999, -9999, -9999);

    private int currNode;
    private int maxNode;

    public bool isPatrolling;

    public AudioSource audioSource;
    public AudioClip cautSound, investSound, alertSound;

    private void Start()
    {
        //Start of in the first state: Patrolling
        if (!isServer) return;
        CmdChangestate(1);

        //Initate data depending if the enemy is patrolling or stationary
        if (patrolPathName != "")
        {
            //Intiate the patrolling path
            patrolPath = GameObject.Find(patrolPathName);
            maxNode = patrolPath.transform.childCount - 1;
            currNode = 0;
        }
        else
        {
            //Intiate the stationary routine 
            stationarySpot = transform.position;
            stationaryDirection = transform.forward;
            isCurrStationaryPatrolling = true;
            InvokeRepeating("TurnToStationaryDirection", timeBetweenEachAngle, timeBetweenEachAngle);
        }

        navMeshPath = new NavMeshPath();
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        //Calculate AI
        if (isServer)
        {
            RpcChangeMaterial();
            Calculatestate();
        }
    }

    void Calculatestate()
    {
        //Patrol State
        if (state == 1)
        {
            //State init
            if (!initState1)
            {
                initState1 = true;
            }

            //State Action
            currSpeed = patrolSpeed;
            if (isPatrolling) Patrol();
            else StationaryPatrol();

            if (currCoolDownTimer > 0.0f) currCoolDownTimer -= Time.deltaTime;

            //State Transitions
            if (player != null && currCoolDownTimer <= 0.0f)
            {
                //If the AI can barely hear the player or barely see the player and is visibile enough, then switch to the suspicious state
                if (canBarelyHearPlayer || (canBarelySeePlayer && player.GetComponent<PlayerController>().GetBrightnessLevel() >= 4))
                {
                    initState1 = false;
                    state = 2;
                    return;
                }
                //If the AI can hear the player then switch to investigative state
                if (canHearPlayer)
                {
                    state = 3;
                    initState1 = false;
                    return;
                }
                //If the AI sees the player and is visible enough then switch to the alert state
                if (canSeePlayer && player.GetComponent<PlayerController>().GetBrightnessLevel() >= 3)
                {
                    state = 4;
                    initState1 = false;
                    return;
                }
            }
        }
        //Cautious State
        if (state == 2)
        {
            //State Action
            currSpeed = patrolSpeed;
            if (isPatrolling) Patrol();
            else StationaryPatrol();

            //State init
            if (!initState2)
            {
                currCautTimer = cautTimer;
                currInvestTimer = investTimer;
                initState2 = true;
                PlaySound(0);
            }
            //If the enemy keeps barely hearing the player or barely seeing the player and is visible enough, then keep the enemy actively suspicous 
            if (player != null && (canBarelyHearPlayer || (canBarelySeePlayer && player.GetComponent<PlayerController>().GetBrightnessLevel() >= 4)))
                currCautTimer = cautTimer;

            float timeToDecrease = Time.deltaTime;

            currInvestTimer -= timeToDecrease;
            currCautTimer -= timeToDecrease;

            //State Transitions
            //If the enemy is still suspicious and the investigative timer has run out then go into the investigative state
            if (currInvestTimer <= 0.0f)
            {
                currCautTimer = 0.0f;
                currInvestTimer = 0.0f;
                initState2 = false;
                state = 3;
                return;
            }
            //If the enemy is still suspicious and the cautious timer has run out then go into the patrolling state
            if (currCautTimer <= 0.0f)
            {
                currCautTimer = 0.0f;
                currInvestTimer = 0.0f;
                CmdChangeHearingState(false, false, null);
                CmdChangeSeeingState(false, false, null);
                initState2 = false;

                state = 1;
                return;
            }
            
            //If the enemy hears the player then go to the investigative state
            if (canHearPlayer)
            {
                currCautTimer = 0.0f;
                currInvestTimer = 0.0f;
                initState2 = false;

                state = 3;
                return;
            }

            //If the AI sees the player and is visible enough then switch to the alert state
            if (canSeePlayer && player.GetComponent<PlayerController>().GetBrightnessLevel() >= 3)
            {
                currCautTimer = 0.0f;
                currInvestTimer = 0.0f;
                initState2 = false;
                
                state = 4;
                return;
            }
        }
        //Investigate State
        if (state == 3)
        {
            //State Action
            currSpeed = investSpeed;
            
            //State init
            if (!initState3)
            {
                PlaySound(1);
                //If the enemy was in a stationary patrol then stop that
                if (!isPatrolling)
                {
                    CancelInvoke("TurnToStationaryDirection");
                    isCurrStationaryPatrolling = false;
                }
                if (player != null && lastPlayerPos == new Vector3(-9999f, -9999f, -9999f)) lastPlayerPos = player.transform.position;
                currInvestTimer = investTimer;
                initState3 = true;
            }

            //If the enemy still barely hears or barely sees the player then update the last known player position
            if (player != null && canBarelyHearPlayer || canHearPlayer)
            {
                lastPlayerPos = player.transform.position;
                currInvestTimer = investTimer;
            }

            //Go to last player position
            RotateToPos(lastPlayerPos);
            SetDestinationAndMove(lastPlayerPos);
            CalculateNewPath();
            currInvestTimer -= Time.deltaTime;

            //State Transitions
            //If the enemy has lost patience with the investigation then go back to patrolling
            if (currInvestTimer <= 0.0f)
            {
                currCautTimer = 0.0f;
                currInvestTimer = 0.0f;
                initState3 = false;
                lastPlayerPos = new Vector3(-9999f, -9999f, -9999f);

                state = 1;
                return;
            }

            //If the enemy can see player and is visible enough then go into the alert state
            if (player != null && (canBarelySeePlayer || canSeePlayer) && (player.GetComponent<PlayerController>().GetBrightnessLevel() > 1 || Vector3.Distance(transform.position, player.transform.position) <= 0.8f))
            {
                initState3 = false;
                currInvestTimer = 0.0f;
                lastPlayerPos = new Vector3(-9999f, -9999f, -9999f);

                state = 4;
                return;
            }

            //If the AI sees the player then switch to the alert state
            if (canSeePlayer)
            {
                currCautTimer = 0.0f;
                currInvestTimer = 0.0f;
                initState3 = false;

                state = 4;
                return;
            }
        }
        //Aggresive State
        if (state == 4)
        {
            //State Action
            currSpeed = aggroSpeed;

            //State init
            if (!initState4)
            {
                PlaySound(2);
                if (player != null) lastPlayerPos = player.transform.position;
                initState4 = true;
            }

            //If the enemy was in a stationary patrol then stop that
            if (!isPatrolling)
            {
                CancelInvoke("TurnToStationaryDirection");
                isCurrStationaryPatrolling = false;
            }

            //If the enemy sees or hears then player then go the player
            if (player != null && canBarelySeePlayer || canSeePlayer || canBarelyHearPlayer || canHearPlayer)
            {
                lastPlayerPos = player.transform.position;
            }

            //Go to last player position
            RotateToPos(lastPlayerPos);
            SetDestinationAndMove(lastPlayerPos);
            CalculateNewPath();

            //If the enemy is close to last player position then go back to patrolling
            if (Vector3.Distance(lastPlayerPos, transform.position) < 1f)
            {
                initState4 = false;
                state = 1;

                currCoolDownTimer = coolDownTimer;

                //If the player is within grabbing distance to the enemy, then capture them
                if (Vector3.Distance(player.transform.position, lastPlayerPos) < 0.1f)
                    RpcSentPlayerToJail();

                return;
            }

            //State Transitions
            //Go back 
            if (Vector3.Distance(lastPlayerPos, transform.position) < 0.3f)
            {
                initState4 = false;
                state = 3;
                return;
            }
        }
    }


    //Update hearing states
    [Command]
    public void CmdChangeHearingState(bool hearingState, bool barelyHearingState, string playerName) {
        canHearPlayer = hearingState;
        canBarelyHearPlayer = barelyHearingState;

        if(playerName != "noChange")
            player = GameObject.Find(playerName);
    }

    //Update seeing states
    [Command]
    public void CmdChangeSeeingState(bool seeingState, bool barelySeeingState, string playerName) {
        canSeePlayer = seeingState;
        canBarelySeePlayer = barelySeeingState;

        if (playerName != "noChange")
            player = GameObject.Find(playerName);
    }

    //Update AI states
    [Command]
    public void CmdChangestate(int state)
    {
        this.state = state;
    }

    //Update enemy material with respect to the AI states
    [ClientRpc]
    public void RpcChangeMaterial()
    {
        if (meshRenderer.material != stateMaterials[state - 1]) meshRenderer.material = stateMaterials[state - 1];
    }

    //Send the player to the respawn area
    [ClientRpc]
    void RpcSentPlayerToJail()
    {
        if (player != null) player.GetComponent<PlayerController>().SendToJail();
    }

    //Update the Nav Mesh Agent destinations
    void SetDestinationAndMove(Vector3 pos)
    {
        Vector3 dest = pos;
        agent.SetDestination(dest);
        agent.speed = currSpeed;
    }

    //Calculate a new path for the Nav Mesh Agent
    bool CalculateNewPath()
    {
        agent.CalculatePath(lastPlayerPos, navMeshPath);
        if (navMeshPath.status != NavMeshPathStatus.PathComplete) { return false; }
        else { return true; }
    }

    //Rotate the enemy to a given position
    void RotateToPos(Vector3 target)
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(target - transform.position), 3.0f * Time.deltaTime);
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
    }

    //Function to tell the enemy to go to the next patrolling point
    //Enemy will keep patrolling until a state transition occurs
    void Patrol()
    {
        Vector3 nextNodePos = patrolPath.transform.GetChild(currNode).position;
        SetDestinationAndMove(nextNodePos);
        CalculateNewPath();
        RotateToPos(nextNodePos);

        if (Vector3.Distance(transform.position, nextNodePos) < 0.3f)
        {
            currNode++;
            if (currNode > maxNode)
                currNode = 0;
        }
    }

    //Function to tell the enemy to patrol at a stationary point
    //Enemy will keep patrolling until a state transition occurs
    void StationaryPatrol()
    {
        if (Vector3.Distance(transform.position, stationarySpot) > 0.5f)
        {
            SetDestinationAndMove(stationarySpot);
            CalculateNewPath();
            RotateToPos(stationarySpot);
        }
        else
        {

            if (transform.forward != stationaryDirection && !isCurrStationaryPatrolling)
                transform.forward = stationaryDirection;
            else
            {
                if (!isCurrStationaryPatrolling)
                {
                    isCurrStationaryPatrolling = true;
                    InvokeRepeating("TurnToStationaryDirection", timeBetweenEachAngle, timeBetweenEachAngle);
                }
            }
        }
    }

    //Turn enemy to the next viewing angle for stationary patrol 
    void TurnToStationaryDirection()
    {
        currentAngleIndex++;
        if (currentAngleIndex >= stationaryRouteAngles.Length) currentAngleIndex = 0;
        transform.eulerAngles += new Vector3(0, stationaryRouteAngles[currentAngleIndex], 0);
    }

    //Play the appropriate enemy sound depending on the state transition
    void PlaySound(int index)
    {
        if (audioSource.isPlaying) audioSource.Stop();
        switch(index)
        {
            case 0:
                audioSource.PlayOneShot(cautSound, 0.5f);
                break;
            case 1:
                audioSource.PlayOneShot(investSound, 0.5f);
                break;
            case 2:
                audioSource.PlayOneShot(alertSound, 0.5f);
                break;
        }
        
    }
}
