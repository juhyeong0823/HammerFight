using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Net.Sockets;
public class UIManager : MonoBehaviour
{
    #region 싱글톤
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

    [SerializeField] private Camera startCamera;

    public Button newClientBtn;
    public Button quitBtn;

    public InputField nameInput;
    public GameObject playerPrefab;
    [HideInInspector] public GameObject createdClient; // 만든놈 오류뜨면 죽여버리게

    private void Start()
    {
        newClientBtn.onClick.AddListener(() =>
        {
            if (nameInput.text.Length > 0 || nameInput.text.Contains(" "))
            {
                string name = nameInput.text;
                CreateClient(name);
                newClientBtn.gameObject.SetActive(false);
            }
            else 
                Debug.Log("닉네임을 넣어야 합니다아");
        });
        quitBtn.onClick.AddListener(() => OnQuit());
    }

    public void CreateClient(string name)
    {
        GameObject obj = Instantiate(playerPrefab, transform.position, Quaternion.identity); // 클라이언트 달린놈 만들고
        obj.name = name; // 이름 설정해서
        Client objClient = obj.GetComponent<Client>(); // 그시키 클라이언트 받아오고?
        objClient.tcp = new TcpClient("127.0.0.1", 13000); // 아이피랑 포트 설정해서
        objClient.ConnectToServer(); // 서버 연결
        createdClient = obj;
        startCamera.gameObject.SetActive(false);
    }

    private void OnApplicationQuit()
    {
         OnQuit();
    }
    
    void OnQuit()
    {
        if (createdClient != null)  createdClient.GetComponent<Client>().DisConnectWithServer();
        startCamera.gameObject.SetActive(true);
        newClientBtn.gameObject.SetActive(true);
        //else Debug.LogError("메인캐릭터 없대");
    }
}
