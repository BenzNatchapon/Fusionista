using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClickToContinue : MonoBehaviour
{
    public string sceneName;
    public float time;
    bool canChange = false;

    public void Count()
    {
        StartCoroutine(ClickToChangeScene());
    }

    IEnumerator ClickToChangeScene()
    {
        //yield return new WaitForSeconds(GetComponent<AudioSource>().clip.length + 1);

        yield return new WaitForSeconds(time);
        canChange = true;
    }

    void Start()
    {
        Count();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && canChange == true)
        {
            canChange = false;
            SceneManager.LoadScene(sceneName);
        }
    }

}
