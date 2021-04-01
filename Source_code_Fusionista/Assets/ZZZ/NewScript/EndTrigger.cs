using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndTrigger : MonoBehaviour
{
    public string sceneName;
    bool used = false;
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
                SceneManager.LoadScene(sceneName);
            }
        }

    }
}
