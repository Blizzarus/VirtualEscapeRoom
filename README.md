# VirtualEscapeRoom
CMSC 495 Capstone Project, UMGC, Fall 2022.
Backend Repo: https://github.com/Blizzarus/VirtualEscapeRoom-Web

Virtual Escape Room is a couch co-op puzzle game, where players work together to solve clues and escape from a locked study.  Any number of players can join via smartphone or any other connected device, so family and friends can all join in on the fun!

The core game is a Unity3D application, utilizing custom animations, lighting effects, and a shared viewpoint to set the scene.  The Unity application connects to a NodeJS backend which hosts a web interface that players can use to investigate clues and submit their answers.  The Node server hosts an API to handle messages between players and the Unity game, so any device on the LAN can join by connecting to the IP of the device running the game, allowing for seamless couch co-op!
