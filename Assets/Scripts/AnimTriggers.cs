using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimTriggers : MonoBehaviour
{
    [SerializeField] GameObject pointLights;
    [SerializeField] GameObject outside;
    [SerializeField] GameObject mainCamera;
    [SerializeField] GameObject altCamera;
    // Start is called before the first frame update

    public void DoorEvents()
    {
        outside.SetActive(true);
        mainCamera.GetComponent<Crepuscular>().enabled = true;
        pointLights.SetActive(false);
    }

    public void CallEnd()
    {
        GameObject.Find("EnvManager").GetComponent<EnvManager>().End();
    }

    public void SafeEventsStart()
    {
        altCamera.SetActive(true);
        mainCamera.SetActive(false);
    }

    public void SafeEventsEnd()
    {
        mainCamera.SetActive(true);
        altCamera.SetActive(false);
    }
}
