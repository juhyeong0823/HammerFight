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
        if(!UIManager.instance.isThereMainChar) // ���� ĳ���� ������ ������ ���� �ƴϸ� �ֳ״� �����κ��� �����͹޾Ƽ� �ϳ��� ��ũ��Ʈ�� �����Ұ���
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
        string readData = ReadData(); // ������ ���ų� ���� �� ������ null�� ���щ�

        // �׽�Ʈ �ڵ�
        if (readData == null) return;

        try
        {
            int idx1 = readData.IndexOf("#");
            string command = readData.Substring(0, idx1);

            string remain = readData.Substring(idx1 + 1, readData.Length- idx1-1); //#�������� �� ��������

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
                        remain = remain.Substring(idx2 + 1, remain.Length - idx2 - 1); // �� �� ģ�� ���� ������ ������
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
            NetworkStream stream = tcp.GetStream(); // ���⼭ ��������
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
        Debug.Log("������");

        SendData(QUIT_COMMAND); // �������� ������� �����϶�� ���
        isThreadAlived = false;  // ������ ������

        UIManager.instance.createdClient = null; // ����ĳ���� ����
        UIManager.instance.isThereMainChar = false; // ����ĳ���� ���ٰ� �˷��ְ�
        Destroy(this.gameObject); // �߿� �׳� ���Ḹ ���� �ٽ� �ʱ�ȭ�ϴ� �͵� �����������
    }
}
