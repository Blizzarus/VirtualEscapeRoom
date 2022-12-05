using SocketIOClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    EnvManager environment;
    SocketIOUnity socket;
    public string gamestate;

    //uncomment the following the test trigger manually
    /*void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            updateGS("puzzle2");
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            updateGS("escape");
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            updateGS("completed");
        }
    }*/

    void Start()
    {
        environment = GameObject.Find("EnvManager").GetComponent<EnvManager>();
        gamestate = "waiting";

        var uri = new Uri("http://localhost:4000");
        socket = new SocketIOUnity(uri, new SocketIOOptions
        {
        /*Query = new Dictionary<string, string>
        {
            {"token", "UNITY" }
        },*/
        Transport = SocketIOClient.Transport.TransportProtocol.WebSocket
        });

        socket.OnConnected += (sender, e) =>
        {
            Debug.Log("Socket Connected!");
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
            Debug.Log("Disconnect: " + e);
        };
        socket.OnReconnectAttempt += (sender, e) =>
        {
            Debug.Log($"{DateTime.Now} Reconnecting: attempt = {e}");
        };

        socket.On("requestGameState", (data) =>
        {
            Debug.Log("Received gamestate request from: " + data);
            socket.Emit("updateGameState", gamestate); //changed event name here
        });

        socket.OnUnityThread("solveEvent", (data) =>
        {
            //Debug.Log("Received solve attempt: " + data.ToString());
            string solveResult = solveAttempt(data);
            //Debug.Log(solveResult);
            string[] splitData = Regex.Split(solveResult.ToString(), "::");
            string resCode = splitData[0];
            string puzzleNumber = splitData[1];
            string message = splitData[2];
            socket.Emit("solveEvent", resCode + "::" + message);
            if (resCode == "VAL") {
                switch (puzzleNumber)
                {
                    case "1":
                        updateGS("puzzle2");
                        break;
                    case "2":
                        updateGS("escape");
                        break;
                    case "3":
                        updateGS("completed");
                        break;
                }
            }
        });


        Debug.Log("Attmepting to connect...");
        socket.Connect();
    }

    public void updateGS(string newGS)
    {
        gamestate = newGS;
        switch(gamestate)
        {
            case "waiting":
                break;
            case "puzzle1":
                GameObject.Find("StartMenu").SetActive(false);
                environment.Begin();
                break;
            case "puzzle2":
                environment.CabinetTrigger();
                break;
            case "escape":
                environment.SafeTrigger();
                break;
            case "completed":
                environment.DoorTrigger();
                break;
        }
        socket.Emit("updateGameState", gamestate);
    }

    string solveAttempt(SocketIOResponse data)
    {
        string[] splitData = Regex.Split(Regex.Replace(data.ToString(), "[][\"]", ""), "::");
        string player = splitData[0];
        string puzzleNumber = splitData[1];
        string code = splitData[2].ToLower();
        Debug.Log(player + " - " + puzzleNumber + " - " + code);
        switch (puzzleNumber)
        {
            case "1":
                if (gamestate != "puzzle1") { return "ERR::1::Invalid puzzle attempted.  How did you do that?!"; }
                if (code == "escape")
                {
                    environment.completors.Insert(0, player);
                    return "VAL::1::Success!  The lock opened!";
                }
                else
                {
                    return "ERR::1::The lock won't open.  Looks like that code wasn't right...";
                }
            case "2":
                if (gamestate != "puzzle2") { return "ERR::2::Invalid puzzle attempted.  How did you do that?!"; }
                if (code == "035131")
                {
                    environment.completors.Insert(1, player);
                    return "VAL::2::Success!  The safe opened!";
                }
                else
                {
                    return "ERR::2::The safe won't open.  Looks like that code wasn't right...";
                }
            case "3":
                if (gamestate != "escape") { return "ERR::3::Escape trigged early.  How did you do that?!"; }
                return "VAL::3::The key worked!  You have escaped!";
            default:
                Debug.Log("Error: invalid puzzle number");
                return "ERR::" + puzzleNumber + "::Error: invalid puzzle number";
        }
    }
}
