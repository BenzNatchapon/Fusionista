using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseOld : MonoBehaviour
{

    public GameObject Canvas;
    public GameObject Camera;
    bool Paused = false;

    void Start()
    {
        Canvas.gameObject.SetActive(false);
        AudioSource audioSource = Camera.GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetKeyDown("escape"))
        {
            if (Paused == true)
            {
                Time.timeScale = 1.0f;
                Canvas.gameObject.SetActive(false);
                //Cursor.visible = false;
                //Cursor.lockState = true;
                //Camera.audioSource.Play();
                Paused = false;
            }
            else
            {
                Time.timeScale = 0.0f;
                Canvas.gameObject.SetActive(true);
                //Cursor.visible = true;
                //Cursor.lockState = false;
                //Camera.audioSource.Pause();
                Paused = true;
            }
        }
    }
    public void Resume()
    {
        Time.timeScale = 1.0f;
        Canvas.gameObject.SetActive(false);
        //Cursor.visible = false;
        //Cursor.lockState = true;
        //Camera.audioSource.Play();
    }
}
