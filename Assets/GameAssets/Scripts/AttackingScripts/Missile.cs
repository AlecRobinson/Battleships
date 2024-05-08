using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    //Prefabs for hit and miss
    public GameObject fire;
    public GameObject missCross;
    public AudioClip[] audioClips = new AudioClip[2];

    //Objects being saved so they can be deleted after sfx has finished
    private AudioSource currentlyPlaying;
    private GameObject missleToDestroy;
    private GameObject otherToDestroy;

    void Start()
    {
        //Angles missle towards floor
        transform.Rotate(new Vector3(90, 0, 0));
    }
    void FixedUpdate()
    {
        //Moves missle towards floor at steady speed
        transform.position -= new Vector3(0, 0.7f, 0);

        //Checks if sfx is playing and deletes the old missle and hit/miss object trigger
        if (currentlyPlaying != null)
        {
            if (!currentlyPlaying.isPlaying)
            {
                Destroy(otherToDestroy);
                Destroy(missleToDestroy);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        currentlyPlaying = gameObject.GetComponent<AudioSource>();          //Getting current audio source
        if (!currentlyPlaying.isPlaying)
        {   
            //Finding out what object has been hit
            if (other.gameObject.tag == "HitAI")
            {
                currentlyPlaying.clip = audioClips[0];                          //Selecting correct sfx
                currentlyPlaying.Play();
                Instantiate(fire, other.transform.position + new Vector3(0, -5, 0), Quaternion.identity);   //Instantiating correct prefab
            }
            if (other.gameObject.tag == "HitPlayer")
            {
                currentlyPlaying.clip = audioClips[0];
                currentlyPlaying.Play();
                Instantiate(fire, other.transform.position + new Vector3(0, -15, 0), Quaternion.identity);
            }
            if (other.gameObject.tag == "Miss")
            {
                currentlyPlaying.clip = audioClips[1];
                currentlyPlaying.Play();
                Instantiate(missCross, other.transform.position + new Vector3(0, -20, 0), Quaternion.identity);
            }
            //Saving objects missle and collision box for deletion once sfx is done
            otherToDestroy = other.gameObject;
            missleToDestroy = gameObject;
        }
    }
}
