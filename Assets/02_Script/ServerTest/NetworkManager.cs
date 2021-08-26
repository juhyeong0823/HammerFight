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
        print("���� ���� �Ϸ�");
        PhotonNetwork.LocalPlayer.NickName = nickNameInput.text;
    }

    public void Disconnect() => PhotonNetwork.Disconnect();

    public override void OnDisconnected(DisconnectCause cause) => print("�������");


    public void JoinLobby() => PhotonNetwork.JoinLobby();

    public override void OnJoinedLobby() => print("�κ� ���� �Ϸ�");

    public void CreateRoom() => PhotonNetwork.CreateRoom(roomInput.text, new RoomOptions { MaxPlayers = 5 });
    public void JoinRoom() => PhotonNetwork.JoinRoom(roomInput.text);
    public void JoinOrCreateRoom() => PhotonNetwork.JoinOrCreateRoom(roomInput.text, new RoomOptions { MaxPlayers = 5 }, null);
    public void JoinRandomRoom() => PhotonNetwork.JoinRandomRoom();
    public void LeaveRoom() => PhotonNetwork.LeaveRoom();

    public override void OnCreatedRoom()
    {
        print("�游���Ϸ�");
    }
    public override void OnJoinedRoom()
    {
        print("�������Ϸ�");
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        print("�游������");

    }
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        print("����������");
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        print("�淣����������");
    }






}
