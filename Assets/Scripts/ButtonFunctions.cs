using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonFunctions : MonoBehaviour
{
    Button button;
    // Start is called before the first frame update
    void Start()
    {
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
                GameObject.Find("StartMenu").SetActive(false);
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
