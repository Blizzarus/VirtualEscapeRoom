using SocketIOClient;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    EnvManager environment;
    SocketIOUnity socket;
    public static GameManager _GameManager { get; private set; }
    // Start is called before the first frame update
    void Awake()
    {
        if(_GameManager == null)
        {
            _GameManager = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
    }

    void Start()
    {
        environment = GameObject.Find("EnvManager").GetComponent<EnvManager>();
        var uri = new Uri("ws://" + environment.localIP + ":10117");
        socket = new SocketIOUnity(uri, new SocketIOOptions
        {
            Query = new Dictionary<string, string>
        {
            {"token", "UNITY" }
        }
            ,
            Transport = SocketIOClient.Transport.TransportProtocol.WebSocket
        });
    }

    // Update is called once per frame
    void Update()
    {
        // To test socket emit
        if (socket == null)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            socket.Emit("Opmessage", "Hello from GameManager");
        }
    }
}
