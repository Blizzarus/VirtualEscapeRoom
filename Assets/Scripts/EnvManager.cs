using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Linq;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.UI;

public class EnvManager : MonoBehaviour
{
    [SerializeField] GameObject mainCamera;
    [SerializeField] GameObject altCamera;
    [SerializeField] Text AddressText;
    [SerializeField] AudioClip padlockOpenSFX;
    [SerializeField] AudioClip cabinetOpenSFX;
    [SerializeField] AudioClip safeClickSFX;
    [SerializeField] AudioClip safeSpinSFX;
    [SerializeField] AudioClip safeOpenSFX;
    [SerializeField] AudioClip winSFX;
    [SerializeField] AudioClip gameMusic;
    public string localIP;
    Animator animator1;
    Animator animator2;
    AudioSource audio;
    // Start is called before the first frame update
    void Start()
    {
        IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
        IPAddress ipAddress = ipHostInfo.AddressList
            .FirstOrDefault(a => a.AddressFamily == AddressFamily.InterNetwork);
        localIP = ipAddress.ToString();

        AddressText.text = "http://" + localIP + ":8080";

        audio = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            StartCoroutine("CabinetFX");
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            StartCoroutine("SafeFX");
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            StartCoroutine("DoorFX");
        }
    }

    public void Begin()
    {
        audio.clip = gameMusic;
        audio.Play();
    }

    public void FX(int puzzle)
    {
        switch (puzzle)
        {
            case 1:
                StartCoroutine("CabinetFX");
                break;
            case 2:
                StartCoroutine("SafeFX");
                break;
            case 3:
                StartCoroutine("DoorFX");
                break;
        }
    }

    IEnumerator CabinetFX()
    {
        GameObject.Find("Cabinet_Lock").SetActive(false);
        animator1 = GameObject.Find("Cabinet_RDoor").GetComponent<Animator>();
        animator2 = GameObject.Find("Cabinet_LDoor").GetComponent<Animator>();
        animator1.SetTrigger("OpenDoor");
        animator2.SetTrigger("OpenDoor");
        audio.PlayOneShot(padlockOpenSFX);
        yield return new WaitForSeconds(0.5f);
        audio.PlayOneShot(cabinetOpenSFX);
    }

    IEnumerator SafeFX()
    {
        animator1 = GameObject.Find("Safe_Door").GetComponent<Animator>();
        altCamera.SetActive(true);
        mainCamera.SetActive(false);
        animator1.SetTrigger("OpenSafe");
        audio.PlayOneShot(safeClickSFX);
        yield return new WaitForSeconds(3);
        audio.PlayOneShot(safeSpinSFX, 1.5f);
        yield return new WaitForSeconds(3);
        audio.PlayOneShot(safeOpenSFX);
        yield return new WaitForSeconds(6);
        mainCamera.SetActive(true);
        altCamera.SetActive(false);
    }

    IEnumerator DoorFX()
    {
        GameObject.Find("Rusty_Key").SetActive(false);
        animator1 = GameObject.Find("Inner_Door").GetComponent<Animator>();
        animator1.SetTrigger("OpenDoor");
        audio.Stop();
        audio.PlayOneShot(padlockOpenSFX);
        yield return new WaitForSeconds(3.5f);
        audio.PlayOneShot(cabinetOpenSFX);
        yield return new WaitForSeconds(2.25f);
        audio.PlayOneShot(winSFX);
        yield return new WaitForSeconds(5);
        End();
    }

    void End()
    {
        GameObject.Find("EndMenu").SetActive(true);
    }
}
