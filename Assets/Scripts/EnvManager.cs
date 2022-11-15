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

        AddressText.text = "http://" + localIP + ":10118";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CabinetFX()
    {

    }

    void SafeFX()
    {

    }

    void DoorFX()
    {

    }
}
