using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : MonoBehaviour
{
    public enum AttackState
    {
        swing,
        upperSwing
    };

    public Player player;


    public AttackState state = AttackState.swing;
    private float attackPower = 10; // 얼마나 멀리 날릴지,
    [SerializeField] private Transform centerOfHammer;


    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("OtherPlayer"))
        {
            Vector3 dir = new Vector3(col.gameObject.transform.position.x - centerOfHammer.position.x,
                col.gameObject.transform.position.y - centerOfHammer.position.y,
                col.gameObject.transform.position.z - centerOfHammer.position.z);

            if (state == AttackState.swing)
            {
                dir = Vector3.zero; // 맞으면 멈추게
            }
            else if (state == AttackState.upperSwing)
            {
                dir = (dir * attackPower * player.upperPower) + player.rigid.velocity; // 내 이동속도와, 망치 무게느낌의 힘
            }
            Client.instance.HittedSend(col.gameObject.name, dir);
        }
    }

}
