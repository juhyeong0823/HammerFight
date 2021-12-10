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

    public List<GameObject> otherPlayers = new List<GameObject>();
    public GameObject otherPlayerPrefab;

    public float otherPlayerSpeed;

    public GameObject CreateOtherPlayer(string _name, Vector3 pos)
    {
        GameObject obj = Instantiate(otherPlayerPrefab, pos, Quaternion.identity);
        obj.name = _name; // ¿Ã∏ß º≥¡§«ÿº≠
        Client.instance.clientsPosDic.Add(obj.name, pos);
        Client.instance.clients.Add(obj.name, obj);
        return obj;
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
