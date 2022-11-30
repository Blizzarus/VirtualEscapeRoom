using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Linq;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.UI;
using System.Timers;
using System;

public class EnvManager : MonoBehaviour
{
    [SerializeField] GameObject mainCamera;
    [SerializeField] GameObject altCamera;
    [SerializeField] Text AddressText;
    [SerializeField] Text StatsText;
    [SerializeField] Text ElapsedTimeText;
    [SerializeField] GameObject Timer;
    [SerializeField] GameObject EndMenu;
    [SerializeField] AudioClip padlockOpenSFX;
    [SerializeField] AudioClip cabinetOpenSFX;
    [SerializeField] AudioClip safeClickSFX;
    [SerializeField] AudioClip safeSpinSFX;
    [SerializeField] AudioClip safeOpenSFX;
    [SerializeField] AudioClip winSFX;
    [SerializeField] AudioClip gameMusic;
    public string localIP;
    public List<string> completors;
    int timeElapsed;
    Animator animator1;
    Animator animator2;
    AudioSource audio;

    void Start()
    {
        IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
        IPAddress ipAddress = ipHostInfo.AddressList
            .LastOrDefault(a => a.AddressFamily == AddressFamily.InterNetwork);
        localIP = ipAddress.ToString();

        AddressText.text = "http://" + localIP + ":8080";

        completors = new List<string>();
        completors.Add("Test1");
        completors.Add("Test2");

        audio = GetComponent<AudioSource>();
    }

    void Update()
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
        InvokeRepeating("UpdateTimer", 0, 1.0f);
        Timer.SetActive(true);
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

    void UpdateTimer()
    {
        timeElapsed++;
        ElapsedTimeText.text = TimeSpan.FromSeconds(timeElapsed).ToString("mm\\:ss");
    }

    void End()
    {
        CancelInvoke();
        StatsText.text = "Your total elapsed time was: " + TimeSpan.FromSeconds(timeElapsed).ToString("mm\\:ss") + "\n" +
            "Puzzle 1 was solved by: " + completors[0] + "\n" +
            "Puzzle 2 was solved by: " + completors[1];
        EndMenu.SetActive(true);
        Timer.SetActive(false);
    }
}
