using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Net.Sockets;
public class UIManager : MonoBehaviour
{
    #region �̱���
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
    [HideInInspector] public GameObject createdClient; // ����� �����߸� �׿�������

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
                Debug.Log("�г����� �־�� �մϴپ�");
        });
        quitBtn.onClick.AddListener(() => OnQuit());
    }

    public void CreateClient(string name)
    {
        GameObject obj = Instantiate(playerPrefab, transform.position, Quaternion.identity); // Ŭ���̾�Ʈ �޸��� �����
        obj.name = name; // �̸� �����ؼ�
        Client objClient = obj.GetComponent<Client>(); // �׽�Ű Ŭ���̾�Ʈ �޾ƿ���?
        objClient.tcp = new TcpClient("127.0.0.1", 13000); // �����Ƕ� ��Ʈ �����ؼ�
        objClient.ConnectToServer(); // ���� ����
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
        //else Debug.LogError("����ĳ���� ����");
    }
}
