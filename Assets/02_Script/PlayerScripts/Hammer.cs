using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : MonoBehaviour
{
    private float attackPower = 30; // �󸶳� �ָ� ������,
    [SerializeField] private Transform centerOfHammer;

    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("OtherPlayer"))
        {
            Vector3 dir = new Vector3(col.gameObject.transform.position.x - centerOfHammer.position.x,
                col.gameObject.transform.position.y - centerOfHammer.position.y,
                col.gameObject.transform.position.z - centerOfHammer.position.z);

            dir = (dir * attackPower) + transform.root.GetComponent<Rigidbody>().velocity; // �� �̵��ӵ���, ��ġ ���Դ����� ��
            Client.instance.HittedSend(col.gameObject.name, dir);
        }
    }
}
