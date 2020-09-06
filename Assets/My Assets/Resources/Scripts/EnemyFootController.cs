using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFootController : MonoBehaviour {

    private AudioSource audSource;
    public AudioClip[] sources;
    public int currSoundIndex =-1;

    // Use this for initialization
    void Start () {
        //For each enemy, start the enemy foot sound mixer
        float randStart = Random.Range(0, 1f);
        InvokeRepeating("CreateEnemySound", randStart, 1.5f);
        audSource = GetComponent<AudioSource>();
    }

    void CreateEnemySound()
    {
        //play the enemy foot sound
        if(currSoundIndex > -1) audSource.PlayOneShot(sources[currSoundIndex], 0.5f);
    }

    //Depending on what the player is standing on, switch over to the respective sound 
    void OnTriggerStay(Collider collider)
    {
        if (collider.gameObject.tag == "Wood") currSoundIndex = 0;
        if (collider.gameObject.tag == "Stone") currSoundIndex = 1;
        if (collider.gameObject.tag == "Carpet") currSoundIndex = 2;
    }
}
