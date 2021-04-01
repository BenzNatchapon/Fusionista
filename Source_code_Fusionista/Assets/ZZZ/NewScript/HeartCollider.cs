using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartCollider : MonoBehaviour
{
    GameObject player;
    bool isHeal = false;

    void Awake()
    {
        player = GameObject.FindWithTag("Player");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "PlayerBody" && isHeal == false)
        {
            isHeal = true;
            player.GetComponent<PlayerMain>().heartHeal();
            var parentHeart = transform.parent;
            Destroy(parentHeart.gameObject);

        }

    }

}
