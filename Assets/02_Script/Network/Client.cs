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
    #region 싱글톤
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
    WaitForSeconds ws = new WaitForSeconds(0.2f);

    const string QUIT_COMMAND = "Quit#;";
    const string MOVE_COMMAND = "Move#";
    const string ROTATE_COMMAND = "Rot#";
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

        foreach(string clientName in otherClients.Keys)
        {
            Debug.Log("실행");
            otherClients[clientName].transform.position = Vector3.Lerp(otherClients[clientName].transform.position, clientsPosDic[clientName], 0.1f);
            Debug.Log(clientsRotDic[clientName]);
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
        if (readData.Contains("Connect#"))// 데이터 널이면
        {
            Debug.Log("연결");
            StartCoroutine(Communicate());
        }
        else if (readData.Contains("DuplicateName#"))
        {
            Debug.Log("연결 실패");
            Destroy(this.gameObject);
            UIManager.instance.createdClient = null;
        }
    }

    public void AttackSend(string animName) // 공격 애니메이션 돌리라고.
    {
        string sendData = $"{ATTACK_COMMAND}{this.name},{animName};";
        SendData(sendData);
    }

    public void HittedSend(string hittedPlayerName, Vector3 dir) // 내 망치가 누군가에게 닿으면, 그 사람 닉네임이랑 날아갈 방향 보내주기
    {
        string sendData = $"{HITTED_COMMAND}{hittedPlayerName},{dir.x},{dir.y},{dir.z};";
        SendData(sendData);
    }

    public void MoveSend()
    {
        string posData = $"{MOVE_COMMAND}{Math.Round(transform.position.x, 2)},{Math.Round(transform.position.y, 2)},{Math.Round(transform.position.z, 2)};";
        SendData(posData);
    }

    public void RotateSend()
    {
        string rotData = $"{ROTATE_COMMAND}{(int)(transform.eulerAngles.x)},{(int)(transform.eulerAngles.y)},{(int)(transform.eulerAngles.z)};";
        SendData(rotData);
    }

    public IEnumerator Communicate()
    {
        while (isThreadAlived)
        {
            yield return ws;

            RotateSend();
            MoveSend();
            ReadStream();
        }
    }

    void ReadStream()
    {
        string readData = ReadData(); // 읽을게 없거나 읽을 수 없으면 null이 반한됌
        if (readData == null) return;
        string remain = readData;
        do
        {
            int idx1 = remain.IndexOf("#");
            int idx2 = remain.IndexOf(";");
            string useData = remain.Substring(0, idx2); // 이용할 데이터 가져오기
            string cmdType = useData.Substring(0, idx1);
            string command = useData.Substring(idx1 + 1, useData.Length - idx1 - 1);  //#다음부터 다 가져오기
            remain = remain.Substring(idx2 + 1, remain.Length - idx2 - 1);
            string[] data = command.Split(',');

            //Debug.Log(cmdType);
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

                case "Rot":
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
                            GameManager.instance.CreateOtherPlayer(data[0], objPos_setting, Vector3.zero);
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
                            obj.PlayAnim(obj.weaponAnim, data[1]);
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

                case "Remove":
                    lock (actionList)
                    {
                        actionList.Add(() =>
                        {
                            GameObject destroyObj = GameObject.Find(data[0]);
                            GameManager.instance.PlayDeadFx(destroyObj.transform.position);
                            Client.instance.otherClients. Remove(destroyObj.name);
                            Client.instance.clientsPosDic.Remove(destroyObj.name);
                            Client.instance.clientsRotDic.Remove(destroyObj.name);
                            Destroy(destroyObj);
                        });
                    }
                    break;
                default:
                    break;
            }
        } while (remain.Length > 0);
    }

    void Hitted(Vector3 dir)
    {
        GetComponent<Player>().Hitted();
        GetComponent<Rigidbody>().velocity = dir; // 나한테 있는 리짓바디에 접근을 못함
    }

    #region
    void SendData(string sendData)
    {
        try
        {
            NetworkStream stream = tcp.GetStream(); // 여기서 오류나네
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
        Debug.Log("나가기");

        SendData(QUIT_COMMAND); // 서버에서 연결끊고 삭제하라고 명령
        isThreadAlived = false;  // 쓰레드 끝내기

        UIManager.instance.createdClient = null; // 메인캐릭터 삭제
        Destroy(this.gameObject); // 중에 그냥 연결만 끊고 다시 초기화하는 것도 생각해줘야함
    }
    #endregion
}
