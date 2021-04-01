using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSound : MonoBehaviour
{
    public AudioClip ClipIn;
    public AudioClip ClipOut;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "PlayerBody")
        {
            playsound(ClipIn);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "PlayerBody")
        {
            playsound(ClipOut);
        }
    }
    public void playsound(AudioClip Clip)
    {
        AudioSource audio = GetComponent<AudioSource>();
        audio.clip = Clip;
        audio.Play();
    }
}
