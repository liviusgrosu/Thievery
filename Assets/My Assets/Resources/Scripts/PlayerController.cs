using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour
{

    public Camera playerCamera;
    public GameObject playerHand;
    public GameObject soundSpherePrefab;
    private GameObject soundSphereInsta;

    public float maxSpeed = 5.0f;
    private float cameraToTorsoOffset;
    private float setSpeed;

    private GameStateController gameSC;

    [Space(10)]
    [Header("Feet and Crouching")]
    public float distanceToTriggerCrouch;
    public GameObject feet;
    public float crouchHeight;
    public float standingHeight;

    private bool isCrouching, isRunning;

    //0 - 1 (lowest - highest)
    private float brightness = 0f;
    //1 - 5 (lowest - highest)
    public int brightnessLevel = 1;
    public int brightnessLevelLength;

    public int loudnessLevelLength;

    private Vector3 lastPos;
    private Vector3 velocity;

    public GameObject canvas;
    public Text gameStateText;
    public Fader fader;

    public float turnAngle, moveUnit;
    private bool turnActive, moveActive;

    private Transform playerSpawn;
    

    void Start() {
        gameSC = GameObject.Find("Game State Controller").GetComponent<GameStateController>();
        gameSC.CmdAddPlayer(this.gameObject.name);

        playerSpawn = GameObject.Find("Player Spawn 1").transform;

        fader.fadeIn();
    }

    void Update()
    {
        //OBSOLETE CODE but necessary 
        if (!isLocalPlayer)
        {
            Destroy(canvas);
            Destroy(playerCamera);
            return;
        } 

        OVRInput.Update();
        //Move the player around with the left thumbstick
        Vector2 val = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);

        float playerY = transform.position.y;

        //Right analog movement, then incrementaly rotate the player turn right
        if (val.x >= 0.9f && !turnActive)
        {
            turnActive = true;
            transform.Rotate(0, turnAngle, 0);
        }

        //Left analog movement, then incrementaly rotate the player left
        if (val.x <= -0.9f && !turnActive)
        {
            turnActive = true;
            transform.Rotate(0, -turnAngle, 0);
        }

        if(val.x == 0)
        {
            turnActive = false;
        }

        //Up analog movement, then incrementaly translate the player forwards
        if (val.y >= 0.9f && !moveActive)
        {
            moveActive = true;
            Vector3 tempVec = new Vector3(playerCamera.transform.forward.x, 0, playerCamera.transform.forward.z);
            transform.position = transform.position + tempVec * moveUnit;
        }

        //Down analog movement, then incrementaly translate the player backwards
        if (val.y <= -0.9f && !moveActive)
        {
            moveActive = true;
            Vector3 tempVec = new Vector3(playerCamera.transform.forward.x, 0, playerCamera.transform.forward.z);
            transform.position = transform.position + tempVec * -moveUnit;
        }

        if(val.y == 0)
        {
            moveActive = false;
        }
        
        //Translate the player with the new position
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);

        //Restart the game if the player has been caught and still has lives
        if(OVRInput.GetUp(OVRInput.RawButton.Y) && gameSC.IsGameLost() && (gameSC.GetCurrPlayersLives() > 0))
        {
             FadeIn();
             gameSC.RestartGame(false);
             transform.position = playerSpawn.position;
             gameStateText.text = "";
        }

        //Check if player is crouching
        CheckIfCrouching();
        //Check the players movement speed and if they're running. This only works if the movement system is analog rather then teleportation
        CheckIfRunning();
        CheckMovingSpeed();
    }


    //Check if the player is crouching depending on the distance of the headsets local coordinate 
    void CheckIfCrouching() {
        isCrouching = Mathf.Abs(playerCamera.transform.position.y - feet.transform.position.y) <= distanceToTriggerCrouch ? true : false;

        if (isCrouching && crouchHeight != GetComponent<CapsuleCollider>().height) {
            GetComponent<CapsuleCollider>().height /= 2.0f;
            GetComponent<CapsuleCollider>().center -= new Vector3(0, 0.5f, 0f);
        }
        else if (!isCrouching && standingHeight != GetComponent<CapsuleCollider>().height) {
            GetComponent<CapsuleCollider>().height *= 2.0f;
            GetComponent<CapsuleCollider>().center += new Vector3(0, 0.5f, 0f);
        }
    }

    //Check if the player is running if they are holding the primary thumbstick
    void CheckIfRunning() {
        isRunning = OVRInput.Get(OVRInput.Button.PrimaryThumbstick) ? true : false;
    }

    //Check the movement speed of the player and store that into a modifier valuie
    void CheckMovingSpeed()
    {
        float speedMod = 1.0f;
        if (isCrouching && !isRunning)
            speedMod = 0.5f;
        else if (isRunning && !isCrouching)
            speedMod = 2.1f;
        else if (isRunning && isCrouching)
            speedMod = 0.9f;
        else
            speedMod = 1.0f;

        setSpeed = maxSpeed * speedMod;

        velocity = (transform.position - lastPos) / Time.deltaTime;
        lastPos = transform.position;
    }

    //Check if the player is moving
    public bool IsPlayerMoving()
    {
        return velocity.magnitude > 0.001f;
    }

    //Check if the player is crouching
    public bool GetCrouching()
    {
        return isCrouching;
    }

    //Check if the player is running
    public bool GetRunning()
    {
        return isRunning;
    }

    //Get the speed level of the player
    public int GetSpeedLevel()
    {
        float speedPerc = Mathf.Clamp01(velocity.magnitude / (maxSpeed * 2.1f));
        return (int)Mathf.Clamp(speedPerc * loudnessLevelLength, 1, 5);
    }

    //Calculate the brightness level of the player
    //If they are crouching then decrease that level by 1
    public void SetBrightnessLevel(float level) {
        brightness = 1.0f - level;
        brightnessLevel = Mathf.RoundToInt(brightness * (float)brightnessLevelLength);

        if (isCrouching) brightnessLevel--;

        if (brightnessLevel <= 0) brightnessLevel = 1;
    }

    //Get the brightness level of the player
    public int GetBrightnessLevel() {
        return brightnessLevel;
    }

    //Get the max speed of the player
    public float GetMaxSpeed()
    {
        return maxSpeed;
    }

    //OBSOLETE CODE
    public bool IsLocalPlayer()
    {
        return isLocalPlayer;
    }

    //Instantiate the player sound sphere
    [Command]
    public void CmdCreateSoundSphere(float radius)
    {
        if (soundSphereInsta == null)
        {
            soundSphereInsta = (GameObject)Instantiate(soundSpherePrefab, transform.position, transform.rotation);
            soundSphereInsta.name = GetComponent<PlayerID>().playerUniqueIdentity + " Sound Sphere";
            soundSphereInsta.GetComponent<SphereCollider>().radius = radius;
            NetworkServer.Spawn(soundSphereInsta);
            Destroy(soundSphereInsta, 0.1f);
        }
    }

    //Spawn the player back at the respawn area
    [Client]
    public void SendToJail()
    {
        transform.position = GameObject.Find("Jail Spawn").transform.position;
        gameSC.CmdAddPlayerToJail(transform.name);

    }

    //Tell the fader to fade in
    void FadeIn()
    {
        fader.fadeIn();
    }

    //Tell the fader to fade out
    void FadeOut()
    {
        fader.fadeOut();
    }

    //If player wins then tell them that they did through UI text and fade out
    public void PlayerWinTrigger()
    {
        FadeOut();
        gameStateText.text = "\n You Win";
    }

    //If player loses then tell them that they did through UI text and fade out
    //Also tell the player that they can respawn if they still have lives
    public void PlayerLoseTrigger()
    {
        FadeOut();
        gameStateText.text = "\n You Lose";
        if(gameSC.GetCurrPlayersLives() > 0) gameStateText.text += "\n Press 'Y' to restart";
        
    }
}
