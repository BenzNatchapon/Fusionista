using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartButton()
    {
        StartCoroutine(StartButtonNow());
    }

    public void ButtonSound()
    {
        //เล่นเสียงตอนเปลี่ยนฉาก โดยกำหนดให้  Volume เท่ากับ 1 (ดังเต็มที่)
        AudioSource audioSource = GetComponent<AudioSource>();
        //audioSource.volume = 1;
        audioSource.Play();

        // รอจดกว่าเสียงตอนเปลี่ยนฉากจะจบ แล้วรออีกประมาณ 0.5 วินาที แล้วเปลี่ยนฉาก
        //yield return new WaitForSeconds(audioSource.clip.length + 0.5f);
    }

    IEnumerator StartButtonNow()
    {
        //yield return new WaitForSeconds(GetComponent<AudioSource>().clip.length + 1);

        yield return new WaitForSeconds(1); // แสดงหน้าต่าง Loading 1 วินาทีก่อนเปลี่ยนฉาก
        SceneManager.LoadScene("SlimeOP");
    }

    public void doExitGame()
    {
        Application.Quit();
        Debug.Log("Game is exiting");
    }
}
