using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapMain_A : MonoBehaviour
{
    public int damageAttack_A = 1;
    public float delayStart = 2.0f;
    public float intervalTime = 2.0f;

    public AudioClip Clip;
    bool sound = false;

    // Start is called before the first frame update
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
        Animator trapAnim = GetComponent<Animator>();
        TrapController trapController = GetComponent<TrapController>();
        trapController.ActionAttack(damageAttack_A);
        trapAnim.SetTrigger("ATK");
        playsound();
    }

    public void playsound()
    {
        if(sound == true)
        {
            AudioSource audio = GetComponent<AudioSource>();
            audio.clip = Clip;
            audio.volume = 0.3f;
            audio.Play();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "zone")
        {
            sound = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "zone")
        {
            sound = false;
        }
    }

}
