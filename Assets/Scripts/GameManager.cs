using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;

public class GameManager : MonoBehaviour
{
    WebSocket ws;
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

    private void Start()
    {
        ws = new WebSocket("ws://192.168.0.16:10117");
        ws.Connect();
        ws.OnMessage += (sender, e) =>
        {
            Debug.Log("Message Received from " + ((WebSocket)sender).Url + ", Data : " + e.Data);
        };
    }

    // Update is called once per frame
    void Update()
    {
        if (ws == null)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ws.Send("Hello");
        }
    }
}
