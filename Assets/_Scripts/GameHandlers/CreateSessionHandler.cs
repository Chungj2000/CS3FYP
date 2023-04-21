using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

/*
 * Script used to generate sessions via Photon Networking.
 * Allows two players to connect to a 2-player multiplayer room where they can interact with one another via the Game.
 */
public class CreateSessionHandler : MonoBehaviourPunCallbacks {

    [SerializeField] private TMP_InputField createSessionKey;
    [SerializeField] private TMP_InputField joinSessionKey;

    private const string play = "GameScene";
    private const int minimumKeyChars = 5;

    [SerializeField] private TextMeshProUGUI invalidCreateSession;
    [SerializeField] private TextMeshProUGUI invalidJoinSession;
    private const string invalidSessionKeyValidation_Message = "MUST BE 5 - 8 CHARACTERS";
    private const string invalidCreateSession_Message = "ROOM CANNOT BE CREATED";
    private const string invalidJoinSession_Message = "INVALID SESSION";
    
    private void Start() {

        PhotonNetwork.ConnectUsingSettings();

    }

    //Event for when connected to the server.
    public override void OnConnectedToMaster() {
        Debug.Log("Connected to the Photon server.");
        PhotonNetwork.JoinLobby();
    }

    //Event for when lobby is joined.
    public override void OnJoinedLobby() {
        Debug.Log("Lobby has been joined.");
    }

    //Try to create a Room with the inputted session key.
    public void CreateSession() {
        Debug.Log("Creating session.");

        //Validate the number of characters for the key.
        if(!CheckCreateSessionKeyValid()) {
            Debug.Log("Invalid session key.");
            invalidCreateSession.text = invalidSessionKeyValidation_Message;
            return;
        }

        RoomOptions options = new RoomOptions();
        options.MaxPlayers = 2;

        Debug.Log("Max players in room set to: " + options.MaxPlayers);
        
        PhotonNetwork.CreateRoom(createSessionKey.text, options, TypedLobby.Default);
    }

    //Try to join a Room with the inputted session key.
    public void JoinSession() {
        Debug.Log("Joining session.");

        //Validate the number of characters for the key.
        if(!CheckJoinSessionKeyValid()) {
            Debug.Log("Invalid session key.");
            invalidJoinSession.text = invalidSessionKeyValidation_Message;
            return;
        }

        PhotonNetwork.JoinRoom(joinSessionKey.text);
    }

    //When a room is created.
    public override void OnCreatedRoom() {
        Debug.Log("Successfully created a room.");
    }

    //When a room is joined.
    public override void OnJoinedRoom() {
        Debug.Log("Player " + PhotonNetwork.LocalPlayer.ActorNumber + " has joined.");
        PhotonNetwork.LoadLevel(play);
    }

    //When creating a room fails.
    public override void OnCreateRoomFailed(short returnCode, string message) {
        Debug.Log("Room creation failed.");
        invalidCreateSession.text = invalidCreateSession_Message;
    }

    //When joining a room fails.
    public override void OnJoinRoomFailed(short returnCode, string message) {
        Debug.Log("Room joining failed.");
        invalidJoinSession.text = invalidJoinSession_Message;
    }

    //Check if the room is has the minimum number expected of characters.
    private bool CheckCreateSessionKeyValid() {
        if(createSessionKey.text.Length < minimumKeyChars) {
            //Debug.Log(createSessionKey.text.Length);
            //Debug.Log("Failed vaidation.");
            return false;
        }  else {
            //Debug.Log(createSessionKey.text.Length);
            //Debug.Log("Successful vaidation.");
            return true;
        }
    }

    private bool CheckJoinSessionKeyValid() {
        if(joinSessionKey.text.Length < minimumKeyChars) {
            //Debug.Log(createSessionKey.text.Length);
            //Debug.Log("Failed vaidation.");
            return false;
        }  else {
            //Debug.Log(createSessionKey.text.Length);
            //Debug.Log("Successful vaidation.");
            return true;
        }
    }

    //Getters.
    public TMP_InputField GetCreateSessionKey() {
        return createSessionKey;
    }

}
