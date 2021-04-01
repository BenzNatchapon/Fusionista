using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapMain_Lava : MonoBehaviour
{
    public int damageAttack_A = 1;
    public float delayStart = 0.1f;
    public float intervalTime = 0.5f;

    void Start()
    {
        Animator trapAnim = GetComponent<Animator>();
        InvokeRepeating("Attack", delayStart, intervalTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Attack()
    {
        TrapController trapController = GetComponent<TrapController>();
        trapController.ActionAttack(damageAttack_A);
    }
}
