using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHealthBar : MonoBehaviour
{
    public static UIHealthBar instance { get; private set; }

    public Image mask;
    public Image gameOver;

    void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        gameOver.enabled = false;
    }

    public void SetValue(float value)
    {
        mask.fillAmount = value;
    }

    public void GameOver()
    {
        gameOver.enabled = true;
    }
}
