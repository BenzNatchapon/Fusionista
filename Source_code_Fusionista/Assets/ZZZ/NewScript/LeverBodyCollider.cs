using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverBodyCollider : MonoBehaviour
{
    GameObject player;
    bool isSlime;
    BoxCollider2D coll;
    Animator animator;
    bool used = false;
    public AudioClip Clip;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        animator = gameObject.GetComponentInParent<Animator>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "PlayerArm")
        {
            isSlime = player.GetComponent<PlayerMain>().slime;

            if (!isSlime && used == false)
            {
                used = true;
                animator.SetTrigger("Go");
                playsound();

            }
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
