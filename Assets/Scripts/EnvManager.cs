using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Linq;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.UI;
using System.Timers;
using System;
using System.Threading;

public class EnvManager : MonoBehaviour
{
    [SerializeField] Text AddressText;
    [SerializeField] Text StatsText;
    [SerializeField] Text ElapsedTimeText;
    [SerializeField] GameObject Timer;
    [SerializeField] GameObject EndMenu;
    [SerializeField] AudioClip padlockOpenSFX;
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
        IPAddress ipAddress = ipHostInfo.AddressList.LastOrDefault
            (a => a.AddressFamily == AddressFamily.InterNetwork);
        localIP = ipAddress.ToString();

        AddressText.text = "http://" + localIP + ":3000";

        completors = new List<string>();
        completors.Add("Test1");
        completors.Add("Test2");

        audio = GetComponent<AudioSource>();
    }

    public void Begin()
    {
        InvokeRepeating("UpdateTimer", 0, 1.0f);
        Timer.SetActive(true);
        audio.clip = gameMusic;
        audio.Play();
    }

    public void CabinetTrigger()
    {
        audio.PlayOneShot(padlockOpenSFX);
        GameObject.Find("Cabinet_Lock").SetActive(false);
        animator1 = GameObject.Find("Cabinet_RDoor").GetComponent<Animator>();
        animator2 = GameObject.Find("Cabinet_LDoor").GetComponent<Animator>();
        animator1.SetTrigger("OpenDoor");
        animator2.SetTrigger("OpenDoor");
    }

    public void SafeTrigger()
    {
        animator1 = GameObject.Find("Safe_Door").GetComponent<Animator>();
        animator1.SetTrigger("OpenSafe");
    }

    public void DoorTrigger()
    {
        GameObject.Find("Rusty_Key").SetActive(false);
        animator1 = GameObject.Find("Inner_Door").GetComponent<Animator>();
        audio.Stop();
        CancelInvoke();
        animator1.SetTrigger("OpenDoor");
    }

    void UpdateTimer()
    {
        timeElapsed++;
        ElapsedTimeText.text = TimeSpan.FromSeconds(timeElapsed).ToString("mm\\:ss");
    }

    public void End()
    {
        StatsText.text = "Your total elapsed time was: " + TimeSpan.FromSeconds(timeElapsed).ToString("mm\\:ss") + "\n" +
            "Puzzle 1 was solved by: " + completors[0] + "\n" +
            "Puzzle 2 was solved by: " + completors[1];
        EndMenu.SetActive(true);
        Timer.SetActive(false);
    }
}
