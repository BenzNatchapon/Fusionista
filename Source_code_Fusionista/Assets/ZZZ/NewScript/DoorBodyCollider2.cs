using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DoorBodyCollider2 : MonoBehaviour
{
    GameObject player;
    bool isSlime;
    BoxCollider2D coll;
    Animator animator;
    AudioSource myAudio;
    public AudioClip Clip;

    void Start()
    {
        coll = GetComponent<BoxCollider2D>();
        player = GameObject.FindWithTag("Player");
        animator = gameObject.GetComponentInParent<Animator>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "PlayerArm")
        {
            isSlime = player.GetComponent<PlayerMain>().slime;

            //Debug.Log(isSlime);

            //if (!isSlime)
            //{
                animator.SetTrigger("T");
                Debug.Log("Door");
                coll.isTrigger = true;
                playsound();

            //}
        }
    }

    public void playsound()
    {
        AudioSource audio = GetComponent<AudioSource>();
        audio.clip = Clip;
        audio.Play();
    }
    void Update()
    {
        
    }
}
