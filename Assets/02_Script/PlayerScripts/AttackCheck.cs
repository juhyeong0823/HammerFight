using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCheck : MonoBehaviour
{
    public LayerMask otherPlayer;

    private void Update()
    {
        if (DistanceCheck())
        {
            // ����� �̵� ���ƹ����� �Ƹ� �Ұ��� PlayerInput�� �߰��Ͽ� ó���ϸ� �� ��
            // ���� �������� ���� �����
        }
    }
    bool DistanceCheck()
    {
        RaycastHit hit;
        return Physics.Raycast(this.transform.position, Vector3.forward, 2f, otherPlayer); // ���� �� ������ �Ÿ��ΰ� 
    }
}
