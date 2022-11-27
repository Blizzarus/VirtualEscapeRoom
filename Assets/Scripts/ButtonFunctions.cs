using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonFunctions : MonoBehaviour
{
    Button button;
    GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        button = GetComponent<Button>();
        button.onClick.AddListener(ButtonFunc);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ButtonFunc()
    {
        switch(gameObject.name)
        {
            case "StartButton":
                gameManager.updateGS("puzzle1");
                break;
            case "GuideButton":
                Application.OpenURL("https://1drv.ms/w/s!Anys2q_0RMamg4N6Tj9zgKf2AGT5fQ?e=6H1RaY");
                break;
            case "RestartButton":
                break;
            case "EndButton":
                break;
            default:
                Debug.LogWarning("Button case not found");
                break;
        }
    }
}
