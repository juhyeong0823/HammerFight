using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Net.Sockets;
public class UIManager : MonoBehaviour
{
    //�̱���
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

    private GameObject createdClient; // ����� �����߸� �׿�������

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
            createdClient = Instantiate(playerPrefab, transform.position, Quaternion.identity); // Ŭ���̾�Ʈ �޸��� �����
            createdClient.name = name; // �̸� �����ؼ�
            Client objClient = createdClient.GetComponent<Client>(); // �׽�Ű Ŭ���̾�Ʈ �޾ƿ���?
            objClient.tcp = new TcpClient("127.0.0.1", 13000); // �����Ƕ� ��Ʈ �����ؼ�
            objClient.ConnectToServer(); // ���� ����
        }
        catch
        {
            Debug.Log("�ȵ� ���ư�");
            Destroy(createdClient);
            createdClient = null;
        }

    }
}
