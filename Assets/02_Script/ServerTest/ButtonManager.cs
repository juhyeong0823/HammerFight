using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ButtonManager : MonoBehaviour
{
    public NetworkManager networkManager;

    public Button btn1;// 서버접속
    public Button btn2;// 연결 끊기
    public Button btn3;// 로비 접속
    public Button btn4;// 방 만들기
    public Button btn5;// 방 참가
    public Button btn6;// 방 참가/ 만들기
    public Button btn7;// 방 랜덤참가
    public Button btn8;// 방 떠나기

    private void Start()
    {
        btn1.onClick.AddListener(() =>
        {
            networkManager.Connect();
        });
        btn2.onClick.AddListener(() =>
        {
            networkManager.Disconnect();
        });
        btn3.onClick.AddListener(() =>
        {
            networkManager.JoinLobby();
        });
        btn4.onClick.AddListener(() =>
        {
            networkManager.CreateRoom();
        });
        btn5.onClick.AddListener(() =>
        {
            networkManager.JoinRoom();
        });
        btn6.onClick.AddListener(() =>
        {
            networkManager.JoinOrCreateRoom();
        });
        btn7.onClick.AddListener(() =>
        {
            networkManager.JoinRandomRoom();
        });
        btn8.onClick.AddListener(() =>
        {
            networkManager.LeaveRoom();
        });

    }
}
