using UnityEngine;
using UnityEngine.Networking;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Threading;

public class Client : MonoBehaviour
{
    #region �̱���
    private static Client Instance = null;

    public static Client instance
    {
        get
        {
            if (Instance == null)
            {
                return null;
            }
            return Instance;
        }
    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    #endregion

    public TcpClient tcp;
    public bool isThreadAlived = true;

    public Dictionary<string, Vector3> clientsPosDic = new Dictionary<string, Vector3>();
    public Dictionary<string, Vector3> clientsRotDic = new Dictionary<string, Vector3>();
    public Dictionary<string, GameObject> otherClients = new Dictionary<string, GameObject>();
    WaitForSeconds ws = new WaitForSeconds(0.1f);

    const string QUIT_COMMAND = "Quit#;";
    const string MOVE_COMMAND = "Move#";
    const string ROTATE_COMMAND = "Rotate#";
    const string ATTACK_COMMAND = "Attack#";
    const string HITTED_COMMAND = "Hitted#";

    List<Action> actionList = new List<Action>();

    private void Update()
    {
        int count = actionList.Count;

        for (int i = 0; i < count; i++)
        {
            actionList[0]();
            actionList.RemoveAt(0);
        }

        Debug.Log("otherClients  Count : " + otherClients.Count);
        Debug.Log("clientsPosDic Count : " + clientsPosDic.Count);
        Debug.Log("clientsRotDic Count : " + clientsRotDic.Count);

        foreach(string clientName in otherClients.Keys)
        {
            Debug.Log("����");
            otherClients[clientName].transform.position = Vector3.Lerp(otherClients[clientName].transform.position, clientsPosDic[clientName], 0.1f);
            otherClients[clientName].transform.rotation = Quaternion.Euler(clientsRotDic[clientName]);
        }
    }

    public void ConnectToServer()
    {
        string readData = "";
        SendData(this.name);
        while (true)
        {
            readData = ReadData();
            if (readData != null) break;
        }
        if (readData.Contains("Connect#"))// ������ ���̸�
        {
            Debug.Log("����");
            StartCoroutine(Communicate());
        }
        else if (readData.Contains("DuplicateName#"))
        {
            Debug.Log("���� ����");
            Destroy(this.gameObject);
            UIManager.instance.createdClient = null;
        }
    }

    public void AttackSend(string animName) // ���� �ִϸ��̼� �������.
    {
        string sendData = $"{ATTACK_COMMAND}{this.name},{animName};";
        SendData(sendData);
    }

    public void HittedSend(string hittedPlayerName, Vector3 dir) // �� ��ġ�� ���������� ������, �� ��� �г����̶� ���ư� ���� �����ֱ�
    {
        string sendData = $"{HITTED_COMMAND}{hittedPlayerName},{dir.x},{dir.y},{dir.z};";
        SendData(sendData);
    }

    public void MoveSend()
    {
        string posData = $"{MOVE_COMMAND}{Math.Round(transform.position.x, 2)},{Math.Round(transform.position.y, 2)},{Math.Round(transform.position.z, 2)};";
        Debug.Log("posData : " + posData);
        SendData(posData);
    }

    public void RotateSend()
    {
        string rotData = $"{ROTATE_COMMAND}{Math.Round(transform.eulerAngles.x, 2)},{Math.Round(transform.eulerAngles.y, 2)},{Math.Round(transform.eulerAngles.z, 2)};";
        Debug.Log("rotData : " + rotData);
        SendData(rotData);
    }

    public IEnumerator Communicate()
    {
        while (isThreadAlived)
        {
            yield return ws;

            MoveSend();
            RotateSend();
            ReadStream();
        }
    }

    void ReadStream()
    {
        string readData = ReadData(); // ������ ���ų� ���� �� ������ null�� ���щ�
        if (readData == null) return;
        string remain = readData;
        do
        {
            int idx1 = remain.IndexOf("#");
            int idx2 = remain.IndexOf(";");
            string useData = remain.Substring(0, idx2); // �̿��� ������ ��������
            string cmdType = useData.Substring(0, idx1);
            string command = useData.Substring(idx1 + 1, useData.Length - idx1 - 1);  //#�������� �� ��������
            remain = remain.Substring(idx2 + 1, remain.Length - idx2 - 1);
            string[] data = command.Split(',');

            Debug.Log(cmdType);
            switch (cmdType)
            {
                case "Move":
                    lock (actionList)
                    {
                        actionList.Add(() =>
                        {
                            clientsPosDic[data[0]] = new Vector3(float.Parse(data[1]), float.Parse(data[2]), float.Parse(data[3]));
                        });
                    }
                    break;

                case "Rotate":
                    lock (actionList)
                    {
                        actionList.Add(() =>
                        {
                            clientsRotDic[data[0]] = new Vector3(float.Parse(data[1]), float.Parse(data[2]), float.Parse(data[3]));
                        });
                    }
                    break;

                case "Setting":
                    lock (actionList)
                    {
                        actionList.Add(() =>
                        {
                            Vector3 objPos_setting = new Vector3(float.Parse(data[1]), float.Parse(data[2]), float.Parse(data[3]));
                            Vector3 objRot_setting = new Vector3(float.Parse(data[4]), float.Parse(data[5]), float.Parse(data[6]));
                            GameManager.instance.CreateOtherPlayer(data[0], objPos_setting, objRot_setting);
                        });
                    }
                    break;

                case "Attack":
                    lock (actionList)
                    {
                        actionList.Add(() =>
                        {
                            if (data[0] == this.name) return;
                            Player obj = GameObject.Find(data[0]).GetComponent<Player>();
                            obj.UpperSwing();
                        });
                    }
                    break;

                case "Hitted":
                    lock (actionList)
                    {
                        actionList.Add(() =>
                        {
                            Vector3 dir = new Vector3(float.Parse(data[0]), float.Parse(data[1]), float.Parse(data[2]));
                            Hitted(dir);
                        });
                    }
                    break;
            }
        } while (remain.Length > 0);
    }

    void Hitted(Vector3 dir)
    {
        GetComponent<Player>().Hitted();
        GetComponent<Rigidbody>().velocity = dir; // ������ �ִ� �����ٵ� ������ ����
        Debug.Log($"���� ���ư� ���� : {dir}");
    }

    #region
    void SendData(string sendData)
    {
        try
        {
            NetworkStream stream = tcp.GetStream(); // ���⼭ ��������
            byte[] buffer = Encoding.UTF8.GetBytes(sendData);

            stream.Write(buffer, 0, sendData.Length);
            stream.Flush();
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
                byte[] buffer = new byte[2048];
                int bufferLength = stream.Read(buffer, 0, buffer.Length);
                string readData = Encoding.UTF8.GetString(buffer, 0, bufferLength);
                return readData;
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
        Destroy(this.gameObject); // �߿� �׳� ���Ḹ ���� �ٽ� �ʱ�ȭ�ϴ� �͵� �����������
    }
    #endregion
}
