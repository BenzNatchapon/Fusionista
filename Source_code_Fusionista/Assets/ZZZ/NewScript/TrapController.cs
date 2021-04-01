using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapController : MonoBehaviour
{
    [System.NonSerialized] public bool cameraRendered = false;
    [System.NonSerialized] public bool attackEnabled = false;
    [System.NonSerialized] public int attackDamage = 1;
    [System.NonSerialized] public Vector2 attackNockBackVector = Vector3.zero;


    Animator trapAnim;

    void Start()
    {
        Animator trapAnim = GetComponent<Animator>();
    }

    public void ActionAttack(int damage)
    {
        attackEnabled = true;
        attackDamage = damage;
    }
}
