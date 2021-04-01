using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterCollider : MonoBehaviour
{
    GameObject player;
    bool isSlime;
    BoxCollider2D collider;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        collider = gameObject.GetComponent<BoxCollider2D>();
    }


    void Update()
    {
        
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "PlayerBody")
        {
            isSlime = player.GetComponent<PlayerMain>().slime;
            if (isSlime)
            {
                collider.isTrigger = false;
            }

        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.tag == "PlayerBody")
        {
            isSlime = player.GetComponent<PlayerMain>().slime;
            if (!isSlime)
            {
                collider.isTrigger = true; ;
            }

        }
    }
}
