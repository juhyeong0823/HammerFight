using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class NetworkManager : MonoBehaviourPunCallbacks    
{
    public InputField roomInput, nickNameInput;
    public Text statusText;


    private void Awake()
    {
        Screen.SetResolution(960, 540, false);
    }

    void Update()
    {
        statusText.text = PhotonNetwork.NetworkClientState.ToString();
    }

    public void Connect() => PhotonNetwork.ConnectUsingSettings();

    public override void OnConnectedToMaster()
    {
        print("서버 접속 완료");
        PhotonNetwork.LocalPlayer.NickName = nickNameInput.text;
    }

    public void Disconnect() => PhotonNetwork.Disconnect();

    public override void OnDisconnected(DisconnectCause cause) => print("연결끊김");


    public void JoinLobby() => PhotonNetwork.JoinLobby();

    public override void OnJoinedLobby() => print("로비 접속 완료");

    public void CreateRoom() => PhotonNetwork.CreateRoom(roomInput.text, new RoomOptions { MaxPlayers = 5 });
    public void JoinRoom() => PhotonNetwork.JoinRoom(roomInput.text);
    public void JoinOrCreateRoom() => PhotonNetwork.JoinOrCreateRoom(roomInput.text, new RoomOptions { MaxPlayers = 5 }, null);
    public void JoinRandomRoom() => PhotonNetwork.JoinRandomRoom();
    public void LeaveRoom() => PhotonNetwork.LeaveRoom();

    public override void OnCreatedRoom()
    {
        print("방만들기완료");
    }
    public override void OnJoinedRoom()
    {
        print("방참가완료");
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        print("방만들기실패");

    }
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        print("방참가실패");
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        print("방랜덤참가실패");
    }






}
