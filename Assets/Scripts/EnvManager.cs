using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Linq;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.UI;

public class EnvManager : MonoBehaviour
{
    [SerializeField] Text AddressText;
    public string localIP;
    // Start is called before the first frame update
    void Start()
    {
        IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
        IPAddress ipAddress = ipHostInfo.AddressList
            .FirstOrDefault(a => a.AddressFamily == AddressFamily.InterNetwork);
        localIP = ipAddress.ToString();

        AddressText.text = "http://" + localIP + ":8080";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void IntroFX()
    {
        // need this?
    }

    public void CabinetFX()
    {
        // animate cabinet open and play sound
    }

    public void SafeFX()
    {
        // animate safe open and play sound
    }

    public void DoorFX()
    {
        // animate key to unock door, open it, and play sound
    }
}
