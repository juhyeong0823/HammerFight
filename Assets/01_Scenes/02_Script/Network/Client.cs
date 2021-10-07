using UnityEngine;
using UnityEngine.Networking;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Collections;

public class Client : MonoBehaviour
{
    public TcpClient tcp;

    private Transform beforeTrm;

    WaitForSeconds ws = new WaitForSeconds(2f);

    private void Awake()
    {
        StartCoroutine(Communicate());
    }

    public void ConnectToServer()
    {
        try
        {
            SendString(this.name);
            ReadStream();
        }
        catch { Debug.Log("에러에러"); }
    }

    IEnumerator Communicate()
    {
        yield return ws;
        if (tcp.GetStream() == null)
        {
            Debug.Log("아이 시팔");
        }
        else
        {
            ReadStream();
            beforeTrm = this.transform;
            string vectorData = "moveData" + (beforeTrm.position.x + "," + beforeTrm.position.y + "," + beforeTrm.position.z).ToString();
            SendTransform(vectorData);
        }
    }

    void ReadStream()
    {
        NetworkStream stream = tcp.GetStream();
        Debug.Log("read");
        byte[] buffer = new byte[1024];
        int readData = stream.Read(buffer, 0, buffer.Length);
        Debug.Log("streamError");
        string data = Encoding.UTF8.GetString(buffer, 0, readData);
        Debug.Log(data);
    }

    void SendString(string sendStr)
    {
        NetworkStream stream = tcp.GetStream(); // 여기서 오류나네
        byte[] buffer = new byte[1024];

        buffer = Encoding.UTF8.GetBytes(sendStr);
        stream.Write(buffer, 0, buffer.Length);
        stream.Flush();
    }

    void SendTransform(string sendStr)
    {
        NetworkStream stream = tcp.GetStream(); // 여기서 오류나네
        byte[] buffer = new byte[1024];

        buffer = Encoding.UTF8.GetBytes(sendStr);
        stream.Write(buffer, 0, buffer.Length);
    }
}
