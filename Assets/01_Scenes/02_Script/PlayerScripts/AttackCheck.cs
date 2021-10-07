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
            // 상대의 이동 막아버리기 아마 불값을 PlayerInput에 추가하여 처리하면 될 듯
            // 닼소 느낌으로 뒤잡 만들기
        }
    }
    bool DistanceCheck()
    {
        RaycastHit hit;
        return Physics.Raycast(this.transform.position, Vector3.forward, 2f, otherPlayer); // 목을 딸 정도의 거리인가 
    }
}
