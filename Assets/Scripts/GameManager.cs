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

    void Start()
    {
        environment = GameObject.Find("EnvManager").GetComponent<EnvManager>();
        setupSocketIOClient();
        gamestate = "waiting";
    }

    void setupSocketIOClient()
    {
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
            Debug.Log("Recieved gamestate request");
            socket.Emit("requestGameState", gamestate);
        });

        socket.On("solveEvent", (data) =>
        {
            Debug.Log("Recieved solve attempt: " + data.ToString());
            string solveResult = solveAttempt(data);
            socket.Emit("solveEvent", solveResult);
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
                environment.FX(1);
                break;
            case "escape":
                environment.FX(2);
                break;
            case "completed":
                environment.FX(3);
                break;
        }
        socket.Emit("updateGameState", gamestate);
    }

    string solveAttempt(SocketIOResponse data)
    {
        string[] splitData = Regex.Split(Regex.Replace(data.ToString(), "[][\"]", ""), "::");
        string player = splitData[0];
        string puzzleNumber = splitData[1];
        string code = splitData[2];
        Debug.Log(code);
        switch (puzzleNumber)
        {
            case "1":
                if (gamestate != "puzzle1") { return "Invalid puzzle attmepted.  How did you do that?!"; }
                if (code == "test")
                {
                    updateGS("puzzle2");
                    environment.completors.Insert(0, player);
                    return "Success!  The lock opened!";
                }
                else
                {
                    return "The lock won't open.  Looks like that code wasn't right...";
                }
            case "2":
                if (gamestate != "puzzle2") { return "Invalid puzzle attmepted.  How did you do that?!"; }
                if (code == "123456")
                {
                    updateGS("escape");
                    environment.completors.Insert(1, player);
                    return "Success!  The safe opened!";
                }
                else
                {
                    return "The safe won't open.  Looks like that code wasn't right...";
                }
            case "3":
                if (gamestate != "escape") { return "Escape trigged early.  How did you do that?!"; }
                updateGS("completed");
                return "The key worked!  You have escaped!";
            default:
                Debug.Log("Error: invalid puzzle number");
                return "Error: invalid puzzle number";
        }
    }
}
