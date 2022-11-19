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
        var uri = new Uri("http://localhost:8080");
        socket = new SocketIOUnity(uri, new SocketIOOptions
        {
            Query = new Dictionary<string, string>
        {
            {"token", "UNITY" }
        }
            ,
            Transport = SocketIOClient.Transport.TransportProtocol.WebSocket
        });

        socket.OnConnected += (sender, e) =>
        {
            Debug.Log("socket.OnConnected");
        };
        socket.OnPing += (sender, e) =>
        {
            //Debug.Log("Ping");
        };
        socket.OnPong += (sender, e) =>
        {
            //Debug.Log("Pong: " + e.TotalMilliseconds);
        };
        socket.OnDisconnected += (sender, e) =>
        {
            Debug.Log("disconnect: " + e);
        };
        socket.OnReconnectAttempt += (sender, e) =>
        {
            Debug.Log($"{DateTime.Now} Reconnecting: attempt = {e}");
        };
        socket.On("test", (response) =>
        {
            Debug.Log(response.ToString());
        });

        Debug.Log("Connecting...");
        socket.Connect();
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
            socket.Emit("test", "Hello from GameManager!");
        }
    }
}
