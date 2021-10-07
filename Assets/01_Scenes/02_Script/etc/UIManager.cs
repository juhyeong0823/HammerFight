using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Net.Sockets;
public class UIManager : MonoBehaviour
{
    //싱글톤
    #region
    private static UIManager Instance = null;

    public static UIManager instance
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


    public Button newClientBtn;
    public InputField nameInput;
    public InputField ipInput;
    public GameObject playerPrefab;

    private GameObject createdClient; // 만든놈 오류뜨면 죽여버리게

    private void Start()
    {
        newClientBtn.onClick.AddListener(() =>
        {
            string name = nameInput.text;
            string ip = ipInput.text;
            CreateClient(name);
            this.gameObject.SetActive(false);
        });
    }



    void CreateClient(string name/*,string ip*/)
    {
        try
        {
            createdClient = Instantiate(playerPrefab, transform.position, Quaternion.identity); // 클라이언트 달린놈 만들고
            createdClient.name = name; // 이름 설정해서
            Client objClient = createdClient.GetComponent<Client>(); // 그시키 클라이언트 받아오고?
            objClient.tcp = new TcpClient("127.0.0.1", 13000); // 아이피랑 포트 설정해서
            objClient.ConnectToServer(); // 서버 연결
        }
        catch
        {
            Debug.Log("안돼 돌아가");
            Destroy(createdClient);
            createdClient = null;
        }

    }
}
