using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollider : MonoBehaviour
{
    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Attacker"))
        {
            print("���ݹ޾ҽ��ϴ�.");
            //this.GetComponent<Character>().hp -= col.gameObject.GetComponent<Character>().atk; // �ӽ��Լ�
        }
    }
}
