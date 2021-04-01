using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSound_Out : MonoBehaviour
{
    public AudioClip ClipOut;

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
