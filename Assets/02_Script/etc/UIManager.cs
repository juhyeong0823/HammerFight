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

    public GameObject deathPanel;
    public GameObject helpPanel;

    public InputField nameInput;
    public InputField ipInput;
    public GameObject playerPrefab;
    [HideInInspector] public GameObject createdClient; // ����� �����߸� �׿�������

    private void Start()
    {
        newClientBtn.onClick.AddListener(() =>
        {
            if (nameInput.text.Length > 0 || nameInput.text.Contains(" "))
            {
                string name = nameInput.text;
                string ip = ipInput.text;
                CreateClient(name,ip);
                newClientBtn.gameObject.SetActive(false);
            }
            else 
                Debug.Log("�г����� �־�� �մϴپ�");
        });
        quitBtn.onClick.AddListener(() => OnQuit());
    }

    public void CreateClient(string name, string ip)
    {
        GameObject obj = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity,GameManager.instance.playerParentTrm); // Ŭ���̾�Ʈ �޸��� �����
        obj.name = name; // �̸� �����ؼ�
        Client objClient = obj.GetComponent<Client>(); // �׽�Ű Ŭ���̾�Ʈ �޾ƿ���?
        objClient.tcp = new TcpClient(ip, 13000); // �����Ƕ� ��Ʈ �����ؼ�
        objClient.ConnectToServer(); // ���� ����
        createdClient = obj;
        startCamera.gameObject.SetActive(false);
        helpPanel.SetActive(false);
    }

    public void OnDeath()
    {
        deathPanel.SetActive(true);
        StartCoroutine(Off());
    }

    IEnumerator Off()
    {
        yield return new WaitForSeconds(2f);
        deathPanel.SetActive(false);
    }

    private void OnApplicationQuit()
    {
         OnQuit();
    }
    
    public void OnQuit()
    {
        Player[] players = GameManager.instance.playerParentTrm.GetComponentsInChildren<Player>();
        foreach (var item in players) Destroy(item.gameObject);

        if (createdClient != null)  createdClient.GetComponent<Client>().DisConnectWithServer();
        startCamera.gameObject.SetActive(true);
        newClientBtn.gameObject.SetActive(true);

        Client.instance.otherClients. Clear();
        Client.instance.clientsPosDic.Clear();
        Client.instance.clientsRotDic.Clear();
        //else Debug.LogError("����ĳ���� ����");
    }
}
