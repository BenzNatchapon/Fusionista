using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapMain_Laser : MonoBehaviour
{
    public int damageAttack_A = 1;
    public float delayStart = 0.1f;
    public float intervalTime = 1.0f;
    GameObject player;
    bool isSlime;

    void Start()
    {
        player = GameObject.FindWithTag("Player");

        Animator trapAnim = GetComponent<Animator>();
        InvokeRepeating("Attack", delayStart, intervalTime);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void Attack()
    {
        isSlime = player.GetComponent<PlayerMain>().slime;
        if (isSlime)
        {
            Debug.Log("check1");
            damageAttack_A = 0;
        }
        else if(!isSlime)
        {
            Debug.Log("check2");
            //damageAttack_A = 1;
            damageAttack_A = 0;
        }
        TrapController trapController = GetComponent<TrapController>();
        trapController.ActionAttack(damageAttack_A);
    }
}
