using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ButtonManager : MonoBehaviour
{
    public NetworkManager networkManager;

    public Button btn1;// ��������
    public Button btn2;// ���� ����
    public Button btn3;// �κ� ����
    public Button btn4;// �� �����
    public Button btn5;// �� ����
    public Button btn6;// �� ����/ �����
    public Button btn7;// �� ��������
    public Button btn8;// �� ������

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
