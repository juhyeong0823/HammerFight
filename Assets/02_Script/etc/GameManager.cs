using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region �̱���
    private static GameManager Instance = null;

    public static GameManager instance
    {
        get
        {
            if(Instance == null)
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

    public GameObject otherPlayerPrefab;

    public float otherPlayerSpeed;

    public void CreateOtherPlayer(string _name, Vector3 pos, Vector3 rot)
    {
        Debug.Log("CreateOtherPlayer");
        GameObject obj = Instantiate(otherPlayerPrefab, pos, Quaternion.Euler(rot));
        obj.name = _name; // �̸� �����ؼ�
        Client.instance.otherClients.Add(obj.name, obj);
        Client.instance.clientsPosDic.Add(obj.name, pos);
        Client.instance.clientsRotDic.Add(obj.name, rot);
    }


    public float GetAnimLength(Animator animator, string animName)
    {
        float time = 0;
        RuntimeAnimatorController ac = animator.runtimeAnimatorController;

        for (int i = 0; i < ac.animationClips.Length; i++)
        {
            if (ac.animationClips[i].name == animName)
            {
                time = ac.animationClips[i].length;
            }
        }

        return time;
    }
}
