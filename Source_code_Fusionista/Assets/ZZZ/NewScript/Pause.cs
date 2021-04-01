using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    bool paused = false;
    public GameObject PauseWindow;
    public GameObject ControlWindow;
    bool showCon = false;

    void Start()
    {
        hideUI();
        hideUICon();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && showCon == true)
        {
            showCon = false;
            hideUICon();
            showUI();
        }

        else if (Input.GetKeyDown(KeyCode.Escape) && showCon == false)
            paused = togglePause();
    }

    //void OnGUI()
    //{
    //    if (paused)
    //    {
    //        GUILayout.Label("Game is paused!");
    //        if (GUILayout.Button("Click me to unpause"))
    //            paused = togglePause();
    //    }
    //}

    void hideUI()
    {
        var getCanvasGroup = PauseWindow.GetComponent<CanvasGroup>();
        getCanvasGroup.alpha = 0f; //this makes everything transparent
        getCanvasGroup.blocksRaycasts = false; //this prevents the UI element to receive input events
    }
    void showUI()
    {
        var getCanvasGroup = PauseWindow.GetComponent<CanvasGroup>();
        getCanvasGroup.alpha = 1;
        getCanvasGroup.blocksRaycasts = true; 
    }

    void hideUICon()
    {
        var getCanvasGroup = ControlWindow.GetComponent<CanvasGroup>();
        getCanvasGroup.alpha = 0f; //this makes everything transparent
        getCanvasGroup.blocksRaycasts = false; //this prevents the UI element to receive input events
    }

    void showUICon()
    {
        var getCanvasGroup = ControlWindow.GetComponent<CanvasGroup>();
        getCanvasGroup.alpha = 1;
        getCanvasGroup.blocksRaycasts = true;
    }

    bool togglePause()
    {
        if (Time.timeScale == 0f)
        {
            Time.timeScale = 1f;
            Camera.main.GetComponent<AudioSource>().UnPause();
            hideUI();
            return (false);
        }
        else
        {
            Time.timeScale = 0f;
            Camera.main.GetComponent<AudioSource>().Pause();
            showUI();
            return (true);
        }
    }

    public void resume()
    {
        if (paused)
        {
            paused = togglePause();
        }
    }

    public void quit()
    {
        paused = togglePause();
        SceneManager.LoadScene("SlimeIntro");
    }

    public void showControlButton()
    {
        showCon = true;
        hideUI();
        showUICon();
    }
}
