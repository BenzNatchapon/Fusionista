using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetTrigger : MonoBehaviour
{
    bool used = false;
    public AudioClip Clip;
    void Start()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("55555");
        if (other.tag == "PlayerBody")
        {
            if (used == false)
            {
                used = true;
                Animator trapAnim = GetComponentInParent<Animator>();
                trapAnim.SetTrigger("Run");
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
}
