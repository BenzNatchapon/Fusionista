using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSound_In : MonoBehaviour
{
    public AudioClip ClipIn;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "PlayerBody")
        {
            playsound(ClipIn);
        }
    }

    public void playsound(AudioClip Clip)
    {
        AudioSource audio = GetComponent<AudioSource>();
        audio.clip = Clip;
        audio.Play();
    }
}
