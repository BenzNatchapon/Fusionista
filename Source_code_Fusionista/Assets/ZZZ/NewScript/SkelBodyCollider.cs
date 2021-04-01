using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkelBodyCollider : MonoBehaviour
{
    GameObject player;
    bool isSlime;


    void Awake()
    {
        player = GameObject.FindWithTag("Player");
        Animator skelAnim = GetComponentInParent<Animator>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        isSlime = player.GetComponent<PlayerMain>().slime;
        if (other.tag == "PlayerSwallow")
        {
            if(isSlime)
            {
                player.GetComponent<PlayerMain>().StartSwitchAnimToSkelAnim();
                Animator skelAnim = GetComponentInParent<Animator>();
                skelAnim.SetTrigger("Destroy");
            }

        }

    }


    public void restore()
    {
        Animator skelAnim = GetComponentInParent<Animator>();
        skelAnim.SetTrigger("Restore");
    }
}
