using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundMixer : MonoBehaviour
{

    public PlayerController player;

    public AudioClip[] surfaceSounds;
    public float woodSoundMult;
    public float stoneSoundMult;
    public float carpetSoundMult;

    public AudioSource source;
    private int currSound;

    public float maxSphereRadius;
    private float currSphereRadius = 0.0f;

    void Update()
    {
        //Calculate the sound level of the player and create a sound sphere 
        CalculateSoundLevel();
        if (source.isPlaying)
        {
            player.CmdCreateSoundSphere(currSphereRadius);
        }
    }

    //Depending on what the player is standing on, it would change the radius of the sound sphere 
    public void CalculateSoundLevel()
    {
        float currMult = 0.0f;
        switch (currSound)
        {
            case 1:
                currMult = woodSoundMult;
                break;
            case 2:
                currMult = stoneSoundMult;
                break;
            case 3:
                currMult = carpetSoundMult;
                break;
        }
        currSphereRadius = ((float)player.GetSpeedLevel() * currMult) / player.GetMaxSpeed() * maxSphereRadius;
    }

    //Calculate the sound level of the player by normalizing it the sound spheres radius limit
    public int GetSoundLevel()
    {
        return Mathf.Clamp(Mathf.RoundToInt((currSphereRadius / maxSphereRadius) * 5.0f), 1, 5);
    }

    //Trigger the sound of the surface the player is stepping on
    public void TriggerSound(int soundIndex)
    {
        currSound = soundIndex;
        if (!source.isPlaying) source.PlayOneShot(surfaceSounds[currSound - 1]);
    }

    //Check if any foot sound is playing
    public bool IsPlayingSound()
    {
        return source.isPlaying;
    }
}
