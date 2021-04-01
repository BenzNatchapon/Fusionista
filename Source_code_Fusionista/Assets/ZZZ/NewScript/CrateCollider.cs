using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateCollider : MonoBehaviour
{
    GameObject player;
    public GameObject heart;
    bool isSlime;
    public bool haveHeart = true;
    public AudioClip Clip;

    void Awake()
    {
        player = GameObject.FindWithTag("Player");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "PlayerArm")
        {
            isSlime = player.GetComponent<PlayerMain>().slime;
            if (!isSlime)
            {

                if(haveHeart == true)
                {
                    Instantiate(heart, this.transform.position, Quaternion.identity);
                }

                destroyCrate();
            }
 
        }

    }

    public void destroyCrate()
    {
        var parentCrate = transform.parent;

        AudioSource audio = GetComponent<AudioSource>();
        audio.clip = Clip;
        audio.volume = 0.5f;
        audio.Play();

        Destroy(parentCrate.gameObject, 0.2f);
    }
}
