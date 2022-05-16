using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region ΩÃ±€≈Ê
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

    public Transform playerParentTrm;

    public GameObject otherPlayerPrefab;
    public GameObject particle;

    public void CreateOtherPlayer(string _name, Vector3 pos, Vector3 rot)
    {
        Debug.Log("CreateOtherPlayer");
        GameObject obj = Instantiate(otherPlayerPrefab, pos, Quaternion.Euler(rot), playerParentTrm);
        obj.name = _name; // ¿Ã∏ß º≥¡§«ÿº≠
        Client.instance.otherClients.Add(obj.name, obj);
        Client.instance.clientsPosDic.Add(obj.name, pos);
        Client.instance.clientsRotDic.Add(obj.name, rot);
    }

    public void PlayDeadFx(Vector3 pos)
    {
        Instantiate(particle, pos, Quaternion.identity);
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
