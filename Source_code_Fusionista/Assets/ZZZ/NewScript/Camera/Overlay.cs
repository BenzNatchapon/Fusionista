using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Overlay : MonoBehaviour
{
    Image overlay;
    public float fadeSpeed = 0.8f;
    Color targetColor;

    // ...
    void Start()
    {
        overlay = GetComponent<Image>();
        overlay.color = Color.black;
        SetOverlayColor(Color.clear);
    }
    void Update()
    {
        if (overlay.color != targetColor)
        {
            overlay.color = Color.Lerp(overlay.color, targetColor,
                 Time.deltaTime * fadeSpeed);
        }
    }
    public void SetOverlayColor(Color color)
    {
        targetColor = color;
    }
}
