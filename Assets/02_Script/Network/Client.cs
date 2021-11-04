using UnityEngine;
using UnityEngine.Networking;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Collections;
using System;
using System.Collections.Generic;

public class Client : MonoBehaviour
{
    public TcpClient tcp;

    public bool isThreadAlived = true;

    const string QUIT_COMMAND = "Quit#";

    WaitForSeconds ws = new WaitForSeconds(1f);

    private void Awake()
    {
        if(!UIManager.instance.isThereMainChar) // 메인 캐릭터 없으면 쓰레드 시작 아니면 애네는 서버로부터 데이터받아서 하나의 스크립트가 관리할거임
            StartCoroutine(Communicate());
    }

    public void ConnectToServer()
    {
        SendData(this.name);
        ReadStream();
        Debug.Log("ConnectToServer complete");
    }

    IEnumerator Communicate()
    {
        while (isThreadAlived)
        {
            yield return ws;

            string sendData = $"Move#{Math.Round(transform.position.x, 2)},{Math.Round(transform.position.y, 2)},{Math.Round(transform.position.z, 2)};";
            SendData(sendData);
            ReadStream();
        }
    }

    void ReadStream()
    {
        string readData = ReadData(); // 읽을게 없거나 읽을 수 없으면 null이 반한됌

        // 테스트 코드
        if (readData == null) return;

        try
        {
            int idx1 = readData.IndexOf("#");
            string command = readData.Substring(0, idx1);

            string remain = readData.Substring(idx1 + 1, readData.Length- idx1-1); //#다음부터 다 가져오기

            Debug.Log(command);
            switch (command)
            {
                case "Move":
                    break;
                case "Setting":
                    do
                    {
                        int idx2 = remain.IndexOf(";");
                        string posDataString = remain.Substring(0, idx2);
                        string [] datas = posDataString.Split(',');

                        Debug.Log(remain);
                        remain = remain.Substring(idx2 + 1, remain.Length - idx2 - 1); // 맨 앞 친구 다음 데이터 끝까지
                        Debug.Log(remain);

                        GameObject obj = UIManager.instance.CreateClient(datas[0]);
                        Vector3 objPos = new Vector3(float.Parse(datas[1]), float.Parse(datas[2]), float.Parse(datas[3]));
                        obj.transform.position = objPos;

                    } while (remain.Length > 0);
                    break;

                default:
                    Debug.Log("readData : " + readData);
                    break;
            }
        }
        catch 
        {
            Debug.Log("ReadStream Error");
        }
    }

    void SendData(string sendData)
    {
        try
        {
            NetworkStream stream = tcp.GetStream(); // 여기서 오류나네
            byte[] buffer = new byte[1024];
            buffer = Encoding.UTF8.GetBytes(sendData);
            stream.Write(buffer, 0, sendData.Length);
            stream.Flush();
            Debug.Log("Send : " + sendData);
        }
        catch
        {
            Debug.LogError("SendData error");
        }
    }

    string ReadData()
    {
        try
        {
            NetworkStream stream = tcp.GetStream();
            if (stream.DataAvailable)
            {
                byte[] buffer = new byte[1024];
                int readData = stream.Read(buffer, 0, buffer.Length);
                return Encoding.UTF8.GetString(buffer, 0, readData);
            }
        }
        catch
        {
            Debug.LogError("ReadData error");
        }
        return null;
    }

    private void OnApplicationQuit()
    {
        DisConnectWithServer();
    }

    public void DisConnectWithServer()
    {
        Debug.Log("나가기");

        SendData(QUIT_COMMAND); // 서버에서 연결끊고 삭제하라고 명령
        isThreadAlived = false;  // 쓰레드 끝내기

        UIManager.instance.createdClient = null; // 메인캐릭터 삭제
        UIManager.instance.isThereMainChar = false; // 메인캐릭터 없다고 알려주고
        Destroy(this.gameObject); // 중에 그냥 연결만 끊고 다시 초기화하는 것도 생각해줘야함
    }
}
