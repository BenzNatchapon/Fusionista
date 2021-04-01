using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserCollider : MonoBehaviour
{
    GameObject player;
    bool isSlime;
    public bool used = false;
    AudioSource myAudio;
    public AudioClip Clip;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }


    void setUsed()
    {
        if(used == true)
        {
            isSlime = player.GetComponent<PlayerMain>().slime;
            if (!isSlime)
            {
                used = false;
            }
        }

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "PlayerBody")
        {
            isSlime = player.GetComponent<PlayerMain>().slime;

            if (!isSlime && used == false)
            {
                //player.GetComponent<PlayerMain>().forceBackToSlime();
                used = true;
                player.GetComponent<PlayerController>().ActionDamage(1);
                playsound();
            }
            
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "PlayerBody")
        {
            setUsed();

        }
    }

    public void playsound()
    {
        AudioSource audio = GetComponent<AudioSource>();
        audio.clip = Clip;
        audio.Play();
    }
}
