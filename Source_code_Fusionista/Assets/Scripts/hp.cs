using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class hp : MonoBehaviour
{
    // Start is called before the first frame update

    //GameObject thing;
    public Text hpText;
    public Text scoreText;
    float hpInt = 0;
    float scoreInt = 0;

    int life = 0;

    LifePanel lifePanel;

    void Start()
    {
        //thing = GameObject.FindWithTag("Player");
        hpInt = PlayerController.nowHp;
        hpText.text = hpInt.ToString("0");
        scoreInt = PlayerController.score;
        scoreText.text = scoreInt.ToString("0");

        life = (int)hpInt;
        lifePanel = GetComponent<LifePanel>();
        lifePanel.UpdateLife(life);
    }

    // Update is called once per frame
    void Update()
    {
        hpInt = PlayerController.nowHp;
        hpText.text = hpInt.ToString("0");
        scoreInt = PlayerController.score;
        scoreText.text = scoreInt.ToString("0");

        life = (int)hpInt;
        lifePanel.UpdateLife(life);
    }
}
