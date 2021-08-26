using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollider : MonoBehaviour
{
    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Attacker"))
        {
            print("공격받았습니다.");
            //this.GetComponent<Character>().hp -= col.gameObject.GetComponent<Character>().atk; // 임시함수
        }
    }
}
